using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model
{
    public class FrequencyLocation
    {
        /// <summary>
        /// Frequency of word pair
        /// </summary>
        public int Frequency;
        /// <summary>
        /// List of sentence indexes where word pair occurs
        /// </summary>
        public List<int> Locations = new List<int>();
    }
}
