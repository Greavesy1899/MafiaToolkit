﻿using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utils.Settings;

namespace Utils
{
    public static class FBXHelper
    {
        [DllImport("libs/M2FBX.dll")]
        private static extern int RunConvertFBX(string source, string dest);
        [DllImport("libs/M2FBX.dll")]
        private static extern int RunConvertMTB(string source, string dest, byte extDesc);
        [DllImport("libs/M2FBX.dll")]
        private static extern int RunCookTriangleCollision(string source, string dest);
        [DllImport("libs/M2FBX.dll")]
        private static extern int RunCookConvexCollision(string source, string dest);

        public static int ConvertFBX(string source, string dest)
        {
            int exitCode = RunConvertFBX(source, dest);
            ThrowMessageBox(exitCode);
            return exitCode;
        }

        public static int ConvertMTB(string source, string dest)
        {
            int exitCode = RunConvertMTB(source, dest, (byte)ToolkitSettings.Format);
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
                case -95:
                    message += "Failed to convert part during FBX > M2T";
                    break;
                case -94:
                    message += "Could not find standalone mesh or LOD Group!";
                    break;
                case -50:
                    message += "Failed to initialize the FBX Importer!";
                    break;
                case -25:
                    message += "pMesh->IsTriangleMesh() did not equal true..Cannot continue!";
                    break;
                case -10:
                    message += "Failed to initialize the PhysX Cooking library!";
                    break;
                default:
                    message += string.Format("Unknown Error {0}", exitCode);
                    break;
            }

            MessageBox.Show(message, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
