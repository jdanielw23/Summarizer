using Summarizer.Model.Utils.Stemming;
using Summarizer.Model.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summarizer.Model.Daniels_Implementation
{
    public class SummarizerDW : SummarizerImplementation
    {
        private int MinWordLength;

        public SummarizerDW(int minWordLength = 3)
        {
            MinWordLength = minWordLength;
        }

        public string SummarizeDocument(string filePath)
        {
            return SummarizeDocumentMethod1(filePath);
            //return SummarizeDocumentMethod2(filePath);
        }

        public string SummarizeDocumentMethod2(string filePath)
        {
            string summary = "";

            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            // Break the text into sentences
            string[] sentences = SplitIntoSentences(text);

            // Move through each sentence forming groups of nouns and storing them in a word frequency matrix
            FrequencyMatrix wordFrequency = new FrequencyMatrix();

            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                StringBuilder key = new StringBuilder();
                foreach(string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    string word = SimplifyWord(rawWord, false);

                    if (IsValidWord(word, false, false, true, false))
                    {
                        wordFrequency.AddToMatrix(word, sentenceIndex);
                    }
                }
            }

            summary = wordFrequency.ToString();

            return summary;
        }

        public string SummarizeDocumentMethod1(string filePath)
        {
            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            // Break the text into sentences
            string[] sentences = SplitIntoSentences(text);

            // This is the data structure for my bigram
            Bigram wordFrequency = new Bigram();
            string prevWord = "";

            // Iterate through each sentence
            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                // Break each sentence into words
                foreach (string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    // Simplify word to its simplest form
                    string word = SimplifyWord(rawWord, true);

                    // Disregard stop words and words shorter than MIN_WORD_LENGTH
                    if (IsValidWord(word,true,true,true,false))
                    {
                        wordFrequency.AddToMatrix(prevWord, word, sentenceIndex);
                        prevWord = word;
                    }
                }
            }

            // Create a simpler word frequency with the two words combined together
            IDictionary<string, FrequencyLocation> finalWordFrequency = wordFrequency.FinalWordFrequency;

            // FURTHER ANALYSIS:
            // Account for word frequencies that are too common?

            /****    Cross-reference sentence indexes in most common word pairs
            IList<int> commonIndexes = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                foreach(int index in finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency).ElementAt(i).Value.Locations.Intersect(
                finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency).ElementAt(i + 1).Value.Locations))
                {
                    if (!commonIndexes.Contains(index))
                        commonIndexes.Add(index);
                }                
            }
            /****/

            // Build summary based on results
            // Options:
            //  First three sentences with top word frequency
            //  First sentence from each of top 3 word frequencies
            //  etc...

            StringBuilder summary = new StringBuilder();

            /****    OPTION 0: Show common Indexes    ****
            int count = 0;
            foreach(int i in commonIndexes)
            {
                summary.AppendLine(sentences[i]);
                count++;
                // Limit the number of sentences to 3
                if (count == 3)
                    break;
            }
            /****/

            /****    OPTION 1: This will print the word frequencies    ****
            foreach (var pair in finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency))
            {
                //// This will write the word pair frequencies, number of sentences, and the list of sentence indexes
                summary.AppendLine(string.Format("{0}: {1}, Num sentences: {2}; {3}\n", 
                    pair.Key, pair.Value.Frequency, 
                    pair.Value.Locations.Count, 
                    pair.Value.Locations.ListData(", ")));
            }
            /****/
            

            /****    OPTION 2: This will print the top 3 or fewer sentences containing the most common word pair    ****
            var mostCommonPair = finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency).ElementAt(0);
            int count = 0;
            foreach (int location in mostCommonPair.Value.Locations)
            {
                summary.AppendLine(sentences[location]);
                count++;
                if (count == 3)
                    break;
            }
            /****/

            /****    OPTION 3: This will print the     ****

            /****/

            /****    OPTION 4: This will print the first sentence from the top 3 pairs    ****
            int count = 0;
            int prevSent = 0;
            foreach (var pair in finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency))
            {
                int sent = pair.Value.Locations[0];
                if (prevSent != sent)
                {
                    summary.AppendLine(sentences[sent]);
                    count++;
                }

                if (count == 3)
                    break;

                prevSent = sent;
            }
            /****/

            return summary.ToString();
        }

        public void SummarizeToNewDocument(string filePath, string newFilePath)
        {
            string summary = SummarizeDocument(filePath);
            System.IO.File.WriteAllText(newFilePath, summary);
        }

        /*****************************************************/
        /***************    PRIVATE METHODS    ***************/
        /*****************************************************/
        private string SimplifyWord(string word, bool stemWord)
        {
            string trimmedWord = word.Trim().ToLower().TrimEnd(',', ':', ';','.');

            if (stemWord)
                trimmedWord = new EnglishStemmer().Stem(trimmedWord);

            return trimmedWord;
        }

        private bool IsValidWord(string word, bool minWordLength, bool lettersOnly, bool noStopWords, bool noVerbs)
        {
            if (minWordLength)
            {
                if (word.Length < MinWordLength)
                    return false;
            }

            if (lettersOnly)
            {
                if (!word.ContainsOnlyLetters())
                    return false;
            }

            if (noStopWords)
            {
                if (Constants.ShorterStopWordList.Contains(word))
                    return false;
            }

            if (noVerbs)
            {

            }

            return true;
        }

        private string[] SplitIntoSentences(string text)
        {
            string pattern = @"[A-Z]([a-z]| )+[a-z][a-zA-Z0-9\-\(\)\/\,\'\:\;\s*\n*]*[\.]";
            IList<string> result = new List<string>();
            
            foreach (var match in Regex.Matches(text, pattern))
            {
                result.Add(match.ToString());
            }

            return result.ToArray();
        }
    }
}
