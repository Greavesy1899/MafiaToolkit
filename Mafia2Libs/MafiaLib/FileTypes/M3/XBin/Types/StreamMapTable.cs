using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class StreamMapTable
    {
        public class StreamMapLine
        {
            public EStreamMapLineType LineType { get; set; }
            public string GameID { get; set; }
            public string MissionID { get; set; }
            public string PartID { get; set; }
            public int TableCommandsOffset { get; set; }
            public int NumTableCommands0 { get; set; }
            public int NumTableCommands1 { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", GameID, MissionID, PartID);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            StreamMapLine[] Lines = new StreamMapLine[count1];

            for (int i = 0; i < count1; i++)
            {
                StreamMapLine Line = new StreamMapLine();
                Line.LineType = (EStreamMapLineType)reader.ReadInt32();
                Line.GameID = GetStringFromBuffer(reader);
                Line.MissionID = GetStringFromBuffer(reader);
                Line.PartID = GetStringFromBuffer(reader);
                Line.TableCommandsOffset = reader.ReadInt32();
                Line.NumTableCommands0 = reader.ReadInt32();
                Line.NumTableCommands1 = reader.ReadInt32();
                reader.ReadInt32(); // Could be bAsync.
                Lines[i] = Line;
            }
        }

        private string GetStringFromBuffer(BinaryReader reader)
        {
            int offset = reader.ReadInt32();

            long position = reader.BaseStream.Position;

            (reader.BaseStream.Position) += (offset - 4);
            string result = StringHelpers.ReadString(reader);

            reader.BaseStream.Position = position;
            return result;
        }
    }
}
