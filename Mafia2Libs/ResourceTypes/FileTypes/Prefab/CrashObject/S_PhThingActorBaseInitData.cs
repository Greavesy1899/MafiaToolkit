using BitStreams;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_PhThingActorBaseInitData : S_GlobalInitData
    {
        public ulong[] Unk0 { get; set; }
        public ulong[] Unk1 { get; set; }

        public S_PhThingActorBaseInitData()
        {
            Unk0 = new ulong[0];
            Unk1 = new ulong[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            Unk0 = PrefabUtils.ReadHashArray(MemStream);
            Unk1 = PrefabUtils.ReadHashArray(MemStream);
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            PrefabUtils.WriteHashArray(MemStream, Unk0);
            PrefabUtils.WriteHashArray(MemStream, Unk1);
        }
    }
}
