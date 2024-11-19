using Gibbed.Illusion.FileFormats;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;
using System.IO;

namespace Gibbed.Illusion.ResourceFormats
{
    // NOTE: Only supports M1: DE and M3 Cutscene formats.
    // This IS NOT for Mafia II. However it would be ideal if we could also support Mafia II.
    public class CutsceneResource : IResourceType
    {
        public class GCRResource
        {
            public string Name { get; set; }
            public byte[] Content { get; set; }

            public override string ToString()
            {
                return string.Format("Name: {0} Size: {1}", Name, Content.Length);
            }
        }

        public GCRResource[] GCREntityRecords { get; set; }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            uint Padding = input.ReadValueU32(endian);
            uint NumGCRS = input.ReadValueU32(endian);

            GCREntityRecords = new GCRResource[NumGCRS];

            for (int i = 0; i < NumGCRS; i++)
            {
                // construct resource and deserialize name
                GCRResource Resource = new GCRResource();
                Resource.Name = input.ReadStringU16(endian);
                Resource.Name += ".gcr";

                // get size and move back 8 bytes
                uint Unk01 = input.ReadValueU32(endian);
                int Size = input.ReadValueS32(endian); // Size includes these 4 bytes and Unk01

                input.Position -= 8;
                Resource.Content = input.ReadBytes(Size);

                // Store Entity record
                GCREntityRecords[i] = Resource;
            }
        }

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteValueU32(0, endian);
            output.WriteValueU32((uint)GCREntityRecords.Length, endian);

            foreach(var Record in GCREntityRecords)
            {
                Record.Name = Record.Name.Replace(".gcr", "");
                output.WriteStringU16(Record.Name, endian);
                output.WriteBytes(Record.Content);
            }
        }
    }
}
