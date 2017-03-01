using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Summarizer.Model.Utils.Stemming;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.util;

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

    struct BigramEntry
    {
        public int wordA, wordB;
        public int ouccrances;
    }
   
    class SummarizerRR : SummarizerImplementation
    {
<<<<<<< HEAD
        private const int   MIN_SENTENCES =                     0; // Minimum number of sentences, less will be rejected.
        private const int   MAX_FILE_SIZE =             1000000000; // Maximum number of bytes a single document can have.
        private const bool  REMOVE_NON_ASCII =                true; // Remove non-ascii characters from the result.
        private const int   MIN_WORD_LENGTH =                    3; // Words smaller than this are ignored.
        private const bool  PERFORM_STEMMING =                true; // Turns stemming on and off.
        private const int   NUM_TOP_WORDS =                     10;
        private const int   NUM_MIN_WORDS_TO_WORK =              6; // Do not change this!
        private const bool  BIBLE_MODE =                     false;
        private const int   MIN_SENTENCE_LENGTH =                3;

=======
        private const int   MIN_SENTENCES = 0; // Minimum number of sentences, 
                                                // less will be rejected.
        private const int   MAX_FILE_SIZE = 1000000000; // Maximum number of bytes
                                                    // a single document can have.
        private const bool  REMOVE_NON_ASCII = true; // Remove non-ascii characters
                                                    // from the result.
        private const int   MIN_WORD_LENGTH = 3; // Words smaller than this are ignored.
        private const bool  PERFORM_STEMMING = false; // Turns stemming on and off.
        private const int   NUM_TOP_WORDS = 10;
        private const int   NUM_MIN_WORDS_TO_WORK = 6; // Do not change this!
       
>>>>>>> origin/master
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
            if (BIBLE_MODE)
            {
                for (int k = 0; k < Utils.Constants.BibleStopWordList.Length; k++)
                {
                    string wordA = " " + Utils.Constants.BibleStopWordList.ElementAt(k) + " ";
                    text = text.Replace(wordA.ToLower(), " ");
                }
            }
            else
            {
                for (int k = 0; k < Utils.Constants.LongStopWordList.Length; k++)
                {
                    string wordA = " " + Utils.Constants.LongStopWordList.ElementAt(k) + " ";
                    text = text.Replace(wordA.ToLower(), " ");
                }
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

                if (words.Length < MIN_SENTENCE_LENGTH)
                    continue;
               
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
                return "Cannot generate a summary; Document is too short. It needs at least "
                    + MIN_SENTENCES + ", but only has " + doc.getSentenceCount() + ".";


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
<<<<<<< HEAD
            summaryString = "Word Frequency Table Time Taken: " + time + " milliseconds.\n";
            totalTimeTaken += time;
=======
            summaryString = "Word Frequency Table Time Taken: " + ((float)time / 1000.0f)
                + " seconds.\n\n";
>>>>>>> origin/master

            // Sort the words from highest to lowest count
            stopwatch.Reset();
            stopwatch.Start();
            List<KeyValuePair<string, WordFreqDataEntry>> SortedWordList = wordFreqTable.ToList();
            SortedWordList.Sort((x, y) => y.Value.count.CompareTo(x.Value.count));
<<<<<<< HEAD
            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds;
            summaryString += "Word entry sorting time: " + ((float)time + " milliseconds.\n");
=======
            stopwatch2.Stop();
            time = stopwatch2.ElapsedMilliseconds;
            summaryString += "Word entry sorting time: " + ((float)time / 1000.0f)
                + " seconds.\n\n";

>>>>>>> origin/master
            totalTimeTaken += time;
            
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

            // Calculate the most significant bigrams
            BigramEntry[] bigrams = new BigramEntry[WF_SIZE * WF_SIZE];

            stopwatch.Reset();
            stopwatch.Start();
            // Fill out the matrix
            for (int i = 0; i < WF_SIZE; i++)
                for (int j = 0; j < WF_SIZE; j++)
                {
                    HashSet<int> a, b;
                    a = SortedWordList.ElementAt(i).Value.occurances;
                    b = SortedWordList.ElementAt(j).Value.occurances;
                    coOccurance[i, j] = new HashSet<int>(a);
                    coOccurance[i, j].IntersectWith(b);

                    bigrams[(j * WF_SIZE) + i].wordA = i;
                    bigrams[(j * WF_SIZE) + i].wordB = j;

                    if (bigrams[(j * WF_SIZE) + i].wordA != bigrams[(j * WF_SIZE) + i].wordB)
                        bigrams[(j * WF_SIZE) + i].ouccrances = coOccurance[i, j].Count;
                    else
                        bigrams[(j * WF_SIZE) + i].ouccrances = 0;
                }

            // Find the highest bigrams
            Array.Sort(bigrams, ((x, y) => y.ouccrances.CompareTo(x.ouccrances)));

            stopwatch.Stop();

<<<<<<< HEAD
            time = stopwatch.ElapsedMilliseconds;
            summaryString += "Bigram generation time: " + time + " milliseconds\n\n";
            totalTimeTaken += time;

            summaryString += "Total Time: " + totalTimeTaken + " milliseconds\n\n";
            // So here we have our top bigrams - there are duplicates in the list, so make
            // sure to iterate by 2 to skip the second entry; It's not worth the amount
            // of time or work to remove them.
            summaryString = "";
=======
            // Rating will be a ratio of the total significant words to the number
            // of words in the bigrams
>>>>>>> origin/master

            float[] rating = new float[3];
            int lastHighestString = -1;
            int prevSentence = -1;
            int[] stringNumber = new int[3];
            string[] finalSentences = new string[3];

            rating[0] = -1;
            rating[1] = -1;
            rating[2] = -1;

            int z = 0;

            int[] used = new int[3];

            for (int k = 0; k < 6; k += 2)
            {
                //summaryString += k + ":: " + SortedWordList.ElementAt(bigrams[k].wordA).Key + " --- " + SortedWordList.ElementAt(bigrams[k].wordB).Key + "\n";

                for (int i = 0; i < coOccurance[bigrams[k].wordA, bigrams[k].wordB].Count; i++)
                {
                    int currentSentenceIndex = coOccurance[bigrams[k].wordA, bigrams[k].wordB].ElementAt(i);

                    string[] words = Clean(doc.getSentence(currentSentenceIndex)).Split(' ');
                    int totalsWords = words.Count();
                    int wordACnt = 0, wordBCnt = 0;

                    for (int j = 0; j < words.Count(); j++)
                    {
                        if (words[j] == SortedWordList.ElementAt(bigrams[k].wordA).Key)
                            wordACnt++;

                        if (words[j] == SortedWordList.ElementAt(bigrams[k].wordB).Key)
                            wordBCnt++;
                    }

                    float r = (float)(wordACnt + wordBCnt) / (float)totalsWords;

                    if (rating[z] < r && currentSentenceIndex != used[0] && currentSentenceIndex != used[1])
                    {
                        used[z] = currentSentenceIndex;
                        rating[z] = r;
                        lastHighestString = currentSentenceIndex;
                    }
                };

                if (lastHighestString <= -1)
                    return summaryString + "\n\nThere was an error. Summary string " + k + " could not be created.";

                stringNumber[z] = lastHighestString;
                prevSentence = lastHighestString;

                lastHighestString = -1;

                z++;
            }

            Array.Sort(stringNumber);

            summaryString += "\"" + (doc.getSentence(stringNumber[0]) +
                             " " + doc.getSentence(stringNumber[1]) +
                             " " + doc.getSentence(stringNumber[2]) + "\"\n\n");

            /*
            var jarRoot = @"E:\Junk\stanford-postagger-2016-10-31";
            var modelsDirectory = jarRoot + @"\models";

            // Loading POS Tagger
            var tagger = new MaxentTagger(modelsDirectory + @"\english-bidirectional-distsim.tagger");
            
            summaryString += "\n\n";

            var text = summaryString;  
            summaryString = "";

            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(text)).toArray();
            foreach (ArrayList sentence in sentences)
            {
<<<<<<< HEAD
                var taggedSentence = tagger.tagSentence(sentence);
                summaryString += SentenceUtils.listToString(taggedSentence, false) + "\n\n";
            }
            //*/
=======
                summaryString += "(" + (union.ElementAt(i) + 1) + "): "
                    + doc.getSentence(union.ElementAt(i)) + "\n\n";
            }//*/
>>>>>>> origin/master

            return summaryString;

            /*
            // Rating will be a ratio of the total significant words to the number of words in the bigrams

            // WE HAVE THE WF TABLE, AND COOCCURANCE MATRIX
            //=============================================
            // NOW FIND THE WORDS TO BUILD THE SUMMARY

            for (int k = 0; k < 3; k++)
            {
                for (int i = 0; i < coOccurance[k*2, (k*2)+1].Count; i++)
                {
<<<<<<< HEAD
                    int currentSentenceIndex = coOccurance[k * 2, (k * 2) + 1].ElementAt(i);

                    string[] words = Clean(doc.getSentence(currentSentenceIndex)).Split(' ');
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

                    if (rating[k] < r && currentSentenceIndex != used[0] && currentSentenceIndex != used[1])
                    {
                        used[k] = currentSentenceIndex;
                        rating[k] = r;
                        lastHighestString = currentSentenceIndex;
                    }
=======
                    summaryString += "(" + (v.occurances.ElementAt(i) + 1) + "): "
                        + doc.getSentence(v.occurances.ElementAt(i)) + "\n\n";
>>>>>>> origin/master
                }

                if (lastHighestString <= -1)
                    return summaryString + "\n\nThere was an error. Summary string " + k + " could not be created.";
                
                stringNumber[k] = lastHighestString;
                prevSentence = lastHighestString;

                lastHighestString = -1;
            }

            Array.Sort(stringNumber);

            summaryString =  doc.getSentence(stringNumber[0]) +
                             " " + doc.getSentence(stringNumber[1]) +
                             " " + doc.getSentence(stringNumber[2]) + "\n\n";

            return summaryString;
                    //*/
        }

    }
    

}
