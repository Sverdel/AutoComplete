using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public sealed class TrieLoader
    {

        /// <summary>
        /// Загрузка словаря из потока
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<Trie> LoadTrie(StreamReader reader)
        {
            Trie result = new Trie();
            string currentLine = await reader.ReadLineAsync();

            int count = 0;

            if (!int.TryParse(currentLine, out count))
            {
                throw new ArgumentException("Incorrect input data format. In first row must be dictionary size");
            }

            for (int i = 0; i < count; i++)
            {
                currentLine = await reader.ReadLineAsync();
                string[] current = currentLine.Split(' ');
                if (current.Length != 2)
                {
                    throw new ArgumentException("Incorrect input data format. Rows with words must be space delimited with occurrence");
                }

                int occurrence = int.Parse(current[1]);

                result.Add(current[0], occurrence);
            }

            return result;
        }
    }
}
