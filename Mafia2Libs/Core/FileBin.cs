using Mafia2Tool;
using ResourceTypes.Misc;
using ResourceTypes.Navigation;
using ResourceTypes.SDSConfig;
using ResourceTypes.Sound;
using System.IO;

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
        private const uint CGameMagic = 0x676D7072;

        public FileBin(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "BIN";
        }

        public override bool Open()
        {
            var filename = GetNameWithoutExtension();

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
            else if (CheckFileMagic(file, CGameMagic))
            {
                CGameData data = new CGameData(file);
                return true;
            }
            else
            {
                // Unsure on how we should handle this. For now we will just try and hope the loader works.
                SoundSectorLoader loader = new SoundSectorLoader(file);
            }

            return false;
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
    }
}
