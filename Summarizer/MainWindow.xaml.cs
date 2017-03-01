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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Summarizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void UploadDocument_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SummarizeDocument();
        }

        private void SwitchUI_Click(object sender, RoutedEventArgs e)
        {
            Window bibleWindow = new BibleMainWindow();
            bibleWindow.Show();
            this.Close();
        }
    }
}
