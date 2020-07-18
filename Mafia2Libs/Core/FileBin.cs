using Mafia2Tool;
using ResourceTypes.Misc;
using ResourceTypes.SDSConfig;
using ResourceTypes.Sound;
using System.IO;

namespace Core.IO
{
    public class FileBin : FileBase
    {
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

            if(filename.Equals("cityareas"))
            {
                CityAreaEditor editor = new CityAreaEditor(file);
                return true;
            }
            else if(filename.Equals("cityshop"))
            {
                CityShopEditor editor = new CityShopEditor(file);
                return true;
            }
            else if (filename.Equals("shopmenu2"))
            {
                ShopMenu2Editor editor = new ShopMenu2Editor(file);
                return true;
            }
            else if(HandleStreamMap(file))
            {
                StreamEditor editor = new StreamEditor(file); 
                return true;
            }
            else if (CGameData.CheckHeader(file))
            {
                CGameData data = new CGameData(file);
                return true;
            }
            else if (filename.Equals("sdsconfig"))
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
                {
                    SdsConfigFile sdsConfig = new SdsConfigFile();
                    sdsConfig.ReadFromFile(reader);
                }
                return true;
            }
            else
            {
                // Unsure on how we should handle this. For now we will just try and hope the loader works.
                SoundSectorLoader loader = new SoundSectorLoader(file);
            }

            return false;
        }

        private bool HandleStreamMap(FileInfo file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                if (reader.ReadInt32() == 1299346515)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
