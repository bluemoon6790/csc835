﻿using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    class Program
    {
        
        //takes a git command and prints the output of the command
        //to the console. The git command is given in the form of
        //"status" or "log" not "git log" or similar.
        //example usage is: gitCommand("log");
        //flags can be added directly to the command such as
        // gitCommand("log --stat");
        static public void gitCommand(string command)
        {
            ProcessStartInfo gitInfo = new ProcessStartInfo();
            gitInfo.CreateNoWindow = true;
            gitInfo.RedirectStandardError = true;
            gitInfo.RedirectStandardOutput = true;
            gitInfo.UseShellExecute = false;

            //file directory of local git install directory
            gitInfo.FileName = @"D:\Applications\Git\bin\git.exe";
            //directory of local repository
            gitInfo.WorkingDirectory = @"C:\Users\John\.git\";

            gitInfo.Arguments = @command; // such as "log" or "status"
            
            //Then create a Process to actually run the command.
            Process gitProcess = new Process();
            gitProcess.StartInfo = gitInfo;
            gitProcess.Start();

            string stderr_str = gitProcess.StandardError.ReadToEnd();  // pick up STDERR
            string stdout_str = gitProcess.StandardOutput.ReadToEnd(); // pick up STDOUT

            Console.WriteLine(stderr_str);
            Console.ReadKey();

        }

        static void Main(string[] args)
        {
            gitCommand("log");//example usage
        }
    }
}
