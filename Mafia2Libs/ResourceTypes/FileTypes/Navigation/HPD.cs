using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.Helpers.Reflection;
using Utils.Logging;

namespace ResourceTypes.Navigation
{
    [PropertyClassAllowReflection, PropertyClassCheckInherited]
    public class HPDData : INavigationData
    {
        public int Unk0 { get; set; }
        public byte[] UnkHeader { get; set; } // 132, todo, figure out what they are
        public HPDEntry[] HPDEntries { get; set; }
        public string Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }

        [PropertyClassAllowReflection]
        public class HPDEntry
        {
            /* Unk00 and Unk01 is Nodes bounding box */
            public int FileID { get; set; }
            public Vector3 BBoxMin { get; set; } // Calculated from OBJ_DATA Nodes
            public Vector3 BBoxMax { get; set; } // Calculated from OBJ_DATA Nodes
            public int Unk2 { get; set; } // 0
            public int FileSize { get; set; }
            public int AccumulatingSize { get; set; }
            public int Unk5 { get; set; } // 100412
            public int Flags { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", FileID, BBoxMin.ToString(), BBoxMax.ToString());
            }
        }

        public HPDData()
        {
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk0 = reader.ReadInt32();
            int entryCount = reader.ReadInt32();
            UnkHeader = reader.ReadBytes(132);


            HPDEntries = new HPDEntry[entryCount];

            for (int i = 0; i < entryCount; i++)
            {
                HPDEntry data = new HPDEntry();
                data.FileID = reader.ReadInt32();

                // The bounding box here is stored as X X -Y -Y Z Z
                // So we have to take this into account, rather than using or util function.
                float minX = reader.ReadSingle();
                float maxX = reader.ReadSingle();
                float minY = -reader.ReadSingle();
                float maxY = -reader.ReadSingle();
                float minZ = reader.ReadSingle();
                float maxZ = reader.ReadSingle();
                data.BBoxMin = new Vector3(minX, minY, minZ);
                data.BBoxMax = new Vector3(maxX, maxY, maxZ);

                // And then after we have deserialized it properly we have to swap it, using a 
                // util function only specific to this type of navigation file.
                data.BBoxMin = SwapVector3(data.BBoxMin);
                data.BBoxMax = SwapVector3(data.BBoxMax);

                data.Unk2 = reader.ReadInt32();
                data.FileSize = reader.ReadInt32();
                data.AccumulatingSize = reader.ReadInt32();
                data.Unk5 = reader.ReadInt32();
                data.Flags = reader.ReadInt32();
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
            writer.Write(UnkHeader);

            for (int i = 0; i < HPDEntries.Length; i++)
            {
                var data = HPDEntries[i];
                writer.Write(data.FileID);

                // We have to do the opposite; so flip Z and Y and inverse Y.
                Vector3 min = SwapVector3(data.BBoxMin);
                Vector3 max = SwapVector3(data.BBoxMax);

                // And then serialize it as usual; X X -Y -Y Z Z
                writer.Write(min.X);
                writer.Write(max.X);
                writer.Write(-min.Y);
                writer.Write(-max.Y);
                writer.Write(min.Z);
                writer.Write(max.Z);

                writer.Write(data.Unk2);
                writer.Write(data.FileSize);
                writer.Write(data.AccumulatingSize);
                writer.Write(data.Unk5);
                writer.Write(data.Flags);
            }

            StringHelpers.WriteString(writer, Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
        }

        private Vector3 SwapVector3(Vector3 vector)
        {
            Vector3 pos = vector;
            float y = pos.Y;
            pos.Y = -pos.Z;
            pos.Z = y;
            return pos;
        }

        private void DebugWriteToFile()
        {
            StreamWriter writer = new StreamWriter("NAV_HPD_DATA content.txt");
            writer.WriteLine(Unk0);
            writer.WriteLine(HPDEntries.Length);
            writer.WriteLine("");

            for(int i = 0; i < HPDEntries.Length; i++)
            {
                var data = HPDEntries[i];
                writer.WriteLine(string.Format("FileID: {0}", data.FileID));
                writer.WriteLine(string.Format("Unk00: {0}", data.BBoxMin));
                writer.WriteLine(string.Format("Unk01: {0}", data.BBoxMax));
                writer.WriteLine(string.Format("Unk02: {0}", data.Unk2));
                writer.WriteLine(string.Format("FileSize: {0}", data.FileSize));
                writer.WriteLine(string.Format("AccumulatingSize: {0}", data.AccumulatingSize));
                writer.WriteLine(string.Format("Unk5: {0}", data.Unk5));
                writer.WriteLine(string.Format("FileFlags: {0}", data.Flags));
                writer.WriteLine("");
            }

            writer.WriteLine("");
            writer.WriteLine(Unk2);
            writer.WriteLine(Unk3);
            writer.WriteLine(Unk4);
            writer.Close();
        }
    }
}
