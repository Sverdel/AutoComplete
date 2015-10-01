using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

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

            loaader.LoadTrie("test2.in");

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(1), "Не уложились по времени выполнения");
        }
    }
}
