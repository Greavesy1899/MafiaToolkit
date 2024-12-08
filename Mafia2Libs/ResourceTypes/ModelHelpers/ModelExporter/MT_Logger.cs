using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public enum MT_LogType
    {
        INFO,
        WARNING,
        ERROR
    }

    public class MT_Logger
    {
        private List<string> OutputLog = new List<string>();

        public void WriteInfo(string Format, params object[] Args)
        {
            WriteLine(MT_LogType.INFO, string.Format(Format, Args));
        }

        public void WriteWarning(string Format, params object[] Args)
        {
            WriteLine(MT_LogType.WARNING, string.Format(Format, Args));
        }

        public void WriteError(string Format, params object[] Args)
        {
            WriteLine(MT_LogType.ERROR, string.Format(Format, Args));
        }

        private void WriteLine(MT_LogType LogType, string Format, params object[] Args)
        {
            string NextMsg = string.Format(Format, Args);
            OutputLog.Add(NextMsg);
        }

        public void Clear()
        {
            OutputLog.Clear();
        }

        public string[] GetOutput()
        {
            return OutputLog.ToArray();
        }
    }
}
