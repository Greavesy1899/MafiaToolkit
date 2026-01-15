using ResourceTypes.CGame;
using ResourceTypes.EntityActivator;
using ResourceTypes.FrameProps;
using ResourceTypes.GameParams;
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
        // Magic numbers for different BIN file types (exposed for factory registration)
        public const uint StreamMapMagic = 0x4D727453;
        public const uint TapIndicesMagic = 0x30504154;
        public const uint SDSConfigMagic = 0x73647370;
        public const uint CityShopsMagic = 0x63747368;
        public const uint CityAreasMagic = 0x63746172;
        public const uint ShopMenu2Magic = 0x73686D32;
        public const uint EntityActivatorMagic = 0x656E7461;
        public const uint TyresMagic = 0x12345678;
        public const uint CGameMagic = 0x676D7072;
        public const uint FramePropsMagic = 0x66726D70;

        public FileBin(FileInfo info) : base(info) { }

        public override string GetExtensionUpper()
        {
            return "BIN";
        }

        public override bool Open()
        {
            // Use the editor factory to create the appropriate editor
            // This decouples Core.IO from Forms - editors are registered in BinEditorRegistration.cs
            if (BinEditorFactory.TryCreateEditor(file))
            {
                return true;
            }

            // Fallback for unregistered types: export to XML
            return HandleUnregisteredFile(file);
        }

        /// <summary>
        /// Handles files that don't have a registered editor by offering XML export.
        /// </summary>
        private bool HandleUnregisteredFile(FileInfo file)
        {
            // Check for specific types that export directly to XML
            if (CheckFileMagic(file, TapIndicesMagic))
            {
                TAPIndices editor = new TAPIndices();
                editor.ReadFromFile(file);
                return true;
            }
            else if (CheckFileMagic(file, EntityActivatorMagic))
            {
                EntityActivator data = new EntityActivator();
                data.ReadFromFile(file);
                return true;
            }
            else if (CheckFileMagic(file, TyresMagic))
            {
                return ExportToXml(file, (f, savePath) =>
                {
                    Tyres loader = new Tyres(f);
                    loader.ConvertToXML(savePath);
                });
            }
            else
            {
                // Default: treat as SoundSectorResource
                return ExportToXml(file, (f, savePath) =>
                {
                    SoundSectorResource loader = new SoundSectorResource(f);
                    loader.ConvertToXML(savePath);
                });
            }
        }

        /// <summary>
        /// Helper method for XML export dialog pattern.
        /// </summary>
        private static bool ExportToXml(FileInfo file, System.Action<FileInfo, string> exportAction)
        {
            SaveFileDialog saveFile = new SaveFileDialog()
            {
                InitialDirectory = Path.GetDirectoryName(file.FullName),
                FileName = Path.GetFileNameWithoutExtension(file.FullName),
                Filter = "XML (*.xml)|*.xml"
            };

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                exportAction(file, saveFile.FileName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a file is a GameParams file by filename.
        /// </summary>
        public static bool IsGameParamsFile(FileInfo file)
        {
            return file.Name.Equals("gameparams.bin", System.StringComparison.OrdinalIgnoreCase);
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
            else if (CheckFileMagic(file, CGameMagic))
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
            else if (CheckFileMagic(file, SDSConfigMagic))
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    SdsConfigFile loader = new SdsConfigFile(file);
                    loader.ConvertFromXML(openFile.FileName);

                    File.Copy(file.FullName, file.FullName + "_old", true);
                    loader.WriteToFile(file.FullName);
                }
            }
            else if (CheckFileMagic(file, FramePropsMagic))
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    FramePropsFile loader = new FramePropsFile(file);
                    loader.ConvertFromXML(openFile.FileName);

                    File.Copy(file.FullName, file.FullName + "_old", true);
                    loader.WriteToFile(file.FullName);
                }
            }
            else if (IsGameParamsFile(file))
            {
                OpenFileDialog openFile = new OpenFileDialog()
                {
                    InitialDirectory = Path.GetDirectoryName(file.FullName),
                    FileName = Path.GetFileNameWithoutExtension(file.FullName),
                    Filter = "XML (*.xml)|*.xml"
                };

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    GameParamsFile loader = new GameParamsFile(file);
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

        /// <summary>
        /// Check if a file starts with the given magic number.
        /// </summary>
        public static bool CheckFileMagic(FileInfo file, uint Magic)
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
