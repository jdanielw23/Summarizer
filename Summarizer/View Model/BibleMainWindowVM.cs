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
        private const int AllChapters = 0;

        private BibleBooks selectedBook;
        private int selectedChapter;
        private string ryanSummary = "";
        private string andrewSummary = "";
        private string danielSummary = "";

        public Collection<string> ChapterNums { get; private set; }

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

        public BibleBooks SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                ResetChapterNumbers(Bible.BookChapters[selectedBook]);
                NotifyPropertyChanged("SelectedBook");
                NotifyPropertyChanged("OriginalText");
            }
        }
        public int SelectedChapter
        {
            get { return selectedChapter; }
            set { selectedChapter = value;
                NotifyPropertyChanged("SelectedChapter");
                NotifyPropertyChanged("OriginalText");
            }
        }

        // READ ONLY PROPERTIES
        public string OriginalText
        {
            get
            {
                if (SelectedChapter == AllChapters)
                    return Bible.Get(SelectedBook).ToString();
                else
                    return Bible.Get(SelectedBook)[SelectedChapter].ToString();
            }
        }

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
            ChapterNums = new ObservableCollection<string>();
            SelectedBook = BibleBooks.Genesis;
            SelectedChapter = AllChapters;

            SummarizeChapter();
        }

        /// <summary>
        /// Called when user clicks Summarize
        /// </summary>
        public void SummarizeChapter()
        {
            RyanSummary = "Working...";
            AndrewSummary = "Working...";
            DanielSummary = "Working...";

            SummarizeUsing(new SummarizerDW());
            SummarizeUsing(new SummarizerRR());
            SummarizeUsing(new SummarizerAS());
        }


        /***************************************************/
        /*************     PRIVATE METHODS     *************/
        /***************************************************/
        private void ResetChapterNumbers(int numChapters)
        {
            ChapterNums.Clear();

            ChapterNums.Add("All");
            for (int i = 1; i <= numChapters; i++)
            {
                ChapterNums.Add(i.ToString());
            }
            SelectedChapter = AllChapters;
        }

        private void SummarizeUsing(SummarizerImplementation implementation)
        {
            string summary = "";
            BackgroundWorker bw = new BackgroundWorker();

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                summary = implementation.Summarize(OriginalText);
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
