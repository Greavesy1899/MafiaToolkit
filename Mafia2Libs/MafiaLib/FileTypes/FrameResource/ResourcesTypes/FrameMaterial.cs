using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using SharpDX;
using Mafia2;

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

        /// <summary>
        /// Construct FrameMaterial by reading the stream.
        /// </summary>
        /// <param name="reader"></param>
        public FrameMaterial(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Construct FrameMaterial with basic settings.
        /// </summary>
        public FrameMaterial() : base()
        {
            numLods = 0;
            lodMatCount = new int[0];
            materials = new List<MaterialStruct[]>();
            bounds = new BoundingBox();
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            numLods = reader.ReadByte();
            lodMatCount = new int[numLods];
            for (int i = 0; i != numLods; i++)
                lodMatCount[i] = reader.ReadInt32();

            materials = new List<MaterialStruct[]>();

            bounds = BoundingBoxExtenders.ReadFromFile(reader);

            for (int i = 0; i != numLods; i++)
            {
                MaterialStruct[] array = new MaterialStruct[lodMatCount[i]];
                for (int d = 0; d != array.Length; d++)
                {
                    array[d] = new MaterialStruct(reader);
                }
                materials.Add(array);
            }
        }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)numLods);
            for (int i = 0; i != numLods; i++)
                writer.Write(lodMatCount[i]);

            bounds.WriteToFile(writer);

            for (int i = 0; i != materials.Count; i++)
            {
                for (int d = 0; d != materials[i].Length; d++)
                {
                    materials[i][d].WriteToFile(writer);
                }
            }
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

        /// <summary>
        /// Construct Material data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public MaterialStruct(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Constructs Material with default data.
        /// </summary>
        public MaterialStruct()
        {
            numFaces = 0;
            startIndex = 0;
            materialHash = 0;
            materialName = "";
            unk3 = 0;
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            numFaces = reader.ReadInt32();
            startIndex = reader.ReadInt32();
            materialHash = reader.ReadUInt64();
            unk3 = reader.ReadInt32();

            Material mat = MaterialsManager.LookupMaterialByHash(materialHash);

            if(mat != null)
                materialName = mat.MaterialName;
            else
                materialName = "UNABLE TO GET FROM MTLs";
        }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numFaces);
            writer.Write(startIndex);
            writer.Write(materialHash);
            writer.Write(unk3);
        }

        /// <summary>
        /// Set shader sampler name
        /// </summary>
        /// <param name="name"></param>
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
