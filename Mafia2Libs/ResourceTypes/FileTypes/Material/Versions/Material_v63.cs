﻿using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Materials
{
    public class Material_v63 : IMaterial
    {
       
       
        public uint Unk0 { get; set; }
        public byte[] Unk1 { get; set; }
        public byte[] Unk2 { get; set; }
        public List<MaterialTexture> Textures { get; set; }
        public List<MaterialSampler_v63> Samplers { get; set; }

        public Material_v63() : base()
        {
            Unk1 = new byte[2];
            Unk2 = new byte[9];
            Samplers = new List<MaterialSampler_v63>();
            Textures = new List<MaterialTexture>();
        }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ulong materialHash = reader.ReadUInt64();
            string materialName = StringHelpers.ReadString32(reader);
            MaterialName.String = materialName;
            MaterialName.Hash = materialHash;
            Unk0 = reader.ReadUInt32();
            Unk1 = reader.ReadBytes(2);
            Flags = (MaterialFlags)reader.ReadInt32();
            Unk2 = reader.ReadBytes(9);
            ShaderID = reader.ReadUInt64();
            ShaderHash = reader.ReadUInt32();

            int parameterCount = reader.ReadInt32();
            Parameters = new List<MaterialParameter>();
            for (int i = 0; i != parameterCount; i++)
            {
                var param = new MaterialParameter(reader);
                Parameters.Add(param);
            }

            int textureCount = reader.ReadInt32();
            Textures = new List<MaterialTexture>();
            for (int i = 0; i != textureCount; i++)
            {
                var texture = new MaterialTexture();
                texture.ReadFromFile(reader);
                Textures.Add(texture);
            }

            int samplerCount = reader.ReadInt32();
            Samplers = new List<MaterialSampler_v63>();
            for (int i = 0; i != samplerCount; i++)
            {
                var sampler = new MaterialSampler_v63();
                sampler.ReadFromFile(reader, version);
                Samplers.Add(sampler);
            }
        }

        public override void WriteToFile(BinaryWriter writer, VersionsEnumerator version)
        {
            // Material Name doesn't use standard hex serialization.
            writer.Write(MaterialName.Hash);
            writer.WriteString32(MaterialName.String);

            // Unknown Values.
            writer.Write(Unk0);
            writer.Write(Unk1);
            writer.Write((int)Flags);
            writer.Write(Unk2);

            // Shader ID and Hash
            writer.Write(ShaderID);
            writer.Write(ShaderHash);

            // Shader Parameters
            writer.Write(Parameters.Count);
            foreach (var param in Parameters)
            {
                param.WriteToFile(writer);
            }

            writer.Write(Textures.Count);
            foreach(var texture in Textures)
            {
                texture.WriteToFile(writer);
            }

            // Shader Samplers
            writer.Write(Samplers.Count);
            foreach (var sampler in Samplers)
            {
                sampler.WriteToFile(writer, version);
            }
        }

        public override HashName GetTextureByID(string SamplerName)
        {
            foreach(var texture in Textures)
            {
                if(texture.ID == SamplerName)
                {
                    return texture.TextureName;
                }
            }

            return null;
        }

        public override bool HasTexture(string Name)
        {
            foreach (var texture in Textures)
            {
                string FileNameLowerCase = texture.TextureName.ToString();
                return FileNameLowerCase.Contains(Name);
            }

            return false;
        }
    }

    public class MaterialSampler_v63 : IMaterialSampler
    {
        public int Unk0 { get; set; }
        public short Unk1 { get; set; }

        public MaterialSampler_v63() : base()
        {
        }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ID = new string(reader.ReadChars(4));
            Unk0 = reader.ReadInt32();
            SamplerStates = reader.ReadBytes(6);
            Unk1 = reader.ReadInt16();
        }

        public override void WriteToFile(BinaryWriter writer, VersionsEnumerator version)
        {
            writer.Write(ID.ToCharArray());
            writer.Write(Unk0);
            writer.Write(SamplerStates);
            writer.Write(Unk1);
        }
    }
}
