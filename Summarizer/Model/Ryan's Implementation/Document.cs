using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Summarizer.Model.Ryan_s_Implementation
{
    // This data structure prevents redundancy, storing only the starts and lengths
    // of setences so that we can look it up rather than duplicate the data.

    // Document class. Handles loading the text data and storing it.
    public class Document
    {
        List<string> Sentences = new List<string>();

        public void TestDocumentClass()
        {
            string text = "In Christ alone my hope is found;" + 
                           "He is my light, my strength, my song; " +
                           "This cornerstone, this solid ground, " + 
                           "Firm through the fiercest drought and storm. " +
                           "What heights of love, what depths of peace, When "+
                           "fears are stilled, when strivings cease! My comforter, " +
                           "my all in all— Here in the love of Christ I stand.";

            LoadSentences(text, false);
            
        }

        private void LoadSentences(string input, bool file)
        {
            Sentences.Clear();

            // Calculate sentence info
            string currentSentence = "";

            if (file)
            {
                using (StreamReader sr = new StreamReader(input))
                {
                    char sPrevChar = '\0';

                    while (sr.Peek() >= 0)
                    {
                        // Read the char
                        char c = Convert.ToChar(sr.Read());

                        // Remove escape senquences
                        if (c == '\r' || c == '\t' || c == '\n' || c == 10 || c == 13)
                            c = ' ';

                        // Append current char to sentence
                        currentSentence += c;

                        // Read into the data and break into sentences for easier access later after
                        // finding the values.
                        if ((sPrevChar == '!' || sPrevChar == '?' || sPrevChar == '.') && c == ' ')
                        {
                            currentSentence = currentSentence.Trim();
                            Sentences.Add(currentSentence);
                            currentSentence = "";
                        }

                        sPrevChar = c;
                    }
                }

            }
            else
            {
                int pos = 0;

                // Remove line breaks, these don't work well.
                input = Regex.Replace(input, @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085]+", " ");

                foreach (Match m in Regex.Matches(input, "([!?.][ \"']+)"))
                {
                    string sentence = input.Substring(pos, m.Index - pos) + m.Value;
                    pos = m.Index + m.Length;
                    Sentences.Add(sentence);
                }

            }
        }

        public Document(string input, bool file)
        {
            LoadSentences(input, file);
        }

        /// <summary>
        /// Return the number of sentences.
        /// </summary>
        /// <returns>The number of sentences contained by the document.</returns>
        public int getSentenceCount()
        {
            return Sentences.Count();
        }

        public string getSentence(int index)
        {
            if (index < 0 || index >= getSentenceCount())
                throw new System.IndexOutOfRangeException("Index is out of range!");
           
            return Sentences[index].Trim();
        }
        
    }


}
