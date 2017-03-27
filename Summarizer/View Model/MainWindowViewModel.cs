using Summarizer.Model.Utils;
using Summarizer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Summarizer.Model.Andrews_Implementation;
using Summarizer.Model.Daniels_Implementation;
using Summarizer.Model.Ryan_s_Implementation;

namespace Summarizer.View_Model
{
    /// <summary>
    /// Interaction logic for MainWindow
    /// 
    /// Created by Daniel Worthington 10/15/2016
    /// </summary>
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private SummarizerImplementation Summarizer;

        private string filePath;
        private string originalText;
        private string summary;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public string OriginalText
        {
            get { return originalText; }
            set { originalText = value;
                NotifyPropertyChanged("OriginalText");
                NotifyPropertyChanged("OriginalTextWordCount");
                NotifyPropertyChanged("PercentReduction");
            }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value;
                NotifyPropertyChanged("Summary");
                NotifyPropertyChanged("SummaryWordCount");
                NotifyPropertyChanged("PercentReduction");
            }
        }

        // READ-ONLY PROPERTIES
        public int OriginalTextWordCount
        { get { return OriginalText.Split(' ').Length; } }

        public int SummaryWordCount
        { get { return Summary.Split(' ').Length; } }

        public double PercentReduction
        { get { return ((double)OriginalTextWordCount - (double)SummaryWordCount) / (double)OriginalTextWordCount; } }

        // CONSTRUCTOR
        public MainWindowViewModel()
        {
            OriginalText = "";
            Summary = "";
        }

        /// <summary>
        /// This function opens the Windows OpenFileDialog so that the user can
        /// select a document from anywhere on the file system.
        /// 
        /// Created by Daniel Worthington 10/15/2016
        /// </summary>
        public void SummarizeDocument()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Documents|*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // User selected a file
            if (result == true)
            {
                // Get the selected file name
                FilePath = dlg.FileName;

                SummarizerImplementations which = SummarizerImplementations.Daniel;

                switch(which)
                {
                    case SummarizerImplementations.Andrew:
                        Summarizer = new SummarizerAS();
                        break;
                    case SummarizerImplementations.Daniel:
                        Summarizer = new SummarizerDW();
                        break;
                    //case SummarizerImplementations.Ryan:
                    //    Summarizer = new SummarizerRR();
                    //    break;
                    default:
                        Summarizer = null;
                        break;
                }
                if (Summarizer == null)
                {
                    Summary = "";
                }
                else
                {
                    OriginalText = System.IO.File.ReadAllText(filePath);
                    Summary = Summarizer.Summarize(OriginalText);
                }

            }
        }


        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
