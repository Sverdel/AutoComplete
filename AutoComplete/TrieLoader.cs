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
        public Trie LoadTrie(string path)
        {
            Trie result = new Trie();

            FileInfo file = new FileInfo(path);

            if (!file.Exists)
            {
                throw new ArgumentException();
            }

            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string currentLine = reader.ReadLine();

                    int count = 0;

                    if (!int.TryParse(currentLine, out count))
                    {
                        ///
                    }

                    for (int i = 0; i < count; i++)
                    {
                        string[] current = reader.ReadLine().Split(' ');
                        if (current.Length != 2)
                        {
                            ///
                        }

                        int occurency = int.Parse(current[1]);

                        result.Add(current[0], occurency);
                    }

                    currentLine = reader.ReadLine();

                    if (!int.TryParse(currentLine, out count))
                    {
                        ///
                    }
                    int good = 0, bad = 0;
                    for (int i = 0; i < count; i++)
                    {
                        currentLine = reader.ReadLine();
                        if (result.Get(currentLine) == null)
                        {
                            bad++;
                        }
                        else
                        {
                            good++;
                        }
                    }

                    Console.WriteLine("{0}, {1}", good, bad);
                }
            }

            return result;
        }
    }
}
