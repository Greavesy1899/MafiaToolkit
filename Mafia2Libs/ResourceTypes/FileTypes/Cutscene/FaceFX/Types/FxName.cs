using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxName
    {
        private uint StringTableIndex;

        public string Text { get; set; }

        public void ReadFromFile(BinaryReader reader, FxArchive OwningArchive)
        {
            StringTableIndex = reader.ReadUInt32();
            Text = OwningArchive.GetFromStringTable(StringTableIndex);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(StringTableIndex);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
