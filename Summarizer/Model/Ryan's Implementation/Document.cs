using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Ryan_s_Implementation
{
    // This data structure prevents redundancy, storing only the starts and lengths
    // of setences so that we can look it up rather than duplicate the data.

    struct SentenceData
    {
        public int start, length;
    }

    // Document class. Handles loading the text data and storing it.

    class Document
    {
        List<string> Sentences = new List<string>();

        public Document(string filePath)
        {
            string theEntireDoc = "";
            Sentences.Clear();

            // Calculate sentence info
            string currentSentence = "";
            
            using (StreamReader sr = new StreamReader(filePath))
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
                        Sentences.Add(currentSentence);
                        currentSentence = "";
                    }

                    sPrevChar = c;
                }
            }
            
            // Open the text file using a stream reader.
            //using ()
            // {
            // Read the stream to a string
            //    theEntireDoc = sr.ReadToEnd();

            // Remove line breaks to make things work easier
            //    theEntireDoc = theEntireDoc.Replace(System.Environment.NewLine, " ");
            // }
            //




            // Fill out the sentence data
            for (int i = 0; i < theEntireDoc.Length; i++)
            {
                char a = theEntireDoc.ElementAt(i);
                char b = ' ';

                //if (i + 1 >= theEntireDoc.Length)
                //    b = ' ';
                //else
                //    b = theEntireDoc.ElementAt(i + 1);

                if ((a == '!' || a == '?' || a == '.') && b == ' ')
                {
                }
            }

            theEntireDoc = "";
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
           
            return Sentences[index];
        }

        public void removeStopWords()
        {

        }

        public string getFullText()
        {
            return "";
        }
    }
}
