using ResourceTypes.Navigation;
using ResourceTypes.Navigation.Traffic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace Core.IO
{
    public class FileNavigation_HPD : FileBase
    {
        public FileNavigation_HPD(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "HPD";
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

        private IRoadmap ConvertFromXml(IRoadmapFactory Factory, string Filename)
        {
            RoadmapXmlSerializer Serializer = new RoadmapXmlSerializer();
            return Serializer.Deserialize(Factory, Filename);
        }

        private void ConvertToXML(IRoadmap Roadmap, string Filename)
        {
            NAVData Data = new NAVData(file);
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(Data);
            RootElement.Save(Filename);
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

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                NAVData Data = new NAVData(file);
                XElement RootElement = ReflectionHelpers.ConvertPropertyToXML<NAVData>(Data);
                RootElement.Save(saveFile.FileName);
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

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                XElement FileElement = XElement.Load(openFile.FileName);
                NAVData NewNavData = ReflectionHelpers.ConvertToPropertyFromXML<NAVData>(FileElement);

                using (NavigationWriter Writer = new NavigationWriter(File.Open(file.FullName, FileMode.Open)))
                {
                    NewNavData.WriteToFile(Writer);
                }
            }
        }
    }
}
