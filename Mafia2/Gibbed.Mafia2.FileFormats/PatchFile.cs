using Gibbed.Illusion.FileFormats;
using Gibbed.IO;
using System;
using System.IO;

namespace Gibbed.Mafia2.FileFormats
{
    public class PatchFile
    {
        public void Deserialize(BinaryReader reader, Endian endian)
        {
            //skip the unk section.
            reader.BaseStream.Position = 32;
            var blockStream = BlockReaderStream.FromStream(reader.BaseStream, endian);

            using (BinaryWriter writer = new BinaryWriter(File.Open("patch_data.bin", FileMode.Create)))
            {
                blockStream.SaveUncompressed(writer.BaseStream);
            }

            var resources = new Archive.ResourceEntry[2];
            for (uint i = 0; i < resources.Length; i++)
            {
                Archive.ResourceHeader resourceHeader;
                using (var data = blockStream.ReadToMemoryStreamSafe(26, endian))
                {
                    resourceHeader = Archive.ResourceHeader.Read(data, endian);
                }

                if (resourceHeader.Size < 30)
                {
                    throw new FormatException();
                }
                resources[i] = new Archive.ResourceEntry()
                {
                    TypeId = (int)resourceHeader.TypeId,
                    Version = resourceHeader.Version,
                    Data = blockStream.ReadBytes((int)resourceHeader.Size - 30),
                    SlotRamRequired = resourceHeader.SlotRamRequired,
                    SlotVramRequired = resourceHeader.SlotVramRequired,
                    OtherRamRequired = resourceHeader.OtherRamRequired,
                    OtherVramRequired = resourceHeader.OtherVramRequired,
                };

                using (BinaryWriter writer = new BinaryWriter(File.Open("patch_" + i + ".bin", FileMode.Create)))
                {
                    writer.Write(resources[i].Data);
                }
            }
        }
    }
}
