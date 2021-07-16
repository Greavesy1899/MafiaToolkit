using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxAnimSet : FxNamedObject
    {
        public uint AnimSet_Unk04 { get; set; }
        public FxName ActorName { get; set; } // Index is NOT stored
        public FxAnimGroup[] AnimGroups { get; set; }

        public FxAnimSet()
        {
            ActorName = new FxName();
            AnimGroups = new FxAnimGroup[0];
        }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            AnimSet_Unk04 = Reader.ReadUInt32(); // Unknown; could be data about published or not.

            // This is not stored in the file
            ActorName.SetIndex(Owner, 1);

            // Read groups.
            uint NumAnimGroups = Reader.ReadUInt32();
            AnimGroups = new FxAnimGroup[NumAnimGroups];
            for (int i = 0; i < NumAnimGroups; i++)
            {
                FxAnimGroup GroupObject = new FxAnimGroup();
                GroupObject.Deserialize(Owner, Reader);

                AnimGroups[i] = GroupObject;
            }
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            Writer.Write(AnimSet_Unk04);

            // Write Groups
            Writer.Write(AnimGroups.Length);
            foreach (FxAnimGroup Group in AnimGroups)
            {
                Group.Serialize(Owner, Writer);
            }
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            ActorName.AddToStringTable(Owner);

            foreach (FxAnimGroup Group in AnimGroups)
            {
                Group.PopulateStringTable(Owner);
            }
        }
    }
}
