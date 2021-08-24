using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ResourceTypes.Wwise.HIRC;
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
        public WemHirc AssignedHirc { get; set; }
        [Browsable(false)]
        public byte[] file { get; set; }
        public bool nameChanged = false;

        public Wem(string aName, string aId, BinaryReader aFile, uint newOffset)
        {
            Name = Path.GetFileName(aName);

            ulong tempId = 0;

            if (UInt64.TryParse(Path.GetFileName(aId.Replace(".wem", "")), out tempId))
            {
                ID = tempId;
            }
            else
            {
                ID = 0;
            }

            file = aFile.ReadBytes((int)aFile.BaseStream.Length);
            LanguageEnum = 0;
            Offset = newOffset;
            AssignedHirc = new WemHirc();
            aFile.Close();
        }

        public Wem(string aName, ulong aID, byte[] aBinary)
        {
            Name = aName;
            ID = aID;
            file = aBinary;
            LanguageEnum = 0;
            AssignedHirc = new WemHirc();
        }

        public Wem(string aName, ulong aID, byte[] aBinary, uint lanEnum, uint fileOffset)
        {
            Name = aName;
            ID = aID;
            file = aBinary;
            LanguageEnum = lanEnum;
            Offset = fileOffset;
            AssignedHirc = new WemHirc();
        }
    }
}
