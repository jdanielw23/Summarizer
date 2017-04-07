using Summarizer.Model.Utils.Stemming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    class StopwordsDAO
    {
        private LinkedList<String> stopwords;
        private bool stem;

        public StopwordsDAO(bool stem)
        {
            // Stopwords list from http://www.ranks.nl/stopwords
            // TODO: find a better way of loading the file...
            string filepath = ""; // Converted this to relative path - Daniel 4/4/17 (moved to below by Andrew)

            // Debug-sensitive filepaths added -- Andrew 5/7/17
#if DEBUG
            filepath = @"..\..\..\Summarizer\Model\Utils\kjv_stopwords.txt";
#else
            filepath = @".\Documents\kjv_stopwords.txt";
#endif
            string[] raw = System.IO.File.ReadAllLines(filepath);
            this.stem = stem;
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
            if (stem)
            {
                EnglishStemmer stemmer = new EnglishStemmer();
                clean_word = stemmer.Stem(clean_word);
            }
            return clean_word;
        }
    }
}
