using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    internal sealed class TrieNode
    {
        private SortedList<KeyValuePair<int, string>, string> _words;

        /// <summary>
        /// Помпаратор для сортировки "дочерних" слов. Сортирует слова сначала по встречаемости в порядке убывания, затем - по алфавиту
        /// </summary>
        private static Comparer<KeyValuePair<int, string>> _comparer = Comparer<KeyValuePair<int, string>>.Create((x, y) =>
        {
            var result = y.Key.CompareTo(x.Key);
            return result == 0
                ? x.Value.CompareTo(y.Value)
                : result;
        });

        internal Dictionary<char, TrieNode> Childs { get; set; }

        
        
        internal TrieNode()
        {
            Childs = new Dictionary<char, TrieNode>();
            _words = new SortedList<KeyValuePair<int, string>, string>(_comparer);
        }

        internal void AddChildWord(string key, int value)
        {
            _words.Add(new KeyValuePair<int, string>(value, key), key);
            
            if (_words.Count > 10)
            {
                _words.RemoveAt(10);
            }
        }

        internal IEnumerable<string> GetWords()
        {
            return _words.Values;
        }
    }
}
