using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public sealed class Trie
    {
        /// <summary>
        /// Коревой элемент словаря
        /// </summary>
        private TrieNode _root;

        public Trie()
        {
            _root = new TrieNode();
        }

        /// <summary>
        /// Добавляем слово в словарь
        /// </summary>
        /// <param name="word">Слово</param>
        /// <param name="occurrence">встречаемость</param>
        public void Add(string word, int occurrence)
        {
            TrieNode current = _root;
            foreach (char keyPart in word)
            {
                if (!current.Childs.ContainsKey(keyPart))
                {
                    current.Childs.Add(keyPart, new TrieNode());
                }

                current = current.Childs[keyPart];
                current.AddChildWord(word, occurrence);
            }
        }

        /// <summary>
        /// Получение вариантов подстановки слов по подстроке
        /// </summary>
        /// <param name="substring"></param>
        /// <returns></returns>
        public IEnumerable<string> Get(string substring)
        {
            TrieNode current = _root;
            foreach (char keyPart in substring)
            {
                if (!current.Childs.ContainsKey(keyPart))
                {
                    return null;
                }

                current = current.Childs[keyPart];
            }

            return current.GetWords();
        }

    }
}
