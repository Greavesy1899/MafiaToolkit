using BitStreams;
using ResourceTypes.Prefab.CrashObject;

namespace ResourceTypes.Prefab.Door
{
    public class S_DoorInitData : S_ActorDeformInitData
    {
        public S_InitHingeData[] Hinges { get; set; }

        public S_DoorInitData()
        {
            Hinges = new S_InitHingeData[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            uint NumHinges = MemStream.ReadUInt32();
            Hinges = new S_InitHingeData[NumHinges];
            for(uint i = 0; i < NumHinges; i++)
            {
                S_InitHingeData Hinge = new S_InitHingeData();
                Hinge.Load(MemStream);
                Hinges[i] = Hinge;
            }
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt32((uint)Hinges.Length);
            foreach(S_InitHingeData Hinge in Hinges)
            {
                Hinge.Save(MemStream);
            }
        }
    }
}
