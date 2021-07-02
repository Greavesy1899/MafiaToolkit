using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.OC3.FaceFX
{
    // TODO: Try and find the name of this, it's not actually called FxClass.

    /*
    * Serialized set of information stored in FxArchives.
    * Seems to note some type of object which is used during the FxArchive. 
    */
    public class FxClass
    {
        public uint Unk01 { get; set; }
        public uint Unk02 { get; set; }
        public string Name { get; set; }
        public ushort Unk03 { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk01 = reader.ReadUInt32();
            Unk02 = reader.ReadUInt32();
            Name = StringHelpers.ReadString32(reader);
            Unk03 = reader.ReadUInt16();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(Unk01);
            writer.Write(Unk02);
            StringHelpers.WriteString32(writer, Name);
            writer.Write(Unk03);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
