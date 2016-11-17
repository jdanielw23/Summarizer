
using java.util;
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
    public class SummarizerDW
    {
        public static int MIN_WORD_LENGTH = 3;

        public static void AnalyzeDocument(string filePath, string newFilePath)
        {
            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            // Break it into words or sentences
            //string[] words = text.Split(' ');
            string[] sentences = SplitIntoSentences(text);
            
            IDictionary<string, Dictionary<string,FrequencyLocation>> wordFrequency = new Dictionary<string, Dictionary<string, FrequencyLocation>>();
            string prevWord = "";

            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                foreach (string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    string word = rawWord.Trim().ToLower().RemoveChars(',', ':', ';');

                    if (word.Length >= MIN_WORD_LENGTH &&
                        word.ContainsOnlyLetters() &&
                        !word.Equals("the") &&
                        !word.Equals("and"))
                    {
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

            // For outputting word frequency
            IDictionary<string, FrequencyLocation> finalWordFrequency = new Dictionary<string, FrequencyLocation>();
            foreach (var pair1 in wordFrequency)
            {
                foreach (var pair2 in pair1.Value.OrderByDescending(kv => kv.Value.Frequency))
                {
                    finalWordFrequency[pair1.Key + " " + pair2.Key] = pair2.Value;
                }                
            }

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

            System.IO.File.WriteAllText(newFilePath, sb.ToString());
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
