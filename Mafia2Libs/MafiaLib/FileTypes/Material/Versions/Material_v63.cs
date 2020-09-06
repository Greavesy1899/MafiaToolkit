using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Materials
{
    public class Material_v63 : IMaterial
    {
        public byte[] Unk0 { get; set; }
        public ulong Unk1 { get; set; }

        public List<MaterialTexture> Textures { get; set; }
        public List<MaterialSampler_v63> Samplers { get; set; }

        public Material_v63() : base()
        {
            Unk0 = new byte[7];
        }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ulong materialHash = reader.ReadUInt64();
            string materialName = StringHelpers.ReadString32(reader);
            MaterialName.String = materialName;
            MaterialName.uHash = materialHash;

            Unk0 = reader.ReadBytes(7);
            Flags = (MaterialFlags)reader.ReadInt32();
            Unk1 = reader.ReadUInt64();
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
            writer.Write(MaterialName.uHash);
            writer.WriteString32(MaterialName.String);

            // Unknown Values.
            writer.Write(Unk0);
            writer.Write((int)Flags);
            writer.Write(Unk1);

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

        public override Hash GetTextureByID(string SamplerName)
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
