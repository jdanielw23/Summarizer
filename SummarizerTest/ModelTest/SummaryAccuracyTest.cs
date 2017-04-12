using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Summarizer.Model.Daniels_Implementation;
using Summarizer.Model;
using Summarizer.Model.Ryan_s_Implementation;
using Summarizer.Model.Andrews_Implementation;
using Summarizer.Model.Utils;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class SummaryAccuracyTest
    {
        [TestMethod]
        public void RuthTest()
        {
            string[] refs = { "1:6", "1:22", "2:15", "3:11", "3:13", "4:17" };
            Assert.IsTrue(BibleBookSummaryTest(BibleBooks.Ruth, refs));
        }

        [TestMethod]
        public void RomansTest()
        {
            string[] refs = { "1:16", "1:17", "6:23", "10:13", "12:1", "12:2" };
            Assert.IsTrue(BibleBookSummaryTest(BibleBooks.Romans, refs));
        }

        [TestMethod]
        public void GalatiansTest()
        {
            string[] refs = { "1:9", "2:20", "3:1", "3:2", "3:3", "5:16" };
            Assert.IsTrue(BibleBookSummaryTest(BibleBooks.Galatians, refs));
        }

        //[TestMethod]
        public void HebrewsTest()
        {
            string[] refs = { "1:6", "3:3", "4:12", "5:10", "11:1", "12:2" };
            Assert.IsTrue(BibleBookSummaryTest(BibleBooks.Hebrews, refs));
        }

        [TestMethod]
        public void JamesTest()
        {
            string[] refs = { "1:22", "2:10", "2:18", "4:7", "5:19", "5:20" };
            Assert.IsTrue(BibleBookSummaryTest(BibleBooks.James, refs));
        }

        /// <summary>
        /// Runs a summary test on the given Bible book. 
        /// </summary>
        /// <param name="book">The book of the Bible to summarize</param>
        /// <param name="desiredRefs">The references for the desired summary</param>
        /// <returns>Returns true if any of our summaries contain one of the supplied references 
        /// or a 3 word phrase from the given references</returns>
        private static bool BibleBookSummaryTest(BibleBooks book, string[] desiredRefs)
        {
            SummarizerImplementation[] implementations =
            {
                new SummarizerDW(),
                new SummarizerRR(),
                new SummarizerAS()
            };

            foreach (var implementation in implementations)
            {
                string summary = implementation.Summarize(Bible.Get(book).ToString());
                foreach (string reference in desiredRefs)
                {
                    // If summary contains at least one desired reference, it's a good summary
                    if (summary.Contains(reference))
                        return true;

                    // Build list of phrases
                    IList<string> phrases = new List<string>();
                    const int PHRASE_LENGTH = 3;
                    int numWords = 0;
                    StringBuilder sb = new StringBuilder();
                    foreach (string word in Bible.Get(book)[reference].ToString().Split(' '))
                    {
                        if (numWords == PHRASE_LENGTH)
                        {
                            phrases.Add(sb.ToString());
                            sb.Clear();
                            numWords = 0;
                        }
                        else
                        {
                            sb.Append(word + " ");
                            numWords++;
                        }
                    }

                    // Check to see if summary contains any phrases
                    foreach (string phrase in phrases)
                    {
                        // If the summary contains at least one phrase, it's a good summary
                        if (summary.Contains(phrase))
                            return true;
                    }
                }
            }
            return false;
        }

        /* OLD TEST
        [TestMethod]
        public void AccuracyTest()
        {
            string desiredRuthSummary = 
                "1:6 Then she arose with her daughters in law, that she might return from the country of Moab: " +
                "for she had heard in the country of Moab how that the LORD had visited his people in giving " +
                "them bread.  " +
                "1:22 So Naomi returned, and Ruth the Moabitess, her daughter in law, with her, which " +
                "returned out of the country of Moab: and they came to Bethlehem in the beginning of barley " +
                "harvest.  " +
                "2:15 And when she was risen up to glean, Boaz commanded his young men, saying, " +
                "Let her glean even among the sheaves, and reproach her not:  2:16 And let fall also " +
                "some of the handfuls of purpose for her, and leave them, that she may glean them, and " +
                "rebuke her not." +
                "3:11 And now, my daughter, fear not; I will do to thee all that thou requirest: " +
                "for all the city of my people doth know that thou art a virtuous woman. " +
                "3:13 Tarry this night, and it shall be in the morning, that if he will perform " +
                "unto thee the part of a kinsman, well; let him do the kinsman's part: but if he " +
                "will not do the part of a kinsman to thee, then will I do the part of a kinsman to " +
                "thee, as the LORD liveth: lie down until the morning. " +
                "4:17 And the women her neighbours gave it a name, saying, There is a son born " +
                "to Naomi; and they called his name Obed: he is the father of Jesse, the father of David.";
            string desiredGalSummary = 
                "1:9 As we said before, so say I now again, if any man preach any " +
                "other gospel unto you than that ye have received, let him be accursed. " +
                "2:20 I am crucified with Christ: neverthless I live; " +
                "yet not I, but Christ liveth in me: and the " +
                "life which I now live in the flesh I live by the faith of the " +
                "Son of God, who loved me, and gave himself for me." +
                "3:1 O foolish Galatians, who hath bewitched you, that ye should not obey the " +
                "truth, before whose eyes Jesus Christ hath been evidently set forth, crucified among you? " +
                "3:2 This only would I learn of you, Received ye the Spirit by " +
                "the works of the law, or by the hearing of faith? " +
                "3:3 Are ye so foolish? having begun in the Spirit, are ye now made perfect by the flesh? " +
                "5:16 This I say then, Walk in the Spirit, and ye shall not fulfil the lust of the flesh.  ";
            string desiredJamesSummary = "Blessed is the man that endureth temptation: for when " +
                "he is tried, he shall reveive the crown of life, which the Lord hath promised " +
                "to them that love him. But be ye doers of the word, and not hearers only, " +
                "deceiving your own selves. For whosoever shall keep the whole law, and yet offend " +
                "in one point, he is guilty of all. But the wisdom that is from above is first " +
                "pure, then peaceable, gentle, and easy to be intreated, full of mercy and good " +
                "fruits, without partiality, and without hypocrisy. Submmit yourselves therefore " +
                "to God. Brethren, if any of you do err from the truth, and one converts him; Let " +
                "him know, that he which converteth the sinner from the error of his way shall " +
                "save a soul from death, and shall hide a multitude of sins.";

            SummarizerImplementation[] implementations =
            {
                new SummarizerDW(),
                new SummarizerRR(),
                new SummarizerAS()
            };

            IDictionary<BibleBooks, int> highestScores = new Dictionary<BibleBooks, int>()
            {
                { BibleBooks.Galatians, 0 },
                { BibleBooks.Ruth, 0 },
                { BibleBooks.James, 0 }
            };
            foreach (var implementation in implementations)
            {
                string summary = implementation.Summarize(Bible.Get(BibleBooks.Galatians).ToString());
                int score = CompareSummary(desiredGalSummary, summary);
                if (score > highestScores[BibleBooks.Galatians])
                    highestScores[BibleBooks.Galatians] = score;

                summary = implementation.Summarize(Bible.Get(BibleBooks.Ruth).ToString());
                score = CompareSummary(desiredRuthSummary, summary);
                if (score > highestScores[BibleBooks.Ruth])
                    highestScores[BibleBooks.Ruth] = score;

                summary = implementation.Summarize(Bible.Get(BibleBooks.James).ToString());
                score = CompareSummary(desiredJamesSummary, summary);
                if (score > highestScores[BibleBooks.James])
                    highestScores[BibleBooks.James] = score;
            }

            StringBuilder sb = new StringBuilder();
            foreach(var kv in highestScores)
            {
                sb.AppendLine(string.Format("{0}: {1}", kv.Key.GetDescription(), kv.Value));
            }
            ErrorHandler.ReportError(sb.ToString());
        }

        private int CompareSummary(string desired, string summary)
        {
            const int REF_MULTIPLIER = 5;

            // DETERMINE NUMBER OF MATCHING REFERENCES
            string refPattern = @"[\d]+:[\d]+";
            IList<string> summaryRefs = new List<string>();
            foreach (var match in Regex.Matches(summary, refPattern))
            {
                summaryRefs.Add(match.ToString());
            }

            int numRefMatches = 0;
            foreach (var match in Regex.Matches(desired, refPattern))
            {
                if (summaryRefs.Contains(match.ToString()))
                    numRefMatches++;
            }

            // DETERMINE NUMBER OF MATCHING PHRASES
            IList<string> phrases = new List<string>();
            const int PHRASE_LENGTH = 5;
            int numWords = 0;
            StringBuilder sb = new StringBuilder();
            foreach (string word in desired.Split(' '))
            {
                if (numWords == PHRASE_LENGTH)
                {
                    phrases.Add(sb.ToString());
                    sb.Clear();
                    numWords = 0;
                }
                else
                {
                    sb.Append(word + " ");
                    numWords++;
                }
            }

            int numPhraseMatches = 0;
            foreach (string phrase in phrases)
            {
                if (summary.Contains(phrase))
                    numPhraseMatches++;
            }

            return (REF_MULTIPLIER * numRefMatches) + numPhraseMatches;
        }
        */
    }
}
