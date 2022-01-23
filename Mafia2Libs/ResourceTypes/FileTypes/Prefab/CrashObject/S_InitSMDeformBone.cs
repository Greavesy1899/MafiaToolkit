using BitStreams;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitSMDeformBone
    {
        public ulong SMJointName { get; set; }
        public C_Vector3 OriginalPosition { get; set; }
        public C_Vector3 Range { get; set; }
        public C_Vector3 MoveAccumulator { get; set; }
        public float Intensity { get; set; }
        public float CRadius { get; set; }

        public S_InitSMDeformBone()
        {
            OriginalPosition = new C_Vector3();
            Range = new C_Vector3();
            MoveAccumulator = new C_Vector3();
        }

        public void Load(BitStream MemStream)
        {
            SMJointName = MemStream.ReadUInt64();

            OriginalPosition = C_Vector3.Construct(MemStream);
            Range = C_Vector3.Construct(MemStream);
            MoveAccumulator = C_Vector3.Construct(MemStream);

            Intensity = MemStream.ReadSingle();
            CRadius = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(SMJointName);

            OriginalPosition.Save(MemStream);
            Range.Save(MemStream);
            MoveAccumulator.Save(MemStream);

            MemStream.WriteSingle(Intensity);
            MemStream.WriteSingle(CRadius);
        }
    }
}
