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

        /// <summary>
        /// Calls the ToString method of each item in the list and returns a string with each item separated by the specified delimiter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ListData<T>(this IList<T> list, string delimiter = "\n")
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToString());
                if (i < list.Count - 1)
                    sb.Append(delimiter);
            }
            return sb.ToString();
        }
    }
}
