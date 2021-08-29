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
        public uint chunkId { get; set; }
        public uint chunkOffset { get; set; }
        public uint chunkLength { get; set; }
        public DIDXChunk(uint iChunkID, uint iChunkOffset, uint iChunkLength)
        {
            chunkId = iChunkID;
            chunkOffset = iChunkOffset;
            chunkLength = iChunkLength;
        }
    }

    public class DIDX
    {
        public uint length { get; set; }
        public long offset { get; set; }
        public List<DIDXChunk> data { get; set; }
        public DIDX(uint iLength)
        {
            length = iLength;
            offset = iLength;
            data = new List<DIDXChunk>();
        }
    }
}
