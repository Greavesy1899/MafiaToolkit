using ResourceTypes.M3.XBin;
using ResourceTypes.XBin.Types;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_SetPosDirPlayer : ICommand
    {
        private readonly uint Magic = 0x72386E2B;

        public XBinVector3 Position { get; set; }
        public XBinVector3 Direction { get; set; }

        public Command_SetPosDirPlayer()
        {
            Position = new XBinVector3();
            Direction = new XBinVector3();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Position.ReadFromFile(reader);
            Direction.ReadFromFile(reader);
        }

        public void WriteToFile(XBinWriter writer)
        {
            Position.WriteToFile(writer);
            Direction.WriteToFile(writer);
        }
        public int GetSize()
        {
            return 24;
        }
        public uint GetMagic()
        {
            return Magic;
        }
    }
}
