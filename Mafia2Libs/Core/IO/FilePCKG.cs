using System;
using System.IO;
using ResourceTypes.PCKG;
using Utils.Logging;
using Utils.Settings;

namespace Core.IO
{
    public class FilePCKG : FileBase
    {
        public FilePCKG(FileInfo info) : base(info)
        {
        }

        public override bool Open()
        {
            string backupFolder = Path.Combine(file.Directory.FullName, "Backups");
            string extractedFolder = Path.Combine(file.Directory.FullName, "extracted", file.Name);

            // Only create backups if enabled.
            if (ToolkitSettings.bBackupEnabled)
            {
                // We should backup file before unpacking..
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                // Place the backup in the folder recently created
                string time = string.Format("{0}_{1}_{2}_{3}_{4}", DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                string filename = ToolkitSettings.AddTimeDataBackup == true ? file.Name.Insert(file.Name.Length - 5, "_" + time) : file.Name;
                File.Copy(file.FullName, Path.Combine(backupFolder, filename), true);
            }

            // Begin the unpacking process.
            Log.WriteLine("Opening PCKG: " + file.Name);
            Package package = new Package(extractedFolder);
            if (package.Unpack(file.FullName))
            {
                Log.WriteLine("Successfully read package");
                return true;
            }

            return false;
        }

        public override void Save()
        {
            string Folder = Path.Combine(file.Directory.FullName, "extracted", file.Name);

            if (Directory.Exists(Folder))
            {
                Package package = new Package(Folder);
                package.Pack(file.FullName);
            }
        }

        public override string GetExtensionUpper()
        {
            return "PCKG";
        }
    }
}
