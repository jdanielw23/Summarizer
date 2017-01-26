﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    class StopwordsDAO
    {
        private LinkedList<String> stopwords;

        public StopwordsDAO()
        {
            // Stopwords list from http://www.ranks.nl/stopwords
            // TODO: find a better way of loading the file...
            string filepath = "C:\\Users\\shubin\\Desktop\\Capstone\\Summarizer\\Summarizer\\Model\\Utils\\stopwords.txt";
            string[] raw = System.IO.File.ReadAllLines(filepath);
            stopwords = new LinkedList<String>();
            foreach (string word in raw)
            {
                stopwords.AddLast(clean(word));
            }
        }

        public bool contains(string word)
        {
            return stopwords.Contains(clean(word));
        }

        private string clean(string word)
        {
            string clean_word = "";
            foreach (char c in word)
            {   // Remove symbols and numbers
                if (char.IsWhiteSpace(c) || char.IsLetter(c))
                {
                    clean_word += char.ToLower(c);
                }
            }
            return clean_word;
        }
    }
}