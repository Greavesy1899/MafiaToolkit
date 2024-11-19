using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Gibbed.IO;
using Toolkit.Mathematics;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Sound
{
    public class PlaneConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object result = null;
            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                float[] values = ConverterUtils.ConvertStringToFloats(stringValue, 4);
                result = new Plane(values);
            }

            return result ?? base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;
            Plane plane = (Plane)value;

            if (destinationType == typeof(string))
            {
                result = plane.ToString();
            }

            return result ?? base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverter(typeof(PlaneConverter)), PropertyClassAllowReflection]
    public class Plane
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Plane() { }

        public Plane(float[] Values)
        {
            X = Values[0];
            Y = Values[1];
            Z = Values[2];
            W = Values[3];
        }

        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            X = Stream.ReadSingle(bIsBigEndian);
            Y = Stream.ReadSingle(bIsBigEndian);
            Z = Stream.ReadSingle(bIsBigEndian);
            W = Stream.ReadSingle(bIsBigEndian);
        }

        public void WriteToFile(MemoryStream Stream, bool bIsBigEndian)
        {
            Stream.Write(X, bIsBigEndian);
            Stream.Write(Y, bIsBigEndian);
            Stream.Write(Z, bIsBigEndian);
            Stream.Write(W, bIsBigEndian);
        }

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
        }
    }

    [PropertyClassCheckInherited, PropertyClassAllowReflection]
    public class SoundSectorBase
    {
        public ushort[] Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public string Name { get; set; }
        public short Unk3 { get; set; }
        public ushort Unk4 { get; set; }
        public ushort Unk5 { get; set; }
        public bool bBasicSceneOnly { get; set; }

        public SoundSectorBase()
        {
            Unk0 = new ushort[0];
            Unk1 = 0;
            Unk2 = 0;
            Name = string.Empty;
        }

        public virtual void ReadSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            // Read array of ushorts, I suppsoe they reference big hash list in main file.
            ushort NumCount = Stream.ReadUInt16(bIsBigEndian);
            Unk0 = new ushort[NumCount];
            for(ushort i = 0; i < NumCount; i++)
            {
                Unk0[i] = Stream.ReadUInt16(bIsBigEndian);
            }

            // The game requires either of these to be valid, otherwise returns null
            Unk1 = Stream.ReadUInt32(bIsBigEndian);
            Unk2 = Stream.ReadUInt32(bIsBigEndian);
            Name = Stream.ReadString8(bIsBigEndian);

            // Between here the game checks if its below 5, if it is return out early

            // Continue
            Unk3 = Stream.ReadInt16(bIsBigEndian);
            if (Unk3 != 0xFF)
            {
                // TODO:
                int z = 0;
            }

            Unk4 = Stream.ReadUInt16(bIsBigEndian);
            Unk5 = Stream.ReadUInt16(bIsBigEndian);
            bBasicSceneOnly = Stream.ReadBoolean();
        }

        public virtual void WriteSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            // Write lookup list
            Stream.Write((ushort)Unk0.Length, bIsBigEndian);
            foreach(ushort Value in Unk0)
            {
                Stream.Write(Value, bIsBigEndian);
            }

            Stream.Write(Unk1, bIsBigEndian);
            Stream.Write(Unk2, bIsBigEndian);
            Stream.WriteString8(Name, bIsBigEndian);
            Stream.Write(Unk3, bIsBigEndian);
            Stream.Write(Unk4, bIsBigEndian);
            Stream.Write(Unk5, bIsBigEndian);
            Stream.WriteByte((byte)(bBasicSceneOnly ? 1 : 0));
        }
    }

    class SoundSectorPrimary : SoundSectorBase
    {
        public SoundSectorPrimary() : base() { }
    }

    class SoundSectorNormal : SoundSectorBase
    {
        public Plane[] Planes { get; set; }

        public SoundSectorNormal() : base()
        {
            Planes = new Plane[0];
        }

        public override void ReadSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            // Read floats, yes, before the base class. How odd.
            byte NumFloats = Stream.ReadByte8(); // Probably vector count
            Planes = new Plane[NumFloats];
            for (byte i = 0; i < Planes.Length; i++)
            {
                Plane NewPlane = new Plane();
                NewPlane.ReadFromFile(Stream, bIsBigEndian);
                Planes[i] = NewPlane;
            }

            // Read base class!
            base.ReadSDS(Stream, Endianess);
        }

        public override void WriteSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            // Write planes
            Stream.WriteByte((byte)Planes.Length);
            foreach(Plane CurPlane in Planes)
            {
                CurPlane.WriteToFile(Stream, bIsBigEndian);
            }

            // Write base class
            base.WriteSDS(Stream, Endianess);
        }
    }

    public class PortalSphere
    { 
        public string Name { get; set; }
        public Vec3 Position { get; set; }
        public float Unk0 { get; set; }
        public float OpenRatio { get; set; }
        public string LinkA { get; set; }
        public byte Unk2 { get; set; }
        public string LinkB { get; set; }
        public byte Unk3 { get; set; }
        public float CostFactor { get; set; }
        public string EntityName { get; set; }
        public byte Unk6 { get; set; }
        public byte bVolumeFactorEnabled { get; set; }
        public float VolumeFactor { get; set; }


        public PortalSphere()
        {
            Name = string.Empty;
            Position = new Vec3();
            Unk0 = 0.0f;
            OpenRatio = 0.0f;
            LinkA = string.Empty;
            Unk2 = 0;
            LinkB = string.Empty;
            Unk3 = 0;
            CostFactor = 0.0f;
            EntityName = string.Empty;
            Unk6 = 0;
            bVolumeFactorEnabled = 0;
            VolumeFactor = 0.0f;
        }

        public void ReadSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            Name = Stream.ReadString8(bIsBigEndian);
            Position.ReadFromFile(Stream, bIsBigEndian);
            Unk0 = Stream.ReadSingle(bIsBigEndian);
            OpenRatio = Stream.ReadSingle(bIsBigEndian);
            LinkA = Stream.ReadString8(bIsBigEndian);
            Unk2 = Stream.ReadByte8();
            LinkB = Stream.ReadString8(bIsBigEndian);
            Unk3 = Stream.ReadByte8();
            CostFactor = Stream.ReadSingle(bIsBigEndian);
            EntityName = Stream.ReadString8(bIsBigEndian);
            Unk6 = Stream.ReadByte8();
            bVolumeFactorEnabled = Stream.ReadByte8();

            if((bVolumeFactorEnabled & 1) != 0) // Guarded at 0x140287126
            {
                byte VolumeFactorAsByte = Stream.ReadByte8();
                VolumeFactor = (VolumeFactorAsByte / 255.0f);
            }
        }

        public void WriteSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            Stream.WriteString8(Name, bIsBigEndian);
            Position.WriteToFile(Stream, bIsBigEndian);
            Stream.Write(Unk0, bIsBigEndian);
            Stream.Write(OpenRatio, bIsBigEndian);
            Stream.WriteString8(LinkA, bIsBigEndian);
            Stream.WriteByte(Unk2);
            Stream.WriteString8(LinkB, bIsBigEndian);
            Stream.WriteByte(Unk3);
            Stream.Write(CostFactor, bIsBigEndian);
            Stream.WriteString8(EntityName, bIsBigEndian);
            Stream.WriteByte(Unk6);
            Stream.WriteByte(bVolumeFactorEnabled);

            if((bVolumeFactorEnabled & 1) != 0) // Guarded at 0x140287126
            {
                byte VolumeFactorAsByte = (byte)(VolumeFactor * 255);
                Stream.WriteByte(VolumeFactorAsByte);
            }
        }
    }

    public class SoundSectorResource
    {
        public string Name { get; set; }
        public ulong[] Hashes { get; set; }
        public SoundSectorBase[] Sectors { get; set; }
        public PortalSphere[] Portals { get; set; }

        public SoundSectorResource()
        {
            Name = string.Empty;
            Hashes = new ulong[0];
            Sectors = new SoundSectorBase[0];
            Portals = new PortalSphere[0];
        }

        public SoundSectorResource(FileInfo info)
        {
            byte[] FileBytes = File.ReadAllBytes(info.FullName);
            using(MemoryStream Stream = new MemoryStream(FileBytes))
            {
                ReadSDS(Stream, Endian.Little);

                XElement XMLFile = ReflectionHelpers.ConvertPropertyToXML(this);
                XMLFile.Save("Output.xml");

                using(MemoryStream WriteStream = new MemoryStream())
                {
                    WriteSDS(WriteStream, Endian.Little);
                    File.WriteAllBytes("Output.bin", WriteStream.ToArray());
                }
            }
        }

        public void ReadSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            Name = Stream.ReadString8(bIsBigEndian);

            // Read all hashes
            uint NumHashes = Stream.ReadUInt32(bIsBigEndian);
            Hashes = new ulong[NumHashes];
            for (uint i = 0; i < NumHashes; i++)
            {
                Hashes[i] = Stream.ReadUInt64(bIsBigEndian);
            }

            // Read Sectors
            uint NumSectors = Stream.ReadUInt32(bIsBigEndian);
            Sectors = new SoundSectorBase[NumSectors];
            for(uint i = 0; i < NumSectors; i++)
            {
                byte SectorType = Stream.ReadByte8();
                if(SectorType == 0)
                {
                    SoundSectorPrimary SectorPrimary = new SoundSectorPrimary();
                    SectorPrimary.ReadSDS(Stream, Endianess);
                    Sectors[i] = SectorPrimary;
                }
                else if(SectorType == 1)
                {
                    SoundSectorNormal SectorNormal = new SoundSectorNormal();
                    SectorNormal.ReadSDS(Stream, Endianess);
                    Sectors[i] = SectorNormal;
                }
            }

            // Read Portals
            uint NumPortals = Stream.ReadUInt32(bIsBigEndian);
            Portals = new PortalSphere[NumPortals];
            for (uint i = 0; i < NumPortals; i++)
            {
                byte PortalType = Stream.ReadByte8();
                if(PortalType == 0)
                {
                    PortalSphere Portal = new PortalSphere();
                    Portal.ReadSDS(Stream, Endianess);
                    Portals[i] = Portal;
                }
                else
                {
                    int z = 0;
                }
            }
        }

        public void WriteSDS(MemoryStream Stream, Endian Endianess)
        {
            bool bIsBigEndian = (Endianess == Endian.Big);

            Stream.WriteString8(Name, bIsBigEndian);

            // Write hashes
            Stream.Write(Hashes.Length, bIsBigEndian);
            foreach (ulong Hash in Hashes)
            {
                Stream.Write(Hash, bIsBigEndian);
            }

            // Write Sectors
            Stream.Write(Sectors.Length, bIsBigEndian);
            foreach(SoundSectorBase Sector in Sectors)
            {
                // TODO: Could probably clean this... but it does for now
                SoundSectorPrimary PrimarySector = (Sector as SoundSectorPrimary);
                if(PrimarySector != null)
                {
                    Stream.WriteByte(0);
                    PrimarySector.WriteSDS(Stream, Endianess);
                }

                SoundSectorNormal NormalSector = (Sector as SoundSectorNormal);
                if (NormalSector != null)
                {
                    Stream.WriteByte(1);
                    NormalSector.WriteSDS(Stream, Endianess);
                }
            }

            // Write portals
            Stream.Write(Portals.Length, bIsBigEndian);
            foreach (PortalSphere Portal in Portals)
            {
                Stream.WriteByte(0);
                Portal.WriteSDS(Stream, Endianess);
            }
        }

        public void WriteToFile(string FileName, bool bIsBigEndian)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                WriteSDS(outStream, (bIsBigEndian ? Endian.Big : Endian.Little));
                File.WriteAllBytes(FileName, outStream.ToArray());
            }
        }

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            SoundSectorResource FileContents = ReflectionHelpers.ConvertToPropertyFromXML<SoundSectorResource>(LoadedDoc);

            // Copy data taken from loaded XML
            Hashes = FileContents.Hashes;
            Sectors = FileContents.Sectors;
            Portals = FileContents.Portals;
            Name = FileContents.Name;
        }
    }
}
