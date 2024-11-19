﻿using ResourceTypes.M3.XBin;
using ResourceTypes.XBin.Types;
using System.Diagnostics;
using System.IO;
using Utils.Logging;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class VehicleInstance
    {
        public XBinVector3 Position { get; set; }
        public XBinVector3 Direction { get; set; }
        public string EntityName { get; set; }
        public uint LoadFlags { get; set; }

        public VehicleInstance()
        {
            Position = new XBinVector3();
            Direction = new XBinVector3();
            EntityName = "";
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Position.ReadFromFile(reader);
            Direction.ReadFromFile(reader);
            EntityName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            LoadFlags = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            Position.WriteToFile(writer);
            Direction.WriteToFile(writer);
            writer.PushStringPtr(EntityName);
            writer.Write(LoadFlags);
        }

        public static int GetSize()
        {
            return 32;
        }
    }

    public class Command_LoadVehicle : ICommand
    {
        private readonly uint Magic = 0xA1C05A78;

        public VehicleInstance[] Instances { get; set; }
        public ESlotType SlotType { get; set; }
        public string SDSName { get; set; }
        public string QuotaID { get; set; }
        public uint GUID { get; set; }
        public uint SlotID { get; set; }

        public Command_LoadVehicle()
        {
            Instances = new VehicleInstance[0];
            SDSName = "";
            QuotaID = "";
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint VehicleOffset = reader.ReadUInt32();
            uint NumInstances0 = reader.ReadUInt32();
            uint NumInstances1 = reader.ReadUInt32();
            ToolkitAssert.Ensure(NumInstances0 == NumInstances1, "Number of instances is incorrect!");

            SlotType = (ESlotType)reader.ReadUInt32();
            SDSName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            QuotaID = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            GUID = reader.ReadUInt32();
            SlotID = reader.ReadUInt32();

            Instances = new VehicleInstance[NumInstances0];
            for(int i = 0; i < Instances.Length; i++)
            {
                VehicleInstance Instance = new VehicleInstance();
                Instance.ReadFromFile(reader);
                Instances[i] = Instance;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.PushObjectPtr("Command_VehicleOffset");
            writer.Write(Instances.Length); // Two because its an array
            writer.Write(Instances.Length); // Two because its an array
            writer.Write((uint)SlotType);
            writer.PushStringPtr(SDSName);
            writer.PushStringPtr(QuotaID);
            writer.Write(GUID);
            writer.Write(SlotID);

            writer.FixUpObjectPtr("Command_VehicleOffset");
            foreach (var Instance in Instances)
            {
                Instance.WriteToFile(writer);
            }
        }

        public int GetSize()
        {
            int TotalSize = 32;
            TotalSize += (Instances.Length * VehicleInstance.GetSize());
            return TotalSize;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
