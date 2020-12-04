using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using ResourceTypes.Materials;
using System.ComponentModel;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameMaterial : FrameEntry
    {

        uint numLods = 0;
        int[] lodMatCount;
        BoundingBox bounds;
        List<MaterialStruct[]> materials;

        public uint NumLods {
            get { return numLods; }
            set { numLods = value; }
        }
        public int[] LodMatCount {
            get { return lodMatCount; }
            set { lodMatCount = value; }
        }
        public BoundingBox Bounds {
            get { return bounds; }
            set { bounds = value; }
        }
        public List<MaterialStruct[]> Materials {
            get { return materials; }
            set { materials = value; }
        }

        public FrameMaterial(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameMaterial(FrameMaterial other)
        {
            bounds = other.bounds;
            numLods = other.numLods;
            lodMatCount = other.lodMatCount;
            materials = new List<MaterialStruct[]>();
            for (int i = 0; i < numLods; i++)
            {
                MaterialStruct[] array = new MaterialStruct[lodMatCount[i]];
                for (int d = 0; d < array.Length; d++)
                {
                    array[d] = new MaterialStruct(other.materials[i][d]);
                }
                materials.Add(array);
            }
        }

        public FrameMaterial() : base()
        {
            numLods = 0;
            lodMatCount = new int[0];
            materials = new List<MaterialStruct[]>();
            bounds = new BoundingBox();
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            numLods = reader.ReadByte8();
            lodMatCount = new int[numLods];
            for (int i = 0; i != numLods; i++)
            {
                lodMatCount[i] = reader.ReadInt32(isBigEndian);
            }

            materials = new List<MaterialStruct[]>();

            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);

            for (int i = 0; i != numLods; i++)
            {
                MaterialStruct[] array = new MaterialStruct[lodMatCount[i]];
                for (int d = 0; d != array.Length; d++)
                {
                    array[d] = new MaterialStruct(reader, isBigEndian);
                }
                materials.Add(array);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)numLods);
            for (int i = 0; i != numLods; i++)
            {
                writer.Write(lodMatCount[i]);
            }

            bounds.WriteToFile(writer);

            for (int i = 0; i != materials.Count; i++)
            {
                for (int d = 0; d != materials[i].Length; d++)
                {
                    materials[i][d].WriteToFile(writer);
                }
            }
        }

        public List<string> CollectAllTextureNames()
        {
            List<string> TextureNames = new List<string>();

            for (int i = 0; i != materials.Count; i++)
            {
                for (int d = 0; d != materials[i].Length; d++)
                {
                    IMaterial FoundMaterial = MaterialsManager.LookupMaterialByHash(materials[i][d].MaterialHash);
                    if(FoundMaterial != null)
                    {
                        List<string> CollectedFromMaterial = FoundMaterial.CollectTextures();
                        if (CollectedFromMaterial != null)
                        {
                            TextureNames.AddRange(CollectedFromMaterial);
                        }
                    }
                }
            }

            return TextureNames;
        }

        public override string ToString()
        {
            return $"Material Block";
        }
    }

    public class MaterialStruct
    {
        int numFaces;
        int startIndex;
        ulong materialHash;
        string materialName;
        int unk3;

        public int NumFaces {
            get { return numFaces; }
            set { numFaces = value; }
        }
        public int StartIndex {
            get { return startIndex; }
            set { startIndex = value; }
        }
        public ulong MaterialHash {
            get { return materialHash; }
            set { materialHash = value; }
        }
        public string MaterialName {
            get { return materialName; }
            set { SetName(value); }
        }
        public int Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }

        public MaterialStruct(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public MaterialStruct(MaterialStruct other)
        {
            numFaces = other.numFaces;
            startIndex = other.startIndex;
            materialHash = other.materialHash;
            materialName = other.materialName;
            unk3 = other.unk3;
        }

        public MaterialStruct()
        {
            numFaces = 0;
            startIndex = 0;
            materialHash = 0;
            materialName = "";
            unk3 = 0;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            numFaces = reader.ReadInt32(isBigEndian);
            startIndex = reader.ReadInt32(isBigEndian);
            materialHash = reader.ReadUInt64(isBigEndian);
            unk3 = reader.ReadInt32(isBigEndian);

            IMaterial mat = MaterialsManager.LookupMaterialByHash(materialHash);

            if(mat != null)
                materialName = mat.GetMaterialName();
            else
                materialName = "UNABLE TO GET FROM MTLs";
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numFaces);
            writer.Write(startIndex);
            writer.Write(materialHash);
            writer.Write(unk3);
        }

        public void SetName(string name)
        {
            materialName = name;
            materialHash = FNV64.Hash(name);
            Console.WriteLine(FNV64.Hash(name));
            Console.WriteLine(FNV32.Hash(name));
        }

        public override string ToString()
        {
            return string.Format("{0}", materialName);
        }
    }
}
