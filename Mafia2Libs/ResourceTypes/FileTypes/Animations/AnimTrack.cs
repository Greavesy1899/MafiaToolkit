using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.IO;
using static ResourceTypes.FrameNameTable.FrameNameTable;

namespace ResourceTypes.Animation2
{
    public class AnimTrack
    {
        public ulong BoneID { get; set; }
        public byte Flags { get; set; }
        public bool IsDataPresent { get; set; }
        public byte DataFlags { get; set; }
        public short NumKeyFrames { get; set; }
        public byte ComponentSize { get; set; }
        public byte TimeSize { get; set; }
        public uint PackedReferenceQuat { get; set; }
        public float Scale { get; set; }
        public float Duration { get; set; }
        public byte[] KeyFrameData { get; set; }
        public float Unk00 { get; set; }
        public UnkDataBlock[] UnkData { get; set; } = new UnkDataBlock[0];
        public AnimTrack()
        {

        }

        public AnimTrack(Stream s)
        {
            Read(s);
        }

        public AnimTrack(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            BoneID = br.ReadUInt64();
            Flags = br.ReadByte();
            IsDataPresent = br.ReadBoolean();
            DataFlags = br.ReadByte();
            NumKeyFrames = br.ReadInt16();
            ComponentSize = br.ReadByte();
            TimeSize = br.ReadByte();
            PackedReferenceQuat = br.ReadUInt32();
            Scale = br.ReadSingle();
            Duration = br.ReadSingle();
            KeyFrameData = br.ReadBytes(GetKeyframeDataSize(NumKeyFrames, ComponentSize, TimeSize));

            if ((Flags & 0x01) == 1)
            {
                short Count = br.ReadInt16();
                Unk00 = br.ReadSingle();
                UnkData = new UnkDataBlock[Count];

                for (int i = 0; i < UnkData.Length; i++)
                {
                    UnkData[i] = new(br);
                }
            }

            //DumpTrackData();
        }

        public int GetKeyframeSize(int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return (3 * ComponentSize + TimeSize + 2) & 0x7F;
        }

        public int GetKeyframeDataSize(int NumRotationFrames, int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return 4 + 4 * (NumRotationFrames * GetKeyframeSize(ComponentSize, TimeSize) / 32);
        }

        private void DumpTrackData()
        {
            string folderPath = "%userprofile%\\Desktop\\AnimTracks";
            string path = Environment.ExpandEnvironmentVariables(folderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(KeyFrameData, 0, KeyFrameData.Length)}.bin"), KeyFrameData);
        }
    }

    public class UnkDataBlock
    {
        public short BoneIndex { get; set; }
        public byte[] Data { get; set; } = new byte[10];

        public UnkDataBlock()
        {

        }

        public UnkDataBlock(Stream s)
        {
            Read(s);
        }

        public UnkDataBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }
        
        public void Read(BinaryReader br)
        {
            BoneIndex = br.ReadInt16();
            Data = br.ReadBytes(10);
        }
    }
}
