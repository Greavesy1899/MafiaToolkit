using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Materials
{
    public class Material_v58 : IMaterial
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public int Unk4 { get; set; }
        public int Unk5 { get; set; }
        public byte Unk6 { get; set; }
        public float Unk7 { get; set; }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ulong materialHash = reader.ReadUInt64();
            string materialName = StringHelpers.ReadString32(reader);
            MaterialName.String = materialName;
            MaterialName.uHash = materialHash;

            Unk0 = reader.ReadByte();
            Unk1 = reader.ReadByte();
            Unk2 = reader.ReadByte();

            Flags = (MaterialFlags)reader.ReadInt32();
            Unk3 = reader.ReadByte();
            Unk4 = reader.ReadInt32();
            Unk5 = reader.ReadInt32();
            Unk6 = reader.ReadByte();
            Unk7 = reader.ReadSingle();

            ShaderID = reader.ReadUInt64();
            ShaderHash = reader.ReadUInt32();

            int parameterCount = reader.ReadInt32();
            Parameters = new List<MaterialParameter>();
            for (int i = 0; i != parameterCount; i++)
            {
                var param = new MaterialParameter(reader);
                Parameters.Add(param);
            }

            int samplerCount = reader.ReadInt32();
            Samplers = new List<MaterialSampler>();
            for (int i = 0; i != samplerCount; i++)
            {
                var shader = new MaterialSampler(reader, version);
                Samplers.Add(shader);
            }
        }

        public override void WriteToFile(BinaryWriter writer, VersionsEnumerator version)
        {
            // Material Name doesn't use standard hex serialization.
            writer.Write(MaterialName.Hex);
            writer.WriteString32(MaterialName.String);

            // Unknown Values.
            writer.Write(Unk0);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write((int)Flags);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            writer.Write(Unk6);
            writer.Write(Unk7);

            // Shader ID and Hash
            writer.Write(ShaderID);
            writer.Write(ShaderHash);

            // Shader Parameters
            writer.Write(Parameters.Count);
            foreach (var param in Parameters)
            {
                param.WriteToFile(writer);
            }

            // Shader Samplers
            writer.Write(Samplers.Count);
            foreach (var shader in Samplers)
            {
                shader.WriteToFile(writer);
            }
        }
    }
}
