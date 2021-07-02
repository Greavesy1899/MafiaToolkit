using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxName
    {
        public string Text { get; set; }

        public FxName()
        {
            Text = "Null";
        }

        public void ReadFromFile(FxArchive OwningArchive, BinaryReader Reader)
        {
            uint StringTableIndex = Reader.ReadUInt32();
            Text = OwningArchive.GetFromStringTable(StringTableIndex);
        }

        public void WriteToFile(FxArchive OwningArchive, BinaryWriter Writer)
        {
            int StringTableIndex = OwningArchive.GetFromStringTable(Text);
            Writer.Write((uint)StringTableIndex);
        }

        public void AddToStringTable(FxArchive OwningArchive)
        {
            OwningArchive.AddToStringTable(Text);
        }

        public void SetIndex(FxArchive OwningArchive, uint Index)
        {
            Text = OwningArchive.GetFromStringTable(Index);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
