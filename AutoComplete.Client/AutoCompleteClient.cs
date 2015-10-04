using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoComplete.Client
{
    class AutoCompleteClient
    {
        /// <summary>
        /// Порт хоста
        /// </summary>
        private static int _port;

        /// <summary>
        /// Адрес хоста
        /// </summary>
        private static string _host;

        /// <summary>
        /// "Флаг" для остановки приложения
        /// </summary>
        private static ManualResetEvent _exitEvent;

        static void Main(string[] args)
        {
            if (!ParseCommandArgs(args))
            {
                Console.WriteLine("[Client] Press any key to exit");
                Console.ReadKey();
                return;
            }

            _exitEvent = new ManualResetEvent(false);
            
            Console.CancelKeyPress += (sender, e) => _exitEvent.Set();

            ConnectAsync();

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
                Console.WriteLine("Should be two input arguments");
                return false;
            }

            _host = args[0];

            if (!int.TryParse(args[1], out _port))
            {
                Console.WriteLine("Incorrect format of second argument");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Подключение к хосту
        /// </summary>
        private static async void ConnectAsync()
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    Console.WriteLine("[Client] Connecting to server");
                    await tcpClient.ConnectAsync(_host, _port);
                    Console.WriteLine("[Client] Connected to server");
                    await ProcessClient(tcpClient);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Client] Unexcpected exception: " + ex.Message);
                _exitEvent.Set();
            }
        }

        /// <summary>
        /// Обработка отправки запросов на сервер
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        private static async Task ProcessClient(TcpClient tcpClient)
        {
            using (var networkStream = tcpClient.GetStream())
            {
                using (var reader = new StreamReader(Console.OpenStandardInput()))
                {
                    while (true)
                    {
                        string request = await reader.ReadLineAsync();

                        var response = await ProcessRequest(request, networkStream);

                        if (!string.IsNullOrEmpty(response))
                        {
                            Console.WriteLine(response);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Обработка запроса: отправка на сервер, получение результата
        /// </summary>
        /// <param name="request"></param>
        /// <param name="networkStream"></param>
        /// <returns></returns>
        private static async Task<string> ProcessRequest(string request, NetworkStream networkStream)
        {
            // отсекаем пустые запросы к серверу. Запрос c null сознательно не отсекаем - флаг отключения клиента
            if (string.IsNullOrEmpty(request) && request != null)
            {
                return null;
            }

            byte[] requestBytes = Encoding.ASCII.GetBytes(request);

            await networkStream.WriteAsync(requestBytes, 0, requestBytes.Length);

            var buffer = new byte[8192];
            var byteCount = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, byteCount);
            
        }
    }
}
