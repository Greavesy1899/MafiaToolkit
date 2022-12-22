using BitStreams;
using System.ComponentModel;
using System.Diagnostics;

namespace ResourceTypes.Prefab.CrashObject
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_ActorDeformInit_Data0
    {
        public ulong Hash { get; set; }
        public uint Unk0 { get; set; }

        public void Load(BitStream MemStream)
        {
            Hash = MemStream.ReadUInt64();
            Unk0 = MemStream.ReadUInt32();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Hash);
            MemStream.WriteUInt32(Unk0);
        }
    }

    public class S_ActorDeformInitData : S_DeformationInitData
    {
        public S_ActorDeformInit_Data0[] Unk0 { get; set; }
        public S_InitActionPointData[] ActionPoints { get; set; }

        public S_ActorDeformInitData() : base()
        {
            Unk0 = new S_ActorDeformInit_Data0[0];
            ActionPoints = new S_InitActionPointData[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            // Load unknown array
            uint NumUnk0 = MemStream.ReadUInt32();
            Unk0 = new S_ActorDeformInit_Data0[NumUnk0];
            for(int i = 0; i < NumUnk0; i++)
            {
                S_ActorDeformInit_Data0 NewEntry = new S_ActorDeformInit_Data0();
                NewEntry.Load(MemStream);
                Unk0[i] = NewEntry;
            }

            // Load ActionPoint data
            uint NumActionPoints = MemStream.ReadUInt32();
            ActionPoints = new S_InitActionPointData[NumActionPoints];
            for(int i = 0; i < NumActionPoints; i++)
            {
                S_InitActionPointData ActionPoint = new S_InitActionPointData();
                ActionPoint.Load(MemStream);
                ActionPoints[i] = ActionPoint;
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            // Save unknown Data
            MemStream.WriteUInt32((uint)Unk0.Length);
            foreach (S_ActorDeformInit_Data0 Entry in Unk0)
            {
                Entry.Save(MemStream);
            }

            // Save ActionPoint Data
            MemStream.WriteUInt32((uint)ActionPoints.Length);
            foreach(S_InitActionPointData ActionPoint in ActionPoints)
            {
                ActionPoint.Save(MemStream);
            }
        }
    }
}
