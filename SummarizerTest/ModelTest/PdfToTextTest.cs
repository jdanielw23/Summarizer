using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Utils;
using System.IO;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class PdfToTextTest
    {
        [TestMethod]
        public void ExtractTextFromPdfTest()
        {
            DirectoryInfo dir = new DirectoryInfo(@"PdfTestFiles\");

            foreach (var file in dir.GetFiles("*.pdf"))
            {
                string result = PdfToText.ExtractTextFromPdf(file.FullName);
                System.IO.File.WriteAllText(@"Results\" + file.Name.Split('.')[0] + ".txt", result);
            }
        }
    }
}
