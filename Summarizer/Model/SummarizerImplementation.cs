using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model
{
    public enum SummarizerImplementations { Ryan, Andrew, Daniel }
    /// <summary>
    /// Created by J. Daniel Worthington
    /// An interface for summarization algorithm implementations
    /// </summary>
    public interface SummarizerImplementation
    {
        /// <summary>
        /// Takes a txt file and returns a summary of the document.
        /// </summary>
        /// <param name="originalText">The text to be summarized</param>
        /// <returns>Summary of the document</returns>
        string Summarize(string originalText);

        /// <summary>
        /// Takes the book name and chapter number of the Bible to Summarize
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="chapterNum"></param>
        /// <returns></returns>
        //string SummarizeBible(string bookName, int chapterNum);
    }
}
