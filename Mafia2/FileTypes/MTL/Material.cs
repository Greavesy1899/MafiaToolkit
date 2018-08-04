using System;
using System.IO;
using System.ComponentModel;

namespace Mafia2
{
    public class Material
    {

        ulong materialNumID;
        string materialNumStr;
        string materialName;

        byte ufo1;
        byte ufo2;
        int ufo3;
        byte ufo4;
        int ufo5;
        int ufo6;

        ulong shaderID;
        string shaderName;

        uint flags;

        int sp_count;
        ShaderParameter[] sp;

        int sps_count;
        ShaderParameterSampler[] sps;

        [Category("Material IDs"), ReadOnly(true)]
        public ulong MaterialNumID {
            get { return materialNumID; }
            set { materialNumID = value; }
        }
        [Category("Material IDs")]
        public string MaterialNumStr {
            get { return materialNumStr; }
            set { materialNumStr = value; }
        }
        [Category("Material IDs")]
        public string MaterialName {
            get { return materialName; }
            set { materialName = value; }
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

        [Category("Shaders")]
        public string ShaderName {
            get { return shaderName; }
            set { shaderName = value; }
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

        public Material(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public override string ToString()
        {
            return string.Format("{0}\t{1}", materialNumStr, materialName);
        }

        public void ReadFromFile(BinaryReader reader)
        {

            materialNumID = reader.ReadUInt64();
            materialNumStr = string.Format("{0:X16}", materialNumID.Swap());

            int nameLength = reader.ReadInt32();
            materialName = new string(reader.ReadChars(nameLength));

            ufo1 = reader.ReadByte();
            ufo2 = reader.ReadByte();
            ufo3 = reader.ReadInt32();
            ufo4 = reader.ReadByte();
            ufo5 = reader.ReadInt32();
            ufo6 = reader.ReadInt32();

            shaderID = reader.ReadUInt64().Swap();
            shaderName = string.Format("{0:X16}", shaderID);

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

        public void WriteToFile(BinaryWriter writer)
        {
            ////material hash code and name.
            writer.Write(materialNumID);
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
            writer.Write(shaderID.Swap());
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

    public struct ShaderParameterSampler
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
            set { file = value; }
        }

        public ShaderParameterSampler(BinaryReader reader)
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

        public override string ToString()
        {
            return string.Format("{0}, {1}", id, file);
        }

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
    }
}
