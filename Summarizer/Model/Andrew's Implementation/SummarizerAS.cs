using System;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using Summarizer.Model.Utils.Stemming;
using Summarizer.Model.Utils;
using Summarizer.Model.Andrew_s_Implementation;

namespace Summarizer.Model.Andrews_Implementation
{
    class SummarizerAS : SummarizerImplementation
    {
        private const int FREQUENCY_TABLE_LEN = 10;     // Length of frequency table.
        private const int MIN_SENT_LENGTH = 3;          // Reject sentences with less words.
        private const int MIN_SENT_NUM = 1;            // Reject doc with less sentences.
        private const double MIN_SENT_PERCENT = 0.70;   // Reject doc with less valid sentences.
        private const bool KEEP_INVALID_SENT = false;   // Keep invalid sentences?
        private const bool REMOVE_NUM = true;           // Remove numbers?
        private const bool REMOVE_SYM = true;           // Remove symbols?
        private const bool STEM = true;                 // Stem words when cleaning?

        public SummarizerAS()
        {
            // Perhaps refactor later to make constructor set above parameters...
        }

        public string SummarizeDocument(string filePath)
        {
            string[] raw_sentences = Cleaner.splitToSentences(
                System.IO.File.ReadAllText(filePath));
            if (raw_sentences == null)
            {
                return "File is empty.";
            }
            for (int i = 0; i < raw_sentences.Length; i++)
            {
                if (raw_sentences[i] == null)
                {
                    raw_sentences[i] = "";
                }
            }

            int sentence_count = raw_sentences.Length;
            int numOfWordsInValidSentences = 0;
            int approxNumOfWords = 0;
            string[] clean_sentences = new string[sentence_count];
            Cleaner cleaner = new Cleaner(REMOVE_SYM, REMOVE_NUM, STEM);

            if (sentence_count < MIN_SENT_NUM)
            {
                return "Less than " + MIN_SENT_NUM
                    + " sentences in document.";
            }
            for (int i = 0; i < sentence_count; i++)
            {
                int sentence_len = raw_sentences[i].Split(' ').Length;
                if (sentence_len >= MIN_SENT_LENGTH)
                {
                    numOfWordsInValidSentences += sentence_len;
                    clean_sentences[i] = cleaner.clean(raw_sentences[i]);
                }
                else if (KEEP_INVALID_SENT)
                {
                    clean_sentences[i] = cleaner.clean(raw_sentences[i]);
                }
                else
                {
                    clean_sentences[i] = "";
                }
                approxNumOfWords += sentence_len;
            }
            double percentValid = (double)numOfWordsInValidSentences / approxNumOfWords;
            if (percentValid < MIN_SENT_PERCENT)
            {
                return "Not enough (<" + (MIN_SENT_PERCENT * 100)
                        + "%) valid sentences in document.";
            }

            string[] chosen; // Should contain top 3 scored sentences.
            WordFrequencies wf = new WordFrequencies(clean_sentences);

            // *** START TEST *** //
            // This bit is to identify new stopwords...
            string test_out = "";
            string[] top_words = wf.Top(20);
            Console.WriteLine("MOST FRQUENT WORDS WITH FREQUENCIES:");
            Console.WriteLine("++++++++++++++++++++++++++++++++++++");
            foreach (string word in top_words)
            {
                Console.WriteLine(String.Format("{0, -15}", word) + wf.Count(word));
            }
            // *** END TEST *** //

            string output = "";
            BigramCounter bc = new BigramCounter(clean_sentences,
                                                 wf.Top(FREQUENCY_TABLE_LEN));
            Scorer scorer = new Scorer(clean_sentences);
            scorer.ScoreWithBigrams(bc);
            //scorer.ScoreWithWordFrequencies(wf);
            chosen = scorer.Top(3);
            IndexFinder ifinder = new IndexFinder(clean_sentences);
            foreach (string sentence in chosen)
            {
                int index = ifinder.Index(sentence);
                output += addline("\"" + Cleaner.RemoveNewlines(raw_sentences[index])
                            + "\" (" + Math.Round(scorer.ScoreOf(sentence), 3) + ")");
            }
            return output;
        }

        private string addline(string line)
        {
            return line + "\n";
        }
    }
}
