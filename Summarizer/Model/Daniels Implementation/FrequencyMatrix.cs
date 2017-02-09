using NHunspell;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Summarizer.Model.Daniels_Implementation
{
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

        public FrequencyMatrix()
        {
            Matrix = new Dictionary<string, FrequencyLocation>();
            Thesaurus = new MyThes("th_en_US_new.dat");
        }

        public int AddToMatrix(string word, int sentenceIndex)
        {
            if (string.IsNullOrEmpty(word.ToString()))
                return 0;

            string similarKey;
            if (ContainsSimilarKey(word, out similarKey))
            {
                Matrix[similarKey].Frequency++;
                if (!Matrix[similarKey].Locations.Contains(sentenceIndex))
                    Matrix[similarKey].Locations.Add(sentenceIndex);

                return Matrix[similarKey].Frequency;
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
                //sb.AppendLine(string.Format("{0} => Frequency: {1}; Locations: {2}", pair.Key, pair.Value.Frequency, pair.Value.Locations.ListData(",")));
                sb.AppendLine(string.Format("{0} => Frequency: {1}", pair.Key, pair.Value.Frequency));
            }
            return sb.ToString();
        }

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
