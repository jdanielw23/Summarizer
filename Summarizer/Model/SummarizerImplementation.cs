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
        string SummarizeDocument(string filePath);
    }
}
