using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ApexSDK
{
    public class ApexClothingAssetLoader
    {
        readonly int type = 41;
        readonly string name = "NxClothingAsset";
        public DataStruct data;

        public struct BoneData
        {
            public float[] Matrix;
            public int Unk0;
            public int Unk1;
            public int Unk2;
            public int Unk3;
            public int Unk4;
        }
        public struct DataStruct
        {
            public int Unk0;
            public int EndOfBonesOffset;
            public List<string> BoneNames;
            public int Unk1;
            public int Unk2;
            public int Unk3;
            public BoneData[] Bones;
            public float Unk4;
            public short Unk5;
            public int unk6;
            public ApexRenderMesh mesh;
        }

        public ApexClothingAssetLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != type)
                return;

            if (StringHelpers.ReadString32(reader) != name)
                return;

            data = new DataStruct();
            data.BoneNames = new List<string>();

            data.Unk0 = reader.ReadInt32();
            data.EndOfBonesOffset = reader.ReadInt32();

			while(reader.BaseStream.Position != data.Unk0 + data.EndOfBonesOffset + 9)
            {
                data.BoneNames.Add(StringHelpers.ReadString(reader));
            }

            data.Unk1 = reader.ReadInt32();
            data.Unk2 = reader.ReadInt32();
            reader.ReadBytes(12);
            data.Unk3 = reader.ReadInt32();
            data.Bones = new BoneData[data.BoneNames.Count];

            for(int i = 0; i != data.BoneNames.Count; i++)
            {
                BoneData bData = new BoneData();
                bData.Matrix = new float[12];

                for(int x = 0; x != 12; x++)
                {
                    bData.Matrix[x] = reader.ReadSingle();
                }

                bData.Unk0 = reader.ReadInt32();
                bData.Unk1 = reader.ReadInt32();
                bData.Unk2 = reader.ReadInt32();
                bData.Unk3 = reader.ReadInt32();
                bData.Unk4 = reader.ReadInt32();
                data.Bones[i] = bData;
            }

            data.Unk4 = reader.ReadSingle();
            data.Unk5 = reader.ReadInt16();
            data.unk6 = reader.ReadInt32();
            reader.ReadInt16();
            data.mesh = new ApexRenderMesh();
            data.mesh.ReadFromFile(reader);
            
        }
    }
}
