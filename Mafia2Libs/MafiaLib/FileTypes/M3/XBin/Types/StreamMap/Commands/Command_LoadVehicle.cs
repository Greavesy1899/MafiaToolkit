using ResourceTypes.M3.XBin;
using SharpDX;
using System;
using System.Diagnostics;
using System.IO;
using Utils.SharpDXExtensions;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class VehicleInstance
    {
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }
        public string EntityName { get; set; }
        public uint LoadFlags { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Position = Vector3Extenders.ReadFromFile(reader);
            Direction = Vector3Extenders.ReadFromFile(reader);
            EntityName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
            LoadFlags = reader.ReadUInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            
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

        public void ReadFromFile(BinaryReader reader)
        {
            uint VehicleOffset = reader.ReadUInt32();
            uint NumInstances0 = reader.ReadUInt32();
            uint NumInstances1 = reader.ReadUInt32();
            Debug.Assert(NumInstances0 == NumInstances1, "Number of instances is incorrect!");

            SlotType = (ESlotType)reader.ReadUInt32();
            SDSName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
            QuotaID = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
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

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
