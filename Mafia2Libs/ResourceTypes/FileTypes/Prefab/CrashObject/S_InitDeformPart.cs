using BitStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.CrashObject
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_InitDrainEnergy
    {
        public uint DrainPart { get; set; } // ID to part. Assume its an index within the DeformParts list? Ref system required maybe.
        public float DrainEnergyCoeff { get; set; } // Game multiplies this by 100.

        public void Load(BitStream MemStream)
        {
            DrainPart = MemStream.ReadUInt32();
            DrainEnergyCoeff = MemStream.ReadSingle() * 100.0f;
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32(DrainPart);
            MemStream.WriteSingle(DrainEnergyCoeff / 100.0f);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_InitDropPart
    {
        public int DropPart { get; set; }
        public float VersionOrEnergy { get; set; }
        public uint Flags { get; set; }

        public void Load(BitStream MemStream)
        {
            DropPart = MemStream.ReadInt32();
            VersionOrEnergy = MemStream.ReadSingle();
            Flags = MemStream.ReadUInt32();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteInt32(DropPart);
            MemStream.WriteSingle(VersionOrEnergy);
            MemStream.WriteUInt32(Flags);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_InitInternalImpulse
    {
        public C_Vector3 Direction { get; set; }
        public C_Vector3 DirectioNormal { get; set; }
        public C_Vector3 Position { get; set; }
        public float Gain { get; set; }
        public float GainSpreadDown { get; set; }
        public float DirectionSpread { get; set; }
        public float VersionOrEnergy { get; set; }
        public float Flags { get; set; }

        public S_InitInternalImpulse()
        {
            Direction = new C_Vector3();
            DirectioNormal = new C_Vector3();
            Position = new C_Vector3();
        }

        public void Load(BitStream MemStream)
        {
            Direction.Load(MemStream);
            DirectioNormal.Load(MemStream);
            Position.Load(MemStream);
            Gain = MemStream.ReadSingle();
            GainSpreadDown = MemStream.ReadSingle();
            DirectionSpread = MemStream.ReadSingle();
            VersionOrEnergy = MemStream.ReadSingle();
            Flags = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            Direction.Save(MemStream);
            DirectioNormal.Save(MemStream);
            Position.Save(MemStream);
            MemStream.WriteSingle(Gain);
            MemStream.WriteSingle(GainSpreadDown);
            MemStream.WriteSingle(DirectionSpread);
            MemStream.WriteSingle(VersionOrEnergy);
            MemStream.WriteSingle(Flags);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformOrigData
    {
        public ulong Hash { get; set; }
        public C_Transform Unk1 { get; set; }
        public ushort[] Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public ushort Unk4 { get; set; }

        public S_InitDeformOrigData()
        {
            Unk1 = new C_Transform();
            Unk2 = new ushort[0];
        }

        public void Load(BitStream MemStream)
        {
            Hash = MemStream.ReadUInt64();
            Unk1.Load(MemStream);

            // Read array
            uint NumUnk2 = MemStream.ReadUInt32();
            Unk2 = new ushort[NumUnk2];
            for (int i = 0; i < NumUnk2; i++)
            {
                Unk2[i] = MemStream.ReadUInt16();
            }

            Unk3 = MemStream.ReadUInt16();
            Unk4 = MemStream.ReadUInt16();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Hash);
            Unk1.Save(MemStream);

            // Write array
            MemStream.WriteInt32(Unk2.Length);
            foreach (ushort Value in Unk2)
            {
                MemStream.WriteUInt16(Value);
            }

            MemStream.WriteUInt16(Unk3);
            MemStream.WriteUInt16(Unk4);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_InitDeformRelData
    {
        public C_Vector3 Unk0 { get; set; }
        public C_Transform Transform { get; set; }

        public S_InitDeformRelData()
        {
            Unk0 = new C_Vector3();
            Transform = new C_Transform();
        }

        public void Load(BitStream MemStream)
        {
            Unk0.Load(MemStream);
            Transform.Load(MemStream);
        }

        public void Save(BitStream MemStream)
        {
            Unk0.Save(MemStream);
            Transform.Save(MemStream);
        }
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_InitDeformPart
    {
        public uint Unk0 { get; set; } // [13 = MOTOR] [15 = SNOW] [12 = EXHAUST] [16 = PLOW] [7 = BUMPER] [5 = WINDOW] [6 = COVER] [3 = LID] [14 = TYRE] [2 = WHEEL] [1 = BODY]
        public uint Unk1 { get; set; } // [2 = ALWAYS DYNAMIC?] [16 = KILL PART] [400 = SNOW] [8192 = AIBOX] [262144 = FADE OFF]
        public byte Unk2 { get; set; }
        public ulong[] Unk3 { get; set; }
        public float Unk4 { get; set; }
        public float Unk5 { get; set; } // Hitpoints Min?
        public float Unk6 { get; set; } // Hitpoints Max
        public C_Vector3 CentreOfMass { get; set; }
        public S_InitInternalImpulse[] DPInternalImpulses { get; set; }
        public S_InitDropPart[] DropParts { get; set; }
        public S_InitDrainEnergy[] DPDrainEnergy { get; set; }
        public uint Unk13 { get; set; }
        public S_InitCollVolumeCollection[] CollisionVolumesCollections { get; set; }
        public S_InitSMDeformBone[] SMDeformBones { get; set; }
        public ushort[] Unk14 { get; set; } // indexes?
        public C_Transform PartTransform { get; set; } // Transform of the part
        public ulong ParentDeformPartName { get; set; }
        public ushort Unk17 { get; set; }
        public byte Unk18 { get; set; }
        public byte Unk19 { get; set; }
        public ushort[] Unk20 { get; set; } // indexes?
        public byte Unk21 { get; set; } // flag to check whether some data is available
        public S_InitDeformOrigData Unk21Data { get; set; } // Only present if Unk21 is valid
        public byte Unk22 { get; set; } // flag to check whether some data is available
        public S_InitDeformRelData Unk22_RelData { get; set; } // Only present if Unk22 is valid.
        public uint Unk23 { get; set; }
        public uint Unk24 { get; set; }
        public S_InitDeformPartCommon CommonData { get; set; }

        public S_InitDeformPart()
        {
            Unk3 = new ulong[0];
            CentreOfMass = new C_Vector3();
            DPDrainEnergy = new S_InitDrainEnergy[0];
            DropParts = new S_InitDropPart[0];
            CollisionVolumesCollections = new S_InitCollVolumeCollection[0];
            SMDeformBones = new S_InitSMDeformBone[0];
            DPInternalImpulses = new S_InitInternalImpulse[0];
            Unk14 = new ushort[0];
            PartTransform = new C_Transform();
            Unk20 = new ushort[0];
            Unk21Data = new S_InitDeformOrigData();
            CommonData = new S_InitDeformPartCommon();
        }

        public void Load(BitStream MemStream)
        {
            // [13 = MOTOR] [15 = SNOW] [12 = EXHAUST] [16 = PLOW] [7 = BUMPER] [5 = WINDOW] [6 = COVER] [3 = LID] [14 = TYRE] [2 = WHEEL] [1 = BODY] [4 = DOOR] [0 = NORMAL?]
            Unk0 = MemStream.ReadUInt32();
            Unk1 = MemStream.ReadUInt32();
            Unk2 = MemStream.ReadBit();

            switch (Unk0)
            {
                case 13:
                case 15:
                case 12:
                case 16:
                case 7:
                case 5:
                case 6:
                case 3:
                case 14:
                case 2:
                case 4:
                case 1:
                case 0:
                    break;
                default:
                    MessageBox.Show("Unknown DeformPart Type", "Toolkit");
                    break;
            }

            // collect hashes
            Unk3 = PrefabUtils.ReadHashArray(MemStream);

            Unk4 = MemStream.ReadSingle();
            Unk5 = MemStream.ReadSingle();
            Unk6 = MemStream.ReadSingle();
            CentreOfMass.Load(MemStream);

            uint NumDPInternalImpulses = MemStream.ReadUInt32();
            DPInternalImpulses = new S_InitInternalImpulse[NumDPInternalImpulses];
            for (int i = 0; i < NumDPInternalImpulses; i++)
            {
                S_InitInternalImpulse Packet = new S_InitInternalImpulse();
                Packet.Load(MemStream);
                DPInternalImpulses[i] = Packet;
            }

            // Read InitDropParts data
            uint NumDropParts = MemStream.ReadUInt32(); // Count
            DropParts = new S_InitDropPart[NumDropParts];
            for (int i = 0; i < DropParts.Length; i++)
            {
                S_InitDropPart DropPart = new S_InitDropPart();
                DropPart.Load(MemStream);
                DropParts[i] = DropPart;
            }

            // Read InitDrainEnergy data
            uint NumInitDrainEnergy = MemStream.ReadUInt32(); // Count
            DPDrainEnergy = new S_InitDrainEnergy[NumInitDrainEnergy];
            for (int i = 0; i < NumInitDrainEnergy; i++)
            {
                S_InitDrainEnergy DrainEnergyEntry = new S_InitDrainEnergy();
                DrainEnergyEntry.Load(MemStream);
                DPDrainEnergy[i] = DrainEnergyEntry;
            }

            // Read CollVolume Collections
            uint NumCollisionArrays = MemStream.ReadUInt32();
            CollisionVolumesCollections = new S_InitCollVolumeCollection[NumCollisionArrays];
            for (uint i = 0; i < NumCollisionArrays; i++)
            {
                S_InitCollVolumeCollection NewCollection = new S_InitCollVolumeCollection();
                NewCollection.Read(MemStream);
                CollisionVolumesCollections[i] = NewCollection;
            }

            // Read SM Deform bones
            uint NumSMDeformBones = MemStream.ReadUInt32();
            SMDeformBones = new S_InitSMDeformBone[NumSMDeformBones];
            for (int i = 0; i < NumSMDeformBones; i++)
            {
                S_InitSMDeformBone DeformBone = new S_InitSMDeformBone();
                DeformBone.Load(MemStream);
                SMDeformBones[i] = DeformBone;
            }

            // UInt16s - indexes?
            uint NumIndexes = MemStream.ReadUInt32();
            Unk14 = new ushort[NumIndexes];

            for (int i = 0; i < NumIndexes; i++)
            {
                Unk14[i] = MemStream.ReadUInt16();
            }

            // Read matrix
            PartTransform.Load(MemStream);

            ParentDeformPartName = MemStream.ReadUInt64();
            Unk17 = MemStream.ReadUInt16();
            Unk18 = MemStream.ReadByte();
            Unk19 = MemStream.ReadByte();

            // UInt16s - indexes?
            uint NumIndexes2 = MemStream.ReadUInt32();
            Unk20 = new ushort[NumIndexes2];
            for (int i = 0; i < NumIndexes2; i++)
            {
                Unk20[i] = MemStream.ReadUInt16();
            }

            // If one - means something is available.
            Unk21 = MemStream.ReadBit();
            if (Unk21 == 1)
            {
                Unk21Data = new S_InitDeformOrigData();
                Unk21Data.Load(MemStream);
            }

            // If one - means something is available.
            Unk22 = MemStream.ReadBit();
            if (Unk22 == 1)
            {
                Unk22_RelData = new S_InitDeformRelData();
                Unk22_RelData.Load(MemStream);
            }

            Unk23 = MemStream.ReadUInt32(); // 0?
            Unk24 = MemStream.ReadUInt32(); // 0?

            CommonData = new S_InitDeformPartCommon();
            CommonData.Load(MemStream);
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt32(Unk0);
            MemStream.WriteUInt32(Unk1);
            MemStream.WriteBit(Unk2);

            PrefabUtils.WriteHashArray(MemStream, Unk3);

            MemStream.WriteSingle(Unk4);
            MemStream.WriteSingle(Unk5);
            MemStream.WriteSingle(Unk6);
            CentreOfMass.Save(MemStream);

            MemStream.WriteUInt32((uint)DPInternalImpulses.Length);
            foreach (S_InitInternalImpulse Value in DPInternalImpulses)
            {
                Value.Save(MemStream);
            }

            // Write S_InitDropPart entries
            MemStream.WriteUInt32((uint)DropParts.Length);
            foreach (S_InitDropPart Value in DropParts)
            {
                Value.Save(MemStream);
            }

            // Write S_InitDrainEnergy entries
            MemStream.WriteUInt32((uint)DPDrainEnergy.Length);
            foreach (S_InitDrainEnergy Value in DPDrainEnergy)
            {
                Value.Save(MemStream);
            }

            // Write Collision Volumes
            MemStream.WriteUInt32((uint)CollisionVolumesCollections.Length);
            foreach (S_InitCollVolumeCollection ColVolume in CollisionVolumesCollections)
            {
                ColVolume.Save(MemStream);
            }

            // Write SM Deform bones
            MemStream.WriteUInt32((uint)SMDeformBones.Length);
            foreach (S_InitSMDeformBone Value in SMDeformBones)
            {
                Value.Save(MemStream);
            }

            // UInt16s - indexes?
            MemStream.WriteUInt32((uint)Unk14.Length);
            foreach (ushort Value in Unk14)
            {
                MemStream.WriteUInt16(Value);
            }

            // Write Matrix
            PartTransform.Save(MemStream);

            MemStream.WriteUInt64(ParentDeformPartName);
            MemStream.WriteUInt16(Unk17);
            MemStream.WriteByte(Unk18);
            MemStream.WriteByte(Unk19);

            // UInt20s - indexes?
            MemStream.WriteUInt32((uint)Unk20.Length);
            foreach (ushort Value in Unk20)
            {
                MemStream.WriteUInt16(Value);
            }

            // Write unknown hash
            MemStream.WriteBit(Unk21);
            if (Unk21 == 1)
            {
                Unk21Data.Save(MemStream);
            }

            MemStream.WriteBit(Unk22);
            if (Unk22 == 1)
            {
                Unk22_RelData.Save(MemStream);
            }

            MemStream.WriteUInt32(Unk23);
            MemStream.WriteUInt32(Unk24);

            CommonData.Save(MemStream);
        }
    }
}
