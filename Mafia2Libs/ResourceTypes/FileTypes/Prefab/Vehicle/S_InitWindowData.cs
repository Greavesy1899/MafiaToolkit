using BitStreams;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    public class S_InitWindowData
    {
        [PropertyForceAsAttribute]
        public ulong WindowFrameName { get; set; }
        public ulong[] CheckBoneFrameName { get; set; }
        [PropertyForceAsAttribute]
        public float Depth { get; set; }
        [PropertyForceAsAttribute]
        public bool bIsOpenable { get; set; }

        public S_InitWindowData()
        {
            CheckBoneFrameName = new ulong[0];
        }

        public void Load(BitStream MemStream)
        {
            WindowFrameName = MemStream.ReadUInt64();
            CheckBoneFrameName = PrefabUtils.ReadHashArray(MemStream);
            Depth = MemStream.ReadSingle();
            bIsOpenable = MemStream.ReadBit();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(WindowFrameName);
            PrefabUtils.WriteHashArray(MemStream, CheckBoneFrameName);
            MemStream.WriteSingle(Depth);
            MemStream.WriteBit(bIsOpenable);
        }
    }
}
