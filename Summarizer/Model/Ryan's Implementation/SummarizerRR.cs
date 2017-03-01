using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Summarizer.Model.Utils.Stemming;

// I'm not going to use R anymore, seems to much work to learn and use it for a single text 
// document. I may fallback on it though if mine doesn't pan out right, but I think this
// approach works better.

namespace Summarizer.Model.Ryan_s_Implementation
{
    struct WordFreqDataEntry
    {
        public HashSet<int> occurances;
        public int count;
    }

    struct Bigram
    {
        string a, b;
    }
    
    class SummarizerRR : SummarizerImplementation
    {
        private const int   MIN_SENTENCES =                     0; // Minimum number of sentences, less will be rejected.
        private const int   MAX_FILE_SIZE =             1000000000; // Maximum number of bytes a single document can have.
        private const bool  REMOVE_NON_ASCII =                true; // Remove non-ascii characters from the result.
        private const int   MIN_WORD_LENGTH =                    3; // Words smaller than this are ignored.
        private const bool  PERFORM_STEMMING =                false; // Turns stemming on and off.
        private const int   NUM_TOP_WORDS =                     10;
        private const int   NUM_MIN_WORDS_TO_WORK =              6; // Do not change this!
       
        private readonly char[] newLineA = { (char)13, (char)0 };
        private readonly char[] newLineB = { (char)10, (char)0 };

        SortedDictionary<string, WordFreqDataEntry> wordFreqTable;

        public SummarizerRR()
        {
              
        }


        private string printArray(List<int> list)
        {
            if (list.Count == 0)
                return "";

            string t = Convert.ToString(list[0]);

            for (int i = 1; i < list.Count; i++)
            {
                t += ", " + Convert.ToString(list[i]);
            }

            return t;
        }


        private string printHashSet(HashSet<int> set)
        {
            if (set.Count == 0)
                return "";

            string t = Convert.ToString(set.ElementAt(0));

            for (int i = 1; i < set.Count; i++)
            {
                t += ", " + Convert.ToString(set.ElementAt(i));
            }

            return t;
        }

        // Cleans a sentence/word
        private string Clean(string text)
        {

            // Remove non-ascii?
            if (REMOVE_NON_ASCII)
            {
                for (int k = 0; k < text.Length; k++)
                {
                    if ((int)text[k] > 255)
                    {
                        text = text.Substring(0, k) + text.Substring(k + 1, text.Length - k - 1);
                    }
                }
            }

            // Trim it
            text = text.Trim();

            // convert to lower case
            text = text.ToLower();

            // remove punctuation
            for (int k = 0; k < Utils.Constants.Punctuation.Length; k++)
                text = text.Replace(Utils.Constants.Punctuation.ElementAt(k), " ");
            
            // Remove stopwords
            for (int k = 0; k < Utils.Constants.BibleStopWordList.Length; k++)
            {
                string wordA = " " + Utils.Constants.BibleStopWordList.ElementAt(k) + " ";
                text = text.Replace(wordA.ToLower(), " ");
            }


            return text;
        }
       
        private void buildWordFrequencyTable(Document doc)
        {
            wordFreqTable = new SortedDictionary<string, WordFreqDataEntry>();

            EnglishStemmer stemmer = new EnglishStemmer();

            for (int i = 0; i < doc.getSentenceCount(); i++)
            {
                string[] words = Clean(doc.getSentence(i)).Split(' ');
               
                for (int k = 0; k < words.Length; k++)
                {
                    string word = "";

                    if (PERFORM_STEMMING)
                        word = stemmer.Stem(words[k]);
                    else
                        word = words[k];


                    if (word == "" || word.Length < MIN_WORD_LENGTH)
                        continue;

                    WordFreqDataEntry value;

                    if (wordFreqTable.TryGetValue(word, out value))
                    {
                        value.count++;
                        value.occurances.Add(i);
                        wordFreqTable[word] = value;
                    }
                    else
                    {
                        WordFreqDataEntry newEntry = new WordFreqDataEntry();
                        newEntry.occurances = new HashSet<int>();
                        newEntry.count = 1;
                        newEntry.occurances.Add(i);

                        wordFreqTable.Add(word, newEntry);
                    }
                }
            }
        }

        public string SummarizeDocument(string filePath)
        {
            // Timer
            long totalTimeTaken = 0;

            // Load the document data
            Document doc = new Document(filePath);

            // If we don't have enough sentences, fail.
            if (doc.getSentenceCount() < MIN_SENTENCES)
                return "Cannot generate a summary; Document is too short. It needs at least " + MIN_SENTENCES + ", but only has " + doc.getSentenceCount() + ".";

            // Default string, if an error occurs or we get no output.
            string summaryString = "Error: Summary could not be created.";

            // Timing for measuring and setting standards.
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Build the WF table
            buildWordFrequencyTable(doc);

            // Caculate the time taken
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds;
            summaryString = "Word Frequency Table Time Taken: " + ((float)time / 1000.0f) + " seconds.\n\n";

            // Sort the words from highest to lowest count
            System.Diagnostics.Stopwatch stopwatch2 = new System.Diagnostics.Stopwatch();
            stopwatch2.Start();
            List<KeyValuePair<string, WordFreqDataEntry>> SortedWordList = wordFreqTable.ToList();
            SortedWordList.Sort((x, y) => y.Value.count.CompareTo(x.Value.count));
            stopwatch2.Stop();
            time = stopwatch2.ElapsedMilliseconds;
            summaryString += "Word entry sorting time: " + ((float)time / 1000.0f) + " seconds.\n\n";

            totalTimeTaken += time;

            summaryString += "\n\n";

            foreach (KeyValuePair<string, WordFreqDataEntry> p in SortedWordList)
            {
                summaryString += p.Key + ": " + p.Value.count + "\n";
            }

            // Build WF co-occurance matrix

            // Find the smallest integer viable for calculating the WFs
            int WF_SIZE = Math.Min(NUM_TOP_WORDS, SortedWordList.Count);

            if (WF_SIZE < NUM_MIN_WORDS_TO_WORK)
                return "Error: Not enough words in the document to generate a summary.";

            // Must be at least six to work.
            if (WF_SIZE < NUM_MIN_WORDS_TO_WORK)
                WF_SIZE = NUM_MIN_WORDS_TO_WORK;

            // Create the matrix
            HashSet<int>[,] coOccurance = new HashSet<int>[WF_SIZE, WF_SIZE];

            // Fill out the matrix
            for (int i = 0; i < WF_SIZE; i++)
                for (int j = 0; j < WF_SIZE; j++)
                    if (i != j)
                    {
                        HashSet<int> a, b;
                        a = SortedWordList.ElementAt(i).Value.occurances;
                        b = SortedWordList.ElementAt(j).Value.occurances;
                        coOccurance[i, j] = new HashSet<int>(a);
                        coOccurance[i, j].IntersectWith(b);
                    }
                    else
                        coOccurance[i, j] = new HashSet<int>();





            // Rating will be a ratio of the total significant words to the number of words in the bigrams

            /*
            float[] rating = new float[3];
            int lastHighestString = -1;
            int[] stringNumber = new int[3];
            string[] finalSentences = new string[3];

            rating[0] = -1;
            rating[1] = -1;
            rating[2] = -1;

            // WE HAVE THE WF TABLE, AND COOCCURANCE MATRIX
            //=============================================
            // NOW FIND THE WORDS TO BUILD THE SUMMARY

            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < coOccurance[k*2, (k*2)+1].Count; i++)
                {
                    int currentWordIndex = coOccurance[k * 2, (k * 2) + 1].ElementAt(i);

                    string[] words = Clean(doc.getSentence(currentWordIndex)).Split(' ');
                    int totalsWords = words.Count();
                    int wordACnt = 0, wordBCnt = 0;

                    for (int j = 0; j < words.Count(); j++)
                    {
                        if (words[j] == SortedWordList.ElementAt(k*2).Key)
                            wordACnt++;

                        if (words[j] == SortedWordList.ElementAt((k*2)+1).Key)
                            wordBCnt++;
                    }

                    float r = (float)(wordACnt + wordBCnt) / (float)totalsWords;

                    if (rating[k] < r)
                    {
                        rating[k] = r;
                        lastHighestString = currentWordIndex;
                    }
                }

                if (lastHighestString <= -1)
                    return "There was an error. Summary string " + k + " could not be created.";

                finalSentences[k] = doc.getSentence(lastHighestString);
                stringNumber[k] = lastHighestString;
                
                lastHighestString = -1;
            }
            

            summaryString += "(" + rating[0] + ") " + finalSentences[0] +
                             "(" + rating[1] + ") " + finalSentences[1] +
                             "(" + rating[2] + ") " + finalSentences[2] + "\n\n";



            //wordFreqTable.TryGetValue("jesus", out v);
            // wordFreqTable.TryGetValue("christ", out u);

            // if (u.occurances == null || v.occurances == null)
            //   return summaryString;

            //HashSet<int> union = new HashSet<int>(u.occurances);
            // union.IntersectWith(v.occurances);

            /*for (int i = 0; i < union.Count; i++)
            {
                summaryString += "(" + (union.ElementAt(i) + 1) + "): " + doc.getSentence(union.ElementAt(i)) + "\n\n";
            }//*/


            /*
            for (int i = 0; i < doc.getSentenceCount(); i++)
            {
                summaryString += Clean(doc.getSentence(i));
            }

            HashSet<int> union = new HashSet<int>(u.occurances);
            union.IntersectWith(v.occurances);
            /*
            WordFreqDataEntry v;

            if (wordFreqTable.TryGetValue("jesus", out v))
            {
                for (int i = 0; i < v.occurances.Count; i++)
                {
                    summaryString += "(" + (v.occurances.ElementAt(i) + 1) + "): " + doc.getSentence(v.occurances.ElementAt(i)) + "\n\n";
                }
            };

    //*/


            return summaryString;
        }
    }
    

}
