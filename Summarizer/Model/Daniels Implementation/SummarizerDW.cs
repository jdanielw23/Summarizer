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
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Contains the main logic for summarizing a document
    /// </summary>
    public class SummarizerDW : SummarizerImplementation
    {
        private int MinSentenceLength;
        private int MaxSentenceLength;
        private int MinWordLength;

        /*********************************************
        NEXT STEPS:
        I somewhat like the current setup.
        
        
        Possible Implementation Enhancements:
            -Maybe instead of just using the frequency of each word, try to account for words
             that occur too frequently (Mean, StdDev).
            -Maybe just take the sentence before and after the highest scoring sentence?
            -Maybe create regex that will separate the document by verse or sentence,
                whichever is most complete.
        **********************************************/

        public SummarizerDW(int minWordLength = 3, int minSentenceLength = 8,
            int maxSentenceLength = 50)
        {
            MinWordLength = minWordLength;
            MinSentenceLength = minSentenceLength;
            MaxSentenceLength = maxSentenceLength;
        }

        /// <summary>
        /// Main interface method for summarizing a document
        /// </summary>
        /// <param name="filePath">The full path to the file to be summarized</param>
        /// <returns>The summary of the document</returns>
        public string SummarizeFile(string filePath)
        {
            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            //return SummarizeDocumentMethod1(filePath);
            return Summarize(text);
        }

        public string SummarizeBible(string bookName, int chapterNum)
        {
            return Summarize(Constants.Bible[bookName][chapterNum].Values.ToList().ListData(""));
        }

        /// <summary>
        /// Algorithm 2: Uses sentence score and thesaurus to look for similar keys
        /// </summary>
        /// <param name="filePath">The full path to the file to be summarized</param>
        /// <returns>The summary of the document</returns>
        public string Summarize(string text)
        {
            Summary summary = new Summary(MinSentenceLength, MaxSentenceLength);

            

            // Break the text into sentences
            string[] sentences = SplitIntoSentences(text);

            /******* Simpler but same thing ******
            IDictionary<string, int> wordFrequency = new Dictionary<string, int>();
            foreach (string rawWord in text.Split(' '))
            {
                string word = SimplifyWord(rawWord, true);

                if (IsValidWord(word, true, false, true, false))
                {
                    if (wordFrequency.ContainsKey(word))
                        wordFrequency[word] += 1;
                    else
                        wordFrequency[word] = 0;
                }
            }
            /****************/

            /***************   ***************/
            FrequencyMatrix wordFrequency = new FrequencyMatrix();
            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                StringBuilder key = new StringBuilder();
                foreach(string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    string word = SimplifyWord(rawWord, false);

                    if (IsValidWord(word, true, true, true, false))
                    {
                        wordFrequency.AddToMatrix(word, sentenceIndex);
                    }
                }
            }
            /********************************/

            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                int sum = 0;
                int numWords = 0;
                foreach (string rawWord in sentences[sentenceIndex].Split(' '))
                {
                    string word = SimplifyWord(rawWord, false);
                    numWords++;
                    //sum += (wordFrequency.ContainsKey(word)) ? wordFrequency[word] : 0;
                    sum += wordFrequency[word].Frequency;
                }
                double score = (numWords == 0) ? 0 : (sum / numWords);

                summary.AddToSummary(new SentenceScore()
                {
                    Sentence = sentences[sentenceIndex].Trim().Capitalize(),
                    Score = score
                });
            }

            //return wordFrequency.ToString();
            return summary.ToString();
        }

        /// <summary>
        /// Algorithm 1: Creates a bigram and returns top 3 sentences
        /// </summary>
        /// <param name="filePath">The full path to the file to be summarized</param>
        /// <returns>The summary of the document</returns>
        public string SummarizeDocumentMethod1(string filePath)
        {
            // Read in the text
            string text = System.IO.File.ReadAllText(filePath);

            // Break the text into sentences
            string[] sentences = SplitIntoSentences(text);

            // This is the data structure for my bigram
            Bigram wordFrequency = new Bigram();

            // Iterate through each sentence
            for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
            {
                string prevWord = "";
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
            IDictionary<string, FrequencyLocation> finalWordFrequency = wordFrequency
                                    .FinalWordFrequency;

            // FURTHER ANALYSIS:
            // Account for word frequencies that are too common?

            /****    Cross-reference sentence indexes in most common word pairs
            IList<int> commonIndexes = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                foreach(int index in finalWordFrequency.OrderByDescending(kv => kv.Value
                                .Frequency).ElementAt(i).Value.Locations.Intersect(
                finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency)
                                .ElementAt(i + 1).Value.Locations))
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
                //// This will write the word pair frequencies, number of sentences,
                //  and the list of sentence indexes
                summary.AppendLine(string.Format("{0}: {1}, Num sentences: {2}; {3}\n", 
                    pair.Key, pair.Value.Frequency, 
                    pair.Value.Locations.Count, 
                    pair.Value.Locations.ListData(", ")));
            }
            /****/
            

            /****    OPTION 2: This will print the top 3 or fewer sentences containing
             *  the most common word pair    ****
            var mostCommonPair = finalWordFrequency.OrderByDescending(kv => kv.Value.Frequency)
                                        .ElementAt(0);
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

        /// <summary>
        /// Summarizes the specified document and writes it to a new file
        /// </summary>
        /// <param name="filePath">File to be summarized</param>
        /// <param name="newFilePath">File to be created from summary</param>
        public void SummarizeToNewDocument(string filePath, string newFilePath)
        {
            string summary = SummarizeFile(filePath);
            System.IO.File.WriteAllText(newFilePath, summary);
        }

        /*****************************************************/
        /***************    PRIVATE METHODS    ***************/
        /*****************************************************/

        /// <summary>
        /// Trims punctuation and spacing off of the supplied word and if specified,
        /// also stems the word
        /// </summary>
        /// <param name="word">The word to be simplified</param>
        /// <param name="stemWord">If true, stems the word</param>
        /// <returns>The trimmed word</returns>
        private string SimplifyWord(string word, bool stemWord)
        {
            string trimmedWord = word.Trim().ToLower().TrimEnd(',', ':', ';', '.', '!', '?','s');

            if (stemWord)
                trimmedWord = new EnglishStemmer().Stem(trimmedWord);

            return trimmedWord;
        }

        /// <summary>
        /// Using various options, checks to see if the supplied word is valid
        /// </summary>
        /// <param name="word">Word to check</param>
        /// <param name="minWordLength">If true, returns false if word.Length is less
        /// than MinWordLength</param>
        /// <param name="lettersOnly">If true, returns false is words contains
        /// anything other than letters</param>
        /// <param name="noStopWords">If true, returns false if the word is a stop word</param>
        /// <param name="noVerbs">If true, returns false if word is a verb
        /// NOTE: Not yet implemented</param>
        private bool IsValidWord(string word, bool minWordLength, bool lettersOnly,
            bool noStopWords, bool noVerbs)
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
                if (Constants.BibleStopWordList.Contains(word))
                    return false;
            }

            if (noVerbs)
            {

            }

            return true;
        }

        /// <summary>
        /// Splits the text of the document into an array of sentences.
        /// </summary>
        /// <param name="text">The text to be split</param>
        /// <returns>An array of sentences</returns>
        public static string[] SplitIntoSentences(string text)
        {
            //string pattern = @"[A-Z]([a-z]| )+[a-z][a-zA-Z0-9\-\(\)\/\,\'\;\:\s*\n*]*[\.]";
            string pattern = @"[^\.\?\!]*[\.\?\!]";
            IList<string> result = new List<string>();
            
            foreach (var match in Regex.Matches(text, pattern))
            {
                result.Add(match.ToString());
            }

            return result.ToArray();
        }
    }
}
