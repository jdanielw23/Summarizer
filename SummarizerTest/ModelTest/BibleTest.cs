using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Utils;
using System.Linq;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class BibleTest
    {
        [TestMethod]
        public void BuildBibleTest()
        {
            string genesis1 = Constants.Bible["Genesis"][1].Values.ToList().ListData("");
            ErrorHandler.ReportError(genesis1);
        }
    }
}
