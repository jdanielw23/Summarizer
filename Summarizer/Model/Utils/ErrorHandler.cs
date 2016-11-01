using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Summarizer.Model.Utils
{
    public static class ErrorHandler
    {
        public static void ReportError(string s)
        {
            MessageBox.Show(s, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
