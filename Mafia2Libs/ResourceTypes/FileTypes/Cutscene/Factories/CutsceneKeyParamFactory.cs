﻿using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public static class CutsceneKeyParamFactory
    {
        // Finds the correct type of KeyParam and returns the data.
        // TODO: Ideally move this to our friend MemoryStream.
        public static IKeyType ReadAnimEntityFromFile(AnimKeyParamTypes KeyParamType, int Size, MemoryStream Reader)
        {
            IKeyType KeyParam = null;
            bool isBigEndian = false;

            
            switch (KeyParamType)
            {
                case AnimKeyParamTypes.KeyType_0:
                    KeyParam = new KeyType_0();
                    break;
                case AnimKeyParamTypes.KeyType_6:
                    KeyParam = new KeyType_6();
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
                case AnimKeyParamTypes.KeyType_39:
                    KeyParam = new KeyType_39();
                    break;
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

            KeyParam = new KeyType_Temp(); //Some keys were broken, don't wanna be fixing those rn

            // We should have our type, lets add our type and size to the KeyParameters and then begin reading them from the file.
            KeyParam.KeyType = (int)KeyParamType;
            KeyParam.Size = Size;
            KeyParam.ReadFromFile(Reader, isBigEndian);

            return KeyParam;
        }
    }
}
