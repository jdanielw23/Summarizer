using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Utils;
using System.Collections.Generic;
using System.Linq;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class ExtensionMethodsTest
    {
        [TestMethod]
        public void CapitalizeTest()
        {
            string text1 = "this is the first Sentence.";
            string text2 = "1:1 this one starts with a verse.";
            string text3 = "This one is already capitalized.";

            Assert.AreEqual("This is the first Sentence.", text1.Capitalize());
            Assert.AreEqual("1:1 This one starts with a verse.", text2.Capitalize());
            Assert.AreEqual(text3, text3.Capitalize());
        }

        [TestMethod]
        public void ToTitleCaseTest()
        {
            string text1 = "this is the first Sentence.";
            string text2 = "1:1 this one starts with a verse.";
            string text3 = "This One Is Already Capitalized.";

            Assert.AreEqual("This Is The First Sentence.", text1.ToTitleCase());
            Assert.AreEqual("1:1 This One Starts With A Verse.", text2.ToTitleCase());
            Assert.AreEqual(text3, text3.ToTitleCase());
        }

        [TestMethod]
        public void ContainsLowerCaseLetterTest()
        {
            string text1 = "TH#IS DO%ES NOT1234/";
            string text2 = "tHIS STAR$TS WITH ONE";
            string text3 = "THIS EN!DS WITH ONe";
            string text4 = "THIS ONE Is IN THE MIDDLE.";
            string text5 = "this is all lower.s$)#1";

            Assert.IsFalse(text1.ContainsLowerCaseLetter());
            Assert.IsTrue(text2.ContainsLowerCaseLetter());
            Assert.IsTrue(text3.ContainsLowerCaseLetter());
            Assert.IsTrue(text4.ContainsLowerCaseLetter());
            Assert.IsTrue(text5.ContainsLowerCaseLetter());
        }

        [TestMethod]
        public void IsLetterTest()
        {
            char[] notLetters = { '1', '2', '!', '#', '%', '/', '+', '=', '{', '(', '_' };
            char[] letters = { 'a', 'b', 'c','e','F','J','I','K','q','Q','z','Z' };

            foreach (char c in notLetters)
            {
                Assert.IsFalse(c.IsLetter());
            }
            foreach (char c in letters)
            {
                Assert.IsTrue(c.IsLetter());
            }
        }

        [TestMethod]
        public void IsUpperTest()
        {
            char[] upper = { 'A', 'B', 'C', 'D', 'Z' };
            char[] notUpper = { 'a', 'z', '1', '$' };

            foreach (char c in upper)
            {
                Assert.IsTrue(c.IsUpper());
            }
            foreach (char c in notUpper)
            {
                Assert.IsFalse(c.IsUpper());
            }
        }

        [TestMethod]
        public void ContainsOnlyLettersTest()
        {
            string text1 = "1:1 not only Letters.";
            string text2 = "OnlyLetters";
            string text3 = "NOT!only%Letter$";
            string text4 = "not only letters.";
            string text5 = "ThisoneisNicEanDLONGandISonlyLetters";

            Assert.IsFalse(text1.ContainsOnlyLetters());
            Assert.IsTrue(text2.ContainsOnlyLetters());
            Assert.IsFalse(text3.ContainsOnlyLetters());
            Assert.IsFalse(text4.ContainsOnlyLetters());
            Assert.IsTrue(text5.ContainsOnlyLetters());
        }

        [TestMethod]
        public void ListDataTest()
        {
            IList<string> list = new List<string>();
            list.Add("Dog");
            list.Add("Cat");
            list.Add("Mouse");
            list.Add("Possum");
            list.Add("Nest of Spiders");
            
            Assert.AreEqual("Dog\nCat\nMouse\nPossum\nNest of Spiders", list.ListData());
            Assert.AreEqual("Dog Cat Mouse Possum Nest of Spiders", list.ListData(" "));
            Assert.AreEqual("Dog, Cat, Mouse, Possum, Nest of Spiders", list.ListData(", "));
        }

        [TestMethod]
        public void ContainsTest()
        {
            int[] ints = { 1, 2, 4, 5, 6, 7, 8 };

            Assert.IsTrue(ints.ToList().Contains(1, 2, 5, 8));
            Assert.IsTrue(ints.ToList().Contains(1, 8, 6));
            Assert.IsTrue(ints.ToList().Contains(7, 1, 2, 5, 8, 5));
            Assert.IsTrue(ints.ToList().Contains(4, 2, 8, 6, 1, 7, 5));

            Assert.IsFalse(ints.ToList().Contains(4, 2, 3, 6));
            Assert.IsFalse(ints.ToList().Contains(3, 9, 10));
            Assert.IsFalse(ints.ToList().Contains(1, 2, 3, 6));
            Assert.IsFalse(ints.ToList().Contains(8, 9, 4, 2));
        }

        [TestMethod]
        public void ContainsOnlyTest()
        {
            int[] ints = { 1, 2, 4, 5, 6, 7, 8 };
            
            Assert.IsTrue(ints.ToList().ContainsOnly(4, 2, 8, 6, 1, 7, 5));
            Assert.IsTrue(ints.ToList().ContainsOnly(1, 2, 4, 5, 6, 7, 8));
            Assert.IsFalse(ints.ToList().ContainsOnly(1, 2, 3, 4, 5, 6, 7, 8));
            Assert.IsFalse(ints.ToList().ContainsOnly(1, 2, 3, 5, 6, 7, 8));
            Assert.IsFalse(ints.ToList().ContainsOnly(1, 2, 3, 5, 6, 7));
        }
    }
}
