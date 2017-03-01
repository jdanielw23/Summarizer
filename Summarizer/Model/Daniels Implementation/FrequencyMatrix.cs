using NHunspell;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summarizer.Model.Daniels_Implementation
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Data Structure used to keep track of word frequencies and sentence locations
    /// </summary>
    public class FrequencyMatrix
    {
        private const int MAX_MEANINGS = 5;
        private MyThes Thesaurus;

        public IDictionary<string, FrequencyLocation> Matrix { get; private set; }

        public FrequencyLocation this[string key]
        {
            get
            {
                /*** Simple ***
                if (!Matrix.ContainsKey(key))
                {
                    return new FrequencyLocation() { Frequency = 0 };
                }
                return Matrix[key];
                /****/

                string similarKey;
                if (ContainsSimilarKey(key, out similarKey))
                {
                    return Matrix[similarKey];
                }
                else
                {
                    return new FrequencyLocation() { Frequency = 0 };
                }
            }
        }

        /// <summary>
        /// Simple constructor initializes a new Matrix and loads the Thesaurus
        /// </summary>
        public FrequencyMatrix()
        {
            Matrix = new Dictionary<string, FrequencyLocation>();
            Thesaurus = new MyThes("th_en_US_new.dat");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key to add to the matrix</param>
        /// <param name="sentenceIndex">Sentence index where this key is located</param>
        /// <returns>The frequency of the word</returns>
        public int AddToMatrix(string key, int sentenceIndex)
        {
            if (string.IsNullOrEmpty(key.ToString()))
                return 0;

            string similarKey;
            if (ContainsSimilarKey(key, out similarKey))
            {
                Matrix[similarKey].Frequency++;
                if (!Matrix[similarKey].Locations.Contains(sentenceIndex))
                    Matrix[similarKey].Locations.Add(sentenceIndex);

                return Matrix[similarKey].Frequency;
            }
            else
            {
                Matrix[key] = new FrequencyLocation();
                Matrix[key].Frequency = 1;
                Matrix[key].Locations.Add(sentenceIndex);

                return 1;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in Matrix.OrderByDescending(kv => kv.Value.Frequency))
            {
                //sb.AppendLine(string.Format("{0} => Frequency: {1}; Locations: {2}", pair.Key, pair.Value.Frequency, pair.Value.Locations.ListData(",")));
                sb.AppendLine(string.Format("{0} => Frequency: {1}", pair.Key, pair.Value.Frequency));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Utilizes the NHunspell library.
        /// Looks up the key in a Thesaurus and returns true if the Matrix contains either the key or any synonyms of the key
        /// </summary>
        /// <param name="key">They key to lookup</param>
        /// <param name="similarKey">The found similar key</param>
        /// <returns></returns>
        private bool ContainsSimilarKey(string key, out string similarKey)
        {
            similarKey = key;
            if (Matrix.ContainsKey(key))
                return true;

            ThesResult tr = Thesaurus.Lookup(key);
            if (tr == null)
                return false;

            int count = 0;
            foreach (ThesMeaning meaning in tr.Meanings)
            {
                //count++;
                foreach (string synonym in meaning.Synonyms)
                {
                    if (Matrix.ContainsKey(synonym))
                    {
                        similarKey = synonym;
                        return true;
                    }
                }
                //if (count > MAX_MEANINGS)
                //    break;
            }

            return false;
        }
    }
}
