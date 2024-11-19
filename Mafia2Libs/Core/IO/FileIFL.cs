using Microsoft.Win32;
using ResourceTypes.ImageFileList;
using System.IO;

namespace Core.IO
{
    public class FileIFL : FileBase
    {
        public FileIFL(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "IFL";
        }

        public override bool Open()
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML (*.xml)|*.xml"
            };

            if (saveFile.ShowDialog() == true)
            {
                ImageFileList IMGFileList = new ImageFileList();

                using (MemoryStream ReaderStream = new MemoryStream(File.ReadAllBytes(file.FullName)))
                {
                    IMGFileList.ReadFromFile(ReaderStream, false);
                }

                IMGFileList.ConvertToXML(saveFile.FileName);
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
                ImageFileList IMGFileList = new ImageFileList();
                IMGFileList.ConvertFromXML(openFile.FileName);

                File.Copy(file.FullName, file.FullName + "_old", true);
                IMGFileList.WriteToFile(file.FullName, false);
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
