using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Daniels_Implementation;
using System.Linq;
using Summarizer.Model.Utils;

namespace SummarizerTest.ModelTest
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// Tests the custom Bigram data structure
    /// </summary>
    [TestClass]
    public class BigramTest
    {
        [TestMethod]
        public void AddToMatrixTest()
        {
            string text =
                "Peter Piper picked a peck of pickled peppers. " +
                "A peck of pickled peppers Peter Piper picked. " +
                "If Peter Piper picked a peck of pickled peppers, " +
                "Where's the peck of pickled peppers Peter Piper picked?";

            string[] sentences = SummarizerDW.SplitIntoSentences(text);

            Bigram bigram = new Bigram();
            for (int index = 0; index < sentences.Length; index++)
            {
                string prevWord = "";
                foreach (string rawWord in sentences[index].Split(' '))
                {
                    string word = rawWord.ToLower().Trim().TrimEnd('.', '?', ',');
                    bigram.AddToMatrix(prevWord, word, index);
                    prevWord = word;
                }
            }

            // Check that text was split correctly into 3 sentences
            Assert.AreEqual(3, sentences.Length);
            // Check frequency of all pairs of words
            Assert.AreEqual(4, bigram.FinalWordFrequency["peter piper"].Frequency);
            Assert.AreEqual(4, bigram.FinalWordFrequency["piper picked"].Frequency);
            Assert.AreEqual(2, bigram.FinalWordFrequency["picked a"].Frequency);
            Assert.AreEqual(3, bigram.FinalWordFrequency["a peck"].Frequency);
            Assert.AreEqual(4, bigram.FinalWordFrequency["peck of"].Frequency);
            Assert.AreEqual(4, bigram.FinalWordFrequency["of pickled"].Frequency);
            Assert.AreEqual(4, bigram.FinalWordFrequency["pickled peppers"].Frequency);
            Assert.AreEqual(2, bigram.FinalWordFrequency["peppers peter"].Frequency);
            Assert.AreEqual(1, bigram.FinalWordFrequency["if peter"].Frequency);
            Assert.AreEqual(1, bigram.FinalWordFrequency["peppers where's"].Frequency);
            Assert.AreEqual(1, bigram.FinalWordFrequency["where's the"].Frequency);
            Assert.AreEqual(1, bigram.FinalWordFrequency["the peck"].Frequency);
            Assert.AreEqual(12, bigram.FinalWordFrequency.Count);

            // Check that each pair contains exactly the right sentence indexes
            Assert.IsTrue(bigram.FinalWordFrequency["peter piper"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["piper picked"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["picked a"].Locations.ContainsOnly(0, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["a peck"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["peck of"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["of pickled"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["pickled peppers"].Locations.ContainsOnly(0, 1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["peppers peter"].Locations.ContainsOnly(1, 2));
            Assert.IsTrue(bigram.FinalWordFrequency["if peter"].Locations.ContainsOnly(2));
            Assert.IsTrue(bigram.FinalWordFrequency["peppers where's"].Locations.ContainsOnly(2));
            Assert.IsTrue(bigram.FinalWordFrequency["where's the"].Locations.ContainsOnly(2));
            Assert.IsTrue(bigram.FinalWordFrequency["the peck"].Locations.ContainsOnly(2));

        }
    }
}
