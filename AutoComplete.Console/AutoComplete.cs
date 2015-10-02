using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete.ConsoleApp
{
    public class AutoComplete
    {
        static void Main(string[] args)
        {
            Trie dictionary = null;
            List<string> output = null;

            Task task = Task.Run(async () =>
            {
                using (Stream stream = Console.OpenStandardInput())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dictionary = await TrieLoader.LoadTrie(reader);

                        string currentLine = reader.ReadLine();
                        int count = 0;

                        count = int.Parse(currentLine);
                        output = new List<string>();
                        for (int i = 0; i < count; i++)
                        {
                            currentLine = reader.ReadLine();
                            var words = dictionary.Get(currentLine);
                            if (words != null)
                            {
                                output.AddRange(words);
                                output.Add(string.Empty);
                            }
                        }
                    }
                }
            });

            task.ContinueWith(t =>
            {
                Console.WriteLine("Some error occured while application running!");
                foreach (var exception in t.Exception.InnerExceptions)
                {
                    Console.WriteLine(exception.Message);
                }

            }, TaskContinuationOptions.OnlyOnFaulted);

            task.Wait();

            Console.WriteLine();
            foreach (var word in output)
            {
                    Console.WriteLine(word);
            }


            Console.ReadLine();
        }
    }
}