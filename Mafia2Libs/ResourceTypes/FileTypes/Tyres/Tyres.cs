using System.IO;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Tyres
{
    public class Tyres
    {
        private static uint Magic = 0x12345678;
        public int Version { get; set; } = 1;
        public Tyre TyreData { get; set; } = new();
        public Tyres()
        {

        }
        public Tyres(FileInfo InFileInfo)
        {
            using (MemoryStream reader = new(File.ReadAllBytes(InFileInfo.FullName)))
            {
                Read(reader);
            }
        }

        public void Read(MemoryStream stream)
        {
            uint _magic = stream.ReadUInt32(false);

            if (_magic != Magic)
            {
                throw new System.Exception("Wrong file type.");
            }

            Version = stream.ReadInt32(false);
            int initBlockOffset = stream.ReadInt32(false);
            int littleEndianBlockOffset = stream.ReadInt32(false);
            int littleEndianBlockSize = stream.ReadInt32(false);
            int bigEndianBlockOffset = stream.ReadInt32(false);
            int bigEndianBlockSize = stream.ReadInt32(false);
            TyreData = new(new MemoryStream(stream.ReadBytes(littleEndianBlockSize)));
            //Big endian data is identical to little endian so we just skip reading it
        }

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                Write(ms);
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(MemoryStream stream)
        {
            stream.Write(Magic, false);
            stream.Write(Version, false);
            stream.Write(28, false);
            stream.Write(28, false);

            using (MemoryStream ms = new())
            {
                TyreData.Write(ms, false);
                stream.Write((int)ms.Length, false);
                stream.Write((int)(ms.Length + 28), false);
                stream.Write((int)ms.Length, false);
                stream.Write(ms.ToArray());
            }

            using (MemoryStream ms = new())
            {
                TyreData.Write(ms, true);
                stream.Write(ms.ToArray());
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
            Tyres FileContents = ReflectionHelpers.ConvertToPropertyFromXML<Tyres>(LoadedDoc);

            // Copy data taken from loaded XML
            Version = FileContents.Version;
            TyreData = FileContents.TyreData;
        }
    }
}
