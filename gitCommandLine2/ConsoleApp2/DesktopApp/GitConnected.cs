using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DesktopApp
{
    class GitConnected
    {

        string wd,fn;
        public GitConnected(string path)
        {
            wd = path;
            fn = @"C:\Program Files\Git\bin\git.exe";
        }
        //takes a git command and prints the output of the command
        //to the console. The git command is given in the form of
        //"status" or "log" not "git log" or similar.
        //example usage is: gitCommand("log");
        //flags can be added directly to the command such as
        // gitCommand("log --stat");
        public string gitCommand(string command)
        {
            ProcessStartInfo gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.UseShellExecute = false;

            //file directory of local git install directory
            gitInfo.FileName = fn;
            //directory of local repository
            gitInfo.WorkingDirectory = wd;

            gitInfo.Arguments = @command; // such as "log" or "status"

            //Then create a Process to actually run the command.
            Process gitProcess = new Process();
            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();

            string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR
            string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

            Console.WriteLine(stderr_str);
            return stdout_str;
            //Console.ReadKey();

        }
    }
}
