using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using System;
using System.IO;
using System.Windows.Forms;
using Utils.Logging;
using Utils.Settings;

namespace Core.IO
{
    public class FileSDS : FileBase
    {
        public FileSDS(FileInfo info) : base(info)
        {
        }

        /// <summary>
        /// Validates that a target path is within the expected base directory to prevent path traversal attacks.
        /// </summary>
        private static bool IsPathWithinDirectory(string targetPath, string baseDirectory)
        {
            string fullTarget = Path.GetFullPath(targetPath);
            string fullBase = Path.GetFullPath(baseDirectory);

            // Ensure base directory ends with separator for accurate comparison
            if (!fullBase.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fullBase += Path.DirectorySeparatorChar;
            }

            return fullTarget.StartsWith(fullBase, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Open()
        {
            string baseDir = file.Directory.FullName;
            string backupFolder = Path.GetFullPath(Path.Combine(baseDir, "BackupSDS"));
            string extractedFolder = Path.GetFullPath(Path.Combine(baseDir, "extracted"));

            // Validate paths are within expected directory (prevent path traversal)
            if (!IsPathWithinDirectory(backupFolder, baseDir) || !IsPathWithinDirectory(extractedFolder, baseDir))
            {
                throw new InvalidOperationException("Invalid path detected - potential path traversal attack.");
            }

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
                string filename = ToolkitSettings.AddTimeDataBackup == true ? file.Name.Insert(file.Name.Length - 4, "_" + time) : file.Name;
                File.Copy(file.FullName, Path.Combine(backupFolder, filename), true);
            }

            // Begin the unpacking process.
            Log.WriteLine("Opening SDS: " + file.Name);
            ArchiveFile archiveFile;
            using (var input = File.OpenRead(file.FullName))
            {
                using (Stream data = ArchiveEncryption.Unwrap(input))
                {
                    archiveFile = new ArchiveFile();
                    archiveFile.Deserialize(data ?? input);
                }
            }

            Log.WriteLine("Successfully unwrapped compressed data");
            archiveFile.SaveResources(file);

            if (File.Exists(file.FullName + ".patch") && GameStorage.Instance.GetSelectedGame().GameType == GamesEnumerator.MafiaII_DE)
            {
                DialogResult result = MessageBox.Show("Detected Patch file. Would you like to unpack?", "Toolkit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    archiveFile.ExtractPatch(new FileInfo(file.FullName + ".patch"));
                }
            }

            return true;
        }

        public override void Save()
        {
            var game = GameStorage.Instance.GetSelectedGame();
            string Folder = Path.GetFullPath(Path.Combine(file.Directory.FullName, "extracted", file.Name));
            SaveSDSWithCustomFolder(game.GameType, Folder);
        }

        public void SaveSDSWithCustomFolder(GamesEnumerator GameType, string Folder)
        {
            // Validate folder path exists and is a directory
            string fullPath = Path.GetFullPath(Folder);
            if (!Directory.Exists(fullPath))
            {
                throw new DirectoryNotFoundException($"Source folder does not exist: {fullPath}");
            }

            ArchiveFile archiveFile = new ArchiveFile();
            archiveFile.Platform = Platform.PC;
            archiveFile.SetGameType(GameType);

            // MII: DE no longer has this data in the header.
            if (GameType == GamesEnumerator.MafiaII)
            {
                archiveFile.Unknown20 = new byte[16] { 55, 51, 57, 55, 57, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            }

            if (GameType == GamesEnumerator.MafiaI_DE || GameType == GamesEnumerator.MafiaIII)
            {
                archiveFile.Version = 20;
            }
            else
            {
                archiveFile.Version = 19;
            }

            // We should now to try pack the SDS.
            if (!archiveFile.BuildResources(Folder))
            {
                MessageBox.Show("Failed to pack SDS.", "Toolkit", MessageBoxButtons.OK);
                return;
            }

            foreach (ResourceEntry entry in archiveFile.ResourceEntries)
            {
                if (entry.Data == null)
                {
                    throw new FormatException();
                }
            }

            using (var output = File.Create(file.FullName))
            {
                archiveFile.Serialize(output, ArchiveSerializeOptions.Compress);
            }

            archiveFile = null;
        }

        public override string GetExtensionUpper()
        {
            return "SDS";
        }
    }
}
