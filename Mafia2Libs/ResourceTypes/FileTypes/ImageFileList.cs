using System.IO;
using System.Xml.Linq;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.ImageFileList
{
    public class ImageFileList
    {
        public class Image
        {
            public string Name { get; set; }
            public Image()
            {

            }

            public Image(MemoryStream reader, bool isBigEndian)
            {
                Read(reader, isBigEndian);
            }

            public void Read(MemoryStream reader, bool isBigEndian)
            {
                reader.ReadUInt64(isBigEndian); //Hash
                Name = reader.ReadString16(isBigEndian);
                reader.ReadInt16(isBigEndian); //Const 50
            }

            public void Write(MemoryStream OutStream, bool bIsBigEndian)
            {
                OutStream.Write(FNV64.Hash(Name), bIsBigEndian);
                OutStream.WriteString16(Name, bIsBigEndian);
                OutStream.Write((short)50, bIsBigEndian);
            }
        }

        public string Name { get; set; }
        public int Unk00 { get; set; }
        public Image[] Images { get; set; } = new Image[0];
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            reader.ReadUInt64(isBigEndian); //Hash
            Name = reader.ReadString16(isBigEndian);
            Unk00 = reader.ReadInt32(isBigEndian);
            int Count = reader.ReadInt32(isBigEndian);
            Images = new Image[Count];

            for (int i = 0; i < Images.Length; i++)
            {
                Images[i] = new Image(reader, isBigEndian);
            }
        }

        public void WriteToFile(MemoryStream OutStream, bool bIsBigEndian)
        {
            OutStream.Write(FNV64.Hash(Name), bIsBigEndian);
            OutStream.WriteString16(Name, bIsBigEndian);
            OutStream.Write(Unk00, bIsBigEndian);
            OutStream.Write(Images.Length, bIsBigEndian);

            foreach (var img in Images)
            {
                img.Write(OutStream, bIsBigEndian);
            }
        }

        public void WriteToFile(string FileName, bool bIsBigEndian)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                WriteToFile(outStream, bIsBigEndian);
                File.WriteAllBytes(FileName, outStream.ToArray());
            }
        }

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            ImageFileList FileContents = ReflectionHelpers.ConvertToPropertyFromXML<ImageFileList>(LoadedDoc);

            // Copy data taken from loaded XML
            Name = FileContents.Name;
            Unk00 = FileContents.Unk00;
            Images = FileContents.Images;
        }
    }
}
