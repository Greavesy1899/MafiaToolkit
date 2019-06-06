using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Utils
{
    public class FBXHelper
    {
        private Dictionary<int, string> errors = new Dictionary<int, string>();

        public FBXHelper()
        {
            errors.Add(-100, "Boundary Box exceeds Mafia II's limits!");
            errors.Add(-99, "pElementNormal->GetReferenceMode() did not equal eDirect!");
            errors.Add(-98, "pElementNormal->GetMappingMode() did not equal eByControlPoint or eByPolygonVertex!");
            errors.Add(-97, "Vertex count > ushort max value! This model cannot be used in Mafia II!");
        }

        public bool Run(string args)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("M2FBX.exe", args)
            {
                CreateNoWindow = false,
                UseShellExecute = false
            };
            Process tool = Process.Start(processStartInfo);
            while (!tool.HasExited) ;
            int exitCode = tool.ExitCode;
            tool.Dispose();

            if (errors.ContainsKey(exitCode))
            {
                ThrowMessageBox(exitCode);
                return false;
            }

            return true;
        }

        private void ThrowMessageBox(int exitCode)
        {
            string message = "An Error has Occured: \n";
            message += errors[exitCode];
            MessageBox.Show(message, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
