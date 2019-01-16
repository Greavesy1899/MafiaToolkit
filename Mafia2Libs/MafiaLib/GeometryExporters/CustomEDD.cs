using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SharpDX;

namespace Mafia2
{
    public class CustomEDD
    {
        string header = "ED10";
        int entryCount;
        List<Entry> entries;

        public int EntryCount {
            get { return entryCount; }
            set { entryCount = value; }
        }
        public List<Entry> Entries {
            get { return entries; }
            set { entries = value; }
        }

        public CustomEDD()
        {
            entries = new List<Entry>();
        }

        public CustomEDD(FileInfo info)
        {
            entries = new List<Entry>();

            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(header.ToCharArray());
            writer.Write(entryCount);
            for (int i = 0; i != entryCount; i++)
                entries[i].WriteToFile(writer);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != 808535109)
                return;

            entryCount = reader.ReadInt32();

            for (int i = 0; i != entryCount; i++)
                entries.Add(new Entry(reader));
        }

        public int ReadFromFbx(FileInfo file)
        {
            string args = "-ConvertToM2T ";
            args += ("\"" + file.FullName + "\" ");
            args += ("\"" + file.Directory.FullName +"/" + "\" ");
            args += ("\"" + 1 + "\" ");
            string arg = "-ConvertToM2T \"" + file.FullName + "\" \"" + file.Directory.FullName + "/" + "\" " + 1;
            ProcessStartInfo processStartInfo = new ProcessStartInfo("M2FBX.exe", arg)
            {
                CreateNoWindow = false,
                UseShellExecute = false
            };
            Process FbxTool = Process.Start(processStartInfo);
            while (!FbxTool.HasExited) ;

            string errorMessage = "";
            int exitCode = FbxTool.ExitCode;
            FbxTool.Dispose();

            switch (exitCode)
            {
                case -100:
                    errorMessage = "An error ocurred: Boundary Box exceeds Mafia II's limits!";
                    break;
                case -99:
                    errorMessage = "An error ocurred: pElementNormal->GetReferenceMode() did not equal eDirect!";
                    break;
                case -98:
                    errorMessage = "An error ocurred: pElementNormal->GetMappingMode() did not equal eByControlPoint or eByPolygonVertex!";
                    break;
                case -97:
                    errorMessage = "An error ocurred: An error ocurred: Boundary Box exceeds Mafia II's limits!";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            using (BinaryReader reader = new BinaryReader(File.Open(file.Directory.FullName + "/frame.edd", FileMode.Open)))
                ReadFromFile(reader);

            return 0;
        }

        public class Entry
        {
            int lodCount;
            Vector3 position;
            Matrix33 rotation;
            string[] lodNames;

            public int LodCount {
                get { return lodCount; }
                set { lodCount = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Matrix33 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public string[] LODNames {
                get { return lodNames; }
                set { lodNames = value; }
            }

            public Entry()
            {
                lodCount = 0;
                position = new Vector3(0);
                rotation = new Matrix33();
                lodNames = new string[lodCount];
            }

            public Entry(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(lodCount);
                position.WriteToFile(writer);
                rotation.WriteToFile(writer);
                for (int i = 0; i != lodCount; i++)
                    writer.Write(lodNames[i]);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                lodCount = reader.ReadInt32();
                position = Vector3Extenders.ReadFromFile(reader);

                Vector3 vec1 = Vector3Extenders.ReadFromFile(reader);
                Vector3 vec2 = Vector3Extenders.ReadFromFile(reader);
                Vector3 vec3 = Vector3Extenders.ReadFromFile(reader);
                rotation = new Matrix33(vec1, vec2, vec3, false);

                lodNames = new string[lodCount];
                for (int i = 0; i != lodCount; i++)
                    lodNames[i] = reader.ReadString();
            }
        }

    }
}
