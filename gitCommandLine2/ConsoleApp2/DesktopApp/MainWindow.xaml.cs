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
using System.IO;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GitConnected gc;
        string remote;
        string branch;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gc = new GitConnected(GitPath.Text);
            remote = RemotePath.Text;

            // Deactivate those text boxes so the user cannot change them anymore.
            GitPath.IsReadOnly = true;
            RemotePath.IsReadOnly = true; 
            SetDir.Visibility = Visibility.Hidden;
            GitPath.Foreground = new SolidColorBrush(Colors.DarkGray);
            GitPath.Background = new SolidColorBrush(Colors.LightGray);
            RemotePath.Foreground = new SolidColorBrush(Colors.DarkGray);
            RemotePath.Background = new SolidColorBrush(Colors.LightGray);

            // Make sure the local repo is tied to something.
            if("" == gc.gitCommand("remote -v"))
            {
                gc.gitCommand("remote add origin " + this.remote);
            }

            // Update status
            Output.Text = gc.gitCommand("status");
            Directory.Text = gc.gitCommand("ls-files -t") + gc.gitCommand("ls-files --other"); 

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            RunUpdate();
        }

        private void RunUpdate()
        {
            Output.Text = gc.gitCommand("status");
            CommitBtn.Visibility = Visibility.Hidden;

            // Check for untracked files.
            string line;
            string status = gc.gitCommand("status -s");
            StringReader read = new StringReader(status);
            bool hasAdd = false;
            bool hasCommit = false;
            while (true)
            {
                line = read.ReadLine();
                if (line != null)
                {
                    if (line.Substring(1, 1) == "M")
                    {
                        AddAll.Visibility = Visibility.Visible;
                        hasAdd = true;
                    }
                    if (line.Substring(0,1) == "M")
                    {
                        hasCommit = true;
                    }
                }
                else
                {
                    break;
                }
            }

            if (hasCommit && !hasAdd)
            {
                CommitBtn.Visibility = Visibility.Visible;
            }

            // Check for files with changes.
            string toAdd = gc.gitCommand("ls-files --other");
            string toCommit = gc.gitCommand("ls-files --modified");
            Directory.Text = gc.gitCommand("ls-files -t") + toAdd;
            
        }

        private void GitPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (GitPath.IsReadOnly == false)
            {
                GitPath.Text = "";
            }
        }

        private void RemotePath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RemotePath.IsReadOnly == false)
            {
                RemotePath.Text = "";
            }
        }

        private void AddAll_Click(object sender, RoutedEventArgs e)
        {
            gc.gitCommand("add .");
            AddAll.Visibility = Visibility.Hidden;
            RunUpdate();
        }

        private void Commit_Click(object sender, RoutedEventArgs e)
        {
            // Create a new window to accept commit message input

            CommitMessage cm = new CommitMessage();
            cm.Owner = this;
            cm.Show();
        }

        public void SendCommit(string message)
        {
            // Read that input and append to git command
            gc.gitCommand("commit -m \"" + message + "\"");
            RunUpdate();
        }

        private void Push(object sender, RoutedEventArgs e)
        {
            this.branch = "master"; // Remove once branch handling is put in place.
            Output.Text = gc.gitCommand("push --set-upstream origin " + this.branch);
            
        }

        private void Pull(object sender, RoutedEventArgs e)
        {
            this.branch = "master"; // Remove once branch handling is put in place.
            Output.Text = gc.gitCommand("pull");

        }
    }
}
