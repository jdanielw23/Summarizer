using Summarizer.Model;
using Summarizer.Model.Andrews_Implementation;
using Summarizer.Model.Daniels_Implementation;
using Summarizer.Model.Ryan_s_Implementation;
using Summarizer.Model.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summarizer.View_Model
{
    public class BibleMainWindowVM : INotifyPropertyChanged
    {
        private enum Implementation { Ryan, Andrew, Daniel }

        private string selectedBook;
        private int selectedChapter;
        private string ryanSummary = "";
        private string andrewSummary = "";
        private string danielSummary = "";
        private string originalText = "";

        public Collection<int> ChapterNums { get; private set; }
        public List<string> ChapterNames { get; private set; }

        public string RyanSummary
        {
            get { return ryanSummary; }
            set
            {
                ryanSummary = value;
                NotifyPropertyChanged("RyanSummary");
                NotifyPropertyChanged("RyanSummaryWordCount");
            }
        }
        public string AndrewSummary
        {
            get { return andrewSummary; }
            set
            {
                andrewSummary = value;
                NotifyPropertyChanged("AndrewSummary");
                NotifyPropertyChanged("AndrewSummaryWordCount");
            }
        }
        public string DanielSummary
        {
            get { return danielSummary; }
            set
            {
                danielSummary = value;
                NotifyPropertyChanged("DanielSummary");
                NotifyPropertyChanged("DanielSummaryWordCount");
            }
        }
        public string OriginalText
        {
            get { return originalText; }
            set
            {
                originalText = value;
                NotifyPropertyChanged("OriginalText");
                NotifyPropertyChanged("OriginalTextWordCount");
            }
        }

        public string SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                ResetChapterNumbers(Constants.BibleBooks[selectedBook]);
                NotifyPropertyChanged("SelectedBook");
            }
        }
        public int SelectedChapter
        {
            get { return selectedChapter; }
            set { selectedChapter = value;
                NotifyPropertyChanged("SelectedChapter");
            }
        }

        // READ ONLY PROPERTIES
        public int OriginalTextWordCount
        { get { return OriginalText.Split(' ').Length; } }

        public int RyanSummaryWordCount
        { get { return RyanSummary.Split(' ').Length; } }

        public int AndrewSummaryWordCount
        { get { return AndrewSummary.Split(' ').Length; } }

        public int DanielSummaryWordCount
        { get { return DanielSummary.Split(' ').Length; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public BibleMainWindowVM()
        {
            ChapterNames = Constants.BibleBooks.Keys.ToList();
            ChapterNums = new ObservableCollection<int>();
            SelectedBook = "Genesis";
            SelectedChapter = 1;

            SummarizeChapter();
        }

        /// <summary>
        /// Called when user clicks GO
        /// </summary>
        public void SummarizeChapter()
        {
            OriginalText = Constants.Bible[SelectedBook][SelectedChapter].Values.ToList().ListData("");

            SummarizeUsing(new SummarizerDW());
            //SummarizeUsing(new SummarizerRR());
            //SummarizeUsing(new SummarizerAS());
        }


        /***************************************************/
        /*************     PRIVATE METHODS     *************/
        /***************************************************/
        private void ResetChapterNumbers(int numChapters)
        {
            ChapterNums.Clear();

            for (int i = 1; i <= numChapters; i++)
            {
                ChapterNums.Add(i);
            }
            SelectedChapter = ChapterNums[0];
        }

        private void SummarizeUsing(SummarizerImplementation implementation)
        {
            if (implementation.GetType().Equals(typeof(SummarizerRR)))
            {
                RyanSummary = "Working...";
            }
            else if (implementation.GetType().Equals(typeof(SummarizerAS)))
            {
                AndrewSummary = "Working...";
            }
            else if (implementation.GetType().Equals(typeof(SummarizerDW)))
            {
                DanielSummary = "Working...";
            }

            string summary = "";
            BackgroundWorker bw = new BackgroundWorker();

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                summary = implementation.SummarizeBible(SelectedBook, SelectedChapter);
            });

            if (implementation.GetType().Equals(typeof(SummarizerRR)))
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    RyanSummary = summary;
                });
            }
            else if (implementation.GetType().Equals(typeof(SummarizerAS)))
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    AndrewSummary = summary;
                });
            }
            else if (implementation.GetType().Equals(typeof(SummarizerDW)))
            {
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                delegate (object o, RunWorkerCompletedEventArgs args)
                {
                    DanielSummary = summary;
                });
            }
            bw.RunWorkerAsync();
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
