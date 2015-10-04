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
                    long stopBytes = 0;

                    long startBytes = GC.GetTotalMemory(true);
                    var result = TrieLoader.LoadAsync(reader).Result;
                    stopBytes = GC.GetTotalMemory(true);

                    Console.WriteLine("Size is " + ((long)(stopBytes - startBytes)).ToString());

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
                var result = TrieLoader.LoadAsync(null).Result;
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
                        var result = TrieLoader.LoadAsync(reader).Result;
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
                        var result = TrieLoader.LoadAsync(reader).Result;
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
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooLessLines()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("5");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooMuchWordsCount()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("100001");
                writer.WriteLine("test 10");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooLessWordsCount()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("0");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooBigOccurrence()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("test 1000001");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooLessOccurrence()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("test 0");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
        public void TrieLoaderTest_TooLongWord()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine("1");
                writer.WriteLine("veryverylongword 10");
                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    try
                    {
                        var result = TrieLoader.LoadAsync(reader).Result;
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
