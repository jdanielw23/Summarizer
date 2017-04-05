using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Utils;
using System.Linq;

namespace SummarizerTest.ModelTest
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// A class to test the Bible structure
    /// </summary>
    [TestClass]
    public class BibleTest
    {
        [TestMethod]
        public void AccessBibleTest()
        {
            string genesis1 = Bible.Get(BibleBooks.Genesis)[1].ToString().Trim();
            string exodus4 = Bible.Get(BibleBooks.Exodus)[4].ToString().Trim();
            string job13_20 = Bible.Get(BibleBooks.Job)[13][20].ToString().Trim();
            string matt12_1 = Bible.Get(BibleBooks.Matthew)["12:1"].ToString().Trim();
            string rev = Bible.Get(BibleBooks.Revelation).ToString().Trim();

            Assert.IsTrue(genesis1.StartsWith("1:1 In the beginning God created the heaven and the earth."));
            Assert.IsTrue(genesis1.EndsWith("1:31 And God saw every thing that he had made, and, behold, it was very good. And the evening and the morning were the sixth day."));

            Assert.IsTrue(exodus4.StartsWith("4:1 And Moses answered and said, But, behold, they will not believe me, nor hearken unto my voice: for they will say, The LORD hath not appeared unto thee."));
            Assert.IsTrue(exodus4.EndsWith("4:31 And the people believed: and when they heard that the LORD had visited the children of Israel, and that he had looked upon their affliction, then they bowed their heads and worshipped."));

            Assert.IsTrue(job13_20.Equals("13:20 Only do not two things unto me: then will I not hide myself from thee."));

            Assert.IsTrue(matt12_1.Equals("12:1 At that time Jesus went on the sabbath day through the corn; and his disciples were an hungred, and began to pluck the ears of corn and to eat."));

            Assert.IsTrue(rev.StartsWith("1:1 The Revelation of Jesus Christ, which God gave unto him, to shew unto his servants things which must shortly come to pass; and he sent and signified it by his angel unto his servant John: "));
            Assert.IsTrue(rev.EndsWith("22:21 The grace of our Lord Jesus Christ be with you all. Amen."));
        }

        [TestMethod]
        public void IterationTest()
        {
            foreach (string verse in Bible.Get(BibleBooks.Galatians)) { }
            foreach (string verse in Bible.Get(BibleBooks.Revelation)[1]) { }
        }
    }
}
