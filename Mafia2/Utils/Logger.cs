using System;
using System.IO;
using System.Text;

namespace Mafia2
{
    public class Log
    {
        public static string LogPath = "log.txt";
        public static bool LoggingEnabled = true;
        public static bool ExtensiveLogging = false; //Doesnt fully work.

        public static void DeleteOldLog()
        {
            if(File.Exists(LogPath))
                File.Delete(LogPath);
        }

        public static void CreateFile(bool append = false)
        {
            if (!File.Exists(LogPath))
            {
                WriteLine("Debugging has started.");
            }
            else
            {
                if (!append)
                    WriteLine("Debugging has started.", LoggingTypes.MESSAGE, false);
            }
        }
        public static void WriteLine(string text, LoggingTypes type = LoggingTypes.MESSAGE, bool append = true)
        {
            using (StreamWriter Writer = new StreamWriter(LogPath, append, Encoding.UTF8))
            {

                if (text != "")
                    Writer.WriteLine(string.Format("[{0}] {1}: {2}", DateTime.Now.TimeOfDay, type, text));
            }
        }
    }
    public enum LoggingTypes
    {
        WARNING,
        ERROR,
        MESSAGE,
        FATAL,
    }
}