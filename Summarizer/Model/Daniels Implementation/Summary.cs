using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    class Summary
    {
        private int MinSentenceLength;
        private int MaxSentenceLength;
        private IList<SentenceScore> _Summary = new List<SentenceScore>();

        public Summary(int minSentenceLength, int maxSentenceLength)
        {
            MinSentenceLength = minSentenceLength;
            MaxSentenceLength = maxSentenceLength;
        }

        public void AddToSummary(SentenceScore sentence)
        {
            if (_Summary.Any(sent => sent.Sentence.Equals(sentence.Sentence)) ||
                sentence.Sentence.Split(' ').Length < MinSentenceLength ||
                sentence.Sentence.Split(' ').Length > MaxSentenceLength ||
                sentence.Sentence.EndsWith("?"))
                return;

            if (_Summary.Count < 3)
            {
                Insert(sentence);
            }
            else if (sentence.Score > MinScore())
            {
                Insert(sentence);
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach(SentenceScore sent in _Summary)
            {
                result.AppendLine(sent.Sentence);
            }
            return result.ToString();
        }

        /*****************************************************/
        /***************    PRIVATE METHODS    ***************/
        /*****************************************************/
        private double MinScore()
        {
            if (_Summary.Count == 0)
                return 0;

            return _Summary.Last().Score;
        }

        private void Insert(SentenceScore sentence)
        {
            if (_Summary.Count == 0)
            {
                _Summary.Add(sentence);
                return;
            }

            for (int i = 0; i < _Summary.Count; i++)
            {
                if (sentence.Score > _Summary[i].Score)
                {
                    _Summary.Insert(i, sentence);
                    break;
                }
                else if (i == _Summary.Count - 1)
                {
                    _Summary.Insert(i+1, sentence);
                    break;
                }
            }
            RemoveLowestScoringSentence();
        }

        private void RemoveLowestScoringSentence()
        {
            if (_Summary.Count > 3)
                _Summary.RemoveAt(3);
        }
    }
}
