using System.IO;
using System.Collections.Generic;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxContainer<T> where T : FxObject
    {
        public List<FxArchive> Archives { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint NumArchives = reader.ReadUInt32();

            Archives = new List<FxArchive>();
            for (int i = 0; i < NumArchives; i++)
            {
                uint ArchiveSize = reader.ReadUInt32();

                FxArchive AnimSetObject = new FxArchive();
                AnimSetObject.ReadFromFile<T>(reader);
                Archives.Add(AnimSetObject);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Archives.Count);

            for (int i = 0; i < Archives.Count; i++)
            {
                // Write Archive to file
                long CurrentPosition = writer.BaseStream.Position;
                writer.Write(-1);
                Archives[i].WriteToFile(writer);
                long EndPosition = writer.BaseStream.Position;

                // Update Size
                long Size = EndPosition - CurrentPosition;
                Size -= 4; // Add 4 to exclude offset

                // Store Size
                writer.BaseStream.Position = CurrentPosition;
                writer.Write((uint)Size);
                writer.BaseStream.Position = EndPosition;
            }
        }
    }
}
