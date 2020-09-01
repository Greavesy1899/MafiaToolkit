using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.IO;

namespace ResourceTypes.Materials
{
    public class MaterialSampler
    {
        string id;
        int[] unkSet0;
        ulong textureHash;
        byte texType;
        byte unkZero;
        byte[] samplerStates;
        int[] unkSet1;
        string file;

        public string ID {
            get { return id; }
            set { id = value; }
        }
        public int[] UnkSet0 {
            get { return unkSet0; }
            set { unkSet0 = value; }
        }
        public ulong TextureHash {
            get { return textureHash; }
            set { textureHash = value; }
        }
        public byte TexType {
            get { return texType; }
            set { texType = value; }
        }
        public byte UnkZero {
            get { return unkZero; }
            set { unkZero = value; }
        }
        public byte[] SamplerStates {
            get { return samplerStates; }
            set { samplerStates = value; }
        }
        public int[] UnkSet1 {
            get { return unkSet1; }
            set { unkSet1 = value; }
        }
        public string File {
            get { return file; }
            set { SetName(value); }
        }

        public MaterialSampler(BinaryReader reader, VersionsEnumerator version)
        {
            ReadFromFile(reader, version);
        }

        public MaterialSampler()
        {
            id = "S000";
            file = "null.dds";
            textureHash = 1;
            unkSet0 = new int[2];
            unkSet1 = new int[2];
            samplerStates = new byte[6] { 3, 3, 2, 0, 0, 0 };
            texType = 2;
            UnkZero = 0;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", id, file);
        }

        public void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            id = new string(reader.ReadChars(4));
            unkSet0 = new int[version == VersionsEnumerator.V_58 ? 4 : 2];
            for (int i = 0; i < unkSet0.Length; i++)
            {
                unkSet0[i] = reader.ReadInt32();
            }
            textureHash = reader.ReadUInt64();
            texType = reader.ReadByte();
            unkZero = reader.ReadByte();
            samplerStates = reader.ReadBytes(6);
            unkSet1 = new int[2]; //these can have erratic values
            for (int i = 0; i < unkSet1.Length; i++)
            {
                unkSet1[i] = reader.ReadInt32();
            }
            int fileLength = reader.ReadInt32();
            file = new string(reader.ReadChars(fileLength));
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(id.ToCharArray());
            for (int i = 0; i < unkSet0.Length; i++)
            {
                writer.Write(unkSet0[i]);
            }
            writer.Write(textureHash);
            writer.Write(texType);
            writer.Write(UnkZero);

            if (samplerStates == null)
                writer.Write(new byte[] { 3, 3, 2, 0, 0, 0 });
            else
                writer.Write(samplerStates);

            for (int i = 0; i < unkSet1.Length; i++)
            {
                writer.Write(unkSet1[i]);
            }
            writer.Write(file.Length);
            writer.Write(file.ToCharArray());
        }

        public void SetName(string name)
        {
            file = name;
            textureHash = FNV64.Hash(name);
        }
    }
}
