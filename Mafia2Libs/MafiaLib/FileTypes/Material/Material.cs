using System.IO;
using System.ComponentModel;
using Gibbed.Illusion.FileFormats.Hashing;
using System.Collections.Generic;
using System.Linq;
using Utils.StringHelpers;
using Utils.Extensions;
using System;

namespace ResourceTypes.Materials
{
    public class Material
    {
        ulong materialHash;
        string materialName;

        byte unk0;
        byte unk1;
        byte unk2;
        MaterialFlags flags;
        byte unk3;
        int unk4;
        int unk5;
        byte unk6;
        float unk7;

        ulong shaderID;
        uint shaderHash;

        Dictionary<string, ShaderParameter> parameters;
        Dictionary<string, ShaderParameterSampler> samplers;

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

        [Category("Unknown")]
        public byte Unk0 {
            get { return unk0; }
            set { unk0 = value; }
        }

        [Category("Unknown")]
        public byte Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }

        [Category("Unknown")]
        public byte Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }

        [Category("Flags"), Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public MaterialFlags Flags {
            get { return flags; }
            set { flags = value; }
        }

        [Category("Unknown"), Description("Only used in v58. (Mafia II: DE)")]
        public byte Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }

        [Category("Unknown")]
        public int Unk4 {
            get { return unk4; }
            set { unk4 = value; }
        }

        [Category("Unknown")]
        public int Unk5 {
            get { return unk5; }
            set { unk5 = value; }
        }
        [Category("Unknown"), Description("Only used in v58. (Mafia II: DE)")]
        public byte Unk6 {
            get { return unk6; }
            set { unk6 = value; }
        }
        [Category("Unknown"), Description("Only used in v58. (Mafia II: DE)")]
        public float Unk7 {
            get { return unk7; }
            set { unk7 = value; }
        }

        [Category("Shader")]
        public ulong ShaderID {
            get { return shaderID; }
            set { shaderID = value; }
        }

        [Category("Shader")]
        public uint ShaderHash {
            get { return shaderHash; }
            set { shaderHash = value; }
        }

        [Browsable(false)]
        public Dictionary<string, ShaderParameter> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

        [Browsable(false)]
        public Dictionary<string, ShaderParameterSampler> Samplers {
            get { return samplers; }
            set { samplers = value; }
        }

        [Category("Shader")]
        public List<ShaderParameter> ParametersList {
            get { return parameters.Values.ToList(); }
        }

        [Category("Shader")]
        public List<ShaderParameterSampler> SamplersList {
            get { return samplers.Values.ToList(); }
        }

        /// <summary>
        /// Construct material and read data.
        /// </summary>
        /// <param name="reader"></param>
        public Material(BinaryReader reader, VersionsEnumerator version)
        {
            ReadFromFile(reader, version);
        }

        /// <summary>
        /// Construct material based on basic material.
        /// </summary>
        public Material()
        {
            materialName = "NEW_MATERIAL";
            materialHash = 1;
            shaderHash = 3388704532;
            shaderID = 4894707398632176459;
            unk0 = 128;
            unk1 = 0;
            unk2 = 0;
            flags = (MaterialFlags)31461376;
            unk3 = 0;
            unk4 = 0;
            unk5 = 0;
            unk6 = 0;
            unk7 = 1.0f;
            parameters = new Dictionary<string, ShaderParameter>();
            samplers = new Dictionary<string, ShaderParameterSampler>();
            var spp = new ShaderParameterSampler();
            samplers.Add(spp.ID, spp);
        }

        public override string ToString()
        {
            return string.Format("{0}", materialName);
        }

        /// <summary>
        /// Read material from library.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {

            materialHash = reader.ReadUInt64();
            materialName = StringHelpers.ReadString32(reader);

            unk0 = reader.ReadByte();
            unk1 = reader.ReadByte();

            if (version == VersionsEnumerator.V_58)
            {
                unk2 = reader.ReadByte();
            }

            flags = (MaterialFlags)reader.ReadInt32();
            unk3 = reader.ReadByte();
            unk4 = reader.ReadInt32();
            unk5 = reader.ReadInt32();

            if (version == VersionsEnumerator.V_58)
            {
                unk6 = reader.ReadByte();
                unk7 = reader.ReadSingle();
            }

            shaderID = reader.ReadUInt64();
            shaderHash = reader.ReadUInt32();

            int spCount = reader.ReadInt32();
            parameters = new Dictionary<string, ShaderParameter>();
            for (int i = 0; i != spCount; i++)
            {
                var param = new ShaderParameter(reader);
                parameters.Add(param.ID, param);
            }

            int spsCount = reader.ReadInt32();
            samplers = new Dictionary<string, ShaderParameterSampler>();
            for (int i = 0; i != spsCount; i++)
            {
                var shader = new ShaderParameterSampler(reader, version);
                samplers.Add(shader.ID, shader);
            }
        }

        public void WriteToFile(BinaryWriter writer, VersionsEnumerator version)
        {
            //material hash code and name.
            writer.Write(materialHash);
            writer.Write(materialName.Length);
            writer.Write(materialName.ToCharArray());

            //unknown values
            writer.Write(unk0);
            writer.Write(unk1);
            if (version == VersionsEnumerator.V_58)
            {
                writer.Write(unk2);
            }
            writer.Write((int)flags);
            writer.Write(unk3);
            writer.Write(unk4);
            writer.Write(unk5);
            if (version == VersionsEnumerator.V_58)
            {
                writer.Write(unk6);
                writer.Write(unk7);
            }
            //Shader and flags
            writer.Write(shaderID);
            writer.Write(shaderHash);

            //Shader Parameter
            writer.Write(parameters.Count);
            foreach (KeyValuePair<string, ShaderParameter> param in parameters)
            {
                param.Value.WriteToFile(writer);
            }

            //Shader Parameter Samplers
            writer.Write(samplers.Count);
            foreach (KeyValuePair<string, ShaderParameterSampler> shader in samplers)
            {
                shader.Value.WriteToFile(writer);
            }
        }

        public void SetName(string name)
        {
            materialName = name;
            materialHash = FNV64.Hash(name);
        }
    }

    public struct ShaderParameter
    {
        string id;
        int paramCount;
        float[] paramaters;

        public string ID {
            get { return id; }
            set { id = value; }
        }
        public float[] Paramaters {
            get { return paramaters; }
            set {
                paramaters = value;
                paramCount = value.Length;
            }
        }

        public ShaderParameter(BinaryReader reader)
        {
            id = new string(reader.ReadChars(4));
            paramCount = reader.ReadInt32() / 4;
            paramaters = new float[paramCount];
            for (int i = 0; i != paramCount; i++)
            {
                paramaters[i] = reader.ReadSingle();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", id, paramCount);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(id.ToCharArray());
            writer.Write(paramCount * 4);
            for (int i = 0; i != paramCount; i++)
            {
                writer.Write(paramaters[i]);
            }
        }
    }

    public class ShaderParameterSampler
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

        public ShaderParameterSampler(BinaryReader reader, VersionsEnumerator version)
        {
            ReadFromFile(reader, version);
        }

        public ShaderParameterSampler()
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
            for(int i = 0; i < unkSet0.Length; i++)
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

    [Flags]
    public enum MaterialFlags : uint //No idea which ones are used.
    {
        flag0 = 0x10,
        flag_1 = 1,
        Alpha = 2,
        flag_4 = 4,
        flag_8 = 8,
        Disable_ZWriting = 16,
        flag_32 = 32,
        flag_64 = 64,
        flag_128 = 128,
        flag_256 = 256,
        flag_512 = 512,
        flag_1024 = 1024,
        flag_2048 = 2048,
        CastShadows = 4096,
        flag_8192 = 8192,
        flag_16384 = 16384,
        flag_32768 = 32768,
        flag_65536 = 65536,
        flag_131072 = 131072,
        flag_262144 = 262144,
        flag_524288 = 524288,
        flag_1048576 = 1048576,
        flag_2097152 = 2097152,
        flag_4194304 = 4194304,
        flag_8388608 = 8388608,
        flag_16777216 = 16777216,
        flag_33554432 = 33554432,
        flag_67108864 = 67108864,
        flag_134217728 = 134217728,
        flag_268435456 = 268435456,
        flag_536870912 = 536870912,
        flag_1073741824 = 1073741824
        //flag_2147483648 = 2147483648
    }
}
