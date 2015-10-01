using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace AutoComplete.Tests
{
    [TestClass]
    public class TrieLoadTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch sw = Stopwatch.StartNew();
            TrieLoader loaader = new TrieLoader();

            FileInfo file = new FileInfo("test.in");

            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    long StopBytes = 0;
                    long StartBytes = GC.GetTotalMemory(true);

                    var result = Trie.LoadTrie(reader);

                    StopBytes = GC.GetTotalMemory(true);

                    Console.WriteLine("Size is " + ((StopBytes - StartBytes)).ToString());

                    string currentLine = reader.ReadLine();
                    int count = 0;

                    count = int.Parse(currentLine);
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

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(10), "Не уложились по времени выполнения");
        }

        private Trie LoadTrie(string path)
        {
            Trie result = null;

            

            return result;
        }
    }
}
