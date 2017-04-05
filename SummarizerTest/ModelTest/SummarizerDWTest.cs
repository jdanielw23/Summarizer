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
using Summarizer.Model.Utils;

namespace SummarizerTest.ModelTest
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// A class to test Daniel's summarization implementation
    /// </summary>
    [TestClass]
    public class SummarizerDWTest
    {
        [TestMethod]
        public void SummarizeAllDocumentsTest()
        {
            const int NUM_BOOKS = 16;
            int count = 0;
            foreach (string path in Directory.EnumerateFiles(@"..\..\..\Summarizer\Documents\The Bible txt - Original"))
            {                
                FileInfo file = new FileInfo(path);
                if (file.Extension.Equals(".txt"))
                {
                    count++;
                    string newFilePath = @"..\..\..\Summarizer\Documents\The Bible txt - Summarized\" + file.Name.Split('.')[0] + "_s" + file.Extension;
                    SummarizerDW summarizer = new SummarizerDW();
                    summarizer.SummarizeToNewDocument(path, newFilePath);
                }
                //if (count > NUM_BOOKS)
                //    break;
            }
        }

        [TestMethod]
        public void SplitIntoSentencesTest()
        {
            string text =
                "1:8 And God called the firmament Heaven. And the evening and the morning were the second day. " +
                "1:9 And God said, Let the waters under the heaven be gathered together " +
                "unto one place, and let the dry land appear: and it was so. " +
                "1:10 And God called the dry land Earth; and the gathering together of " +
                "the waters called he Seas: and God saw that it was good. " +
                "1:11 And God said, Let the earth bring forth grass, the herb yielding " +
                "seed, and the fruit tree yielding fruit after his kind, whose seed is " +
                "in itself, upon the earth: and it was so. " +
                "1:12 And the earth brought forth grass, and herb yielding seed after " +
                "his kind, and the tree yielding fruit, whose seed was in itself, after " +
                "his kind: and God saw that it was good. " +
                "1:13 And the evening and the morning were the third day. " +
                "1:14 And God said, Let there be lights in the firmament of the heaven " +
                "to divide the day from the night; and let them be for signs, and for " +
                "seasons, and for days, and years: " +
                "1:15 And let them be for lights in " +
                "the firmament of the heaven to give light upon the earth: and it was so. " +
                "1:16 And God made two great lights; the greater light to rule the day, " +
                "and the lesser light to rule the night: he made the stars also? " +
                "1:17 And God set them in the firmament of the heaven to give light " +
                "upon the earth, " +
                "1:18 And to rule over the day and over the night, and " +
                "to divide the light from the darkness: and God saw that it was good. " +
                "1:19 And the evening and the morning were the fourth day!";

            string[] sentences = SummarizerDW.SplitIntoSentences(text);
            Assert.AreEqual(11, sentences.Length);
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
        public void TrimCharactersTest()
        {
            string[] toTrim = new string[] { "furnace", "furnace,", "furnace;", "furnace:" };

            foreach (string word in toTrim)
            {
                Assert.AreEqual("furnace", word.TrimEnd(',', ':', ';'));
            }
        }


        /****    ONE TIME SCRIPTS    ****
        
        [TestMethod]
        public void FixTxtFiles()
        {
            foreach (string path in Directory.EnumerateFiles(@"D:\My Libraries\My Documents\The Bible txt"))
            {
                FileInfo file = new FileInfo(path);
                if (file.Extension.Equals(".txt"))
                {
                    string newFilePath = @"D:\My Libraries\My Documents\The Bible txt\Fixed\" + file.Name;

                    StringBuilder newText = new StringBuilder();
                    StringBuilder lines = new StringBuilder();

                    string text = System.IO.File.ReadAllText(path);
                    bool first = true;
                    foreach (string line in text.Split('\r'))
                    {
                        lines.Append(line.Trim()).Append(" ");
                        if (first)
                        {
                            newText.AppendLine(line);
                            first = false;
                        }
                    }

                    string pattern = @"[\d]+:[\d]+[\D]*";
                    IList<string> verses = new List<string>();
                    foreach (var match in Regex.Matches(lines.ToString(), pattern))
                    {
                        verses.Add(match.ToString());
                    }

                    foreach (string verse in verses)
                    {
                        newText.AppendLine(verse);
                    }

                    System.IO.File.WriteAllText(newFilePath, newText.ToString());
                }
            }
        }
        /*******************/

        /***************
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
        /****/
    }
}
