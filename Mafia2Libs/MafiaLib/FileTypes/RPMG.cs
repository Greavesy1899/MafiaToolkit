using System.IO;

namespace ResourceTypes.Misc
{
    public class RPMG
    {
        public RPMG(BinaryReader reader)
        {
            Read(reader);
        }

        public void Read(BinaryReader reader)
        {
            reader.ReadBytes(4); //header
            int count = reader.ReadInt32();

            reader.ReadInt32();
            type1Chunk type1 = new type1Chunk();
            type1.ReadFromFile(reader);

            reader.ReadInt32();
            type2Chunk type2 = new type2Chunk();
            type2.ReadFromFile(reader);

            reader.ReadInt32();
            type3Chunk type3 = new type3Chunk();
            type3.ReadFromFile(reader);

            reader.ReadInt32();
            type4Chunk type4 = new type4Chunk();
            type4.ReadFromFile(reader);

            reader.ReadInt32();
            type5Chunk type5 = new type5Chunk();
            type5.ReadFromFile(reader);
        }

        public class type1Chunk
        {
            int stringLength;
            string[] paths;

            public void ReadFromFile(BinaryReader reader)
            {
                stringLength = reader.ReadInt32();
                paths = new string(reader.ReadChars(stringLength)).Split('\0');
            }
        }

        public class type2Chunk
        {
            long unk01;

            public void ReadFromFile(BinaryReader reader)
            {
                unk01 = reader.ReadInt64();
            }
        }

        public class type3Chunk
        {
            //needs work.
            int stringLength;
            string unsplitPath;

            public void ReadFromFile(BinaryReader reader)
            {
                stringLength = reader.ReadInt32();
                unsplitPath = new string(reader.ReadChars(stringLength));
            }

        }

        public class type4Chunk
        {
            int unk1;
            short unk2;

            public void ReadFromFile(BinaryReader reader)
            {
                unk1 = reader.ReadInt32();
                unk2 = reader.ReadInt16();
            }
        }

        public class type5Chunk
        {
            int stringLength;
            string[] paths;

            public void ReadFromFile(BinaryReader reader)
            {
                stringLength = reader.ReadInt32();
                paths = new string(reader.ReadChars(stringLength)).Split('\0');
            }
        }
    }
}
