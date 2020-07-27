using System;
using System.IO;

namespace Core.IO
{
    public class FileFactory
    {
        public static FileBase ConstructFromFileInfo(FileInfo info)
        {
            string extension = info.Extension.Replace(".", "").ToUpper();
            switch (extension)
            {
                case "DDS":
                    return new FileTextureDDS(info);
                case "SDS":
                    return new FileSDS(info);
                case "FR":
                    return new FileFrameResource(info);
                case "MTL":
                    return new FileMaterialLibrary(info);
                case "LUA":
                case "SHP":
                case "AP":
                    return new FileLua(info);
                case "EDS":
                    return new FileEntityDataStorage(info);
                case "SPE":
                    return new FileSpeech(info);
                case "TBL":
                    return new FileTable(info);
                case "TRA":
                    return new FileTranslokator(info);
                case "ACT":
                    return new FileActor(info);
                case "XML":
                    return new FileXML(info);
                case "BIN":
                    return new FileBin(info);
                default:
                    Console.WriteLine(string.Format("Default selected. info.Extension is {0}", info.Extension));
                    return new FileBase(info);
            }
        }

    }
}
