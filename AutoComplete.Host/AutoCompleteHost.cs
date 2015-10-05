using AutoComplete;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCopmlete.Host
{
    class AutoCompleteHost
    {
        /// <summary>
        /// Словарь, осуществляющий поиск по подстроке
        /// </summary>
        private static Trie _dictionary;

        /// <summary>
        /// Порт, на котором поднимаем tcp-хост
        /// </summary>
        private static int _port;

        /// <summary>
        /// Путь в словарю
        /// </summary>
        private static string _path;

        /// <summary>
        /// "Флаг" для остановки приложения
        /// </summary>
        private static ManualResetEvent _exitEvent;

        static void Main(string[] args)
        {
            if (!ParseCommandArgs(args))
            {
                Console.WriteLine("[Server] Press any key to exit");
                Console.ReadKey();
                return;
            }

            _exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) => _exitEvent.Set(); 

            Console.WriteLine("[Server] Begin initialization");
            InitDictionaryAsync().Wait();
            
            StartListenerAsync();
            Console.WriteLine("[Server] TCP host successfuly started");

            _exitEvent.WaitOne();
        }

        /// <summary>
        /// Осуществляет проверку аргументов командной строки
        /// </summary>
        /// <param name="args"></param>
        /// <returns>True, если аргументы соответствуют требуемому формату</returns>
        private static bool ParseCommandArgs(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("[Server] Should be two input arguments");
                return false;
            }

            _path = args[0];
            if (!File.Exists(_path))
            {
                Console.WriteLine("[Server] Specified dictionary path did not exists");
            }

            if (!int.TryParse(args[1], out _port))
            {
                Console.WriteLine("[Server] Incorrect format of second argument");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Инициализация словаря
        /// </summary>
        /// <returns></returns>
        private static async Task InitDictionaryAsync()
        {
            using (FileStream stream = File.OpenRead(_path))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    _dictionary = await TrieLoader.LoadAsync(reader);
                }
            }
        }

        /// <summary>
        /// Запускает tcp-хост
        /// </summary>
        /// <param name="port"></param>
        private static async void StartListenerAsync()
        {
            try
            {
                var tcpListener = TcpListener.Create(_port);
                tcpListener.Start();

                while (true)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    ProcessClientAsync(tcpClient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Server] Unexcpected exception: " + ex.Message);
                _exitEvent.Set();
            }
        }

        /// <summary>
        /// Осуществляем работу с клиентами tcp-хоста
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        private static async void ProcessClientAsync(TcpClient tcpClient)
        {
            try
            {
                using (tcpClient)
                {
                    Console.WriteLine("[Server] Client has connected");
                    using (var networkStream = tcpClient.GetStream())
                    {
                        var buffer = new byte[8192];
                        while (true)
                        {
                            var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                            if (byteCount == 0) // при штатной остановке клиента в поток приходит 0 байт. Фиксируем факт отключения клиента
                            {
                                Console.WriteLine("[Server] Client has been disconected");
                                break;
                            }

                            byte[] response = ProcessRequest(buffer, byteCount);
                            await networkStream.WriteAsync(response, 0, response.Length);
                        }
                    }
                }
            }
            catch (IOException) //при нештатной остановке клиента при вызове метода networkStream.ReadAsync получаем IOException. Фиксируем факт отключения клиента
            {
                Console.WriteLine("[Server] Client has been disconected");
            }
            catch (Exception ex) // Проблем с одним клиентов не должны приводить к падению сервера. Просто фиксируем факт ошибки в логе
            {
                Console.WriteLine("[Server] " + ex.Message);
            }
        }

        /// <summary>
        /// Осуществляет преобразование входного потока байт в строку, обработку запроса и формирование ответа клиенту в виде массива байт
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        private static byte[] ProcessRequest(byte[] buffer, int byteCount)
        {
            string resultString = null;
            try
            {
                // Подготовка и проверка входных данных
                var request = Encoding.ASCII.GetString(buffer, 0, byteCount);

                if (string.IsNullOrEmpty(request) || !request.StartsWith("get ", StringComparison.OrdinalIgnoreCase))
                {
                    resultString = "Incorrect request. Command should start with 'get' keyword";
                }

                string[] requestParts = request.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (requestParts.Length != 2)
                {
                    resultString = "Incorrect request format. Use 'get <prefix>' format";
                }

                // Непосредственно обработка запроса
                if (resultString == null)
                {
                    var result = _dictionary.Get(requestParts[1]);
                    resultString = result == null
                        ? string.Format("Could not find '{0}'", requestParts[1])
                        : string.Join(Environment.NewLine, result);
                }
            }
            catch (Exception ex)
            {
                resultString = "Unexcpected exception: " + ex.Message;
            }

            return Encoding.ASCII.GetBytes(resultString); 
        }
    }
}
