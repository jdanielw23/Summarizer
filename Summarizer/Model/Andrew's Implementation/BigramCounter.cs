using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class BigramCounter
    {
        private Matrix cooccurrance;
        private string[] words;

        public BigramCounter(string[] sentences, string[] constituents)
        {
            words = constituents;
            int num_sent = sentences.Length;
            int num_cons = constituents.Length;
            Matrix occurrance = new Matrix(num_sent, num_cons);
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
            }
            cooccurrance = occurrance.Dot(occurrance.Transpose());
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
