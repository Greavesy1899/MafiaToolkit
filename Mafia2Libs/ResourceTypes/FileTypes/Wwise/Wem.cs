using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ResourceTypes.Wwise
{
    public class Wem
    {
        public string Name { get; set; }
        public ulong ID { get; set; }
        public uint LanguageEnum { get; set; }
        [Browsable(false)]
        public uint Offset { get; set; }
        [Browsable(false)]
        public WemHirc AssignedHirc { get; set; }
        [Browsable(false)]
        public byte[] File { get; set; }
        public bool NameChanged = false;

        public Wem(string aName, string aID, BinaryReader aFile, uint newOffset)
        {
            Name = Path.GetFileName(aName);

            ulong TempID = 0;

            if (UInt64.TryParse(Path.GetFileName(aID.Replace(".wem", "")), out TempID))
            {
                ID = TempID;
            }
            else
            {
                ID = 0;
            }

            File = aFile.ReadBytes((int)aFile.BaseStream.Length);
            LanguageEnum = 0;
            Offset = newOffset;
            AssignedHirc = new WemHirc();
            aFile.Close();
        }

        public Wem(string aName, ulong aID, byte[] aBinary)
        {
            Name = aName;
            ID = aID;
            File = aBinary;
            LanguageEnum = 0;
            AssignedHirc = new WemHirc();
        }

        public Wem(string aName, ulong aID, byte[] aBinary, uint lanEnum, uint FileOffset)
        {
            Name = aName;
            ID = aID;
            File = aBinary;
            LanguageEnum = lanEnum;
            Offset = FileOffset;
            AssignedHirc = new WemHirc();
        }
    }
}
