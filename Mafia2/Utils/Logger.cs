using System;
using System.IO;
using System.Text;

namespace Mafia2
{
    public class Log
    {
        public static string LogPath = "Logging/log.txt";
        public static bool LoggingEnabled = true;
        public static bool ExtensiveLogging = false; //Doesnt fully work.

        public static void DeleteOldLog()
        {
            if(File.Exists(LogPath))
                File.Delete(LogPath);
        }

        public static void CreateFile(bool append = false)
        {
            //if (!LoggingEnabled)
                return;

            if (!Directory.Exists("Logging"))
                Directory.CreateDirectory("Logging");

            if (!File.Exists(LogPath))
            {
                WriteLine("Debugging has started.");
            }
            else
            {
                if (!append)
                    WriteLine("Debugging has started.", LoggingTypes.MESSAGE, LogCategoryTypes.APPLICATION, false);
            }
        }
        public static void WriteLine(string text, LoggingTypes logType = LoggingTypes.MESSAGE, LogCategoryTypes catType = LogCategoryTypes.FUNCTION, bool append = true)
        {
            //if (!LoggingEnabled)
                return;

            string logfile = "Logging/LOG_" + LogCategoryTypes.APPLICATION + ".txt";

            using (StreamWriter Writer = new StreamWriter(logfile, append, Encoding.UTF8))
            {

                if (text != "")
                    Writer.WriteLine(string.Format("[{0}] {1}: {2}", DateTime.Now.TimeOfDay, logType, text));
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

    public enum LogCategoryTypes
    {
        APPLICATION,
        FUNCTION,
        IO
    }
}