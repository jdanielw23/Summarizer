using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Andrew_s_Implementation
{
    class Scorer
    {
        private const int 
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
            reset();
            this.ScoreWithBigrams(bc);
            Table sorted_bigrams = bc.Table();
            DTable scored_sentences_clone = scored_sentences.Clone();
            scored_sentences_clone.Top(0); // to sort it...
            int score = sorted_bigrams.Size();
            int process_length = scored_sentences.Size() * sorted_bigrams.Size();
            Monitor monitor = new Monitor("score filtering", process_length);
            scored_sentences = new DTable();
            foreach (string bigram in sorted_bigrams)
            {
                foreach (string sentence in scored_sentences_clone)
                {

                    monitor.Ping();
                }
                score--;
            }
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
            if (n < scored_sentences.Size())
            {
                return scored_sentences.Top(n);
            }
            return scored_sentences.Top(scored_sentences.Size());
        }

        /*
         * Checks if the sentence contains the bigram.
         * The bigram is to be the two words concatenated with a space.
         */
        private bool contains(string sentence, string bigram)
        {
            string[] bigram_words = bigram.Split(' ');
            string[] sentence_words = sentence.Split(' ');
            bool first = false;
            bool second = false;
            foreach (string word in sentence_words)
            {
                if (word == bigram_words[0])
                {
                    first = true;
                }
                if (word == bigram_words[1])
                {
                    second = true;
                }
            }
            return first && second;
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
