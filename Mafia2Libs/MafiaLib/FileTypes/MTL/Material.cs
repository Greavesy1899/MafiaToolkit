using System;
using System.IO;
using System.ComponentModel;
using Gibbed.Illusion.FileFormats.Hashing;
using System.Collections.Generic;

namespace Mafia2
{
    public class Material
    {
        ulong materialHash;
        string materialName;

        byte ufo1;
        byte ufo2;
        MaterialFlags flags;
        byte ufo4;
        int ufo5;
        int ufo6;

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

        [Category("Flags"), Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public MaterialFlags Flags {
            get { return flags; }
            set { flags = value; }
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

        [Category("Shader")]
        public Dictionary<string, ShaderParameter> Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

        [Category("Shader")]
        public Dictionary<string, ShaderParameterSampler> Samplers {
            get { return samplers; }
            set { samplers = value;}
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
            shaderHash = 3388704532;
            shaderID = 4894707398632176459;
            ufo1 = 128;
            ufo2 = 0;
            flags = (MaterialFlags)31461376;
            ufo4 = 0;
            ufo5 = 0;
            ufo6 = 0;
            parameters = new Dictionary<string, ShaderParameter>();
            samplers = new Dictionary<string, ShaderParameterSampler>();
            var spp = new ShaderParameterSampler();
            samplers.Add(spp.ID, spp);
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
            materialName = Functions.ReadString32(reader);

            ufo1 = reader.ReadByte();
            ufo2 = reader.ReadByte();
            flags = (MaterialFlags)reader.ReadInt32();
            ufo4 = reader.ReadByte();
            ufo5 = reader.ReadInt32();
            ufo6 = reader.ReadInt32();

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
                var shader = new ShaderParameterSampler(reader);
                samplers.Add(shader.ID, shader);
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
            writer.Write((int)flags);
            writer.Write(ufo4);
            writer.Write(ufo5);
            writer.Write(ufo6);

            ////Shader and flags
            writer.Write(shaderID);
            writer.Write(shaderHash);

            ////Shader Parameter
            writer.Write(parameters.Count);
            foreach (KeyValuePair<string, ShaderParameter> param in parameters)
            {
                param.Value.WriteToFile(writer);
            }

            ////Shader Parameter Samplers
            writer.Write(samplers.Count);
            foreach (KeyValuePair<string, ShaderParameterSampler> shader in samplers)
            {
                shader.Value.WriteToFile(writer);
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
