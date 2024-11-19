using System.IO;
using Microsoft.Win32;
using ResourceTypes.SoundTable;

namespace Core.IO
{
    public class FileSoundTable : FileBase
    {
        public FileSoundTable(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "STBL";
        }

        public override bool Open()
        {
            // TODO: Make editor

            SaveFileDialog saveFile = new SaveFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML (*.xml)|*.xml"
            };

            if (saveFile.ShowDialog() == true)
            {
                SoundTable Table = new SoundTable();

                using (MemoryStream ReaderStream = new MemoryStream(File.ReadAllBytes(file.FullName)))
                {
                    Table.ReadFromFile(ReaderStream, false);
                }

                Table.ConvertToXML(saveFile.FileName);
            }

            return true;
        }

        public override void Save()
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML (*.xml)|*.xml"
            };

            if (openFile.ShowDialog() == true)
            {
                SoundTable Table = new SoundTable();
                Table.ConvertFromXML(openFile.FileName);

                File.Copy(file.FullName, file.FullName + "_old", true);
                Table.WriteToFile(file.FullName, false);
            }
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Convert To (.xml)";
        }

        public override bool CanContextMenuSave()
        {
            return true;
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Convert From (.xml)";
        }
    }
}
