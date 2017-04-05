using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Andrew_s_Implementation;
using Summarizer.Model.Andrews_Implementation;

using static System.Threading.Thread;

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
            int kkCount = bc.Count("k", "k");
            string top1 = bc.Table().Top(1)[0];

            // Assert
            Assert.AreEqual("k d", top1);
            Assert.AreEqual(0, kkCount);
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
            Sleep(1000);
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
            int size = t.Size();
            

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

        [TestMethod]
        public void MatrixTest()
        {
            // Arrange
            Matrix m = new Matrix(5, 5);
            Matrix dot = new Matrix(5, 1);
            int[,] matrix = new int[5, 5];
            int[,] trans = new int[5, 5];
            int[] product = { 40, 40 + 75, 40 + (75*2), 40 + (75*3), 40 + (75*4) };
            for (int i = 0; i < 5; i++)
            {
                int shift = i * 5;
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = shift + j;
                    m.Set(i, j, shift + j);
                    trans[j, i] = shift + j;
                }
            }

            // Act
            Matrix transpose = m.Transpose();
            dot.Set(0, 0, 1);
            dot.Set(1, 0, 2);
            dot.Set(2, 0, 3);
            dot.Set(3, 0, 4);
            dot.Set(4, 0, 5);
            Matrix dot_product = m.Dot(dot);

            // Assert
            Assert.AreEqual(1, dot.Get(0, 0));
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(product[i], dot_product.Get(i, 0));
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Assert.AreEqual(trans[i, j], transpose.Get(i, j));
                }
            }
        }

        [TestMethod]
        public void MonitorTest()
        {
            Assert.IsTrue(true);
            // This class does not require testing.
        }

        [TestMethod]
        public void ScorerTest()
        {
            // Arrange
            string[] sentences = {"a b c d e f g",
                                    "c k d x z",
                                    "j k l m n x z d b"};
            string[] constituents = { "k", "d"};
            Scorer s = new Scorer(sentences);
            BigramCounter bc = new BigramCounter(sentences, constituents);
            double sent2_score = 1;
            string top_sent = sentences[1];

            // Act
            s.ScoreWithComplexBigrams(bc); // This is the only scoring method used.
                                           // The others are therefore not tested here.
            double actual_score = s.ScoreOf(sentences[1]);
            string top1 = s.Top(1)[0];

            // Assert
            Assert.AreEqual(top_sent, top1);
            Assert.AreEqual(sent2_score, actual_score);
        }

        [TestMethod]
        public void SummarizerASTest()
        {
            // Arrange
            string doc = "Hello, this is a test. This is the first test of the moon-exploding device."
                            + " We will not perform this test again. Good luck, everyone. ";
            string long_doc = doc;
            string weird_doc = "Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha. Ha.";
            weird_doc += weird_doc + weird_doc;
            for (int i = 0; i < 100; i++)
            {
                long_doc += doc;
            }
            SummarizerAS summarizer = new SummarizerAS();

            // Act
            string actual1 = summarizer.Summarize(doc);
            string actual2 = summarizer.Summarize(long_doc);
            string actual3 = summarizer.Summarize(weird_doc);

            // Assert
            Assert.AreEqual("Less than 24 sentences in document.", actual1);
            Assert.AreNotEqual("Less than 24 sentences in document.", actual2);
            Assert.AreNotEqual("Not enough (<70%) valid sentences in document.", actual2);
            Assert.AreNotEqual("File is empty.", actual2);
            Assert.AreEqual("Not enough (<70%) valid sentences in document.", actual3);
        }

        [TestMethod]
        public void TableTest()
        {
            // Arrange
            Table t = new Table();
            int[] values = { 1, 2, 24, 3, 4, 5, 6 };
            string[] keys = { "one", "two", "hello", "three", "four", "five", "six" };

            // Act
            for (int i = 0; i < keys.Length; i++)
            {
                t.Add(keys[i], values[i]);
            }
            int size = t.Size();


            // Assert
            Assert.IsFalse(t.Has("pig"));
            Assert.IsTrue(t.Has("one"));
            t.Remove("one");
            Assert.IsFalse(t.Has("one"));
            Assert.AreEqual(keys.Length, size);
            Assert.AreEqual(2, t.ValueOf("two"));
            Assert.AreNotEqual(2, t.ValueOf("three"));
            Assert.AreEqual("hello", t.Top(1)[0]);
        }

        [TestMethod]
        public void TestTest()
        {
            Assert.IsTrue(true);
            // This class does not require testing.
        }

        [TestMethod]
        public void WordFrequenciesTest()
        {
            // Arrange
            string text = "a b b c c c d d d d e e e e e f f f f f f"; // 1, 2, 3, 4, 5, 6...
            string[] text_arr = text.Split(' ');
            string expected_top3 = "fed";
            int expected_c_count = 3;

            // Act
            WordFrequencies wf = new WordFrequencies(text);
            WordFrequencies arr_wf = new WordFrequencies(text_arr);

            // Assert
            Assert.AreEqual(expected_top3, string.Join("", wf.Top(3)));
            Assert.AreEqual(expected_top3, string.Join("", arr_wf.Top(3)));
            Assert.AreEqual(expected_c_count, wf.Count("c"));
            Assert.AreEqual(expected_c_count, arr_wf.Count("c"));
            Assert.AreEqual(string.Join("", wf.Top(6)), string.Join("", arr_wf.Top(6)));
        }
    }
}
