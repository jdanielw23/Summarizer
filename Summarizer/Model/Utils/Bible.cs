using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Summarizer.Model.Utils
{
    public class Bible
    {
        private static IDictionary<BibleBooks, Book> bible = BuildBible();
        private static Collection<string> verses;

        public static Collection<string> Verses
        {
            get
            {
                if (verses == null)
                {
                    verses = new Collection<string>();
                    foreach (Book b in bible.Values)
                    {
                        verses.AddAll(b.Verses);
                    }
                }
                return verses;
            }
        }

        /// <summary>
        /// Returns the book of the Bible with the supplied name
        /// </summary>
        public static Book Get(BibleBooks bookName)
        {
            if (bible.ContainsKey(bookName))
                return bible[bookName];
            return bible[BibleBooks.Genesis];
        }

        /// <summary>
        /// Returns the entire Bible as a single string of text.
        /// </summary>
        public static string All
        { get { return bible.Values.ListData(""); } }

        /// <summary>
        /// A dictionary containing the name of each book in the Bible as the key
        /// with the number of chapters in that book as the value
        /// </summary>
        public static IDictionary<BibleBooks, int> BookChapters
        {
            get
            {
                return new Dictionary<BibleBooks, int>()
                {
                    { BibleBooks.Genesis, 50 },
                    { BibleBooks.Exodus, 40 },
                    { BibleBooks.Leviticus, 27 },
                    { BibleBooks.Numbers, 36 },
                    { BibleBooks.Deuteronomy, 34 },
                    { BibleBooks.Joshua, 24 },
                    { BibleBooks.Judges, 21 },
                    { BibleBooks.Ruth, 4 },
                    { BibleBooks.FirstSamuel, 31 },
                    { BibleBooks.SecondSamuel, 24 },
                    { BibleBooks.FirstKings, 22 },
                    { BibleBooks.SecondKings, 25 },
                    { BibleBooks.FirstChronicles, 29 },
                    { BibleBooks.SecondChronicles, 36 },
                    { BibleBooks.Ezra, 10 },
                    { BibleBooks.Nehemiah, 13 },
                    { BibleBooks.Esther, 10 },
                    { BibleBooks.Job, 42 },
                    { BibleBooks.Psalm, 150 },
                    { BibleBooks.Proverbs, 31 },
                    { BibleBooks.Ecclesiastes, 12 },
                    { BibleBooks.SongOfSolomon, 8 },
                    { BibleBooks.Isaiah, 66 },
                    { BibleBooks.Jeremiah, 52 },
                    { BibleBooks.Lamentations, 5 },
                    { BibleBooks.Ezekiel, 48 },
                    { BibleBooks.Daniel, 12 },
                    { BibleBooks.Hosea, 14 },
                    { BibleBooks.Joel, 3 },
                    { BibleBooks.Amos, 9 },
                    { BibleBooks.Obadiah, 1 },
                    { BibleBooks.Jonah, 4 },
                    { BibleBooks.Micah, 7 },
                    { BibleBooks.Nahum, 3 },
                    { BibleBooks.Habakkuk, 3 },
                    { BibleBooks.Zephaniah, 3 },
                    { BibleBooks.Haggai, 2 },
                    { BibleBooks.Zechariah, 14 },
                    { BibleBooks.Malachi, 4 },
                    { BibleBooks.Matthew, 28 },
                    { BibleBooks.Mark, 16 },
                    { BibleBooks.Luke, 24 },
                    { BibleBooks.John, 21 },
                    { BibleBooks.Acts, 28 },
                    { BibleBooks.Romans, 16 },
                    { BibleBooks.FirstCorinthians, 16 },
                    { BibleBooks.SecondCorinthians, 13 },
                    { BibleBooks.Galatians, 6 },
                    { BibleBooks.Ephesians, 6 },
                    { BibleBooks.Philippians, 4 },
                    { BibleBooks.Colossians, 4 },
                    { BibleBooks.FirstThessalonians, 5 },
                    { BibleBooks.SecondThessalonians, 3 },
                    { BibleBooks.FirstTimothy, 6 },
                    { BibleBooks.SecondTimothy, 4 },
                    { BibleBooks.Titus, 3 },
                    { BibleBooks.Philemon, 1 },
                    { BibleBooks.Hebrews, 13 },
                    { BibleBooks.James, 5 },
                    { BibleBooks.FirstPeter, 5 },
                    { BibleBooks.SecondPeter, 3 },
                    { BibleBooks.FirstJohn, 5 },
                    { BibleBooks.SecondJohn, 1 },
                    { BibleBooks.ThirdJohn, 1 },
                    { BibleBooks.Jude, 1 },
                    { BibleBooks.Revelation, 22 }
                };
            }
        }

        public class Book : IEnumerator, IEnumerable
        {
            private IDictionary<int, Chapter> book;
            private Collection<string> verses;
            int position = -1;

            public Collection<string> Verses
            {
                get
                {
                    if (verses == null)
                    {
                        verses = new Collection<string>();
                        foreach(Chapter c in book.Values)
                        {
                            verses.AddAll(c.Verses);
                        }
                    }
                    return verses;
                }
            }

            public Book(IDictionary<int, Chapter> chapters)
            {
                book = chapters;
            }

            /// <summary>
            /// Returns the chapter for the given chapter number
            /// </summary>
            public Chapter this[int chapterNumber]
            { get { return Get(chapterNumber); } }

            /// <summary>
            /// Returns the chapter for the given chapter number
            /// </summary>
            public Chapter Get(int chapterNumber)
            {
                if (book.ContainsKey(chapterNumber))
                    return book[chapterNumber];
                return book[1];
            }

            /// <summary>
            /// Returns the verse for the given reference string
            /// </summary>
            public Verse this[string reference]
            { get { return Get(reference); } }

            /// <summary>
            /// Returns the verse for the given reference string
            /// </summary>
            public Verse Get(string reference)
            {
                string exceptionText = "Improperly formatted reference";
                if (!reference.Contains(":"))
                    throw new Exception(exceptionText);

                string[] chapVerse = reference.Split(':');

                if (chapVerse.Length < 2)
                    throw new Exception(exceptionText);

                int chap = Int32.Parse(chapVerse[0]);
                int verse = Int32.Parse(chapVerse[1]);

                return this[chap][verse];
            }

            /// <summary>
            /// Converts the entire book into a single string of text
            /// </summary>
            public override string ToString()
            {
                return book.Values.ListData("");
            }

            //IEnumerator and IEnumerable require these methods.
            public IEnumerator GetEnumerator()
            {
                return (IEnumerator) Verses.GetEnumerator();
            }

            //IEnumerator
            public bool MoveNext()
            {
                position++;                
                return (position < Verses.Count);
            }

            //IEnumerable
            public void Reset()
            { position = 0; }

            //IEnumerable
            public object Current
            { get { return Verses[position]; } }
        }

        public class Chapter : IEnumerator, IEnumerable
        {
            private IDictionary<int, Verse> chapter;
            private Collection<string> verses;
            int position = -1;

            public Collection<string> Verses
            {
                get
                {
                    if (verses == null)
                    {
                        verses = new Collection<string>();
                        foreach (Verse v in chapter.Values)
                            verses.Add(v.ToString());
                    }
                    return verses;
                }
            }

            public Chapter(IDictionary<int, Verse> verses)
            {
                chapter = verses;
            }

            /// <summary>
            /// Returns the verse for the given verse number
            /// </summary>
            public Verse this[int verseNumber]
            { get { return Get(verseNumber); } }

            /// <summary>
            /// Returns the verse for the given verse number
            /// </summary>
            public Verse Get(int verseNumber)
            {
                if (chapter.ContainsKey(verseNumber))
                    return chapter[verseNumber];
                return chapter[1];
            }

            /// <summary>
            /// Converts the entire chapter into a single string of text
            /// </summary>
            public override string ToString()
            {
                return chapter.Values.ListData("");
            }

            //IEnumerator and IEnumerable require these methods.
            public IEnumerator GetEnumerator()
            {
                return Verses.GetEnumerator();
            }

            //IEnumerator
            public bool MoveNext()
            {
                position++;
                return (position < Verses.Count);
            }

            //IEnumerable
            public void Reset()
            { position = 0; }

            //IEnumerable
            public object Current
            {
                get { return Verses[position]; }
            }
        }

        public class Verse
        {
            private string Text;

            public Verse(string text)
            {
                Text = text;
            }

            /// <summary>
            /// Returns the text of the verse
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Text;
            }
        }

        private static IDictionary<BibleBooks, Book> BuildBible()
        {
            IDictionary<BibleBooks, Book> books = new Dictionary<BibleBooks, Book>();
            List<BibleBooks> BibleNames = BookChapters.Keys.ToList();
            string documents = "";
#if DEBUG
            documents = @"..\..\..\Summarizer\Documents\The Bible txt - Original";
#else
            documents = @".\Documents\The Bible txt - Original";
#endif
            int bkInd = 0;
            foreach (string path in Directory.EnumerateFiles(documents))
            {
                IDictionary<int, Chapter> chapters = new Dictionary<int, Chapter>();

                string text = System.IO.File.ReadAllText(path);
                BibleBooks book = BibleNames[bkInd];

                // Break the book into chapters
                for (int chap = 1; chap <= BookChapters[book]; chap++)
                {
                    IDictionary<int, Verse> verses = new Dictionary<int, Verse>();

                    // This regex will get verses that start with this chapter number
                    string regexPattern = string.Format(@"{0}:[\d]+[\D]*", chap);

                    int verseNum = 1;
                    foreach (var verse in Regex.Matches(text, regexPattern))
                    {
                        // This prevents including chapter "11:1" as "1:1"
                        int txtVerse = Int32.Parse(verse.ToString().Split(':')[1].Split(' ')[0]);
                        if (txtVerse < verseNum)
                            break;

                        verses[verseNum] = new Verse(verse.ToString());
                        verseNum++;
                    }
                    chapters[chap] = new Chapter(verses);
                }
                books[book] = new Book(chapters);

                // This prevents it from trying to add the entire bible
                bkInd++;
                if (bkInd == 66)
                    break;
            }
            return books;
        }
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum BibleBooks
    {
        [Description("Genesis")]
        Genesis,
        [Description("Exodus")]
        Exodus,
        [Description("Leviticus")]
        Leviticus,
        [Description("Numbers")]
        Numbers,
        [Description("Deuteronomy")]
        Deuteronomy,
        [Description("Joshua")]
        Joshua,
        [Description("Judges")]
        Judges,
        [Description("Ruth")]
        Ruth,
        [Description("1 Samuel")]
        FirstSamuel,
        [Description("2 Samuel")]
        SecondSamuel,
        [Description("1 Kings")]
        FirstKings,
        [Description("2 Kings")]
        SecondKings,
        [Description("1 Chronicles")]
        FirstChronicles,
        [Description("2 Chronicles")]
        SecondChronicles,
        [Description("Ezra")]
        Ezra,
        [Description("Nehemiah")]
        Nehemiah,
        [Description("Esther")]
        Esther,
        [Description("Job")]
        Job,
        [Description("Psalm")]
        Psalm,
        [Description("Proverbs")]
        Proverbs,
        [Description("Ecclesiastes")]
        Ecclesiastes,
        [Description("Song of Solomon")]
        SongOfSolomon,
        [Description("Isaiah")]
        Isaiah,
        [Description("Jeremiah")]
        Jeremiah,
        [Description("Lamentations")]
        Lamentations,
        [Description("Ezekiel")]
        Ezekiel,
        [Description("Daniel")]
        Daniel,
        [Description("Hosea")]
        Hosea,
        [Description("Joel")]
        Joel,
        [Description("Amos")]
        Amos,
        [Description("Obadiah")]
        Obadiah,
        [Description("Jonah")]
        Jonah,
        [Description("Micah")]
        Micah,
        [Description("Nahum")]
        Nahum,
        [Description("Habakkuk")]
        Habakkuk,
        [Description("Zephaniah")]
        Zephaniah,
        [Description("Haggai")]
        Haggai,
        [Description("Zechariah")]
        Zechariah,
        [Description("Malachi")]
        Malachi,
        [Description("Matthew")]
        Matthew,
        [Description("Mark")]
        Mark,
        [Description("Luke")]
        Luke,
        [Description("John")]
        John,
        [Description("Acts")]
        Acts,
        [Description("Romans")]
        Romans,
        [Description("1 Corinthians")]
        FirstCorinthians,
        [Description("2 Corinthians")]
        SecondCorinthians,
        [Description("Galatians")]
        Galatians,
        [Description("Ephesians")]
        Ephesians,
        [Description("Philippians")]
        Philippians,
        [Description("Colossians")]
        Colossians,
        [Description("1 Thessalonians")]
        FirstThessalonians,
        [Description("2 Thessalonians")]
        SecondThessalonians,
        [Description("1 Timothy")]
        FirstTimothy,
        [Description("2 Timothy")]
        SecondTimothy,
        [Description("Titus")]
        Titus,
        [Description("Philemon")]
        Philemon,
        [Description("Hebrews")]
        Hebrews,
        [Description("James")]
        James,
        [Description("1 Peter")]
        FirstPeter,
        [Description("2 Peter")]
        SecondPeter,
        [Description("1 John")]
        FirstJohn,
        [Description("2 John")]
        SecondJohn,
        [Description("3 John")]
        ThirdJohn,
        [Description("Jude")]
        Jude,
        [Description("Revelation")]
        Revelation
    }
}
