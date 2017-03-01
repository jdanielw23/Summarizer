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
        private string summary;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public string Summary
        {
            get { return summary; }
            set { summary = value;
                NotifyPropertyChanged("Summary");
            }
        }

        public MainWindowViewModel()
        {
            //Summarizer = new SummarizerDW();
            //Summarizer = new SummarizerAS();
            //Summarizer = new SummarizerRR();

            Summary = "";
        }

        /// <summary>
        /// This function opens the Windows OpenFileDialog so that the user can select a document from anywhere on the file system.
        /// 
        /// Created by Daniel Worthington 10/15/2016
        /// </summary>
        public void SummarizeDocument()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Documents|*.docx;*.doc;*.txt";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // User selected a file
            if (result == true)
            {
                // Get the selected file name
                FilePath = dlg.FileName;

                int which = 3; // 1: Andrew's, 2: Daniel's, 3: Ryan's

                switch(which)
                {
                    case 1:
                        Summarizer = new SummarizerAS();
                        break;
                    case 2:
                        Summarizer = new SummarizerDW();
                        break;
                    case 3:
                        Summarizer = new SummarizerRR();
                        break;
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
                    Summary = Summarizer.SummarizeDocument(filePath);
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
