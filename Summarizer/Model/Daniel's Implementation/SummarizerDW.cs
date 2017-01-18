using Summarizer.Model.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summarizer.Model
{
    public class SummarizerDW : SummarizerImplementation
    {
        public static int MIN_WORD_LENGTH = 3;

        public string SummarizeDocument(string filePath)
        {
            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            // Break the text into sentences
            string[] sentences = SplitIntoSentences(text);
            
            // This is the data structure for my bigram
            IDictionary<string, Dictionary<string,FrequencyLocation>> wordFrequency = new Dictionary<string, Dictionary<string, FrequencyLocation>>();
            string prevWord = "";

            // Iterate through each sentence
            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                // Break each sentence into words
                foreach (string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    // Simplify word to its simplest form
                    string word = SimplifyWord(rawWord);

                    // Disregard stop words and words shorter than MIN_WORD_LENGTH
                    if (IsValidWord(word))
                    {
                        // Build word frequency matrix
                        if (!string.IsNullOrEmpty(prevWord))
                        {
                            // If the word is already in the table increment the count
                            if (wordFrequency.ContainsKey(prevWord))
                            {
                                if (wordFrequency[prevWord].ContainsKey(word))
                                {
                                    wordFrequency[prevWord][word].Frequency++;
                                    wordFrequency[prevWord][word].Locations.Add(sentenceIndex);
                                }
                                else
                                {
                                    wordFrequency[prevWord][word] = new FrequencyLocation();
                                    wordFrequency[prevWord][word].Frequency = 1;
                                    wordFrequency[prevWord][word].Locations.Add(sentenceIndex);
                                }
                            }
                            // otherwise, add it with count = 1
                            else
                            {
                                wordFrequency[prevWord] = new Dictionary<string, FrequencyLocation>();
                                wordFrequency[prevWord][word] = new FrequencyLocation();
                                wordFrequency[prevWord][word].Frequency = 1;
                                wordFrequency[prevWord][word].Locations.Add(sentenceIndex);
                            }
                        }

                        prevWord = word;
                    }
                }
            }

            // Create a simpler word frequency with the two words combined together
            IDictionary<string, FrequencyLocation> finalWordFrequency = new Dictionary<string, FrequencyLocation>();
            foreach (var pair1 in wordFrequency)
            {
                foreach (var pair2 in pair1.Value.OrderByDescending(kv => kv.Value.Frequency))
                {
                    finalWordFrequency[pair1.Key + " " + pair2.Key] = pair2.Value;
                }                
            }

            // Build summary based on results
            // Options:
            //  First three sentences with top word frequency
            //  First sentence from each of top 3 word frequencies
            //  etc...

            StringBuilder sb = new StringBuilder();
            int count = 0;
            int prevSent = 0;
            foreach (var pair in finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency))
            {
                //// This will write the word pair frequencies
                //sb.AppendLine(string.Format("{0}: {1}\n", pair.Key, pair.Value.Frequency));

                //// This will write all of the sentences that contain the most popular pair of words
                //foreach (int loc in pair.Value.Locations)
                //    sb.AppendLine(sentences[loc] + "...");

                int sent = pair.Value.Locations[0];
                if (prevSent != sent)
                {
                    sb.AppendLine(sentences[sent]);
                    count++;
                }

                if (count == 3)
                    break;

                prevSent = sent;
            }

            
            return sb.ToString();
        }

        public void SummarizeToNewDocument(string filePath, string newFilePath)
        {
            string summary = SummarizeDocument(filePath);
            System.IO.File.WriteAllText(newFilePath, summary);
        }

        private static string SimplifyWord(string word)
        {
            return word.Trim().ToLower().RemoveChars(',', ':', ';');
        }

        private static bool IsValidWord(string word)
        {
            return word.Length >= MIN_WORD_LENGTH &&
                        word.ContainsOnlyLetters() &&
                        !word.Equals("the") &&
                        !word.Equals("and");
        }

        private static string[] SplitIntoSentences(string text)
        {
            string pattern = @"[A-Z]([a-z]| )+[a-z][a-zA-Z0-9\-\(\)\/\,\'\:\;\s*\n*]*[\.]";
            IList<string> result = new List<string>();
            
            foreach (var match in Regex.Matches(text, pattern))
            {
                result.Add(match.ToString());
            }

            //string[] sentences = Regex.Split(text, @"(?<=[.\n])");

            //foreach(string sentence in sentences)
            //{
            //    // Only count sentences that contain at least one lower case letter
            //    if (Regex.IsMatch(sentence, "[a-z]+"))
            //        result.Add(sentence);

            //    // Don't count a sentence if it contains a new line character
            //    //if (!sentence.Contains("\n"))
            //    //    result.Add(sentence);
            //}

            return result.ToArray();
        }
    }
}
