using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxMasterBoneListEntry 
    {
        public FxName BoneName { get; set; }
        public FxBoneTransform Transform { get; set; }
        byte[] UnkQuaternion { get; set; } // 4 floats.. could be quat?
        public uint PossibleIdx2 { get; set; }
        public float Unk0 { get; set; }
        public FxBoneLink[] Links { get; set; }

        public FxMasterBoneListEntry()
        {
            BoneName = new FxName();
        }

        public void ReadFromFile(FxArchive Owner, BinaryReader reader)
        {
            BoneName.ReadFromFile(Owner, reader);
            Transform = new FxBoneTransform();
            Transform.ReadFromFile(reader);
            UnkQuaternion = reader.ReadBytes(16);
            PossibleIdx2 = reader.ReadUInt32();
            Unk0 = reader.ReadSingle();

            uint NumLinks = reader.ReadUInt32();
            Links = new FxBoneLink[NumLinks];
            for (int i = 0; i < NumLinks; i++)
            {
                FxBoneLink LinkObject = new FxBoneLink();
                LinkObject.ReadFromFile(reader);
                Links[i] = LinkObject;
            }
        }

        public void WriteToFile(FxArchive Owner, BinaryWriter writer)
        {
            BoneName.WriteToFile(Owner, writer);
            Transform.WriteToFile(writer);
            writer.Write(UnkQuaternion);
            writer.Write(PossibleIdx2);
            writer.Write(Unk0);

            writer.Write(Links.Length);
            foreach (FxBoneLink Link in Links)
            {
                Link.WriteToFile(writer);
            }
        }

        public void PopulateStringTable(FxArchive Owner)
        {
            BoneName.AddToStringTable(Owner);
        }

        public override string ToString()
        {
            return BoneName.ToString();
        }
    }
}
