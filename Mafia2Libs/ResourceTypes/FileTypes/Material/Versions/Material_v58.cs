﻿using System;
using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;
using Utils.Types;

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
        public List<MaterialSampler_v58> Samplers { get; set; }

        public Material_v58() : base()
        {
            Samplers = new List<MaterialSampler_v58>();
        }

        public Material_v58(IMaterial OtherMaterial) : base(OtherMaterial)
        {
            // TODO: I wonder if we could make v57 and v58 use the same interface?
            if (OtherMaterial.GetMTLVersion() == VersionsEnumerator.V_57)
            {
                Material_v57 CastedMaterial = (OtherMaterial as Material_v57);
                Unk0 = CastedMaterial.Unk0;
                Unk1 = CastedMaterial.Unk1;
                Unk3 = CastedMaterial.Unk3;
                Unk4 = CastedMaterial.Unk4;
                Unk5 = CastedMaterial.Unk5;

                Samplers = new List<MaterialSampler_v58>();
                foreach (var Sampler in CastedMaterial.Samplers)
                {
                    MaterialSampler_v58 NewSampler = new MaterialSampler_v58(Sampler);
                    Samplers.Add(NewSampler);
                }
            }
            else if (OtherMaterial.GetMTLVersion() == VersionsEnumerator.V_58)
            {
                Material_v58 CastedMaterial = (OtherMaterial as Material_v58);
                Unk0 = CastedMaterial.Unk0;
                Unk1 = CastedMaterial.Unk1;
                Unk3 = CastedMaterial.Unk3;
                Unk4 = CastedMaterial.Unk4;
                Unk5 = CastedMaterial.Unk5;
                Unk6 = CastedMaterial.Unk6;
                Unk7 = CastedMaterial.Unk7;

                Samplers = new List<MaterialSampler_v58>();
                foreach (var Sampler in CastedMaterial.Samplers)
                {
                    MaterialSampler_v58 NewSampler = new MaterialSampler_v58(Sampler);
                    Samplers.Add(NewSampler);
                }
            }
            else
            {
                string message = string.Format("Version {0} cannot be converted from Version {1}", GetMTLVersion(), OtherMaterial.GetMTLVersion());
                Console.WriteLine(message);
                return;
            }

            Parameters = new List<MaterialParameter>();
            foreach(var Param in OtherMaterial.Parameters)
            {
                MaterialParameter NewParam = new MaterialParameter(Param);
                Parameters.Add(NewParam);
            }
        }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ulong materialHash = reader.ReadUInt64();
            string materialName = StringHelpers.ReadString32(reader);
            MaterialName.String = materialName;
            MaterialName.Hash = materialHash;

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
            Samplers = new List<MaterialSampler_v58>();
            for (int i = 0; i != samplerCount; i++)
            {
                var shader = new MaterialSampler_v58();
                shader.ReadFromFile(reader, version);
                Samplers.Add(shader);
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
                shader.WriteToFile(writer, version);
            }
        }

        public override void SetTextureFor(string SamplerOrTextureID, string NewTextureName)
        {
            foreach (IMaterialSampler Sampler in Samplers)
            {
                if (Sampler.ID.Equals(SamplerOrTextureID))
                {
                    // Don't check the cast so we crash on purpose because this 
                    // should never cause an error.
                    MaterialSampler_v58 CastedSampler = (Sampler as MaterialSampler_v58);
                    CastedSampler.TextureName.Set(NewTextureName);
                }
            }
        }

        public override void SetupFromPreset(MaterialPreset Preset)
        {
            base.SetupFromPreset(Preset);

            if (Preset == MaterialPreset.Default)
            {
                MaterialSampler_v58 NewSampler = new MaterialSampler_v58();
                NewSampler.ID = "S000";

                Samplers.Add(NewSampler);
            }
        }

        public override HashName GetTextureByID(string SamplerName)
        {
            foreach (var sampler in Samplers)
            {
                if (sampler.ID == SamplerName)
                {
                    HashName TextureFile = new HashName();
                    TextureFile.String = sampler.GetFileName();
                    TextureFile.Hash = sampler.GetFileHash();
                    return TextureFile;
                }
            }

            return null;
        }

        public override bool HasTexture(string Name)
        {
            foreach (var sampler in Samplers)
            {
                string FileNameLowerCase = sampler.GetFileName().ToLower();
                return FileNameLowerCase.Contains(Name);
            }

            return false;
        }

        public override List<string> CollectTextures()
        {
            List<string> FoundTextures = new List<string>();
            foreach (var Sampler in Samplers)
            {
                FoundTextures.Add(Sampler.GetFileName());
            }

            return FoundTextures;
        }

        public override IMaterialSampler GetSamplerByKey(string SamplerKey)
        {
            foreach (IMaterialSampler Sampler in Samplers)
            {
                if (Sampler.ID.Equals(SamplerKey))
                {
                    return Sampler;
                }
            }

            return null;
        }

        public override VersionsEnumerator GetMTLVersion()
        {
            return VersionsEnumerator.V_58;
        }
    }

    public class MaterialSampler_v58 : IMaterialSampler
    {
        private string _name { get => MaterialParameterNames.GetName(ID); }
        public int[] UnkSet0 { get; set; }
        public HashName TextureName { get; set; }
        public byte TexType { get; set; }
        public byte UnkZero { get; set; }
        public int[] UnkSet1 { get; set; }

        public MaterialSampler_v58() : base()
        {
            UnkSet0 = new int[4];
            UnkSet1 = new int[2];
            TextureName = new HashName();
        }

        public MaterialSampler_v58(IMaterialSampler OtherSampler) : base(OtherSampler)
        {
            ID = OtherSampler.ID;
            SamplerStates = OtherSampler.SamplerStates;

            // TODO: Setup is essentially the same, maybe we can somehow make v57 and v58 share the same interface?
            if (OtherSampler.GetVersion() == VersionsEnumerator.V_57)
            {
                MaterialSampler_v57 CastedSampler = (OtherSampler as MaterialSampler_v57);             
                TextureName = new HashName(CastedSampler.TextureName);
                TexType = CastedSampler.TexType;
                UnkZero = CastedSampler.UnkZero;
                UnkSet1 = CastedSampler.UnkSet1;

                UnkSet0 = new int[4];
                Array.Copy(CastedSampler.UnkSet0, 0, UnkSet0, 0, 2);
            }
            else if (OtherSampler.GetVersion() == VersionsEnumerator.V_58)
            {
                MaterialSampler_v58 CastedSampler = (OtherSampler as MaterialSampler_v58);
                TextureName = new HashName(CastedSampler.TextureName);
                TexType = CastedSampler.TexType;
                UnkZero = CastedSampler.UnkZero;
                UnkSet1 = CastedSampler.UnkSet1;

                UnkSet0 = new int[4];
                Array.Copy(CastedSampler.UnkSet0, 0, UnkSet0, 0, 2);
            }
            else
            {
                string message = string.Format("Version {0} cannot be converted from Version {1}", GetVersion(), OtherSampler.GetVersion());
                Console.WriteLine(message);
            }
        }

        public override void ReadFromFile(BinaryReader reader, VersionsEnumerator version)
        {
            ID = new string(reader.ReadChars(4));
            UnkSet0 = new int[4];
            for (int i = 0; i < UnkSet0.Length; i++)
            {
                UnkSet0[i] = reader.ReadInt32();
            }
            ulong TextureHash = reader.ReadUInt64();
            TexType = reader.ReadByte();
            UnkZero = reader.ReadByte();
            SamplerStates = reader.ReadBytes(6);
            UnkSet1 = new int[2]; // these can have erratic values
            for (int i = 0; i < 2; i++)
            {
                UnkSet1[i] = reader.ReadInt32();
            }
            TextureName.String = StringHelpers.ReadString32(reader);
            TextureName.Hash = TextureHash;
        }

        public override void WriteToFile(BinaryWriter writer, VersionsEnumerator version)
        {
            writer.Write(ID.ToCharArray());
            for (int i = 0; i < 4; i++)
            {
                writer.Write(UnkSet0[i]);
            }
            writer.Write(TextureName.Hash);
            writer.Write(TexType);
            writer.Write(UnkZero);
            writer.Write(SamplerStates);

            for (int i = 0; i < 2; i++)
            {
                writer.Write(UnkSet1[i]);
            }
            writer.WriteString32(TextureName.String);
        }

        public override VersionsEnumerator GetVersion()
        {
            return VersionsEnumerator.V_58;
        }

        public override string GetFileName()
        {
            return TextureName.String;
        }

        public override ulong GetFileHash()
        {
            return TextureName.Hash;
        }

        public override string ToString()
        {
            return string.Format("ID: {0} Name: {1} File: {2}", ID, _name, GetFileName());
        }
    }
}
