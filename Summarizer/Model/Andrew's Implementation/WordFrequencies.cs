using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    public class WordFrequencies
    {
        private Table words;

        public WordFrequencies(string text)
        {
            string clean_text = clean(text);
            words = new Table();
            string[] words_arr = clean_text.Split(' ');
            Monitor monitor = new Monitor("word frequency calculation",
                                            words_arr.Length);
            foreach (string word in words_arr)
            {
                words.Add(word, 1);
                monitor.Ping();
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
            //Monitor monitor = new Monitor("second cleaning", text.Length);
            //foreach (char c in text)
            //{
            //    if (char.IsWhiteSpace(c))
            //    {
            //        new_string += " ";
            //    }
            //    else if (char.IsLetter(c))
            //    {
            //        new_string += char.ToLower(c);
            //    }
            //    monitor.Ping();
            //}
            new_string = text.ToLower();
            return new_string;
        }
    }
}
