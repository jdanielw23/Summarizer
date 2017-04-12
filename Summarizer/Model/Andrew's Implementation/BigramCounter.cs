using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    public class BigramCounter
    {
        private Matrix cooccurrance;
        private string[] words;

        public BigramCounter(string[] sentences, string[] constituents)
        {
            words = constituents;
            int num_sent = sentences.Length;
            int num_cons = constituents.Length;
            Matrix occurrance = new Matrix(num_sent, num_cons);
            Monitor monitor = new Monitor("bigram calculation", num_sent);
            for (int i = 0; i < num_sent; i++)
            {
                for (int j = 0; j < num_cons; j++)
                {
                    if (found(constituents[j], sentences[i]))
                    {
                        occurrance.Set(i, j, 1);
                    }
                    else
                    {
                        occurrance.Set(i, j, 0);
                    }
                }
                monitor.Ping();
            }
            cooccurrance = occurrance.Transpose().Dot(occurrance);
            cooccurrance.ZeroDiagonal();
        }

        public int Count(string first, string second)
        {
            int first_index = -1;
            int second_index = -1;
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word == first)
                {
                    first_index = i;
                }
                if (word == second)
                {
                    second_index = i;
                }
            }
            if (first_index != -1 && second_index != -1)
            {
                return cooccurrance.Get(first_index, second_index);
            }
            return 0;
        }

        public Table Table()
        {
            // find number of unique, non-zero entries...
            int len = words.Length;
            int count = (int)(Math.Pow(len, 2) - len) / 2;
            return top(count);
        }

        public void Print()
        {
            string format = "|";
            string bar = "-";
            int width = words.Length + 1;
            object[] row = new object[width];
            for (int i = 0; i < width; i++)
            {
                format += " {" + i + ",7} |";
                bar += "----------";
            }
            p(bar);
            row[0] = "";
            for (int i = 0; i < words.Length; i++)
            {
                row[i + 1] = words[i];
            }
            p(String.Format(format, row));
            p(bar);
            for (int i = 0; i < words.Length; i++)
            {
                row = new object[width];
                row[0] = words[i];
                for (int j = 0; j < words.Length; j++)
                {
                    row[j + 1] = cooccurrance.Get(i, j);
                }
                p(String.Format(format, row));
                p(bar);
            }
        }

        private void p(string line)
        {
            Console.WriteLine(line);
        }

        private Table top(int n)
        {
            if (n > words.Length)
            {
                n = words.Length;
            }
            Table pairs = new Table();
            int length = words.Length;
            int skip = 0;
            double score = 0;
            string key;
            int value;
            foreach (string word in words)
            {
                for (int i = skip; i < length; i++)
                {
                    key = word + " " + words[i];
                    value = this.Count(word, words[i]);
                    pairs.Add(key, value);
                }
                skip++;
            }
            Table top_n = new Table();
            string[] top_keys = pairs.Top(n);
            for (int i = 0; i < top_keys.Length; i++)
            {
                value = pairs.ValueOf(top_keys[i]);
                top_n.Add(top_keys[i], value);
            }
            return top_n;
        }

        private bool found(string term, string sentence)
        {
            foreach(string word in sentence.Split(' '))
            {
                if (word == term)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
