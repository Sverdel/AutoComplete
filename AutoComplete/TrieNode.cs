using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public sealed class TrieNode
    {
        //private static Comparer<KeyValuePair<int, string>> _comparer;
        private static Comparer<int> _comparer;

        public char Key { get; set; }

        public int? Occurrence { get; set; }

        public bool HasValue { get { return Occurrence.HasValue; } }

        public Dictionary<char, TrieNode> Childs { get; set; }

        //public SortedList<KeyValuePair<int, string>, string> ChildWods { get; private set; }
        public SortedList<int, string> ChildWods { get; private set; }

        static TrieNode()
        {
            //_comparer = Comparer<KeyValuePair<int, string>>.Create((x, y) =>
            //{
            //    var result = y.Key.CompareTo(x.Key);
            //    return result == 0
            //        ? x.Value.CompareTo(y.Value)
            //        : result;
            //});
            _comparer = Comparer<int>.Create((x, y) =>
            {
                var result = y.CompareTo(x);
                return result == 0 ? 1 : result;
            });
        }

        public TrieNode()
        {
            Childs = new Dictionary<char, TrieNode>();

            

            //ChildWods = new SortedList<KeyValuePair<int, string>, string>(_comparer);
            ChildWods = new SortedList<int, string>(_comparer);
        }

        internal void AddChildWord(string key, int value)
        {
            //ChildWods.Add(new KeyValuePair<int, string>(value, key), key);
            ChildWods.Add(value, key);

            if (ChildWods.Count > 10)
            {
                ChildWods.ElementAt(10);
            }

        }
    }
}
