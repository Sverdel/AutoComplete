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
            FileInfo file = new FileInfo("test.in");

            using (FileStream stream = file.OpenRead())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    long StopBytes = 0;

                    long StartBytes = GC.GetTotalMemory(true);
                    var result = TrieLoader.Load(reader).Result;
                    StopBytes = GC.GetTotalMemory(true);

                    Console.WriteLine("Size is " + ((long)(StopBytes - StartBytes)).ToString());

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrieLoaderTest_NullStream()
        {
            try
            {
                var result = TrieLoader.Load(null).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrieLoaderTest_EndStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    reader.ReadToEnd();

                    try
                    {
                        var result = TrieLoader.Load(reader).Result;
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TrieLoaderTest_IncorrectFirstRow()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("Error");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.Load(reader).Result;
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TrieLoaderTest_IncorrectWordRow()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("5");
                writer.WriteLine("incorrect");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.Load(reader).Result;
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
        }

    }
}
