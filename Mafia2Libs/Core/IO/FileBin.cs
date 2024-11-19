using Mafia2Tool;
using ResourceTypes.CGame;
using ResourceTypes.EntityActivator;
using ResourceTypes.Navigation;
using ResourceTypes.SDSConfig;
using ResourceTypes.Sound;
using ResourceTypes.Tyres;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    public class FileBin : FileBase
    {
        private const uint StreamMapMagic = 0x4D727453;
        private const uint TapIndicesMagic = 0x30504154;
        private const uint SDSConfigMagic = 0x73647370;
        private const uint CityShopsMagic = 0x63747368;
        private const uint CityAreasMagic = 0x63746172;
        private const uint ShopMenu2Magic = 0x73686D32;
        private const uint EntityActivatorMagic = 0x656E7461;
        private const uint TyresMagic = 0x12345678;
        private const uint CGameMagic = 0x676D7072;

        public FileBin(FileInfo info) : base(info) { }

        public override string GetExtensionUpper()
        {
            return "BIN";
        }

        public override bool Open()
        {
            if (CheckFileMagic(file, CityAreasMagic))
            {
                CityAreaEditor editor = new CityAreaEditor(file);
                return true;
            }
            else if (CheckFileMagic(file, CityShopsMagic))
            {
                CityShopEditor editor = new CityShopEditor(file);
                return true;
            }
            else if (CheckFileMagic(file, ShopMenu2Magic))
            {
                ShopMenu2Editor editor = new ShopMenu2Editor(file);
                return true;
            }
            else if(CheckFileMagic(file, SDSConfigMagic))
            {
                SdsConfigFile SDSConfigEditor = new SdsConfigFile();
                SDSConfigEditor.ReadFromFile(file);
                return true;
            }
            else if(CheckFileMagic(file, TapIndicesMagic))
            {
                TAPIndices editor = new TAPIndices();
                editor.ReadFromFile(file);
                return true;
            }
            else if(CheckFileMagic(file, StreamMapMagic))
            {
                StreamEditor editor = new StreamEditor(file); 
                return true;
            }
            else if(CheckFileMagic(file, EntityActivatorMagic))
            {
                EntityActivator data = new EntityActivator();
                data.ReadFromFile(file);
                return true;
            }
            else if(CheckFileMagic(file, TyresMagic))
            {
                SaveFileDialog saveFile = new SaveFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // Unsure on how we should handle this. For now we will just try and hope the loader works.
                    Tyres loader = new Tyres(file);
                    loader.ConvertToXML(saveFile.FileName);
                }
            }
            else if (CheckFileMagic(file, CGameMagic))
            {
                SaveFileDialog saveFile = new SaveFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // Unsure on how we should handle this. For now we will just try and hope the loader works.
                    CGame loader = new CGame(file);
                    loader.ConvertToXML(saveFile.FileName);
                }
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    // Unsure on how we should handle this. For now we will just try and hope the loader works.
                    SoundSectorResource loader = new SoundSectorResource(file);
                    loader.ConvertToXML(saveFile.FileName);
                }
            }

            return false;
        }

        public override void Save()
        {
            if (CheckFileMagic(file, TyresMagic))
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    Tyres loader = new Tyres(file);
                    loader.ConvertFromXML(openFile.FileName);

                    File.Copy(file.FullName, file.FullName + "_old", true);
                    loader.WriteToFile(file.FullName);
                }
            }
            if (CheckFileMagic(file, CGameMagic))
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    CGame loader = new CGame(file);
                    loader.ConvertFromXML(openFile.FileName);

                    File.Copy(file.FullName, file.FullName + "_old", true);
                    loader.WriteToFile(file.FullName);
                }
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    SoundSectorResource loader = new SoundSectorResource(file);
                    loader.ConvertFromXML(openFile.FileName);

                    File.Copy(file.FullName, file.FullName + "_old", true);
                    loader.WriteToFile(file.FullName, false);
                }
            }
        }

        private bool CheckFileMagic(FileInfo file, uint Magic)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                if (reader.ReadInt32() == Magic)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanContextMenuOpen()
        {
            return true;
        }

        public override bool CanContextMenuSave()
        {
            return true;
        }

        public override string GetContextMenuOpenTitle()
        {
            return "Convert To (.xml)";
        }

        public override string GetContextMenuSaveTitle()
        {
            return "Convert From (.xml)";
        }
    }
}
