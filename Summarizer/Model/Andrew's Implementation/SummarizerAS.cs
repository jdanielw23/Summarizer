using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Summarizer.Model
{
    class SummarizerAS
    {
        private const int FREQUENCY_TABLE_LEN = 10;     // Length of frequency table.
        private const int MIN_SENT_LENGTH = 10;          // Reject sentences with less words.
        private const double MIN_SENT_PERCENT = 0.70;   // Reject doc with less valid sentences.
        private const bool KEEP_INVALID_SENT = false;   // Keep invalid sentences?
        private const bool REMOVE_NUM = true;           // Remove numbers?
        private const bool REMOVE_SYM = false;          // Remove symbols?
        private const bool STEM = true;                 // Stem words when cleaning?

        public static string getSummary(string path)
        {
            string rawText = System.IO.File.ReadAllText(path);
            if (rawText == "" || rawText == null)
            {
                return "File is empty.";
            }

            // Regex from stackoverflow.com/question/4957226/split-text-into-sentences-in-c-sharp
            string[] sentences = Regex.Split(rawText, @"(?<=[\.!\?])\s+");

            int approxNumOfWords = rawText.Split(' ').Length;
            int numOfWordsInValidSentences = 0;
            LinkedList<String> filteredSentences = new LinkedList<String>();
            foreach (string sentence in sentences)
            {
                int length = sentence.Split(' ').Length;
                if (length >= MIN_SENT_LENGTH)
                {
                    numOfWordsInValidSentences += length;
                    filteredSentences.AddLast(sentence);
                }
                else if (KEEP_INVALID_SENT)
                {
                    filteredSentences.AddLast(sentence);
                }
            }
            double percentValid = (double) numOfWordsInValidSentences / approxNumOfWords;

            Console.WriteLine("Percent valid: " + (percentValid * 100) + "%");
            Console.WriteLine("Total words: " + approxNumOfWords);
            Console.WriteLine("Valid-sentence words: " + numOfWordsInValidSentences);
            return "Done.";
        }
    }
}
