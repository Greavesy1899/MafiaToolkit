using BitStreams;
using System;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class C_GUID
    {
        [PropertyForceAsAttribute]
        public uint Part0 { get; private set; }
        [PropertyForceAsAttribute]
        public uint Part1 { get; private set; }
        public ulong Hash { get { return GenerateHash();  } set { SplitHash(value); } }

        public void Load(BitStream MemStream)
        {
            Part0 = MemStream.ReadUInt32();
            Part1 = MemStream.ReadUInt32();

            GenerateHash();
        }

        public void Save(BitStream MemStream)
        {
            SplitHash(Hash);

            MemStream.WriteUInt32(Part0);
            MemStream.WriteUInt32(Part1);
        }

        private ulong GenerateHash()
        {
            // Get as bytes
            byte[] Part0Bytes = BitConverter.GetBytes(Part0);
            byte[] Part1Bytes = BitConverter.GetBytes(Part1);

            // Copy bytes into array
            byte[] HashBytes = new byte[8];
            Array.Copy(Part0Bytes, 0, HashBytes, 0, 4);
            Array.Copy(Part1Bytes, 0, HashBytes, 4, 4);

            // Convert to ulong hash
            return BitConverter.ToUInt64(HashBytes);
        }

        private void SplitHash(ulong Value)
        {
            byte[] GuidBytes = BitConverter.GetBytes(Value);
            byte[] LeftHand = new byte[4];
            Array.Copy(GuidBytes, LeftHand, 4);

            byte[] RightHand = new byte[4];
            Array.Copy(GuidBytes, 4, RightHand, 0, 4);

            Part0 = BitConverter.ToUInt32(LeftHand, 0);
            Part1 = BitConverter.ToUInt32(RightHand, 0);
        }
    }
}
