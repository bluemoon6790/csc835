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
using DesktopApp;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GitConnected gc;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gc = new GitConnected(GitPath.Text);
            GitPath.IsReadOnly = true;
            Output.Text = gc.gitCommand("status");
            SetDir.Visibility = Visibility.Hidden;
            GitPath.Foreground = new SolidColorBrush(Colors.DarkGray);
            GitPath.Background = new SolidColorBrush(Colors.LightGray);
            Directory.Text = gc.gitCommand("ls-files -t") + gc.gitCommand("ls-files --other");
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            RunUpdate();
        }

        private void RunUpdate()
        {
            Output.Text = gc.gitCommand("status"); 
            
            // Check for untracked files.
            string toAdd = gc.gitCommand("ls-files --other");
            if ("" != toAdd)
            {
                AddAll.Visibility = Visibility.Visible;
            }
            
            // Check for files with changes.
            string toCommit = gc.gitCommand("ls-files --modified");
            Directory.Text = gc.gitCommand("ls-files -t") + toAdd;
            
        }

        private void GitPath_GotFocus(object sender, RoutedEventArgs e)
        {
            GitPath.Text = "";
        }

        private void AddAll_Click(object sender, RoutedEventArgs e)
        {
            gc.gitCommand("add .");
            AddAll.Visibility = Visibility.Hidden;
            RunUpdate();
        }
    }
}
