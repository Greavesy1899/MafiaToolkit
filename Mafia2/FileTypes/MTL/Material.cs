using System;
using System.IO;
using System.ComponentModel;
using Gibbed.Illusion.FileFormats.Hashing;

namespace Mafia2
{
    public class Material
    {

        ulong materialHash;
        string materialName;

        byte ufo1;
        byte ufo2;
        int ufo3;
        byte ufo4;
        int ufo5;
        int ufo6;

        ulong shaderID;

        uint flags;

        int sp_count;
        ShaderParameter[] sp;

        int sps_count;
        ShaderParameterSampler[] sps;

        [Category("Material IDs"), ReadOnly(true)]
        public ulong MaterialHash {
            get { return materialHash; }
            set { materialHash = value; }
        }

        [Category("Material IDs")]
        public string MaterialName {
            get { return materialName; }
            set { SetName(value); }
        }

        [Category("UFOs")]
        public byte UFO1 {
            get { return ufo1; }
            set { ufo1 = value; }
        }

        [Category("UFOs")]
        public byte UFO2 {
            get { return ufo2; }
            set { ufo2 = value; }
        }

        [Category("UFOs")]
        public int UFO3 {
            get { return ufo3; }
            set { ufo3 = value; }
        }

        [Category("UFOs")]
        public byte UFO4 {
            get { return ufo4; }
            set { ufo4 = value; }
        }

        [Category("UFOs")]
        public int UFO5 {
            get { return ufo5; }
            set { ufo5 = value; }
        }

        [Category("UFOs")]
        public int UFO6 {
            get { return ufo6; }
            set { ufo6 = value; }
        }

        [Category("Shaders")]
        public ulong ShaderID {
            get { return shaderID; }
            set { shaderID = value; }
        }

        [Category("Flags")]
        public uint Flags {
            get { return flags; }
            set { flags = value; }
        }

        [Category("SP")]
        public ShaderParameter[] SP {
            get { return sp; }
            set {
                sp = value;
                sp_count = value.Length;
            }
        }

        [Category("SPS")]
        public ShaderParameterSampler[] SPS {
            get { return sps; }
            set {
                sps = value;
                sps_count = value.Length;
            }
        }

        /// <summary>
        /// Construct material and read data.
        /// </summary>
        /// <param name="reader"></param>
        public Material(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Construct material based on basic material.
        /// </summary>
        public Material()
        {
            materialName = "NEW_MATERIAL";
            materialHash = 1;
            flags = 3388704532;
            shaderID = 4894707398632176459;
            ufo1 = 128;
            ufo2 = 0;
            ufo3 = 31461376;
            ufo4 = 0;
            ufo5 = 0;
            ufo6 = 0;
            sp_count = 0;
            sp = new ShaderParameter[0];
            sps_count = 1;
            sps = new ShaderParameterSampler[1];
            sps[0] = new ShaderParameterSampler();
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", materialHash, materialName);
        }

        /// <summary>
        /// Read material from library.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {

            materialHash = reader.ReadUInt64();

            int nameLength = reader.ReadInt32();
            materialName = new string(reader.ReadChars(nameLength));

            ufo1 = reader.ReadByte();
            ufo2 = reader.ReadByte();
            ufo3 = reader.ReadInt32();
            ufo4 = reader.ReadByte();
            ufo5 = reader.ReadInt32();
            ufo6 = reader.ReadInt32();

            shaderID = reader.ReadUInt64();

            flags = reader.ReadUInt32();

            sp_count = reader.ReadInt32();
            sp = new ShaderParameter[sp_count];
            for (int i = 0; i != sp_count; i++)
            {
                sp[i] = new ShaderParameter(reader);
            }

            sps_count = reader.ReadInt32();
            sps = new ShaderParameterSampler[sps_count];
            for (int i = 0; i != sps_count; i++)
            {
                sps[i] = new ShaderParameterSampler(reader);
            }

        }

        /// <summary>
        /// Write material to library.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            ////material hash code and name.
            writer.Write(materialHash);
            writer.Write(materialName.Length);
            writer.Write(materialName.ToCharArray());

            ////UFO values
            writer.Write(ufo1);
            writer.Write(ufo2);
            writer.Write(ufo3);
            writer.Write(ufo4);
            writer.Write(ufo5);
            writer.Write(ufo6);

            ////Shader and flags
            writer.Write(shaderID);
            writer.Write((uint)flags);

            ////Shader Parameter
            writer.Write(sp_count);
            foreach (ShaderParameter sp in sp)
            {
                sp.WriteToFile(writer);
            }

            ////Shader Parameter Samplers
            writer.Write(sps_count);
            foreach (ShaderParameterSampler sps in sps)
            {
                sps.WriteToFile(writer);
            }
        }

        /// <summary>
        /// Set shader sampler name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            materialName = name;
            materialHash = FNV64.Hash(name);
        }
    }

    public struct ShaderParameter
    {

        string chunk;
        int floatCount;
        float[] floats;

        public string Chunk {
            get { return chunk; }
            set { chunk = value; }
        }
        public float[] Floats {
            get { return floats; }
            set {
                floats = value;
                floatCount = value.Length;
            }
        }

        public ShaderParameter(BinaryReader reader)
        {

            chunk = new string(reader.ReadChars(4));

            floatCount = reader.ReadInt32() / 4;

            floats = new float[floatCount];
            for (int i = 0; i != floatCount; i++)
            {
                floats[i] = reader.ReadSingle();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", chunk, floatCount);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(chunk.ToCharArray());
            writer.Write(floatCount * 4);
            foreach (float f in floats)
            {
                writer.Write(f);
            }
        }
    }

    public class ShaderParameterSampler
    {

        string id;
        int[] ufo_x1;
        ulong textureHash;
        byte texType;
        byte unkZero;
        byte[] samplerStates;
        int[] ufo_x2;
        string file;

        public string ID {
            get { return id; }
            set { id = value; }
        }
        public int[] UFO_X1 {
            get { return ufo_x1; }
            set { ufo_x1 = value; }
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
        public int[] UFO_X2 {
            get { return ufo_x2; }
            set { ufo_x2 = value; }
        }
        public string File {
            get { return file; }
            set { SetName(value); }
        }

        /// <summary>
        /// Construct sampler data on read data.
        /// </summary>
        /// <param name="reader"></param>
        public ShaderParameterSampler(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public ShaderParameterSampler()
        {
            id = "S000";
            file = "null.dds";
            textureHash = 1;
            ufo_x1 = new int[2];
            ufo_x2 = new int[2];
            samplerStates = new byte[6] { 3, 3, 2, 0, 0, 0 };
            texType = 2;
            UnkZero = 0;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", id, file);
        }

        /// <summary>
        /// Read data to library.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            id = new string(reader.ReadChars(4));
            ufo_x1 = new int[2];
            ufo_x1[0] = reader.ReadInt32();
            ufo_x1[1] = reader.ReadInt32();
            textureHash = reader.ReadUInt64();
            texType = reader.ReadByte();
            unkZero = reader.ReadByte();
            samplerStates = reader.ReadBytes(6);
            ufo_x2 = new int[2]; //these can have erratic values
            ufo_x2[0] = reader.ReadInt32();
            ufo_x2[1] = reader.ReadInt32();
            int fileLength = reader.ReadInt32();
            file = new string(reader.ReadChars(fileLength));
        }

        /// <summary>
        /// write data to library.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(id.ToCharArray());
            writer.Write(ufo_x1[0]);
            writer.Write(ufo_x1[1]);
            writer.Write(textureHash);
            writer.Write(texType);
            writer.Write(UnkZero);

            if (samplerStates == null)
                writer.Write(new byte[] { 3, 3, 2, 0, 0, 0 });
            else
                writer.Write(samplerStates);

            writer.Write(ufo_x2[0]);
            writer.Write(ufo_x2[1]);
            writer.Write(file.Length);
            writer.Write(file.ToCharArray());
        }

        /// <summary>
        /// Set shader sampler name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            file = name;
            textureHash = FNV64.Hash(name);
        }
    }
}
