using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model
{
    /// <summary>
    /// Created by J. Daniel Worthington
    /// An interface for summarization algorithm implementations
    /// </summary>
    public interface SummarizerImplementation
    {
        /// <summary>
        /// Takes a txt file and returns a summary of the document.
        /// </summary>
        /// <param name="filePath">Full file path to the txt document to be summarized</param>
        /// <returns>Summary of the document</returns>
        string SummarizeFile(string filePath);

        /// <summary>
        /// Takes the book name and chapter number of the Bible to Summarize
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="chapterNum"></param>
        /// <returns></returns>
        string SummarizeBible(string bookName, int chapterNum);
    }
}
