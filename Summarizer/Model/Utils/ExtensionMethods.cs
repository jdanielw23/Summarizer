using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Capitalizes the first letter in a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                output.Append(input[i].ToString().ToUpper());

                if (input[i].IsLetter())
                {
                    output.Append(input.Substring(i + 1));
                    break;
                }
            }
            return output.ToString();
        }

        /// <summary>
        /// Capitalizes each word in a string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static bool ContainsLowerCaseLetter(this string value)
        {
            foreach (char c in value.ToArray())
            {
                if (c.IsLetter() && !c.IsUpper())
                    return true;
            }
            return false;
        }

        public static bool IsLetter(this char value)
        {
            return value.IsUpper() || (value >= 97 && value <= 122);
        }

        public static bool IsUpper(this char value)
        {
            return value <= 91 && value >= 65;
        }

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
        /// Calls the ToString method of each item in the list and returns a 
        /// string with each item separated by the specified delimiter.
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

        public static bool Contains<T>(this IList<T> list, params T[] items)
        {
            foreach (T item in items)
            {
                if (!list.Contains(item))
                    return false;
            }
            return true;
        }

        public static bool ContainsOnly<T>(this IList<T> list, params T[] items)
        {
            if (list.Count != items.Length)
                return false;
            return list.Contains(items);
        }

    }
}
