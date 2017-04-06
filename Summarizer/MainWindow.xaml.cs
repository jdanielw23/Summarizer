using Summarizer.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Summarizer
{
    /// <summary>
    /// Interaction logic for BibleMainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowVM ViewModel = new MainWindowVM();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void SwitchUI_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchUI();
        }

        private void SummarizeChapter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SummarizeChapter();
        }

        private void UploadDocument_Click(object sender, RoutedEventArgs e)
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
                FilePath.Text = dlg.FileName;
                ViewModel.UploadDocument(dlg.FileName);
            }
        }
    }
}
