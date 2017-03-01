using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Structure for holding a key's frequency as well as a list of the sentence
    /// indexes where the key occurs
    /// </summary>
    public class FrequencyLocation
    {
        /// <summary>
        /// Frequency of key
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// List of sentence indexes where key occurs
        /// </summary>
        public List<int> Locations = new List<int>();
    }
}
