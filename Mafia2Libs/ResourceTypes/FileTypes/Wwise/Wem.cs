using System;
using System.IO;

namespace ResourceTypes.Wwise
{
    public class Wem
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public int Length { get; set; }
        [System.ComponentModel.ReadOnly(true)]
        public uint Offset { get; set; }
        public string Language { get; set; }
        //public WemHirc assignedHirc { get; set; } For future BNK editor
        public uint UnkOffset;
        public byte[] File;

        public Wem(string aName, string aId, BinaryReader aFile, uint newOffset)
        {
            Name = Path.GetFileName(aName);
            Id = Convert.ToUInt32(Path.GetFileName(aId.Replace(".wem", "")));
            File = aFile.ReadBytes((int)aFile.BaseStream.Length);
            Length = File.Length;
            Language = "0";
            Offset = newOffset;
            //assignedHirc = new WemHirc();
            aFile.Close();
        }

        public Wem(string aName, uint aID, byte[] aBinary)
        {
            Name = aName;
            Id = aID;
            Length = aBinary.Length;
            File = aBinary;
            Language = "0";
            //assignedHirc = new WemHirc();
        }
        public Wem(string aName, uint aID, byte[] aBinary, string lang, uint fileOffset)
        {
            Name = aName;
            Id = aID;
            Length = aBinary.Length;
            File = aBinary;
            Language = lang;
            Offset = fileOffset;
            //assignedHirc = new WemHirc();
        }
    }
}
