using System.IO;

namespace Core.IO
{
    public class FileFactory
    {
        public static FileBase ConstructFromFileInfo(FileInfo info)
        {
            string extension = info.Extension.Replace(".", "").ToUpper();
            FileBase File = null;

            switch (extension)
            {
                case "XBIN":
                    File = new FileXBin(info);
                    break;
                case "XLAYBIN":
                    File = new FileXlaybin(info);
                    break;
                case "CUT":
                    return new FileCutscene(info);
                case "DDS":
                    File = new FileTextureDDS(info);
                    break;
                case "SDS":
                    File = new FileSDS(info);
                    break;
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
                    File = new FileXML(info);
                    break;
                case "BIN":
                    return new FileBin(info);
                case "PRF":
                    return new FilePrefab(info);
                case "NHV":
                    return new FileNavigation_HPD(info);
                case "NAV":
                    return new FileNavigation_NAV(info);
                case "NOV":
                    return new FileNavigation_OBJ(info);
                case "FXA":
                    return new FileFxActor(info);
                case "FAS":
                    return new FileFxAnimSet(info);
                case "PCK":
                    return new FilePCK(info);
                case "BNK":
                    return new FileBNK(info);
                case "STBL":
                    return new FileSoundTable(info);
                default:
                    return new FileBase(info);
            }

            return File;
        }

    }
}
