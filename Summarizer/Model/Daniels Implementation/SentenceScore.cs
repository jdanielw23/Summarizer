using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Structure for holding a sentence and it's score
    /// </summary>
    public struct SentenceScore
    {
        public string Sentence { get; set; }
        public double Score { get; set; }
    }
}
