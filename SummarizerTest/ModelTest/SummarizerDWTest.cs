using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Summarizer.Model.Daniels_Implementation;
using Summarizer.Model.Utils.Stemming;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class SummarizerDWTest
    {
        [TestMethod]
        public void SummarizeAllDocumentsTest()
        {
            foreach (string path in Directory.EnumerateFiles(@"D:\My Libraries\My Documents\The Bible txt"))
           // foreach (string path in Directory.EnumerateFiles(@"Documents"))
            {
                FileInfo file = new FileInfo(path);
                if (file.Extension.Equals(".txt"))
                {
                    //string newFilePath = @"SummarizedDocuments\" + file.Name;
                    string newFilePath = @"D:\My Libraries\My Documents\The Bible txt\Summarized\" + file.Name;
                    SummarizerDW summarizer = new SummarizerDW();
                    summarizer.SummarizeToNewDocument(path, newFilePath);
                }
            }
            
            //Process.Start(newFilePath);
        }

        [TestMethod]
        public void SentenceRegexTest()
        {
            // This is a pattern to recognize an acceptable sentence
            string pattern = @"[A-Z]([a-z]| )+[a-z][a-zA-Z0-9\-\(\)\/\,\'\:\;\s*\n*]*[\.]";

            string true1 = "This is an acceptable sentence.";
            string true2 = "This is an acceptable sentence\n even though it is split.";
            string true3 = "This is: an acceptable; sentence.";
            string true4 = "This is: an acceptable; sentence.";

            string false1 = "THIS IS NOT AN ACCEPTABLE SENTENCE";

            Assert.AreEqual(true, Regex.IsMatch(true1, pattern));
            Assert.AreEqual(true, Regex.IsMatch(true2, pattern));
            Assert.AreEqual(true, Regex.IsMatch(true3, pattern));
            Assert.AreEqual(true, Regex.IsMatch(true4, pattern));

            Assert.AreEqual(false, Regex.IsMatch(false1, pattern));
        }

        [TestMethod]
        public void StemmerTest()
        {
            string[] maintain = new string[] { "maintain", "maintains", "maintaining", "maintained" };
            string[] walk = new string[] { "walk", "walking", "walked", "walks" };

            IStemmer stemmer = new EnglishStemmer();

            foreach(string word in maintain)
            {
                Assert.AreEqual("maintain", stemmer.Stem(word));
            }

            foreach (string word in walk)
            {
                Assert.AreEqual("walk", stemmer.Stem(word));
            }

        }

        [TestMethod]
        public void SeparateBible()
        {
            string biblePath = @"D:\My Libraries\My Documents\The Bible txt\Bible_KJV.txt";

            string[] bibleText = System.IO.File.ReadAllLines(biblePath);

            IList<string> books = new List<string>();
            
            int newLineCount = 0;
            StringBuilder bk = new StringBuilder();
            foreach (string nxtLine in bibleText)
            {
                if (nxtLine.Equals(""))
                    newLineCount++;
                else
                    newLineCount = 0;

                if (newLineCount < 4)
                    bk.AppendLine(nxtLine);
                else
                {
                    books.Add(bk.ToString());
                    bk.Clear();
                    newLineCount = 0;
                }
            }

            IList<string> bookPaths = new List<string>();
            int bookNum = 1;
            foreach (string book in books)
            {
                if (book.Length < 100)
                    continue;

                string bookName = book.Split('\n')[0].Trim();
                if (bookName.Contains(":"))
                    bookName = bookName.Split(' ').Last().Trim();

                string bookPath = string.Format(@"D:\My Libraries\My Documents\The Bible txt\{0:00} - {1}.txt", bookNum, bookName);

                System.IO.File.WriteAllText(bookPath, book);
                bookNum++;
                //bookPaths.Add(bookPath);
            }

            //System.IO.File.WriteAllLines(@"D:\My Libraries\My Documents\The Bible txt\BookPaths.txt", bookPaths);
        }
    }
}
