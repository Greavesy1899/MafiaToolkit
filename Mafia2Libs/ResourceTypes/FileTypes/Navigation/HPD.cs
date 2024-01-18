using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.Helpers.Reflection;
using Utils.Logging;
using Utils.MathHelpers;

namespace ResourceTypes.Navigation
{
    [PropertyClassAllowReflection, PropertyClassCheckInherited]
    public class HPDData : INavigationData
    {
        [PropertyClassAllowReflection, PropertyClassCheckInherited]
        public class ADDITIONNALDATADESC
        {
            public uint ID { get; set; }
            public uint Offset { get; set; }
            public uint Size { get; set; }

            public void ReadFromFile(BinaryReader Reader)
            {
                ID = Reader.ReadUInt32();
                Offset = Reader.ReadUInt32();
                Size = Reader.ReadUInt32();
            }

            public void WriteToFile(BinaryWriter Writer)
            {
                Writer.Write(ID);
                Writer.Write(Offset);
                Writer.Write(Size);
            }
        }

        public int Unk0 { get; set; }
        public int ProjectType { get; set; }
        public float BalancedDirect_OriginX { get; set; }
        public float BalancedDirect_OriginY { get; set; }
        public float BalancedDirect_OriginZ { get; set; }
        public float BalancedDirect_EdgeLength { get; set; }
        public ADDITIONNALDATADESC[] NodesAdditionalData { get; set; }
        public ADDITIONNALDATADESC[] EdgesAdditionalData { get; set; }
        public uint ConcreteNodeTotalSize { get; set; }
        public uint ConcreteEdgeTotalSize { get; set; }
        public uint AiMeshManagement { get; set; }
        public uint NumPODEntries { get; set; }
        public HPDEntry[] HPDEntries { get; set; }
        public string Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }

        [PropertyClassAllowReflection]
        public class HPDEntry
        {
            /* Unk00 and Unk01 is Nodes bounding box */
            public int UniqueID { get; set; }
            public Vector3 BBoxMin { get; set; } // Calculated from OBJ_DATA Nodes
            public Vector3 BBoxMax { get; set; } // Calculated from OBJ_DATA Nodes
            public int Level { get; set; } // 0
            public uint FileSize { get; set; }
            public uint FileOffset { get; set; }
            public uint Tag1 { get; set; } // 100412
            public uint Tag2 { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", UniqueID, BBoxMin.ToString(), BBoxMax.ToString());
            }
        }

        public HPDData()
        {
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk0 = reader.ReadInt32();
            int entryCount = reader.ReadInt32();
            ProjectType = reader.ReadInt32();
            BalancedDirect_OriginX = reader.ReadSingle();
            BalancedDirect_OriginY = reader.ReadSingle();
            BalancedDirect_OriginZ = reader.ReadSingle();
            BalancedDirect_EdgeLength = reader.ReadSingle();

            NodesAdditionalData = new ADDITIONNALDATADESC[4];
            EdgesAdditionalData = new ADDITIONNALDATADESC[4];
            for(uint i = 0; i < NodesAdditionalData.Length; i++)
            {
                NodesAdditionalData[i] = new ADDITIONNALDATADESC();
                NodesAdditionalData[i].ReadFromFile(reader);
            }

            for (uint i = 0; i < EdgesAdditionalData.Length; i++)
            {
                EdgesAdditionalData[i] = new ADDITIONNALDATADESC();
                EdgesAdditionalData[i].ReadFromFile(reader);
            }

            ConcreteNodeTotalSize = reader.ReadUInt32();
            ConcreteEdgeTotalSize = reader.ReadUInt32();
            AiMeshManagement = reader.ReadUInt32();
            NumPODEntries = reader.ReadUInt32();


            HPDEntries = new HPDEntry[entryCount];

            for (int i = 0; i < entryCount; i++)
            {
                HPDEntry data = new HPDEntry();
                data.UniqueID = reader.ReadInt32();

                // The bounding box here is stored as X X -Y -Y Z Z
                // So we have to take this into account, rather than using or util function.
                float minX = reader.ReadSingle();
                float maxX = reader.ReadSingle();
                float minY = reader.ReadSingle();
                float maxY = reader.ReadSingle();
                float minZ = reader.ReadSingle();
                float maxZ = reader.ReadSingle();
                data.BBoxMin = new Vector3(minX, minY, minZ);
                data.BBoxMax = new Vector3(maxX, maxY, maxZ);

                data.Level = reader.ReadInt32();
                data.FileSize = reader.ReadUInt32();
                data.FileOffset = reader.ReadUInt32();
                data.Tag1 = reader.ReadUInt32();
                data.Tag2 = reader.ReadUInt32();
                HPDEntries[i] = data;
            } 
            
            Unk2 = StringHelpers.ReadString(reader);          
            Unk3 = reader.ReadInt32();          
            Unk4 = reader.ReadInt32();

            ToolkitAssert.Ensure(reader.BaseStream.Position == reader.BaseStream.Length, "Expected to read the whole of the HPD file!");
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Unk0);
            writer.Write(HPDEntries.Length);
            writer.Write(ProjectType);
            writer.Write(BalancedDirect_OriginX);
            writer.Write(BalancedDirect_OriginY);
            writer.Write(BalancedDirect_OriginZ);
            writer.Write(BalancedDirect_EdgeLength);

            for(uint i = 0; i < NodesAdditionalData.Length; i++)
            {
                NodesAdditionalData[i].WriteToFile(writer);
            }

            for (uint i = 0; i < EdgesAdditionalData.Length; i++)
            {
                EdgesAdditionalData[i].WriteToFile(writer);
            }

            writer.Write(ConcreteNodeTotalSize);
            writer.Write(ConcreteEdgeTotalSize);
            writer.Write(AiMeshManagement);
            writer.Write(NumPODEntries);

            for (int i = 0; i < HPDEntries.Length; i++)
            {
                var data = HPDEntries[i];
                writer.Write(data.UniqueID);
                writer.Write(data.BBoxMin.X);
                writer.Write(data.BBoxMax.X);
                writer.Write(data.BBoxMin.Y);
                writer.Write(data.BBoxMax.Y);
                writer.Write(data.BBoxMin.Z);
                writer.Write(data.BBoxMax.Z);
                writer.Write(data.Level);
                writer.Write(data.FileSize);
                writer.Write(data.FileOffset);
                writer.Write(data.Tag1);
                writer.Write(data.Tag2);
            }

            StringHelpers.WriteString(writer, Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
        }
    }
}
