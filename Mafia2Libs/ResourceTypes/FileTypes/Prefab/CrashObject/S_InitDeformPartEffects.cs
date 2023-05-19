using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.CrashObject
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformPartEffect_Pack
    {
        public short Unk0 { get; set; }
        public short Unk1 { get; set; }
        public float Unk2 { get; set; }
        public float Unk3 { get; set; }
        public float Unk4 { get; set; }
        public float Unk5 { get; set; }
        public short Unk6 { get; set; }

        public void Load(BitStream MemStream)
        {
            Unk0 = MemStream.ReadInt16();
            Unk1 = MemStream.ReadInt16();
            Unk2 = MemStream.ReadSingle();
            Unk3 = MemStream.ReadSingle();
            Unk4 = MemStream.ReadSingle();
            Unk5 = MemStream.ReadSingle();
            Unk6 = MemStream.ReadInt16();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteInt16(Unk0);
            MemStream.WriteInt16(Unk1);
            MemStream.WriteSingle(Unk2);
            MemStream.WriteSingle(Unk3);
            MemStream.WriteSingle(Unk4);
            MemStream.WriteSingle(Unk5);
            MemStream.WriteInt16(Unk6);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformPartEffects
    {
        public C_Transform EffectMatrix { get; set; }
        public S_InitDeformPartEffect_Pack[] Unk1 { get; set; } // FIXED ARRAY TO 3
        public short Unk2 { get; set; }
        public short Unk3 { get; set; }
        public short Unk4 { get; set; }
        public float Unk5 { get; set; }
        public float Unk6 { get; set; }
        public float Unk7 { get; set; }
        public float Unk8 { get; set; }
        public short Unk9 { get; set; }
        public short Unk10 { get; set; }
        public short Unk11 { get; set; }
        public short Unk12 { get; set; }
        public short Unk13 { get; set; }
        public short Unk14 { get; set; }
        public float Unk15 { get; set; }
        public float Unk16 { get; set; }
        public float Unk17 { get; set; }
        public float Unk18 { get; set; }
        public short Unk19 { get; set; }
        public short Unk20 { get; set; }
        public short Unk21 { get; set; }
        public short ParticleBreakID { get; set; }
        public short ParticleHingeVersionID { get; set; }
        public short Unk24 { get; set; }
        public short Unk25 { get; set; }
        public short Unk26 { get; set; }
        public short Unk27 { get; set; }
        public short Unk28 { get; set; }
        public short SnowParticleID_0 { get; set; }
        public short SnowParticleID_1 { get; set; }
        public short SnowParticleID_2 { get; set; }
        public short SnowParticleID_3 { get; set; }
        public short Unk33 { get; set; }
        public byte Unk34 { get; set; }
        public float ParticleScale { get; set; }
        public int Unk36 { get; set; }
        public float Unk37 { get; set; }
        public int Unk38 { get; set; }
        public int Unk39 { get; set; }

        public S_InitDeformPartEffects()
        {
            EffectMatrix = new C_Transform();
            Unk1 = new S_InitDeformPartEffect_Pack[3];
            for(int i = 0; i < Unk1.Length; i++)
            {
                Unk1[i] = new S_InitDeformPartEffect_Pack();
            }
        }

        public void Load(BitStream MemStream)
        {
            EffectMatrix.Load(MemStream);

            // packet of data (what is it?)
            Unk1 = new S_InitDeformPartEffect_Pack[3];
            for (int i = 0; i < Unk1.Length; i++)
            {
                S_InitDeformPartEffect_Pack NewPack = new S_InitDeformPartEffect_Pack();
                NewPack.Load(MemStream);
                Unk1[i] = NewPack;
            }

            Unk2 = MemStream.ReadInt16();
            Unk3 = MemStream.ReadInt16();
            Unk4 = MemStream.ReadInt16();
            Unk5 = MemStream.ReadSingle();
            Unk6 = MemStream.ReadSingle();
            Unk7 = MemStream.ReadSingle();
            Unk8 = MemStream.ReadSingle();
            Unk9 = MemStream.ReadInt16();
            Unk10 = MemStream.ReadInt16();
            Unk11 = MemStream.ReadInt16();
            Unk12 = MemStream.ReadInt16();
            Unk13 = MemStream.ReadInt16();
            Unk14 = MemStream.ReadInt16();
            Unk15 = MemStream.ReadSingle();
            Unk16 = MemStream.ReadSingle();
            Unk17 = MemStream.ReadSingle();
            Unk18 = MemStream.ReadSingle();
            Unk19 = MemStream.ReadInt16();
            Unk20 = MemStream.ReadInt16();
            Unk21 = MemStream.ReadInt16();
            ParticleBreakID = MemStream.ReadInt16();
            ParticleHingeVersionID = MemStream.ReadInt16();
            Unk24 = MemStream.ReadInt16();
            Unk25 = MemStream.ReadInt16();
            Unk26 = MemStream.ReadInt16();
            Unk27 = MemStream.ReadInt16();
            Unk28 = MemStream.ReadInt16();
            SnowParticleID_0 = MemStream.ReadInt16();
            SnowParticleID_1 = MemStream.ReadInt16();
            SnowParticleID_2 = MemStream.ReadInt16();
            SnowParticleID_3 = MemStream.ReadInt16();
            Unk33 = MemStream.ReadInt16();
            Unk34 = MemStream.ReadBit();
            ParticleScale = MemStream.ReadSingle();
            Unk36 = MemStream.ReadInt32();
            Unk37 = MemStream.ReadSingle();
            Unk38 = MemStream.ReadInt32();
            Unk39 = MemStream.ReadInt32();
        }

        public void Save(BitStream MemStream)
        {
            EffectMatrix.Save(MemStream);

            // Write whatever this is
            foreach (S_InitDeformPartEffect_Pack Value in Unk1)
            {
                Value.Save(MemStream);
            }

            MemStream.WriteInt16(Unk2);
            MemStream.WriteInt16(Unk3);
            MemStream.WriteInt16(Unk4);
            MemStream.WriteSingle(Unk5);
            MemStream.WriteSingle(Unk6);
            MemStream.WriteSingle(Unk7);
            MemStream.WriteSingle(Unk8);
            MemStream.WriteInt16(Unk9);
            MemStream.WriteInt16(Unk10);
            MemStream.WriteInt16(Unk11);
            MemStream.WriteInt16(Unk12);
            MemStream.WriteInt16(Unk13);
            MemStream.WriteInt16(Unk14);
            MemStream.WriteSingle(Unk15);
            MemStream.WriteSingle(Unk16);
            MemStream.WriteSingle(Unk17);
            MemStream.WriteSingle(Unk18);
            MemStream.WriteInt16(Unk19);
            MemStream.WriteInt16(Unk20);
            MemStream.WriteInt16(Unk21);
            MemStream.WriteInt16(ParticleBreakID);
            MemStream.WriteInt16(ParticleHingeVersionID);
            MemStream.WriteInt16(Unk24);
            MemStream.WriteInt16(Unk25);
            MemStream.WriteInt16(Unk26);
            MemStream.WriteInt16(Unk27);
            MemStream.WriteInt16(Unk28);
            MemStream.WriteInt16(SnowParticleID_0);
            MemStream.WriteInt16(SnowParticleID_1);
            MemStream.WriteInt16(SnowParticleID_2);
            MemStream.WriteInt16(SnowParticleID_3);
            MemStream.WriteInt16(Unk33);
            MemStream.WriteBit(Unk34);
            MemStream.WriteSingle(ParticleScale);
            MemStream.WriteInt32(Unk36);
            MemStream.WriteSingle(Unk37);
            MemStream.WriteInt32(Unk38);
            MemStream.WriteInt32(Unk39);
        }
    }
}
