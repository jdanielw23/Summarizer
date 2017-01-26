using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    public class Bigram
    {
        private IDictionary<string, Dictionary<string, FrequencyLocation>> _Bigram = new Dictionary<string, Dictionary<string, FrequencyLocation>>();

        private IDictionary<string, FrequencyLocation> finalWordFrequency;
        public IDictionary<string, FrequencyLocation> FinalWordFrequency
        {
            get {
                if (finalWordFrequency == null)
                    GenerateFinalWordFrequency();

                return finalWordFrequency; }
        }

        public int AddToMatrix(string prevWord, string word, int sentenceIndex)
        {
            // Check for valid prevWord
            if (string.IsNullOrEmpty(prevWord))
                return 0;

            // If the word is already in the table increment the count
            if (_Bigram.ContainsKey(prevWord))
            {
                if (_Bigram[prevWord].ContainsKey(word))
                {
                    _Bigram[prevWord][word].Frequency++;
                    if (!_Bigram[prevWord][word].Locations.Contains(sentenceIndex))
                        _Bigram[prevWord][word].Locations.Add(sentenceIndex);

                    return _Bigram[prevWord][word].Frequency;
                }
                else
                {
                    _Bigram[prevWord][word] = new FrequencyLocation();
                    _Bigram[prevWord][word].Frequency = 1;
                    _Bigram[prevWord][word].Locations.Add(sentenceIndex);

                    return 1;
                }
            }
            // otherwise, add it with count = 1
            else
            {
                _Bigram[prevWord] = new Dictionary<string, FrequencyLocation>();
                _Bigram[prevWord][word] = new FrequencyLocation();
                _Bigram[prevWord][word].Frequency = 1;
                _Bigram[prevWord][word].Locations.Add(sentenceIndex);

                return 1;
            }
        }

        public void GenerateFinalWordFrequency()
        {
            finalWordFrequency = new Dictionary<string, FrequencyLocation>();
            foreach (var pair1 in _Bigram)
            {
                foreach (var pair2 in pair1.Value.OrderByDescending(kv => kv.Value.Frequency))
                {
                    finalWordFrequency[pair1.Key + " " + pair2.Key] = pair2.Value;
                }
            }
        }
    }
}
