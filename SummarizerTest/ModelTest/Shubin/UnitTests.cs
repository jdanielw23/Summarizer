using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Andrew_s_Implementation;
using System.Threading;

namespace SummarizerTest.ModelTest.Shubin
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void BigramCounterTest()
        {
            // Arrange
            string[] sentences = {"a b c d e f g",
                                    "c k d x z",
                                    "j k l m n x z d b"};
            string[] constituents = { "k", "d"};
            BigramCounter bc = new BigramCounter(sentences, constituents);

            // Act
            int kdCount = bc.Count("k", "d");
            string top1 = bc.Table().Top(1)[0];

            // Assert
            Assert.AreEqual("k d", top1);
            Assert.AreEqual(2, kdCount);
        }

        [TestMethod]
        public void CleanerTest()
        {
            // Arrange
            Cleaner cleaner = new Cleaner();
            string dirty = ".$5chrysanthymum is blues     ";
            string clean = "chrysanthymum blue";
            string not_clean1 = clean + "s";
            string not_clean2 = "chrysanthymum is blue";
            string not_clean3 = clean + "@#$@";
            string not_clean4 = clean + "234";

            // Act
            string actual = cleaner.clean(dirty);

            // Assert
            Assert.AreEqual(clean, actual);
            Assert.AreNotEqual(not_clean1, actual);
            Assert.AreNotEqual(not_clean2, actual);
            Assert.AreNotEqual(not_clean3, actual);
            Assert.AreNotEqual(not_clean4, actual);
        }

        [TestMethod]
        public void ClockTest()
        {
            // Arrange
            string expected = "Time elapsed was 00:00:01.0";
            Clock clock = new Clock();

            // Act
            Thread.Sleep(1000);
            string actual = clock.query().Substring(0, expected.Length);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DTableTest()
        {
            // Arrange
            DTable t = new DTable();
            double[] values = { 1.1, 1.2, 24.2, 1.3, 1.4, 1.5, 1.6 };
            string[] keys = { "one", "two", "hello", "three", "four", "five", "six" };

            // Act
            for (int i = 0; i < keys.Length; i++)
            {
                t.Add(keys[i], values[i]);
            }
            double size = t.Size();
            

            // Assert
            Assert.IsFalse(t.Has("pig"));
            Assert.IsTrue(t.Has("one"));
            t.Remove("one");
            Assert.IsFalse(t.Has("one"));
            Assert.AreEqual(keys.Length, size);
            Assert.AreEqual(1.2, t.ValueOf("two"));
            Assert.AreNotEqual(1.2, t.ValueOf("three"));
            Assert.AreEqual("hello", t.Top(1)[0]);
        }

        [TestMethod]
        public void IndexFinderTest()
        {
            // Arrange
            string[] source = { "zero", "one", "two", "three" };
            IndexFinder finder = new IndexFinder(source);
            int expected = 2;

            // Act
            int actual = finder.Index("two");

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreNotEqual(1, actual);
        }
    }
}
