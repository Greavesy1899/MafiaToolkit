using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class FrameMaterial : FrameEntry
    {

        uint allMat = 0;
        int[] matCount;
        Bounds bounds;
        List<MaterialStruct[]> materials;

        public List<MaterialStruct[]> Materials {
            get { return materials; }
            set { materials = value; }
        }

        public FrameMaterial(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            allMat = reader.ReadByte();
            matCount = new int[allMat];
            for (int i = 0; i != allMat; i++)
                matCount[i] = reader.ReadInt32();

            materials = new List<MaterialStruct[]>();

            bounds = new Bounds(reader);

            for (int i = 0; i != allMat; i++)
            {
                MaterialStruct[] array = new MaterialStruct[matCount[i]];
                for (int d = 0; d != array.Length; d++)
                {
                    array[d] = new MaterialStruct(reader);
                }
                materials.Add(array);
            }
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)allMat);
            for (int i = 0; i != allMat; i++)
                writer.Write(matCount[i]);

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

    public struct MaterialStruct
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
            set { materialName = value; }
        }
        public int Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }

        public MaterialStruct(BinaryReader reader)
        {
            numFaces = reader.ReadInt32();
            startIndex = reader.ReadInt32();
            materialHash = reader.ReadUInt64();
            materialName = string.Format("{0:X16}", materialHash.Swap());
            unk3 = reader.ReadInt32();
            materialName = MaterialsLib.GetMatName(materialName);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(numFaces);
            writer.Write(startIndex);
            writer.Write(materialHash);
            writer.Write(unk3);
        }

        public override string ToString()
        {
            return string.Format("{0}", materialName);
        }
    }
}
