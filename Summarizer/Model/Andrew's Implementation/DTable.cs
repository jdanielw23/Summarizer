using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    /// <summary>
    /// This class is a hashtable specialized for string keys and double values.
    /// It is the same as Table, but uses doubles for the values.
    /// </summary>
    class DTable : IEnumerable<string>
    {
        private Hashtable table;
        private int sorts = 0;
        private string[] sorted_keys; // highest to lowest by values
        private int count;

        public DTable()
        {
            table = new Hashtable();
            count = 0;
        }

        public DTable Clone()
        {
            DTable retval = new DTable();
            foreach (string key in sorted_keys)
            {
                retval.Add(key, (double)table[key]);
            }
            retval.Top(0); // Just so it is sorted...
            return retval;
        }

        public void Add(string key, double value)
        {
            if (table.ContainsKey(key))
            {
                double new_value = (double)table[key] + value;
                table[key] = new_value;
            }
            else if (!string.IsNullOrWhiteSpace(key))
            {
                table.Add(key, value);
                count++;
            }
        }

        public void Remove(string key)
        {
            if (table.ContainsKey(key))
            {
                table.Remove(key);
                count--;
            }
        }

        public int Size()
        {
            return count;
        }

        public bool Has(string key)
        {
            return table.ContainsKey(key);
        }

        public double ValueOf(string key)
        {
            if (table.ContainsKey(key))
            {
                return (double)table[key];
            }
            return 0;
        }

        public string[] Top(int n)
        {
            if (count == 0)
            {
                return null;
            }
            sorted_keys = new string[count];
            int index = 0;


            ICollection keys = table.Keys;
            foreach (string key in keys)
            {
                sorted_keys[index] = key;
                index++;
            }

            quicksort(0, count - 1);
            reverseSort();
            string[] top_n = new string[n];
            int size;
            size = (n < count) ? n : count;
            top_n = new string[size];
            for (int i = 0; i < size; i++)
            {
                top_n[i] = sorted_keys[i];
            }
            if (n > count)
            { // fill the remaining slots with "<null>"
                for (int i = size; i < n; i++)
                {
                    top_n[i] = "<null>";
                }
            }
            return top_n;
        }

        public IEnumerator<string> GetEnumerator()
        {
            List<string> list = new List<string>();
            foreach (string term in table.Keys)
            {
                list.Add(term);
            }
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return table.Keys.GetEnumerator();
        }

        // Sorting implementation derrived from Java implementation at...
        // http://www.vogella.com/tutorials/JavaAlgorithmsQuicksort/article.html

        private void quicksort(int low, int high)
        {
            int i = low;
            int j = high;
            sorts++;
            if (sorts % 1000 == 0) Console.WriteLine("Sorting... ("
                                                    + (sorts / 1000) + ")");
            double pivot = ValueOf(sorted_keys[low + (high - low) / 2]);
            while (i <= j)
            {
                while (val(i) < pivot)
                {
                    i++;
                }
                while (val(j) > pivot)
                {
                    j--;
                }
                if (i <= j)
                {
                    exchange(i, j);
                    i++;
                    j--;
                }
            }
            if (low < i) quicksort(low, j);
            if (i < high) quicksort(i, high);
        }

        private void exchange(int i, int j)
        {
            string liason = sorted_keys[i];
            sorted_keys[i] = sorted_keys[j];
            sorted_keys[j] = liason;
        }

        private double val(int index)
        {
            return ValueOf(sorted_keys[index]);
        }

        private void reverseSort()
        {
            int count = sorted_keys.Length;
            for (int i = 0; i < count / 2; i++)
            {
                exchange(i, count - i - 1);
            }
        }
    }
}
