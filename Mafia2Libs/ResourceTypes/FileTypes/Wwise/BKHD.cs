using System;
using System.IO;
using System.Collections.Generic;

namespace ResourceTypes.Wwise
{
    public class BKHD
    {
        public int Length { get; set; }
        public int Version { get; set; }
        public uint ID { get; set; }
        public int LangID { get; set; }
        public int Feedback { get; set; }
        public uint ProjectID { get; set; }
        public long Offset { get; set; }
        public List<int> Data { get; set; }
        public BKHD(BinaryReader br)
        {
            Length = br.ReadInt32();
            Version = br.ReadInt32();
            ID = br.ReadUInt32();
            LangID = br.ReadInt32();
            Feedback = br.ReadInt32();
            ProjectID = br.ReadUInt32();
            Offset = Length + 8;
            Data = new List<int>();
        }

        public BKHD()
        {
            Length = 20;
            Version = 113;
            ID = 0;
            LangID = 0;
            Feedback = 0;
            ProjectID = 0;
            Offset = 28;
        }
    }

    public class DIDXChunk
    {
        public uint ChunkID { get; set; }
        public uint ChunkOffset { get; set; }
        public uint ChunkLength { get; set; }
        public DIDXChunk(uint iChunkID, uint iChunkOffset, uint iChunkLength)
        {
            ChunkID = iChunkID;
            ChunkOffset = iChunkOffset;
            ChunkLength = iChunkLength;
        }
    }

    public class DidX
    {
        public uint Length { get; set; }
        public long Offset { get; set; }
        public List<DIDXChunk> Data { get; set; }
        public DidX(uint iLength)
        {
            Length = iLength;
            Offset = iLength;
            Data = new List<DIDXChunk>();
        }
    }
}
