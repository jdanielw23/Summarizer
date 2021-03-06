﻿using Summarizer.Model;
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
    public class MainWindowVM : INotifyPropertyChanged
    {
        private const int AllChapters = 0;

        private BibleBooks selectedBook;
        private int selectedChapter;
        private string originalText = "";
        private string ryanSummary = "";
        private string andrewSummary = "";
        private string danielSummary = "";

        public Collection<string> ChapterNums { get; private set; }
        public bool ShowBibleSelection { get; set; }

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
        public string OriginalText
        {
            get
            {
                if (ShowBibleSelection)
                {
                    if (SelectedChapter == AllChapters)
                        return Bible.Get(SelectedBook).ToString();
                    else
                        return Bible.Get(SelectedBook)[SelectedChapter].ToString();
                }
                else
                    return originalText;
            }
            set
            {
                originalText = value;
                NotifyPropertyChanged("OriginalText");
                NotifyPropertyChanged("OriginalTextWordCount");
            }
        }

        // READ ONLY PROPERTIES
        public string Title
        { get { return (ShowBibleSelection) ? "Bible Summarizer" : "Document Summarizer"; } }

        public string SwitchToText
        { get { return (ShowBibleSelection) ? "Switch to Document Summarizer" : "Switch to Bible Summarizer"; } }

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
        public MainWindowVM()
        {
            ShowBibleSelection = true;
            ChapterNums = new ObservableCollection<string>();
            ResetBibleUI();
        }

        public void SwitchUI()
        {
            ShowBibleSelection = !ShowBibleSelection;
            if (ShowBibleSelection)
            {
                ResetBibleUI();
                NotifyPropertyChanged("OriginalTextWordCount");
            }
            else
            {
                OriginalText = "";
                RyanSummary = "";
                AndrewSummary = "";
                DanielSummary = "";
            }
            NotifyPropertyChanged("ShowBibleSelection");
            NotifyPropertyChanged("Title");
            NotifyPropertyChanged("SwitchToText");
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

        /// <summary>
        /// Called when user clicks Upload
        /// </summary>
        public void UploadDocument(string filePath)
        {
            OriginalText = System.IO.File.ReadAllText(filePath);

            SummarizeUsing(new SummarizerDW());
            SummarizeUsing(new SummarizerRR());
            SummarizeUsing(new SummarizerAS());
        }


        /***************************************************/
        /*************     PRIVATE METHODS     *************/
        /***************************************************/
        private void ResetBibleUI()
        {
            SelectedBook = BibleBooks.Genesis;
            SelectedChapter = AllChapters;

            SummarizeChapter();
        }

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
