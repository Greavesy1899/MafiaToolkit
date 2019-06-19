using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Utils
{
    public static class FBXHelper
    {
        [DllImport("libs/M2FBX.dll")]
        private static extern int RunConvertFBX(string source, string dest);

        [DllImport("libs/M2FBX.dll")]
        private static extern int RunConvertM2T(string source, string dest);

        [DllImport("libs/M2FBX.dll")]
        private static extern int RunCookCollision(string source, string dest);

        public static int ConvertFBX(string source, string dest)
        {
            int exitCode = RunConvertFBX(source, dest);
            ThrowMessageBox(exitCode);
            return exitCode;
        }

        public static int ConvertM2T(string source, string dest)
        {
            int exitCode = RunConvertM2T(source, dest);
            ThrowMessageBox(exitCode);
            return exitCode;
        }

        public static int CookCollision(string source, string dest)
        {
            int exitCode = RunCookCollision(source, dest);
            ThrowMessageBox(exitCode);
            return exitCode;
        }

        private static void ThrowMessageBox(int exitCode)
        {
            if (exitCode == 0)
                return;

            string message = "An Error has Occured: \n";
            switch(exitCode)
            {
                case -100:
                    message += "Boundary Box exceeds Mafia II's limits!";
                    break;
                case -98:
                    message += "pElementNormal->GetMappingMode() did not equal eByControlPoint or eByPolygonVertex!";
                    break;
                case -97:
                    message += "Vertex count > ushort max value! This model cannot be used in Mafia II!";
                    break;
                case -99:
                    message += "pElementNormal->GetReferenceMode() did not equal eDirect!";
                    break;
                default:
                    message += string.Format("Unknown Error {0}", exitCode);
                    break;
            }

            MessageBox.Show(message, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
