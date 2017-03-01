using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Daniels_Implementation;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class SummaryTest
    {
        [TestMethod]
        public void AddToSummaryTest()
        {
            Summary summary = new Summary(3, 20);

            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "This is a long enough sentence.",
                Score = 100
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "Too Short.",
                Score = 400
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "This sentence is going to be way too long and I mean way too long like over 20 words long you know what I mean?",
                Score = 500
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "This is also a long enough sentence.",
                Score = 125
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "And this is a long enough sentence.",
                Score = 305
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "And this is also a long enough sentence.",
                Score = 50
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "This is sentences has too low of a score.",
                Score = 20
            });
            summary.AddToSummary(new SentenceScore()
            {
                Sentence = "This is the highest scoring sentence.",
                Score = 355
            });

            Assert.AreEqual(3, summary.NumSentences);
            Assert.AreEqual(355, summary[0].Score);
            Assert.AreEqual(305, summary[1].Score);
            Assert.AreEqual(125, summary[2].Score);
        }
    }
}
