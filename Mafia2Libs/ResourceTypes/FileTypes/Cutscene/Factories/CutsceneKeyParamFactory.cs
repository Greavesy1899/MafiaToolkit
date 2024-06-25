using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Utils.Logging;
using System;

namespace ResourceTypes.Cutscene.KeyParams
{
    public static class CutsceneKeyParamFactory
    {
        // Finds the correct type of KeyParam and returns the data.
        // TODO: Ideally move this to our friend MemoryStream.
        public static IKeyType ReadAnimEntityFromFile(BinaryReader br)
        {
            IKeyType KeyParam = null;

            AnimKeyParamTypes KeyParamType = (AnimKeyParamTypes)br.ReadInt32();

            switch (KeyParamType)
            {
                case AnimKeyParamTypes.KeyType_0:
                    KeyParam = new KeyType_0();
                    break;
                case AnimKeyParamTypes.KeyType_6:
                    KeyParam = new KeyType_6();
                    break;
                case AnimKeyParamTypes.KeyType_7:
                    KeyParam = new KeyType_7();
                    break;
                case AnimKeyParamTypes.KeyType_13:
                    KeyParam = new KeyType_13();
                    break;
                case AnimKeyParamTypes.KeyType_18:
                    KeyParam = new KeyType_18();
                    break;
                case AnimKeyParamTypes.KeyType_19:
                    KeyParam = new KeyType_19();
                    break;
                case AnimKeyParamTypes.KeyType_21:
                    KeyParam = new KeyType_21();
                    break;
                case AnimKeyParamTypes.KeyType_27:
                    KeyParam = new KeyType_27();
                    break;
                case AnimKeyParamTypes.KeyType_28:
                    KeyParam = new KeyType_28();
                    break;
                //case AnimKeyParamTypes.KeyType_39:
                //    KeyParam = new KeyType_39();
                //    break;
                case AnimKeyParamTypes.KeyType_40:
                    KeyParam = new KeyType_40();
                    break;
                case AnimKeyParamTypes.KeyType_43:
                    KeyParam = new KeyType_43();
                    break;
                case AnimKeyParamTypes.KeyWeatherParam:
                    KeyParam = new KeyWeatherParam();
                    break;
                default:
                    KeyParam = new KeyType_Temp();
                    break;
            }

            //KeyParam = new KeyType_Temp(); //Some keys were broken, don't wanna be fixing those rn

            // We should have our type, lets add our type and size to the KeyParameters and then begin reading them from the file.
            KeyParam.KeyType = (int)KeyParamType;

            KeyParam.ReadFromFile(br);

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, $"Failed to read key type: {KeyParamType}");

            return KeyParam;
        }

        private static void DumpKeyData(IKeyType KeyParam, BinaryReader br)
        {
            string folderPath = "%userprofile%\\Desktop\\KeyParams";
            string path = Environment.ExpandEnvironmentVariables(folderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            switch (KeyParam.KeyType)
            {
                case 27:
                    var data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                    File.WriteAllBytes(Path.Combine(path, $"KeyParam_Type_{KeyParam.KeyType}_{data.GetHashCode()}.bin"), data);
                    br.BaseStream.Position = 4;
                    break;
            }
        }
    }
}
