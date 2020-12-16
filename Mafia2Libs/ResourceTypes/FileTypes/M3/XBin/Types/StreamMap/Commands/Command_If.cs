using ResourceTypes.M3.XBin;
using System;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_If : ICommand
    {
        private readonly uint Magic = 0x1EFE290F;

        public string Variable { get; set; }
        public ECommandIfOperator Operator { get; set; }
        public string Value { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Variable = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            Operator = (ECommandIfOperator)reader.ReadInt32();
            Value = XBinCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(-1); // Variable
            writer.Write((uint)Operator);
            writer.Write(-1); // Value
        }
        public int GetSize()
        {
            return 12;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
