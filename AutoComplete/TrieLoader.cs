﻿using System;
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
        public static async Task<Trie> Load(StreamReader reader)
        {
            if (reader == null || reader.EndOfStream)
            {
                throw new ArgumentException("Incorrect StreamReader state");
            }

            Trie result = new Trie();
            string currentLine = await reader.ReadLineAsync();

            int wordsCount = 0;

            if (!int.TryParse(currentLine, out wordsCount))
            {
                throw new FormatException("Incorrect input data format. In first row must be dictionary size");
            }

            for (int i = 0; i < wordsCount; i++)
            {
                currentLine = await reader.ReadLineAsync();
                string[] current = currentLine.Split(' ');
                if (current.Length != 2)
                {
                    throw new FormatException("Incorrect input data format. Rows with words must contains word and occurrence delimeted by space");
                }

                int occurrence = int.Parse(current[1]);

                result.Add(current[0], occurrence);
            }

            return result;
        }
    }
}
