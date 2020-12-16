using SharpDX;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_6 : IKeyType
    {
        public class PositionData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public Vector3 Position { get; set; } // Could be new position

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", Position, Unk01, Unk02);
            }
        }

        public int NumPositions { get; set; }
        public PositionData[] Positions { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumPositions = stream.ReadInt32(isBigEndian);
            Positions = new PositionData[NumPositions];

            for (int i = 0; i < NumPositions; i++)
            {
                PositionData position = new PositionData();
                position.Unk01 = stream.ReadInt32(isBigEndian);
                position.Unk02 = stream.ReadInt32(isBigEndian);
                position.Unk03 = stream.ReadByte8();
                position.Position = Vector3Extenders.ReadFromFile(stream, isBigEndian);
                Positions[i] = position;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(NumPositions, isBigEndian);

            for(int i = 0; i < Positions.Length; i++)
            {
                PositionData Entry = Positions[i];
                stream.Write(Entry.Unk01, isBigEndian);
                stream.Write(Entry.Unk02, isBigEndian);
                stream.WriteByte(Entry.Unk03);
                Entry.Position.WriteToFile(stream, isBigEndian);
            }

            stream.Write(Unk05, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumPositions: {0}", Positions.Length);
        }
    }
}
