using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Library to use R in C#
using RDotNet;


using System.Windows;

// This class handles text analysis of a given document. It builds a word-frequency
// table and attempts
// to create a summary of the document passed in.

namespace Summarizer.Model.Utils
{
    class RTextAnalyzer
    {
        REngine engine = null;

        public RTextAnalyzer()
        {
            REngine.SetEnvironmentVariables();
            engine = REngine.GetInstance();
            engine.Initialize();           
        }

        ~RTextAnalyzer()
        {
            engine.Dispose();
        }

        public void test(string filepath)
        {
            MessageBox.Show("Analysis can begin", "TextAnalysis",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);

            engine.Evaluate("cname <- file.path(\"C:\", \"texts\")");
            engine.Evaluate("cname");
            engine.Evaluate("dir(cname)");

            engine.Evaluate("library(tm)");
            engine.Evaluate("docs <- Corpus(DirSource(cname))");
            engine.Evaluate("summary(docs)");

            engine.Evaluate("docs <- tm_map(docs, removePunctuation)   ");
            engine.Evaluate("docs <- tm_map(docs, removeNumbers)   ");
            engine.Evaluate("docs <- tm_map(docs, tolower)   ");
            engine.Evaluate("docs <- tm_map(docs, removeWords, stopwords(\"english\"))   ");
            engine.Evaluate("docs <- tm_map(docs, stripWhitespace)   ");
            engine.Evaluate("docs <- tm_map(docs, PlainTextDocument)   ");
            engine.Evaluate("dtm <- DocumentTermMatrix(docs)");
            engine.Evaluate("tdm <- TermDocumentMatrix(docs)   ");
            engine.Evaluate("freq <- colSums(as.matrix(dtm))");
            engine.Evaluate("length(freq)   ");
            engine.Evaluate("ord <- order(freq)   ");

            engine.Evaluate("m <- as.matrix(dtm)   ");
            engine.Evaluate("dim(m)   ");
            engine.Evaluate("write.csv(m, file=\"dtm.csv\") ");
        }
    }
}
