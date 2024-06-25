using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_6 : IKeyType
    {
        public class PositionData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public Vector3 Position { get; set; } // Could be new position

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", Position, KeyFrameStart, KeyFrameEnd);
            }
        }

        public PositionData[] Positions { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumPositions = br.ReadInt32();
            Positions = new PositionData[NumPositions];

            for (int i = 0; i < NumPositions; i++)
            {
                PositionData position = new PositionData();
                position.KeyFrameStart = br.ReadInt32();
                position.KeyFrameEnd = br.ReadInt32();
                position.Unk03 = br.ReadByte();
                position.Position = Vector3Utils.ReadFromFile(br);
                Positions[i] = position;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Positions.Length);

            for(int i = 0; i < Positions.Length; i++)
            {
                PositionData Entry = Positions[i];
                bw.Write(Entry.KeyFrameStart);
                bw.Write(Entry.KeyFrameEnd);
                bw.Write(Entry.Unk03);
                Entry.Position.WriteToFile(bw);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("Positions: {0}", Positions.Length);
        }
    }
}
