using ResourceTypes.Cutscene.KeyParams;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeBase
    {
        public short Unk01 { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public byte Unk02 { get; set; }
        public ulong Hash0 { get; set; }
        public ulong Hash1 { get; set; }
        public string Name3 { get; set; }
        public int Unk03 { get; set; }
        public int Unk04 { get; set; }
        public int Unk044 { get; set; }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt16(isBigEndian);

            if(Unk01 == 0)
            {
                // Nothing here. return.
                return;
            }

            Name1 = stream.ReadString16(isBigEndian);
            Name2 = stream.ReadString16(isBigEndian);

            if (!string.IsNullOrEmpty(Name2))
            {
                Unk02 = stream.ReadByte8();
            }

            Hash0 = stream.ReadUInt64(isBigEndian);
            Hash1 = stream.ReadUInt64(isBigEndian);
            Name3 = stream.ReadString16(isBigEndian);
            Unk03 = stream.ReadInt32(isBigEndian);
            Unk04 = stream.ReadInt32(isBigEndian);
            Unk044 = stream.ReadInt32(isBigEndian);
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {

        }
    }

    public class AeBaseData
    {
        [Browsable(false)]
        public int DataType { get; set; } // We might need an enumator for this
        [Browsable(false)]
        public int Size { get; set; } // Total Size of the data. includes Size and DataType.
        [Browsable(false)]
        public int KeyDataSize { get; set; } // Size of all the keyframes? Also count and the Unk01?
        public int Unk00 { get; set; }
        public int Unk01 { get; set; }
        public int NumKeyFrames { get; set; } // Number of keyframes. Start with 0xE803 or 1000
        public IKeyType[] KeyFrames { get; set; }
        //public int Unk02 { get; set; } // 0x70 - 112
        //public int Unk03 { get; set; } // 0x15 - 21
        //public float Unk04 { get; set; } // 0x803F - 1.0f;
        //public int Unk05 { get; set; } // 0x0
        //public int Unk06 { get; set; } // 0xFFFF
        //public byte Unk07 { get; set; } // 0x0

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            DataType = stream.ReadInt32(isBigEndian);
            Size = stream.ReadInt32(isBigEndian);
            Unk00 = stream.ReadInt32(isBigEndian);
            KeyDataSize = stream.ReadInt32(isBigEndian);
            Unk01 = stream.ReadInt32(isBigEndian);
            NumKeyFrames = stream.ReadInt32(isBigEndian);

            KeyFrames = new IKeyType[NumKeyFrames];

            for (int i = 0; i < NumKeyFrames; i++)
            {
                Debug.Assert(stream.Position != stream.Length, "Reached the end to early?");

                int Header = stream.ReadInt32(isBigEndian);
                Debug.Assert(Header == 1000, "Keyframe magic did not equal 1000");
                int Size = stream.ReadInt32(isBigEndian);
                int KeyType = stream.ReadInt32(isBigEndian);
                AnimKeyParamTypes KeyParamType = (AnimKeyParamTypes)KeyType;

                IKeyType KeyParam = CutsceneKeyParamFactory.ReadAnimEntityFromFile(KeyParamType, Size, stream);
                KeyFrames[i] = KeyParam;
            }
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
        }

        public override string ToString()
        {
            return string.Format("{0}", DataType);
        }
    }
}
