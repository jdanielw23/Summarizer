using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class Scorer
    {
        private DTable scored_sentences;

        public Scorer(string[] sentences)
        {
            scored_sentences = new DTable();
            foreach (string sentence in sentences)
            {
                scored_sentences.Add(sentence, 0);
            }
        }

        public void ScoreWithBigrams(BigramCounter bc)
        {
            // TODO: implement scoring algorithm.
            reset(); // Reset any previous scoring...
            foreach (string sentence in scored_sentences)
            {
                string[] words = sentence.Split(' ');
                int length = words.Length;
                int skip = 0;
                double score = 0;
                foreach (string word in words)
                {
                    for (int i = skip; i < length; i++)
                    {
                        score += bc.Count(word, words[i]);
                    }
                    skip++;
                }
                scored_sentences.Add(sentence, score / length);
            }
        }

        public void ScoreWithWordFrequencies(WordFrequencies wf)
        {
            // Scores sentences with sum of frequencies of
            // constituent words, divided by number of words.
            reset(); // Reset any previous scoring...
            foreach (string sentence in scored_sentences)
            {
                int score = 0;
                int wordcount = 0;
                foreach (string word in sentence.Split(' '))
                {
                    score += wf.Count(word);
                    wordcount++;
                }
                double true_score = (double) score / wordcount;
                scored_sentences.Add(sentence, true_score);
            }
        }

        public double ScoreOf(string key)
        {
            return scored_sentences.ValueOf(key);
        }

        public string[] Top(int n)
        {
            return scored_sentences.Top(n);
        }

        private void reset()
        {
            DTable words = scored_sentences;
            scored_sentences = new DTable();
            foreach (string word in words)
            {
                scored_sentences.Add(word, 0);
            }
        }
    }
}
