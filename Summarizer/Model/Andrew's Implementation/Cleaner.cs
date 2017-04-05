using Summarizer.Model.Utils;
using Summarizer.Model.Utils.Stemming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    public class Cleaner
    {
        private bool removeSymbols;
        private bool removeNums;
        private bool stem;

        public Cleaner(bool removeSymbols = true, bool removeNums = true, bool stem = true)
        {
            this.removeSymbols = removeSymbols;
            this.removeNums = removeNums;
            this.stem = stem;
        }

        public string clean(string text)
        {
            // Translate system return characters into basic whitespace...
            string raw = String.Join(" ", Regex.Split(text, "\r\n|\r|\n"));
            string clean = "";
            StopwordsDAO dao = new StopwordsDAO(stem);
            EnglishStemmer stemmer = new EnglishStemmer();
            foreach (string word in raw.Split(' '))
            {
                string clean_word = "";
                if (removeSymbols || removeNums)
                {
                    foreach (char c in word)
                    {   // Remove symbols and/or numbers, if the parameters specify
                        if ((char.IsWhiteSpace(c) || char.IsLetter(c)
                            || (char.IsDigit(c) && !removeNums))
                            || !removeSymbols)
                        {
                            clean_word += char.ToLower(c);
                        }
                    }
                }
                else
                {
                    clean_word = word.ToLower();
                }
                if (!dao.contains(clean_word))
                {
                    if (stem)
                    {
                        clean_word = stemmer.Stem(clean_word);
                    }
                }
                else
                {
                    clean_word = "";
                }
                clean += ((clean_word == "") ? "" : (clean_word + " "));
            }
            for (int i = clean.Length; i >= 0; i--)
            {
                if (clean.EndsWith(" "))
                {
                    clean = clean.Substring(0, i - 1);
                }
                else
                {
                    break;
                }
            }
            return clean;
        }

        public static string RemoveNewlines(string source)
        {
            return String.Join(" ", Regex.Split(source, "\r\n|\r|\n"));
        }

        public static string[] splitToSentences(string text)
        {
            if (text == null || text == "")
            {
                return null;
            }
            return Regex.Split(text, @"(?<=[\.!\?])\s+");
        }
    }
}
