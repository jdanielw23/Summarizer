using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class WordFrequencies
    {
        private Table words;

        public WordFrequencies(string text)
        {
            string clean_text = clean(text);
            words = new Table();
            foreach (string word in clean_text.Split(' '))
            {
                words.Add(word, 1);
            }
        }

        public WordFrequencies(string[] text) : this(string.Join(" ", text)) { }

        public string[] Top(int n)
        {
            return words.Top(n);
        }

        public int Count(string word)
        {
            return words.ValueOf(word);
        }

        private string clean(string text)
        {
            string new_string = "";
            if (text == "" || text == null) return "";
            foreach (char c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    new_string += " ";
                }
                else if (char.IsLetter(c))
                {
                    new_string += char.ToLower(c);
                }
            }
            return new_string;
        }
    }
}
