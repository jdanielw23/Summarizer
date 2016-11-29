using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    public static class ExtensionMethods
    {
        public static bool ContainsOnlyLetters(this string word)
        {
            foreach (char c in word)
            {
                if (c < 0x41 || (c > 0x5A && c < 0x61) || c > 0x7A)
                    return false;
            }

            return true;
        }

        public static string RemoveChars(this string word, params char[] chars)
        {
            for (int i = 0; i < word.Length; i++)
            {
                foreach (char c in chars)
                {
                    if (word[i].Equals(c))
                        word.Remove(i);
                }
            }
            return word;
        }
    }
}
