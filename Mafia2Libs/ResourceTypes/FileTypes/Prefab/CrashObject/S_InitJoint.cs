using BitStreams;
using System;
using System.Diagnostics;
using Utils.Logging;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_InitPartBreakEnergy
    {
        public int PartID { get; set; }
        public float BreakEnergy { get; set; }

        public void Load(BitStream MemStream)
        {
            PartID = MemStream.ReadInt32();
            BreakEnergy = MemStream.ReadSingle();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteInt32(PartID);
            MemStream.WriteSingle(BreakEnergy);
        }
    }

    public class S_InitJoint
    {
        public ushort Unk0 { get; set; }
        public ushort Unk1 { get; set; }
        public ushort Unk2 { get; set; }
        public ushort Unk3 { get; set; }
        public S_InitJointSet[] JointSets { get; set; }
        public C_Transform Unk4 { get; set; } // transform 0
        public C_Transform Unk5 { get; set; } // transform 1
        public string Unk6 { get; set; }
        public C_Vector3 Unk7 { get; set; } // Vector3?
        public S_InitPartBreakEnergy[] PartBreakEnergyList { get; set; }

        public S_InitJoint()
        {
            JointSets = new S_InitJointSet[0];
            Unk4 = new C_Transform();
            Unk5 = new C_Transform();
            Unk6 = String.Empty;
            Unk7 = new C_Vector3();
            PartBreakEnergyList = new S_InitPartBreakEnergy[0];
        }

        public void Load(BitStream MemStream)
        {
            Unk0 = MemStream.ReadUInt16();
            Unk1 = MemStream.ReadUInt16();
            Unk2 = MemStream.ReadUInt16();
            Unk3 = MemStream.ReadUInt16();

            uint NumJointSets = MemStream.ReadUInt32();
            JointSets = new S_InitJointSet[NumJointSets];
            for (int i = 0; i < NumJointSets; i++)
            {
                S_InitJointSet NewSet = new S_InitJointSet();
                NewSet.Load(MemStream);
                JointSets[i] = NewSet;
            }

            // Read Matrices
            Unk4.Load(MemStream);
            Unk5.Load(MemStream);

            // If one - means something is available.
            bool bIsValidString = MemStream.ReadBit();
            if(bIsValidString)
            {
                Unk6 = MemStream.ReadString32();
            }

            Unk7.Load(MemStream);

            // Load Part Break Energy array
            uint NumPartBreakEnergies = MemStream.ReadUInt32();
            PartBreakEnergyList = new S_InitPartBreakEnergy[NumPartBreakEnergies];
            for(uint i = 0; i < NumPartBreakEnergies; i++)
            {
                S_InitPartBreakEnergy PartBreakEnergy = new S_InitPartBreakEnergy();
                PartBreakEnergy.Load(MemStream);
                PartBreakEnergyList[i] = PartBreakEnergy;
            }
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt16(Unk0);
            MemStream.WriteUInt16(Unk1);
            MemStream.WriteUInt16(Unk2);
            MemStream.WriteUInt16(Unk3);

            // Write Joints
            MemStream.WriteUInt32((uint)JointSets.Length);
            foreach(S_InitJointSet JointSet in JointSets)
            {
                JointSet.Save(MemStream);
            }

            // Write Matrices
            Unk4.Save(MemStream);
            Unk5.Save(MemStream);

            // Write string
            bool bIsValidString = (Unk6 != String.Empty);
            MemStream.WriteBit(bIsValidString);
            if (bIsValidString)
            {
                MemStream.WriteString32(Unk6);
            }

            Unk7.Save(MemStream);

            // Save PartBreakEnergy
            MemStream.WriteInt32(PartBreakEnergyList.Length);
            foreach(S_InitPartBreakEnergy PartBreakEntry in PartBreakEnergyList)
            {
                PartBreakEntry.Save(MemStream);
            }
        }
    }
}
