using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.Model.Utils
{
    public static class DocumentHelper
    {        
        public static DocX TrySave(this DocX document)
        {
            try
            {
                document.Save();
            }
            catch (Exception)
            {
                ErrorHandler.ReportError("ERROR: The File may already be open.\n\nPlease close it and try again.");
            }
            return document;
        }
        
    }
}
