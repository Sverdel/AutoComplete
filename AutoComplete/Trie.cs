using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public sealed class Trie
    {
        public TrieNode Root { get; set; }

        public Trie()
        {
            Root = new TrieNode();
        }

        public void Add(string key, int value)
        {
            TrieNode current = Root;
            foreach (char keyPart in key)
            {
                if (!current.Childs.ContainsKey(keyPart))
                {
                    current.Childs.Add(keyPart, new TrieNode { Key = keyPart });
                }

                current = current.Childs[keyPart];
                current.AddChildWord(key, value);
            }

            current.Occurrence = value;
        }

        public string[] Get(string key)
        {
            TrieNode current = Root;
            foreach (char keyPart in key)
            {
                if (!current.Childs.ContainsKey(keyPart))
                {
                    return null;
                }

                current = current.Childs[keyPart];
            }

            return current.ChildWods.Values.ToArray();
        }
    }
}
