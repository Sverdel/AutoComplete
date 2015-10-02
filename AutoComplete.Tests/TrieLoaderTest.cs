using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace AutoComplete.Tests
{
    [TestClass]
    public class TrieLoaderTest
    {
        [TestMethod]
        public void LoadTest()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int found = 0, notfound = 0;
            TrieLoader loaader = new TrieLoader();

            FileInfo file = new FileInfo("test.in");

            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var result = TrieLoader.LoadTrie(reader).Result;
                    string currentLine = reader.ReadLine();
                    int count = 0;

                    count = int.Parse(currentLine);
                    
                    for (int i = 0; i < count; i++)
                    {
                        currentLine = reader.ReadLine();
                        if (result.Get(currentLine) == null)
                        {
                            notfound++;
                        }
                        else
                        {
                            found++;
                        }
                    }

                    Console.WriteLine("{0}, {1}", found, notfound);
                }
            }

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(10), "Не уложились по времени выполнения");

            Assert.AreEqual(9379, found, "Не совпало количество найденных слов");
            Assert.AreEqual(5621, notfound, "Не совпало количество ненайденных слов");
        }
        
    }
}
