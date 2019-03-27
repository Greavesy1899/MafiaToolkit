using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.AnimatedTexture
{
    public class AnimatedTextureLoader
    {
        private ulong fileHash;
        private string fileName;
        private int unk0;
        private int textureCount;
        private TextureAnim[] textures;

        public AnimatedTextureLoader(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            fileHash = reader.ReadUInt64();
            fileName = StringHelpers.ReadString16(reader);
            unk0 = reader.ReadInt32();
            textureCount = reader.ReadInt32();

            textures = new TextureAnim[textureCount];
            for(int i = 0; i != textureCount; i++)
                textures[i] = new TextureAnim(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(fileHash);
            writer.Write((short)fileName.Length);
            writer.Write(fileName.ToCharArray());
            writer.Write(unk0);
            writer.Write(textureCount);
            for (int i = 0; i != textureCount; i++)
                textures[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("Hash: {0}, Name: {1}, Count: {2}", fileHash, fileName, textureCount);
        }

        public class TextureAnim
        {
            private ulong hash;
            private string name;
            private short flag;

            public TextureAnim(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();
                name = StringHelpers.ReadString16(reader);
                flag = reader.ReadInt16();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(hash);
                writer.Write((short)name.Length);
                writer.Write(name.ToCharArray());
                writer.Write(flag);
            }

            public override string ToString()
            {
                return string.Format("Hash: {0}, Name: {1}, Flag: {2}", hash, name, flag);
            }
        }
    }
}
