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
            Monitor monitor = new Monitor("scorer setup", sentences.Length);
            foreach (string sentence in sentences)
            {
                scored_sentences.Add(sentence, 0);
                monitor.Ping();
            }
        }

        public void ScoreWithComplexBigrams(BigramCounter bc)
        {
            /*
             * TODO: Score such that top sentence is the highest scoring
             * sentence of sentences with the highest scoring bigram, the
             * second is the highest scoring sentence with the second
             * highest scoring bigram (and which does not contain the
             * highest scoring bigram), and the third... etc.
             */
        }

        public void ScoreWithBigrams(BigramCounter bc)
        {
            reset(); // Reset any previous scoring...
            Monitor monitor = new Monitor("score calculation", scored_sentences.Size());
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
                        /*
                         * For each sentence, this inner bit
                         * of code will be run a total of
                         * 0.5(n^2 + n) times, where n is the 
                         * number of words in the sentence.
                         * (i.e. the value of "length")
                         */
                    }
                    skip++;
                }
                double equalizer = 0.5 * (Math.Pow(length, 2) + length);
                scored_sentences.Add(sentence, score / equalizer);
                monitor.Ping();
            }
        }

        public void ScoreWithWordFrequencies(WordFrequencies wf)
        {
            // Scores sentences with sum of frequencies of
            // constituent words, divided by number of words.
            reset(); // Reset any previous scoring...
            Monitor monitor = new Monitor("score calculation", scored_sentences.Size());
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
                monitor.Ping();
            }
        }

        public double ScoreOf(string key)
        {
            return scored_sentences.ValueOf(key);
        }

        public string[] Top(int n)
        {
            return scored_sentences.Top(3);
        }

        private void reset()
        {
            Monitor monitor = new Monitor("reset of scores", scored_sentences.Size());
            DTable words = scored_sentences;
            scored_sentences = new DTable();
            foreach (string word in words)
            {
                scored_sentences.Add(word, 0);
                monitor.Ping();
            }
        }
    }
}
