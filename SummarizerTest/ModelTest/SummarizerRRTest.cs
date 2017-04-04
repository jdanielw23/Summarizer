using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Ryan_s_Implementation;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class SummarizerRRTest
    {
        [TestMethod]
        public void TestSummarizerTooShort()
        {
            SummarizerRR summarizer = new SummarizerRR();     
            string summarizerTooShort = summarizer.Summarize("This text is way too short.");
            Assert.Equals(summarizerTooShort, SummarizerRR.TooShortMessage);
        }
    }
}
