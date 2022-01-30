using BitStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.CrashObject
{
    public class S_LiftFloor
    {
        public int Unk0 { get; set; }
        public byte Unk1 { get; set; } // bit
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }

        public void Load(BitStream MemStream)
        {
            Unk0 = MemStream.ReadInt32();
            Unk1 = MemStream.ReadBit();
            Unk2 = MemStream.ReadInt32();
            Unk3 = MemStream.ReadInt32();
            Unk4 = MemStream.ReadInt32();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteInt32(Unk0);
            MemStream.WriteBit(Unk1);
            MemStream.WriteInt32(Unk2);
            MemStream.WriteInt32(Unk3);
            MemStream.WriteInt32(Unk4);
        }
    }

    public class S_InternalDoor
    {
        public ulong Unk0 { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public int Unk5 { get; set; }
        public int Unk6 { get; set; }
        public int Unk7 { get; set; }
        public int Unk8 { get; set; }
        public byte Unk9 { get; set; }

        public void Load(BitStream MemStream)
        {
            Unk0 = MemStream.ReadUInt64();
            Unk1 = MemStream.ReadInt32();
            Unk2 = MemStream.ReadInt32();
            Unk3 = MemStream.ReadInt32();
            Unk4 = MemStream.ReadInt32();
            Unk5 = MemStream.ReadInt32();
            Unk6 = MemStream.ReadInt32();
            Unk7 = MemStream.ReadInt32();
            Unk8 = MemStream.ReadInt32();
            Unk9 = MemStream.ReadBit();
        }

        public void Save(BitStream MemStream)
        {
            MemStream.WriteUInt64(Unk0);
            MemStream.WriteInt32(Unk1);
            MemStream.WriteInt32(Unk2);
            MemStream.WriteInt32(Unk3);
            MemStream.WriteInt32(Unk4);
            MemStream.WriteInt32(Unk5);
            MemStream.WriteInt32(Unk6);
            MemStream.WriteInt32(Unk7);
            MemStream.WriteInt32(Unk8);
            MemStream.WriteBit(Unk9);
        }
    }

    public class S_LiftInitData : S_ActorDeformInitData
    {
        public ulong BodyModelFrameHash { get; set; }
        public ulong ButtonFrameHash { get; set; }
        public ulong LightFrameHash { get; set; }
        public S_LiftFloor[] Floors { get; set; }
        public S_InternalDoor[] InternalDoors { get; set; }
        public ulong[] AIEnterActionFrameHashes { get; set; }
        public ulong[] AILeaveActionFrameHashes { get; set; }

        public S_LiftInitData() : base()
        {
            Floors = new S_LiftFloor[0];
            InternalDoors = new S_InternalDoor[0];
            AIEnterActionFrameHashes = new ulong[0];
            AILeaveActionFrameHashes = new ulong[0];
        }

        public override void Load(BitStream MemStream)
        {
            base.Load(MemStream);

            BodyModelFrameHash = MemStream.ReadUInt64();
            ButtonFrameHash = MemStream.ReadUInt64();
            LightFrameHash = MemStream.ReadUInt64();

            int NumFloors = MemStream.ReadInt32();
            Floors = new S_LiftFloor[NumFloors];
            for(int i = 0; i < NumFloors; i++)
            {
                S_LiftFloor NewData = new S_LiftFloor();
                NewData.Load(MemStream);
                Floors[i] = NewData;
            }

            int NumDoors = MemStream.ReadInt32();
            InternalDoors = new S_InternalDoor[NumDoors];
            for (int i = 0; i < NumDoors; i++)
            {
                S_InternalDoor NewData = new S_InternalDoor();
                NewData.Load(MemStream);
                InternalDoors[i] = NewData;
            }

            AIEnterActionFrameHashes = PrefabUtils.ReadHashArray(MemStream);
            AILeaveActionFrameHashes = PrefabUtils.ReadHashArray(MemStream);
        }

        public override void Save(BitStream MemStream)
        {
            base.Save(MemStream);

            MemStream.WriteUInt64(BodyModelFrameHash);
            MemStream.WriteUInt64(ButtonFrameHash);
            MemStream.WriteUInt64(LightFrameHash);

            // Write all floors
            MemStream.WriteInt32(Floors.Length);
            foreach(S_LiftFloor Floor in Floors)
            {
                Floor.Save(MemStream);
            }

            // Write all doors
            MemStream.WriteInt32(InternalDoors.Length);
            foreach (S_InternalDoor Door in InternalDoors)
            {
                Door.Save(MemStream);
            }

            PrefabUtils.WriteHashArray(MemStream, AIEnterActionFrameHashes);
            PrefabUtils.WriteHashArray(MemStream, AILeaveActionFrameHashes);
        }
    }
}
