using System.IO;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.CGame
{
    public class CGame
    {
        public static uint Magic = 0x676D7072;
        public IGameChunk[] Chunks { get; set; } = new IGameChunk[0];
        public CGame()
        {

        }

        public CGame(FileInfo InFileInfo)
        {
            Read(InFileInfo.FullName);
        }

        public CGame(string fileName)
        {
            Read(fileName);
        }

        public CGame(Stream s)
        {
            Read(s);
        }

        public CGame(BinaryReader br)
        {
            Read(br);
        }

        public void Read(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            uint _magic = br.ReadUInt32();

            if (Magic != _magic)
            {
                throw new System.Exception("Not a Game file.");
            }

            int ChunkCount = br.ReadInt32();
            Chunks = new IGameChunk[ChunkCount];

            for (int i = 0; i < Chunks.Length; i++)
            {
                Chunks[i] = GameChunkFactory.ReadFromFile(br);
            }
        }

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                Write(ms);
                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(MemoryStream ms)
        {
            using (BinaryWriter bw = new(ms))
            {
                Write(bw);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Magic);
            bw.Write(Chunks.Length);

            foreach (var chunk in Chunks)
            {
                bw.Write(chunk.GetChunkType());
                chunk.Write(bw);
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
            CGame FileContents = ReflectionHelpers.ConvertToPropertyFromXML<CGame>(LoadedDoc);

            // Copy data taken from loaded XML
            Chunks = FileContents.Chunks;
        }
    }
}
