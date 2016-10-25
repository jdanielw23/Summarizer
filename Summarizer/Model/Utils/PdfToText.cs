using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace Summarizer.Model.Utils
{
    public static class PdfToText
    {
        public static string ExtractTextFromPdf(string path)
        {
            PDDocument doc = null;
            try
            {
                doc = PDDocument.load(path);
                PDFTextStripper stripper = new PDFTextStripper();
                return stripper.getText(doc);
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }
    }
}
