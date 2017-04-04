using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Ryan_s_Implementation;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class DocumentTestRR
    {
        [TestMethod]
        public void TestDocumentGetSentenceCount()
        {
            // Constant strings for testing the text.
            const string sentence1 = "Lorem ipsum.";
            const string text = sentence1 + " This is just text to count? The number of sentences. There should be four sentences in this text. Actually 5!";

            // The second tells it whether the text is a file location or not.
            Document doc1 = new Document(text, false);

            // Make sure it meets the expected conditions.
            Assert.AreEqual(5, doc1.getSentenceCount());
        }

        [TestMethod]
        public void TestDocumentGetSentence()
        {
            // Constant strings for testing the text.
            const string sentence1 = "Lorem ipsum.";
            const string sentence2 = "This is just text to count.";
            const string sentence3 = "The number of sentences!?";
            const string sentence4 = "There should be four sentences; in this text?";
            const string sentence5 = "Actually 5!";

            const string text = sentence1 + " " + sentence2 + " " + sentence3 + " " + sentence4 + " " + sentence5;

            // The second tells it whether the text is a file location or not.
            Document doc1 = new Document(text, false);

            // Make sure it meets the expected conditions.
            Assert.AreEqual(sentence1, doc1.getSentence(0));
            Assert.AreEqual(sentence2, doc1.getSentence(1));
            Assert.AreEqual(sentence3, doc1.getSentence(2));
            Assert.AreEqual(sentence4, doc1.getSentence(3));
            Assert.AreEqual(sentence5, doc1.getSentence(4));
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "An index of -1 was allowed.")]
        public void TestDocumentGetSentenceRangeUnderflow()
        {
            // Constant strings for testing the text.
            const string sentence1 = "Lorem ipsum.";
            const string sentence2 = "This is just text to count.";
            const string sentence3 = "The number of sentences!?";
            const string sentence4 = "There should be four sentences; in this text?";
            const string sentence5 = "Actually 5!";

            const string text = sentence1 + " " + sentence2 + " " + sentence3 + " " + sentence4 + " " + sentence5;

            // The second tells it whether the text is a file location or not.
            Document doc1 = new Document(text, false);
            
            string temp = doc1.getSentence(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException), "An index of 10000 was allowed.")]
        public void TestDocumentGetSentenceRangeOverflow()
        {
            // Constant strings for testing the text.
            const string sentence1 = "Lorem ipsum.";
            const string sentence2 = "This is just text to count.";
            const string sentence3 = "The number of sentences!?";
            const string sentence4 = "There should be four sentences; in this text?";
            const string sentence5 = "Actually 5!";

            const string text = sentence1 + " " + sentence2 + " " + sentence3 + " " + sentence4 + " " + sentence5;

            // The second tells it whether the text is a file location or not.
            Document doc1 = new Document(text, false);

            string temp = doc1.getSentence(10000);
        }
    }
}
