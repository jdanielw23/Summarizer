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
                approxNumOfWords += sentence_len;
            }
            double percentValid = (double)numOfWordsInValidSentences / approxNumOfWords;
            if (percentValid < MIN_SENT_PERCENT)
            {
                return "Not enough (<" + (MIN_SENT_PERCENT * 100)
                        + "%) valid sentences in document.";
            }

            // TODO: determine the most frequent words and/or bigrams...

            string[] chosen = { "", "", "" }; // Should contain top 3 scored sentences.
            WordFrequencies wf = new WordFrequencies(clean_sentences);

            // TODO: score sentences, and put top three in "chosen"...

            string output = "";
            int test = 20;
            string[] test_arr = wf.Top(test);
            for (int i = 0; i < test; i++)
            {
                output += addline(test_arr[i] + " occurrs " + wf.Count(test_arr[i]) + " times.");
            }
            return output;
        }

        private string addline(string line)
        {
            return line + "\n";
        }
    }
}
