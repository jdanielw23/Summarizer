using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Summarizer.Model.Utils;
using System.IO;
using Novacode;

namespace SummarizerTest.ModelTest
{
    [TestClass]
    public class PdfToTextTest
    {
        [TestMethod]
        public void ExtractTextFromPdfTest()
        {
            DirectoryInfo dir = new DirectoryInfo(@"D:\My Libraries\My Projects\Visual Studio 2015\Summarizer\Summarizer\Documents");

            foreach (var file in dir.GetFiles("*.pdf"))
            {
                string result = PdfToText.ExtractTextFromPdf(file.FullName);
                string filename = @"Results\" + file.Name.Split('.')[0] + ".docx";

                using (DocX document = DocX.Create(filename))
                {
                    document.InsertParagraph(result);
                    document.TrySave();
                }
            }
        }
    }
}
