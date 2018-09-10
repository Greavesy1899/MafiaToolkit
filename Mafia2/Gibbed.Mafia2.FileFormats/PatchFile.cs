using Gibbed.Illusion.FileFormats;
using Gibbed.IO;
using System;
using System.IO;

namespace Gibbed.Mafia2.FileFormats
{
    public class PatchFile
    {
        public FileInfo file;

        public const uint Signature = 0xD010F0F;
        public const uint Signature2 = 0xF0F0010D;
        private int UnkCount1;
        private int[] UnkInts1;
        private int UnkCount2;
        private int[] UnkInts2;
        private int UnkTotal; //UnkCount1 and UnkCount2 added together.

        public void Deserialize(BinaryReader reader, Endian endian)
        {
            if (reader.ReadInt32() != Signature)
                return;

            if (reader.ReadInt32() != 2)
                return;

            if (reader.ReadInt64() != Signature2)
                return;

            UnkCount1 = reader.ReadInt32();
            UnkInts1 = new int[UnkCount1];
            for (int i = 0; i != UnkCount1; i++)
                UnkInts1[i] = reader.ReadInt32();

            UnkCount2 = reader.ReadInt32();
            UnkInts2 = new int[UnkCount2];
            for (int i = 0; i != UnkCount2; i++)
                UnkInts2[i] = reader.ReadInt32();

            UnkTotal = reader.ReadInt32();

            if (UnkCount1 + UnkCount2 != UnkTotal)
                throw new FormatException();

            if (UnkTotal == 0)          
                return;

            int pos = (int)reader.BaseStream.Position;

            var blockStream = BlockReaderStream.FromStream(reader.BaseStream, endian);


            using (BinaryWriter writer = new BinaryWriter(File.Open("patches/patch_of_" + file.Name + ".bin", FileMode.Create)))
            {
                blockStream.SaveUncompressed(writer.BaseStream);
            }

            reader.BaseStream.Position = pos;
            blockStream = BlockReaderStream.FromStream(reader.BaseStream, endian);

            //return;

            var resources = new Archive.ResourceEntry[UnkTotal];
            for (uint i = 0; i < resources.Length; i++)
            {
                Archive.ResourceHeader resourceHeader;
                //always complains about hash errors; had to mix it up.
                
                using (var data = blockStream.ReadToMemoryStream(26))
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
                    Data = blockStream.ReadBytes((int)resourceHeader.Size - 26),
                    SlotRamRequired = resourceHeader.SlotRamRequired,
                    SlotVramRequired = resourceHeader.SlotVramRequired,
                    OtherRamRequired = resourceHeader.OtherRamRequired,
                    OtherVramRequired = resourceHeader.OtherVramRequired,
                };

                if (!Directory.Exists("patches/"))
                    Directory.CreateDirectory("patches/");

                using (BinaryWriter writer = new BinaryWriter(File.Open("patches/"+file.Name + "_" + i + ".bin", FileMode.Create)))
                {
                    writer.Write(resources[i].Data);
                }
            }
        }
    }
}
