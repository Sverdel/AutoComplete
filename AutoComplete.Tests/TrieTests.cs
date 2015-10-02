using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoComplete;

namespace AutoComplete.Tests
{
    [TestClass]
    public class TrieTests
    {
        [TestMethod]
        public void TrieConstructorTest()
        {
            Trie trie = new Trie();

            Assert.IsNotNull(trie, "node must have a value");
        }

        [TestMethod]
        public void TrieAddTest()
        {
            Trie dictioanry = new Trie();
            dictioanry.Add("a", 100);

            PrivateObject privateTrie = new PrivateObject(dictioanry);
            TrieNode root = (TrieNode)privateTrie.GetField("_root");

            Assert.IsNotNull(root, "root must have a value");
            Assert.IsNotNull(root.Leaves, "root.Leaves have a value");
            Assert.AreEqual(1, root.Leaves.Count, "Incorrect leaves count"); 
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrieAddTest_Duplicate()
        {
            Trie dictioanry = new Trie();
            dictioanry.Add("a", 100);
            dictioanry.Add("a", 100);
        }

        [TestMethod]
        public void GetTest_Single()
        {
            Trie dictioanry = new Trie();
            dictioanry.Add("a", 100);

            var result = dictioanry.Get("a");

            Assert.IsNotNull(result, "did not found word");
            Assert.AreEqual(1, result.Count(), "Incorrect words count");
        }

        [TestMethod]
        public void GetTest_Null()
        {
            Trie dictioanry = new Trie();
            dictioanry.Add("a", 100);

            var result = dictioanry.Get("b");

            Assert.IsNull(result, "result must be null");
        }

        [TestMethod]
        public void GetTest_Multiple()
        {
            Trie dictioanry = new Trie();

            string[] input = new string[4] { "a", "ab", "abc", "bbc" };
            dictioanry.Add(input[0], 100);
            dictioanry.Add(input[1], 200);
            dictioanry.Add(input[2], 300);
            dictioanry.Add(input[3], 400);

            var result = dictioanry.Get("a");

            Assert.IsNotNull(result, "did not found word");
            Assert.AreEqual(3, result.Count(), "Incorrect words count");

            
            
        }
    }
}
