using Summarizer.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    public class FrequencyMatrix
    {
        public IDictionary<string, FrequencyLocation> Matrix { get; private set; }

        public FrequencyMatrix()
        {
            Matrix = new Dictionary<string, FrequencyLocation>();
        }

        public int AddToMatrix(string word, int sentenceIndex)
        {
            if (string.IsNullOrEmpty(word))
                return 0;

            if (Matrix.ContainsKey(word))
            {
                Matrix[word].Frequency++;
                if (!Matrix[word].Locations.Contains(sentenceIndex))
                    Matrix[word].Locations.Add(sentenceIndex);

                return Matrix[word].Frequency;
            }
            else
            {
                Matrix[word] = new FrequencyLocation();
                Matrix[word].Frequency = 1;
                Matrix[word].Locations.Add(sentenceIndex);

                return 1;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in Matrix.OrderByDescending(kv => kv.Value.Frequency))
            {
                sb.AppendLine(string.Format("{0} => Frequency: {1}; Locations: {2}", pair.Key, pair.Value.Frequency, pair.Value.Locations.ListData(",")));
            }
            return sb.ToString();
        }
    }
}
