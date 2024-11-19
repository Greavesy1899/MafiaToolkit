using System.ComponentModel;
using System.IO;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class FxCustom
    {
        [Browsable(false)]
        public int Type { get; set; }
        [Browsable(false)]
        public HIRCObject Parent { get; set; }
        public uint ID { get; set; }
        public FXBase FXBase { get; set; }
        public FxCustom(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            Parent = ParentObject;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            FXBase = new FXBase(ParentObject, br);
        }

        public FxCustom(HIRCObject ParentObject)
        {
            Parent = ParentObject;
            Type = 0;
            ID = 0;
            FXBase = new FXBase(Parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            FXBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int Length = 4 + FXBase.GetLength();

            return Length;
        }
    }
}
