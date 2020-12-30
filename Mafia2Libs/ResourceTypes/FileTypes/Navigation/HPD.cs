using SharpDX;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    public class HPDData
    {
        int unk0;
        byte[] remainingHeader; //132
        public List<unkStruct> unkData;
        string unk2;
        int unk3;
        int unk4;
        ulong unkFooter;

        public class unkStruct
        {
            /* Unk00 and Unk01 is Nodes bounding box */
            public int fileID;
            public Vector3 unk0; // Calculated from OBJ_DATA Nodes
            public Vector3 unk1; // Calculated from OBJ_DATA Nodes
            public int unk2; // 0
            public int fileSize;
            public int accumulatingSize;
            public int unk5; // 100412
            public int fileFlags;

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", fileID, unk0.ToString(), unk1.ToString());
            }
        }

        public HPDData()
        {
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            int entryCount = reader.ReadInt32();
            remainingHeader = reader.ReadBytes(132);


            unkData = new List<unkStruct>();

            for (int i = 0; i < entryCount; i++)
            {
                unkStruct data = new unkStruct();
                data.fileID = reader.ReadInt32();

                // The bounding box here is stored as X X -Y -Y Z Z
                // So we have to take this into account, rather than using or util function.
                float minX = reader.ReadSingle();
                float maxX = reader.ReadSingle();
                float minY = -reader.ReadSingle();
                float maxY = -reader.ReadSingle();
                float minZ = reader.ReadSingle();
                float maxZ = reader.ReadSingle();
                data.unk0 = new Vector3(minX, minY, minZ);
                data.unk1 = new Vector3(maxX, maxY, maxZ);

                // And then after we have deserialized it properly we have to swap it, using a 
                // util function only specific to this type of navigation file.
                data.unk0 = SwapVector3(data.unk0);
                data.unk1 = SwapVector3(data.unk1);

                data.unk2 = reader.ReadInt32();
                data.fileSize = reader.ReadInt32();
                data.accumulatingSize = reader.ReadInt32();
                data.unk5 = reader.ReadInt32();
                data.fileFlags = reader.ReadInt32();
                unkData.Add(data);
            } 
            
            unk2 = StringHelpers.ReadString(reader);          
            unk3 = reader.ReadInt32();          
            unk4 = reader.ReadInt32();
            //unkFooter = reader.ReadUInt64();

            if (Debugger.IsAttached)
            {
                DebugWriteToFile();
                AddTest();
            }
        }

        private void AddTest()
        {
            unkStruct data = new unkStruct();
            data.fileID = 1156;
            data.unk0 = new Vector3(-1125.855f, 1398.833f, 23.14296f);
            data.unk1 = new Vector3(-1016.458f, 1368.833f, 16.67241f);
            data.unk2 = 0;
            data.fileSize = 46412;
            data.accumulatingSize = unkData[unkData.Count-1].accumulatingSize + unkData[unkData.Count-1].fileSize;
            data.unk2 = 100731;
            data.fileFlags = 16583800;
            unkData.Add(data);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(unk0);
            writer.Write(unkData.Count);
            writer.Write(remainingHeader);

            for (int i = 0; i < unkData.Count; i++)
            {
                var data = unkData[i];
                writer.Write(data.fileID);

                // We have to do the opposite; so flip Z and Y and inverse Y.
                Vector3 min = SwapVector3(data.unk0);
                Vector3 max = SwapVector3(data.unk1);

                // And then serialize it as usual; X X -Y -Y Z Z
                writer.Write(min.X);
                writer.Write(max.X);
                writer.Write(-min.Y);
                writer.Write(-max.Y);
                writer.Write(min.Z);
                writer.Write(max.Z);

                writer.Write(data.unk2);
                writer.Write(data.fileSize);
                writer.Write(data.accumulatingSize);
                writer.Write(data.unk5);
                writer.Write(data.fileFlags);
            }

            StringHelpers.WriteString(writer, unk2);
            writer.Write(unk3);
            writer.Write(unk4);
            //writer.Write(unkFooter);
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
            writer.WriteLine(unk0);
            writer.WriteLine(unkData.Count);
            writer.WriteLine("");

            for(int i = 0; i < unkData.Count; i++)
            {
                var data = unkData[i];
                writer.WriteLine(string.Format("FileID: {0}", data.fileID));
                writer.WriteLine(string.Format("Unk00: {0}", data.unk0));
                writer.WriteLine(string.Format("Unk01: {0}", data.unk1));
                writer.WriteLine(string.Format("Unk02: {0}", data.unk2));
                writer.WriteLine(string.Format("FileSize: {0}", data.fileSize));
                writer.WriteLine(string.Format("AccumulatingSize: {0}", data.accumulatingSize));
                writer.WriteLine(string.Format("Unk5: {0}", data.unk5));
                writer.WriteLine(string.Format("FileFlags: {0}", data.fileFlags));
                writer.WriteLine("");
            }

            writer.WriteLine("");
            writer.WriteLine(unk2);
            writer.WriteLine(unk3);
            writer.WriteLine(unk4);
            //writer.WriteLine(unkFooter);
            writer.Close();
        }
    }
}
