using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AnimEntity
    {
        public ulong Hash0 { get; set; }
        public HashName FrameName { get; set; }
        public int EntityFlags { get; set; }

        // Util for loading
        // AeFrame uses this as it is NOT reversed.
        // When AeFrame is solved, we can remove this.
        protected uint Size;

        // Util offset when saving
        protected long SizeOffset;

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            ulong TempFrameHash = stream.ReadUInt64(isBigEndian);
            Hash0 = stream.ReadUInt64(isBigEndian);
            string TempFrameName = stream.ReadString16(isBigEndian);
            EntityFlags = stream.ReadInt32(isBigEndian);

            uint AeIdentifier = stream.ReadUInt32(isBigEndian); // 117
            Debug.Assert(AeIdentifier == 117, "AnimEntity Identifier should be 117.");
            Size = stream.ReadUInt32(isBigEndian);

            // Store the frame hash information.
            FrameName = new HashName();
            FrameName.String = TempFrameName;
            FrameName.Hash = TempFrameHash;
        }

        public virtual void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(FrameName.Hash, isBigEndian);
            stream.Write(Hash0, isBigEndian);
            stream.WriteString16(FrameName.String, isBigEndian);
            stream.Write(EntityFlags, isBigEndian);
            stream.Write(117, isBigEndian);
            SizeOffset = stream.Position;
            stream.Write(-1, isBigEndian);
        }

        // Util function to keep size updated.
        protected void UpdateSize(MemoryStream stream, bool isBigEndian)
        {
            long CurrentPosition = stream.Position;
            stream.Seek(SizeOffset, SeekOrigin.Begin);
            uint ByteSize = (uint)(CurrentPosition - SizeOffset) + 4;
            stream.Write(ByteSize, isBigEndian);
            stream.Seek(CurrentPosition, SeekOrigin.Begin);
        }

        public virtual AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AnimEntity;
        }

        public override string ToString()
        {
            return AnimEntityTypes.AnimEntity.ToString();
        }
    }
}
