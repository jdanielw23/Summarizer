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
        private string selectedBook;
        private int selectedChapter;

        public Collection<int> ChapterNums { get; private set; }
        public List<string> ChapterNames { get; private set; }

        public string RyanSummary { get; private set; }
        public string AndrewSummary { get; private set; }
        public string DanielSummary { get; private set; }

        public string OriginalText { get; private set; }

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

        /// <summary>
        /// Constructor
        /// </summary>
        public BibleMainWindowVM()
        {
            ChapterNames = Constants.BibleBooks.Keys.ToList();
            ChapterNums = new ObservableCollection<int>();
            SelectedBook = "Genesis";
            SelectedChapter = 1;
        }

        public void SummarizeChapter()
        {
            OriginalText = Constants.Bible[SelectedBook][SelectedChapter].Values.ToList().ListData("");
            NotifyPropertyChanged("OriginalText");
        }

        private void ResetChapterNumbers(int numChapters)
        {
            ChapterNums.Clear();

            for (int i = 1; i <= numChapters; i++)
            {
                ChapterNums.Add(i);
            }
            SelectedChapter = ChapterNums[0];
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
