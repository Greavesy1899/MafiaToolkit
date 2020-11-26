using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Utils.Helpers
{
    public static class PhysXHelper
    {
        private const string ExecutablePath = "libs\\M2PhysX.exe";

        public static int CookTriangleCollision(string source, string dest)
        {
            if(!DoesExecutableExist())
            {
                MessageBox.Show("Missing M2PhysX.dll! Cannot cook triangle mesh.");
                return -1;
            }

            string Arguments = string.Format("-CookTriangleMesh {0} {1}", source, dest);
            int ExitCode = RunExecutable(Arguments);
            return ExitCode;
        }

        public static int MultiCookTriangleCollision(string source, string dest)
        {
            if (!DoesExecutableExist())
            {
                MessageBox.Show("Missing M2PhysX.dll! Cannot use multi cook");
                return -1;
            }

            string Arguments = string.Format("-MultiCookTriangleMesh {0} {1}", source, dest);
            int ExitCode = RunExecutable(Arguments);
            return ExitCode;
        }

        private static int RunExecutable(string Arguments)
        {
            string ExePath = GetExecutablePath();
            Process PhysXProcess = new Process();
            PhysXProcess.StartInfo.FileName = ExePath;
            PhysXProcess.StartInfo.Arguments = Arguments;
            PhysXProcess.StartInfo.CreateNoWindow = true;
            PhysXProcess.StartInfo.UseShellExecute = false;
            PhysXProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            PhysXProcess.Start();

            while (!PhysXProcess.HasExited)
            {
                // Waiting
            }

            return PhysXProcess.ExitCode;
        }

        private static bool DoesExecutableExist()
        {
            return File.Exists(ExecutablePath);
        }

        private static string GetExecutablePath()
        {
            return Environment.CurrentDirectory + "\\" + ExecutablePath;
        }
    }
}
