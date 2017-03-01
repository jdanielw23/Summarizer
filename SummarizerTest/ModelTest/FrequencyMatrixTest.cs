using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Daniels_Implementation;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class FrequencyMatrixTest
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

            FrequencyMatrix frequency = new FrequencyMatrix();
            for (int index = 0; index < sentences.Length; index++)
            {
                foreach (string rawWord in sentences[index].Split(' '))
                {
                    string word = rawWord.ToLower().Trim().TrimEnd('.', '?', ',');
                    frequency.AddToMatrix(word, index);
                }
            }

            Assert.AreEqual(4, frequency["peter"].Frequency);
            Assert.AreEqual(4, frequency["piper"].Frequency);
            Assert.AreEqual(4, frequency["picked"].Frequency);
            Assert.AreEqual(3, frequency["a"].Frequency);
            Assert.AreEqual(4, frequency["peck"].Frequency);
            Assert.AreEqual(4, frequency["of"].Frequency);
            Assert.AreEqual(4, frequency["pickled"].Frequency);
            Assert.AreEqual(4, frequency["peppers"].Frequency);
            Assert.AreEqual(1, frequency["if"].Frequency);
            Assert.AreEqual(1, frequency["where's"].Frequency);
            Assert.AreEqual(1, frequency["the"].Frequency);
            Assert.AreEqual(11, frequency.Matrix.Count);
        }
    }
}
