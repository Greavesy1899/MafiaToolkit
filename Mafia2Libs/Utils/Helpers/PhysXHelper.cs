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

            string ExePath = GetExecutablePath();
            string Arguments = string.Format("-CookTriangleMesh {0} {1}", source, dest);
            Process PhysXProcess = Process.Start(ExePath, Arguments);

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
