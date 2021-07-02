using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxContainer<T> where T : FxObject
    {
        public FxArchive[] Archives { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint NumArchives = reader.ReadUInt32();

            Archives = new FxArchive[NumArchives];
            for (int i = 0; i < NumArchives; i++)
            {
                uint ArchiveSize = reader.ReadUInt32();

                FxArchive AnimSetObject = new FxArchive();
                AnimSetObject.ReadFromFile<T>(reader);
                Archives[i] = AnimSetObject;
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Archives.Length);

            for (int i = 0; i < Archives.Length; i++)
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
