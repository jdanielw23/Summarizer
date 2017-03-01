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
    public partial class BibleMainWindow : Window
    {
        private BibleMainWindowVM ViewModel = new BibleMainWindowVM();

        public BibleMainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void SwitchUI_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void SummarizeChapter_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SummarizeChapter();
        }
    }
}
