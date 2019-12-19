using SharpDX;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Actors
{
    public interface IActorExtraDataInterface
    {
        void ReadFromFile(MemoryStream stream, bool isBigEndian);
        void WriteToFile(MemoryStream writer, bool isBigEndian);
        int GetSize();
    }

    public class ActorPhysicsBase : IActorExtraDataInterface
    {
        public class HitData
        {
            public float SpeedMax { get; set; }
            public float SpeedVolMax { get; set; }
            public int MinVol { get; set; }
            public int MaxVol { get; set; }
            public float FrequencyLow { get; set; }
            public float FrequencyHigh { get; set; }
        }

        public int MoveOnInit { get; set; }
        public float ActivateImpulse { get; set; }
        public float DeactivateThreshold { get; set; }
        public float HitPoints { get; set; }
        public int MaterialID { get; set; }
        public float StaticFriction { get; set; }
        public float DynamicFriction { get; set; }
        public float Restitution { get; set; }
        public int POType { get; set; }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 POPos { get; set; }

        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 POSize { get; set; }
        public HitData[] HitInfo { get; set; }
        public int SlideID { get; set; }
        public float SlideSpeedMax { get; set; }
        public float SlideSpeedVolMax { get; set; }
        public int SlideMinVol { get; set; }
        public int SlideMaxVol { get; set; }
        public float SlideFrequencyLow { get; set; }
        public float SlideFrequencyHigh { get; set; }
        public int RollID { get; set; }
        public float RollSpeedMax { get; set; }
        public float RollSpeedVolMax { get; set; }
        public int RollMinVol { get; set; }
        public int RollMaxVol { get; set; }
        public float RollFrequencyLow { get; set; }
        public float RollFrequencyHigh { get; set; }
        public int SndChngVersionID { get; set; }
        public int SndBreakID { get; set; }
        public int SndDelayID { get; set; }
        public int SndDelayDelay { get; set; }
        public int ParticleHitID { get; set; }
        public int ParticleBreakID { get; set; }
        public int ParticleChngVersionID { get; set; }
        public int ParticleSlideID { get; set; }
        public float ParticleHitSpeedMin { get; set; }
        public int GarbageID { get; set; }

        public virtual int GetSize()
        {
            return 240;
        }

        public virtual void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            MoveOnInit = reader.ReadInt32(isBigEndian);
            ActivateImpulse = reader.ReadSingle(isBigEndian);
            HitPoints = reader.ReadSingle(isBigEndian);
            DeactivateThreshold = reader.ReadSingle(isBigEndian);
            StaticFriction = reader.ReadInt32(isBigEndian);
            DynamicFriction = reader.ReadSingle(isBigEndian);
            Restitution = reader.ReadSingle(isBigEndian);
            reader.Seek(212, SeekOrigin.Begin);
            POType = reader.ReadInt32(isBigEndian);
            POPos = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            POSize = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            reader.Seek(44, SeekOrigin.Begin);
            HitInfo = new HitData[3];

            for (int i = 0; i < 3; i++) HitInfo[i] = new HitData();
            for (int i = 0; i < 3; i++) HitInfo[i].SpeedMax = reader.ReadSingle(isBigEndian);
            for (int i = 0; i < 3; i++) HitInfo[i].SpeedVolMax = reader.ReadSingle(isBigEndian);
            for (int i = 0; i < 3; i++) HitInfo[i].MinVol = reader.ReadInt32(isBigEndian);
            for (int i = 0; i < 3; i++) HitInfo[i].MaxVol = reader.ReadInt32(isBigEndian);
            for (int i = 0; i < 3; i++) HitInfo[i].FrequencyLow = reader.ReadSingle(isBigEndian);
            for (int i = 0; i < 3; i++) HitInfo[i].FrequencyHigh = reader.ReadSingle(isBigEndian);

            SlideID = reader.ReadInt32(isBigEndian);
            SlideSpeedMax = reader.ReadSingle(isBigEndian);
            SlideSpeedVolMax = reader.ReadSingle(isBigEndian);
            SlideMinVol = reader.ReadInt32(isBigEndian);
            SlideMaxVol = reader.ReadInt32(isBigEndian);
            SlideFrequencyHigh = reader.ReadSingle(isBigEndian);
            SlideFrequencyLow = reader.ReadSingle(isBigEndian);
            RollID = reader.ReadInt32(isBigEndian);
            RollSpeedMax = reader.ReadSingle(isBigEndian);
            RollSpeedVolMax = reader.ReadSingle(isBigEndian);
            RollMinVol = reader.ReadInt32(isBigEndian);
            RollMaxVol = reader.ReadInt32(isBigEndian);
            RollFrequencyLow = reader.ReadSingle(isBigEndian);
            RollFrequencyHigh = reader.ReadSingle(isBigEndian);
            SndChngVersionID = reader.ReadInt32(isBigEndian);
            SndBreakID = reader.ReadInt32(isBigEndian);
            SndDelayID = reader.ReadInt32(isBigEndian);
            SndDelayDelay = reader.ReadInt32(isBigEndian);
            ParticleHitID = reader.ReadInt32(isBigEndian);
            ParticleBreakID = reader.ReadInt32(isBigEndian);
            ParticleChngVersionID = reader.ReadInt32(isBigEndian);
            ParticleSlideID = reader.ReadInt32(isBigEndian);
            ParticleHitSpeedMin = reader.ReadSingle(isBigEndian);
            GarbageID = reader.ReadInt32(isBigEndian);
        }

        public virtual void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            //writer.Write(new byte[this.GetSize()]);
            writer.Write(MoveOnInit, isBigEndian);
            writer.Write(ActivateImpulse, isBigEndian);
            writer.Write(HitPoints, isBigEndian);
            writer.Write(DeactivateThreshold, isBigEndian);
            writer.Write(StaticFriction, isBigEndian);
            writer.Write(DynamicFriction, isBigEndian);
            writer.Write(Restitution, isBigEndian);
            writer.Seek(212, SeekOrigin.Begin);
            writer.Write(POType, isBigEndian);
            Vector3Extenders.WriteToFile(POPos, writer, isBigEndian);
            Vector3Extenders.WriteToFile(POSize, writer, isBigEndian);
            writer.Seek(44, SeekOrigin.Begin);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].SpeedMax, isBigEndian);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].SpeedVolMax, isBigEndian);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].MinVol, isBigEndian);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].MaxVol, isBigEndian);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].FrequencyLow, isBigEndian);
            for (int i = 0; i < 3; i++) writer.Write(HitInfo[i].FrequencyHigh, isBigEndian);
            writer.Write(SlideID, isBigEndian);
            writer.Write(SlideSpeedMax, isBigEndian);
            writer.Write(SlideSpeedVolMax, isBigEndian);
            writer.Write(SlideMinVol, isBigEndian);
            writer.Write(SlideMaxVol, isBigEndian);
            writer.Write(SlideFrequencyHigh, isBigEndian);
            writer.Write(SlideFrequencyLow, isBigEndian);
            writer.Write(RollID, isBigEndian);
            writer.Write(RollSpeedMax, isBigEndian);
            writer.Write(RollSpeedVolMax, isBigEndian);
            writer.Write(RollMinVol, isBigEndian);
            writer.Write(RollMaxVol, isBigEndian);
            writer.Write(RollFrequencyLow, isBigEndian);
            writer.Write(RollFrequencyHigh, isBigEndian);
            writer.Write(SndChngVersionID, isBigEndian);
            writer.Write(SndBreakID, isBigEndian);
            writer.Write(SndDelayID, isBigEndian);
            writer.Write(SndDelayDelay, isBigEndian);
            writer.Write(ParticleHitID, isBigEndian);
            writer.Write(ParticleBreakID, isBigEndian);
            writer.Write(ParticleChngVersionID, isBigEndian);
            writer.Write(ParticleSlideID, isBigEndian);
            writer.Write(ParticleHitSpeedMin, isBigEndian);
            writer.Write(GarbageID, isBigEndian);
        }
    }

    public class ActorCleanEntity : IActorExtraDataInterface
    {
        public float Radius { get; set; }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BBoxSize { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorCleanEntityFlags flags { get; set; }

        public int GetSize()
        {
            return 20;
        }

        public ActorCleanEntity(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }
        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Radius = stream.ReadSingle(isBigEndian);
            BBoxSize = Vector3Extenders.ReadFromFile(stream, isBigEndian);
            flags = (ActorCleanEntityFlags)stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(Radius, isBigEndian);
            Vector3Extenders.WriteToFile(BBoxSize, writer, isBigEndian);
            writer.Write((int)flags, isBigEndian);
        }
    }

    public class ActorRadio : ActorPhysicsBase
    {
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorRadioFlags Flags { get; set; }
        public float Range { get; set; }
        public float NearRange { get; set; }
        public float Volume { get; set; }
        public int CurveID { get; set; }
        public string Program { get; set; }
        public string Playlist { get; set; }
        public string Station { get; set; }

        public ActorRadio(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }
        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            reader.Seek(240, SeekOrigin.Begin);
            Flags = (ActorRadioFlags)reader.ReadInt32(isBigEndian);
            Range = reader.ReadSingle(isBigEndian);
            NearRange = reader.ReadSingle(isBigEndian);
            Volume = reader.ReadSingle(isBigEndian);
            Volume /= 100.0f;
            CurveID = reader.ReadInt32(isBigEndian);
            Program = reader.ReadStringBuffer(256);
            Playlist = reader.ReadStringBuffer(256);
            Station = reader.ReadStringBuffer(256);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);
            writer.Seek(240, SeekOrigin.Begin);
            writer.Write((int)Flags, isBigEndian);
            writer.Write(Range, isBigEndian);
            writer.Write(NearRange, isBigEndian);
            Volume *= 100.0f;
            writer.Write(Volume, isBigEndian);
            writer.Write(CurveID, isBigEndian);
            writer.WriteStringBuffer(256, Program, '\0');
            writer.WriteStringBuffer(256, Playlist, '\0');
            writer.WriteStringBuffer(256, Station, '\0');
        }

        public override int GetSize()
        {
            return 1028;
        }
    }

    public class ActorTrafficCar : IActorExtraDataInterface
    {
        int type;
        BoundingBox bbox;
        float unk0;
        float unk1;
        float unk2;
        float unk3;
        int maxElements;
        int pie;
        string tableName;
        string areaName;
        float carUnk4;
        float carUnk5;
        int carUnk6;
        int spawnedParking;
        int parking;
        string crewGenerator;
        int dirtyMin;
        int dirtyMax;
        int damageMin;
        int damageMax;
        int zero;


        public int Type { get { return type; } set { type = value; } }
        public BoundingBox BoundingBox { get { return bbox; } set { bbox = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public float Unk1 { get { return unk1; } set { unk1 = value; } }
        public float Unk2 { get { return unk2; } set { unk2 = value; } }
        public float Unk3 { get { return unk3; } set { unk3 = value; } }
        public int MaxElements { get { return maxElements; } set { maxElements = value; } }
        public int Pie { get { return pie; } set { pie = value; } }
        public string TableName { get { return tableName; } set { tableName = value; } }
        public string AreaName { get { return areaName; } set { areaName = value; } }
        public float CarUnk4 { get { return carUnk4; } set { carUnk4 = value; } }
        public float CarUnk5 { get { return carUnk5; } set { carUnk5 = value; } }
        public int CarUnk6 { get { return carUnk6; } set { carUnk6 = value; } }
        public int SpawnedParking { get { return spawnedParking; } set { spawnedParking = value; } }
        public int Parking { get { return parking; } set { parking = value; } }
        public string CrewGenerator { get { return crewGenerator; } set { crewGenerator = value; } }
        public int DirtyMin { get { return dirtyMin; } set { dirtyMin = value; } }
        public int DirtyMax { get { return dirtyMax; } set { dirtyMax = value; } }
        public int DamageMin { get { return damageMin; } set { damageMin = value; } }
        public int DamageMax { get { return damageMax; } set { damageMax = value; } }
        public int Zero { get { return zero; } set { zero = value; } }
        public ActorTrafficCar(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bbox = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
            unk0 = reader.ReadSingle(isBigEndian);
            unk1 = reader.ReadSingle(isBigEndian);
            unk2 = reader.ReadSingle(isBigEndian);
            unk3 = reader.ReadSingle(isBigEndian);
            maxElements = reader.ReadInt32(isBigEndian);
            pie = reader.ReadInt32(isBigEndian);
            tableName = reader.ReadStringBuffer(32).TrimEnd('\0');
            areaName = reader.ReadStringBuffer(64).TrimEnd('\0');
            carUnk4 = reader.ReadSingle(isBigEndian);
            carUnk5 = reader.ReadSingle(isBigEndian);
            carUnk6 = reader.ReadInt32(isBigEndian);
            spawnedParking = reader.ReadInt32(isBigEndian);
            parking = reader.ReadInt32(isBigEndian);
            crewGenerator = reader.ReadStringBuffer(32).TrimEnd('\0');
            dirtyMin = reader.ReadInt32(isBigEndian);
            dirtyMax = reader.ReadInt32(isBigEndian);
            damageMin = reader.ReadInt32(isBigEndian);
            damageMax = reader.ReadInt32(isBigEndian);
            zero = reader.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(type, isBigEndian);
            BoundingBoxExtenders.WriteToFile(bbox, writer, isBigEndian);
            writer.Write(unk0, isBigEndian);
            writer.Write(unk1, isBigEndian);
            writer.Write(unk2, isBigEndian);
            writer.Write(unk3, isBigEndian);
            writer.Write(maxElements, isBigEndian);
            writer.Write(pie, isBigEndian);
            writer.WriteStringBuffer(32, tableName, '\0');
            writer.WriteStringBuffer(64, areaName, '\0');
            writer.Write(carUnk4, isBigEndian);
            writer.Write(carUnk5, isBigEndian);
            writer.Write(carUnk6, isBigEndian);
            writer.Write(spawnedParking, isBigEndian);
            writer.Write(parking, isBigEndian);
            writer.WriteStringBuffer(32, crewGenerator, '\0');
            writer.Write(dirtyMin, isBigEndian);
            writer.Write(dirtyMax, isBigEndian);
            writer.Write(damageMin, isBigEndian);
            writer.Write(damageMax, isBigEndian);
            writer.Write(zero, isBigEndian);
        }

        public int GetSize()
        {
            return 220;
        }
    }

    public class ActorTrafficHuman : IActorExtraDataInterface
    {
        int type;
        BoundingBox bbox;
        float unk0;
        float unk1;
        float unk2;
        float unk3;
        int maxElements;
        int pie;
        string tableName;
        string areaName;
        float zDistance;
        float agregationRange;
        int agregationCount;

        public int Type { get { return type; } set { type = value; } }
        public BoundingBox BoundingBox { get { return bbox; } set { bbox = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public float Unk1 { get { return unk1; } set { unk1 = value; } }
        public float Unk2 { get { return unk2; } set { unk2 = value; } }
        public float Unk3 { get { return unk3; } set { unk3 = value; } }
        public int MaxElements { get { return maxElements; } set { maxElements = value; } }
        public int Pie { get { return pie; } set { pie = value; } }
        public string TableName { get { return tableName; } set { tableName = value; } }
        public string AreaName { get { return areaName; } set { areaName = value; } }
        public float ZDistance { get { return zDistance; } set { zDistance = value; } }
        public float AgregationRange { get { return agregationRange; } set { agregationRange = value; } }
        public int AgregationCount { get { return agregationCount; } set { agregationCount = value; } }

        public ActorTrafficHuman(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bbox = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
            unk0 = reader.ReadSingle(isBigEndian);
            unk1 = reader.ReadSingle(isBigEndian);
            unk2 = reader.ReadSingle(isBigEndian);
            unk3 = reader.ReadSingle(isBigEndian);
            maxElements = reader.ReadInt32(isBigEndian);
            pie = reader.ReadInt32(isBigEndian);
            tableName = reader.ReadStringBuffer(32).TrimEnd('\0');
            areaName = reader.ReadStringBuffer(64).TrimEnd('\0');
            zDistance = reader.ReadSingle(isBigEndian);
            agregationRange = reader.ReadSingle(isBigEndian);
            agregationCount = reader.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(type, isBigEndian);
            BoundingBoxExtenders.WriteToFile(bbox, writer, isBigEndian);
            writer.Write(unk0, isBigEndian);
            writer.Write(unk1, isBigEndian);
            writer.Write(unk2, isBigEndian);
            writer.Write(unk3, isBigEndian);
            writer.Write(maxElements, isBigEndian);
            writer.Write(pie, isBigEndian);
            writer.WriteStringBuffer(32, tableName, '\0');
            writer.WriteStringBuffer(64, areaName, '\0');
            writer.Write(zDistance, isBigEndian);
            writer.Write(agregationRange, isBigEndian);
            writer.Write(agregationCount, isBigEndian);
        }

        public int GetSize()
        {
            return 160;
        }
    }

    public class ActorTrafficTrain : IActorExtraDataInterface
    {
        int type;
        BoundingBox bbox;
        float unk0;
        float unk1;
        float unk2;
        float unk3;
        int maxElements;
        int pie;
        string tableName;
        string areaName;
        string crewGenerator;

        public int Type { get { return type; } set { type = value; } }
        public BoundingBox BoundingBox { get { return bbox; } set { bbox = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public float Unk1 { get { return unk1; } set { unk1 = value; } }
        public float Unk2 { get { return unk2; } set { unk2 = value; } }
        public float Unk3 { get { return unk3; } set { unk3 = value; } }
        public int MaxElements { get { return maxElements; } set { maxElements = value; } }
        public int Pie { get { return pie; } set { pie = value; } }
        public string TableName { get { return tableName; } set { tableName = value; } }
        public string AreaName { get { return areaName; } set { areaName = value; } }
        public string CrewGenerator { get { return crewGenerator; } set { crewGenerator = value; } }

        public ActorTrafficTrain(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bbox = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
            unk0 = reader.ReadSingle(isBigEndian);
            unk1 = reader.ReadSingle(isBigEndian);
            unk2 = reader.ReadSingle(isBigEndian);
            unk3 = reader.ReadSingle(isBigEndian);
            maxElements = reader.ReadInt32(isBigEndian);
            pie = reader.ReadInt32(isBigEndian);
            tableName = reader.ReadStringBuffer(32).TrimEnd('\0');
            areaName = reader.ReadStringBuffer(64).TrimEnd('\0');
            crewGenerator = reader.ReadStringBuffer(32).TrimEnd('\0');
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(type, isBigEndian);
            BoundingBoxExtenders.WriteToFile(bbox, writer, isBigEndian);
            writer.Write(unk0, isBigEndian);
            writer.Write(unk1, isBigEndian);
            writer.Write(unk2, isBigEndian);
            writer.Write(unk3, isBigEndian);
            writer.Write(maxElements, isBigEndian);
            writer.Write(pie, isBigEndian);
            writer.WriteStringBuffer(32, tableName, '\0');
            writer.WriteStringBuffer(64, areaName, '\0');
            writer.WriteStringBuffer(32, crewGenerator, '\0');
        }

        public int GetSize()
        {
            return 180;
        }
    }

    public class ActorWardrobe : IActorExtraDataInterface
    {
        byte[] cameraPos;
        string doorName;
        string soundName;
        string humanAnimationName;
        int textID;
        float unk0;
        int testPrimitive;
        byte[] posData;

        public byte[] CameraPos { get { return cameraPos;  } set { cameraPos = value; } }
        public string DoorName { get { return doorName; } set { doorName = value; } }
        public string SoundName { get { return soundName; } set { soundName = value; } }
        public string HumanAnimationName { get { return humanAnimationName; } set { humanAnimationName = value; } }
        public int TextID { get { return textID; } set { textID = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public int TestPrimitive { get { return testPrimitive; } set { testPrimitive = value; } }
        public byte[] PosData { get { return posData; } set { posData = value; } }

        public ActorWardrobe(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            cameraPos = reader.ReadBytes(72);
            doorName = reader.ReadStringBuffer(32).TrimEnd('\0');
            soundName = reader.ReadStringBuffer(32).TrimEnd('\0');
            humanAnimationName = reader.ReadStringBuffer(32).TrimEnd('\0');
            textID = reader.ReadInt32(isBigEndian);
            unk0 = reader.ReadSingle(isBigEndian);
            testPrimitive = reader.ReadInt32(isBigEndian);
            posData = reader.ReadBytes(28);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(cameraPos);
            writer.WriteStringBuffer(32, doorName, '\0');
            writer.WriteStringBuffer(32, soundName, '\0');
            writer.WriteStringBuffer(32, humanAnimationName, '\0');
            writer.Write(textID, isBigEndian);
            writer.Write(unk0, isBigEndian);
            writer.Write(testPrimitive, isBigEndian);
            writer.Write(posData);
        }

        public int GetSize()
        {
            return 208;
        }
    }

    public class ActorLight : IActorExtraDataInterface
    {
        int size;
        byte[] padding;
        int unk01;
        byte unk02;
        Matrix uMatrix0;
        int unk07;
        int unk08;
        int unk09;
        byte count;
        int unk10;
        Hash[] sceneLinks;
        Hash[] frameLinks;
        int[] frameIdxLinks;
        int flags;
        float[] unkFloat1 = new float[7];
        int unk_int;
        float[] unkFloat2 = new float[5];
        byte unk_byte1;
        float[] unkFloat3 = new float[17];
        byte unk_byte2;
        float[] unkFloat4 = new float[5];
        Hash nameLight;
        int unk_int2;
        float[] unkFloat5 = new float[20];
        Hash[] names = new Hash[4];
        BoundingBox boundingBox;
        byte unk_byte3;
        Matrix uMatrix1;
        int instanced;
        int type;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public byte Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Matrix UnkMatrix0 {
            get { return uMatrix0; }
            set { uMatrix0 = value; }
        }
        public int Unk07 {
            get { return unk07; }
            set { unk07 = value; }
        }
        public int Unk08 {
            get { return unk08; }
            set { unk08 = value; }
        }
        public int Unk09 {
            get { return unk09; }
            set { unk09 = value; }
        }
        public byte Count {
            get { return count; }
            set { count = value; }
        }
        public int Unk10 {
            get { return unk10; }
            set { unk10 = value; }
        }
        public Hash[] SceneLinks {
            get { return sceneLinks; }
            set { sceneLinks = value; }
        }
        public Hash[] FrameLinks {
            get { return frameLinks; }
            set { frameLinks = value; }
        }
        public int[] FrameIDXLinks {
            get { return frameIdxLinks; }
            set { frameIdxLinks = value; }
        }
        public float[] UnkFloats1 {
            get { return unkFloat1; }
            set { unkFloat1 = value; }
        }
        public int UnkInt1 {
            get { return unk_int; }
            set { unk_int = value; }
        }
        public float[] UnkFloats2 {
            get { return unkFloat2; }
            set { unkFloat2 = value; }
        }
        public byte UnkByte1 {
            get { return unk_byte1; }
            set { unk_byte1 = value; }
        }
        public float[] UnkFloats3 {
            get { return unkFloat3; }
            set { unkFloat3 = value; }
        }
        public byte UnkByte2 {
            get { return unk_byte2; }
            set { unk_byte2 = value; }
        }
        public float[] UnkFloats4 {
            get { return unkFloat4; }
            set { unkFloat4 = value; }
        }
        public Hash NameLight {
            get { return nameLight; }
            set { nameLight = value; }
        }
        public int UnkInt2 {
            get { return unk_int2; }
            set { unk_int2 = value; }
        }
        public float[] UnkFloats5 {
            get { return unkFloat5; }
            set { unkFloat5 = value; }
        }
        public Hash[] UnkHashes {
            get { return names; }
            set { names = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMinimum {
            get { return boundingBox.Minimum; }
            set { boundingBox.Minimum = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMaximum {
            get { return boundingBox.Maximum; }
            set { boundingBox.Maximum = value; }
        }
        public byte UnkByte3 {
            get { return unk_byte3; }
            set { unk_byte3 = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Matrix UnkMatrix1 {
            get { return uMatrix1; }
            set { uMatrix1 = value; }
        }
        public int Instanced {
            get { return instanced; }
            set { instanced = value; }
        }
        public int Type {
            get { return type; }
            set { type = value; }
        }
        public ActorLight(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            size = stream.ReadInt32(isBigEndian);
            if (size < 2305)
            {
                padding = stream.ReadBytes(9);
                unk01 = stream.ReadInt32(isBigEndian);
                unk02 = stream.ReadByte8();
                uMatrix0 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
                unk07 = stream.ReadInt32(isBigEndian);
                unk08 = stream.ReadInt32(isBigEndian);
                unk09 = stream.ReadInt32(isBigEndian);
                count = stream.ReadByte8();
                unk10 = stream.ReadInt32(isBigEndian);

                frameLinks = new Hash[count];
                sceneLinks = new Hash[count];
                frameIdxLinks = new int[count];
                for (int i = 0; i < count; i++)
                {
                    sceneLinks[i] = new Hash(stream, isBigEndian);
                    frameLinks[i] = new Hash(stream, isBigEndian);
                    frameIdxLinks[i] = stream.ReadInt32(isBigEndian);
                }

                //flags = reader.ReadInt32();

                for (int i = 0; i < 7; i++)
                    unkFloat1[i] = stream.ReadSingle(isBigEndian);

                unk_int = stream.ReadInt32(isBigEndian);

                for (int i = 0; i < 5; i++)
                    unkFloat2[i] = stream.ReadSingle(isBigEndian);

                unk_byte1 = stream.ReadByte8();

                for (int i = 0; i < 17; i++)
                    unkFloat3[i] = stream.ReadSingle(isBigEndian);

                unk_byte2 = stream.ReadByte8();

                for (int i = 0; i < 5; i++)
                    unkFloat4[i] = stream.ReadSingle(isBigEndian);

                nameLight = new Hash(stream, isBigEndian);

                unk_int2 = stream.ReadInt32(isBigEndian);

                for (int i = 0; i < 20; i++)
                    unkFloat5[i] = stream.ReadSingle(isBigEndian);

                for (int i = 0; i < 4; i++)
                    names[i] = new Hash(stream, isBigEndian);

                boundingBox = BoundingBoxExtenders.ReadFromFile(stream, isBigEndian);
                unk_byte3 = stream.ReadByte8();
                uMatrix1 = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            }
            stream.Seek(2308, SeekOrigin.Begin);
            instanced = stream.ReadInt32(isBigEndian);
            type = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(new byte[2316]);
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write(size, isBigEndian);
            writer.Write(this.padding);
            writer.Write(unk01, isBigEndian);
            writer.WriteByte(unk02);
            uMatrix0.WriteToFile(writer, isBigEndian);
            writer.Write(unk07, isBigEndian);
            writer.Write(unk08, isBigEndian);
            writer.Write(unk09, isBigEndian);
            writer.WriteByte(count);
            writer.Write(unk10, isBigEndian);

            for (int i = 0; i < count; i++)
            {
                sceneLinks[i].WriteToFile(writer, isBigEndian);
                frameLinks[i].WriteToFile(writer, isBigEndian);
                writer.Write(frameIdxLinks[i], isBigEndian);
            }

            for (int i = 0; i < 7; i++)
                writer.Write(unkFloat1[i], isBigEndian);

            writer.Write(unk_int, isBigEndian);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat2[i], isBigEndian);

            writer.WriteByte(unk_byte1);

            for (int i = 0; i < 17; i++)
                writer.Write(unkFloat3[i], isBigEndian);

            writer.WriteByte(unk_byte2);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat4[i], isBigEndian);

            nameLight.WriteToFile(writer, isBigEndian);

            writer.Write(unk_int2, isBigEndian);

            for (int i = 0; i != 20; i++)
                writer.Write(unkFloat5[i], isBigEndian);

            for (int i = 0; i != 4; i++)
                names[i].WriteToFile(writer, isBigEndian);

            boundingBox.WriteToFile(writer, isBigEndian);
            writer.WriteByte(unk_byte3);
            uMatrix1.WriteToFile(writer, isBigEndian);
            writer.Seek(2308, SeekOrigin.Begin);
            writer.Write(instanced, isBigEndian);
            writer.Write(type, isBigEndian);
        }

        public int GetSize()
        {
            return 2316;
        }
    }

    public class ActorDoor : ActorPhysicsBase
    {
        byte disabled;
        byte closesPortals;
        byte physicalOpen;
        byte physicalClose;
        float closedMagnitude;
        byte automaticOpen;
        byte automaticClose;
        short automaticCloseDelay; //MAYBE??
        int unk0;
        float unk1;
        int locked;
        int lockedSoundID;
        int lockTime;
        int unlockTime;
        int lockSoundID;
        int unlockSoundID;
        int lockpickSoundID;
        int lockpickClicksCount;
        int openingSoundID;
        int closingSoundID;
        int closingSound2ID;
        int closingSound3ID;
        int closingSoundMagnitude;
        int closingSound2Magnitude;
        int closingSound3Magnitude;
        int movingSoundID;
        int kickingSoundID;
        int kickable;
        int closedSoundID;
        int openHint;
        int closeHint;
        int lockpickHint;
        int kickHint;
        int actorActionsEnabled;
        int pushAwayMode;
        int pushAwayReaction;
        public byte Disabled { get { return disabled; } set { disabled = value; } }
        public byte ClosesPortals { get { return closesPortals; } set { closesPortals = value; } }
        public byte PhysicalOpen { get { return physicalOpen; } set { physicalOpen = value; } }
        public byte PhysicalClose { get { return physicalClose; } set { physicalClose = value; } }
        public float ClosedMagnitude { get { return closedMagnitude; } set { closedMagnitude = value; } }
        public byte AutomaticOpen { get { return automaticOpen; } set { automaticOpen = value; } }
        public byte AutomaticClose { get { return automaticClose; } set { automaticClose = value; } }
        public short AutomaticCloseDelay { get { return automaticCloseDelay; } set { automaticCloseDelay = value; } }
        public int Unk0 { get { return unk0; } set { unk0 = value; } }
        public float Unk1 { get { return unk1; } set { unk1 = value; } }
        public int Locked { get { return locked; } set { locked = value; } }
        public int LockedSoundID { get { return lockedSoundID; } set { lockedSoundID = value; } }
        public int LockTime { get { return lockTime; } set { lockTime = value; } }
        public int UnlockTime { get { return unlockTime; } set { unlockTime = value; } }
        public int LockSoundID { get { return lockSoundID; } set { lockSoundID = value; } }
        public int UnlockSoundID { get { return unlockSoundID; } set { unlockSoundID = value; } }
        public int LockpickSoundID { get { return lockpickSoundID; } set { lockpickSoundID = value; } }
        public int LockpickClicksCount { get { return lockpickClicksCount; } set { lockpickClicksCount = value; } }
        public int OpeningSoundID { get { return openingSoundID; } set { openingSoundID = value; } }
        public int ClosingSoundID { get { return closingSoundID; } set { closingSoundID = value; } }
        public int ClosingSound2ID { get { return closingSound2ID; } set { closingSound2ID = value; } }
        public int ClosingSound3ID { get { return closingSound3ID; } set { closingSound3ID = value; } }
        public int ClosingSoundMagnitude { get { return closingSoundMagnitude; } set { closingSoundMagnitude = value; } }
        public int ClosingSound2Magnitude { get { return closingSound2Magnitude; } set { closingSound2Magnitude = value; } }
        public int ClosingSound3Magnitude { get { return closingSound3Magnitude; } set { closingSound3Magnitude = value; } }
        public int MovingSoundID { get { return movingSoundID; } set { movingSoundID = value; } }
        public int KickingSoundID { get { return kickingSoundID; } set { kickingSoundID = value; } }
        public int Kickable { get { return kickable; } set { kickable = value; } }
        public int ClosedSoundID { get { return closedSoundID; } set { closedSoundID = value; } }
        public int OpenHint { get { return openHint; } set { openHint = value; } }
        public int CloseHint { get { return closeHint; } set { closeHint = value; } }
        public int LockpickHint { get { return lockpickHint; } set { lockpickHint = value; } }
        public int KickHint { get { return kickHint; } set { kickHint = value; } }
        public int ActorActionsEnabled { get { return actorActionsEnabled; } set { actorActionsEnabled = value; } }
        public int PushAwayMode { get { return pushAwayMode; } set { pushAwayMode = value; } }
        public int PushAwayReaction { get { return pushAwayReaction; } set { pushAwayReaction = value; } }

        public ActorDoor(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }
        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            reader.Seek(240, SeekOrigin.Begin);
            disabled = reader.ReadByte8();
            closesPortals = reader.ReadByte8();
            physicalOpen = reader.ReadByte8();
            physicalClose = reader.ReadByte8();
            closedMagnitude = reader.ReadSingle(isBigEndian);
            automaticOpen = reader.ReadByte8();
            automaticClose = reader.ReadByte8();
            automaticCloseDelay = reader.ReadInt16(isBigEndian);
            unk0 = reader.ReadInt32(isBigEndian);
            unk1 = reader.ReadSingle(isBigEndian);
            locked = reader.ReadInt32(isBigEndian);
            lockedSoundID = reader.ReadInt32(isBigEndian);
            lockTime = reader.ReadInt32(isBigEndian);
            unlockTime = reader.ReadInt32(isBigEndian);
            lockSoundID = reader.ReadInt32(isBigEndian);
            unlockSoundID = reader.ReadInt32(isBigEndian);
            lockpickSoundID = reader.ReadInt32(isBigEndian);
            lockpickClicksCount = reader.ReadInt32(isBigEndian);
            openingSoundID = reader.ReadInt32(isBigEndian);
            closingSoundID = reader.ReadInt32(isBigEndian);
            closingSound2ID = reader.ReadInt32(isBigEndian);
            closingSound3ID = reader.ReadInt32(isBigEndian);
            closingSoundMagnitude = reader.ReadInt32(isBigEndian);
            closingSound2Magnitude = reader.ReadInt32(isBigEndian);
            closingSound3Magnitude = reader.ReadInt32(isBigEndian);
            movingSoundID = reader.ReadInt32(isBigEndian);
            kickingSoundID = reader.ReadInt32(isBigEndian);
            kickable = reader.ReadInt32(isBigEndian);
            closedSoundID = reader.ReadInt32(isBigEndian);
            openHint = reader.ReadInt32(isBigEndian);
            closeHint = reader.ReadInt32(isBigEndian);
            lockpickHint = reader.ReadInt32(isBigEndian);
            kickHint = reader.ReadInt32(isBigEndian);
            actorActionsEnabled = reader.ReadInt32(isBigEndian);
            pushAwayMode = reader.ReadInt32(isBigEndian);
            pushAwayReaction = reader.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);
            writer.Seek(240, SeekOrigin.Begin);
            writer.WriteByte(disabled);
            writer.WriteByte(closesPortals);
            writer.WriteByte(physicalOpen);
            writer.WriteByte(physicalClose);
            writer.Write(closedMagnitude, isBigEndian);
            writer.WriteByte(automaticOpen);
            writer.WriteByte(automaticClose);
            writer.Write(automaticCloseDelay, isBigEndian);
            writer.Write(unk0, isBigEndian);
            writer.Write(unk1, isBigEndian);
            writer.Write(locked, isBigEndian);
            writer.Write(lockedSoundID, isBigEndian);
            writer.Write(lockTime, isBigEndian);
            writer.Write(unlockTime, isBigEndian);
            writer.Write(lockSoundID, isBigEndian);
            writer.Write(unlockSoundID, isBigEndian);
            writer.Write(lockpickSoundID, isBigEndian);
            writer.Write(lockpickClicksCount, isBigEndian);
            writer.Write(openingSoundID, isBigEndian);
            writer.Write(closingSoundID, isBigEndian);
            writer.Write(closingSound2ID, isBigEndian);
            writer.Write(closingSound3ID, isBigEndian);
            writer.Write(closingSoundMagnitude, isBigEndian);
            writer.Write(closingSound2Magnitude, isBigEndian);
            writer.Write(closingSound3Magnitude, isBigEndian);
            writer.Write(movingSoundID, isBigEndian);
            writer.Write(kickingSoundID, isBigEndian);
            writer.Write(kickable, isBigEndian);
            writer.Write(closedSoundID, isBigEndian);
            writer.Write(openHint, isBigEndian);
            writer.Write(closeHint, isBigEndian);
            writer.Write(lockpickHint, isBigEndian);
            writer.Write(kickHint, isBigEndian);
            writer.Write(actorActionsEnabled, isBigEndian);
            writer.Write(pushAwayMode, isBigEndian);
            writer.Write(pushAwayReaction, isBigEndian);
        }

        public override int GetSize()
        {
            return 364;
        }
    }

    public class ActorSoundEntity : IActorExtraDataInterface
    {
        ActorSoundEntityBehaviourFlags behFlags;
        int audioType;
        int behaviourType;
        float volume;
        float pitch;
        string file;
        float randomPauseMin;
        float randomPauseMax;
        float randomGroupPauseMin;
        float randomGroupPauseMax;
        int randomGroupSoundsMin;
        int randomGroupSoundsMax;
        float randomVolumeMin;
        float randomVolumeMax;
        float randomPitchMin;
        float randomPitchMax;
        float randomPosRangeX;
        float randomPosRangeY;
        float randomPosRangeZ;
        ActorSoundEntityPlayType playFlags;
        string[] randomWaves;

        float near;
        float far;
        //WHEREVER YOU AREEEE!!
        int monoDistance;
        int curveID;
        float innerAngle;
        float outerAngle;
        float outerVolume;

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorSoundEntityBehaviourFlags BehaviourFlags {
            get { return behFlags; }
            set { behFlags = value; }
        }
        public int AudioType {
            get { return audioType; }
            set { audioType = value; }
        }
        public int BehaviourType {
            get { return behaviourType; }
            set { behaviourType = value; }
        }
        public float Volume {
            get { return volume; }
            set { volume = value; }
        }
        public float Pitch {
            get { return pitch; }
            set { pitch = value; }
        }
        public string File {
            get { return file; }
            set { file = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomGroupPauseMax {
            get { return randomGroupPauseMax; }
            set { randomGroupPauseMax = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomGroupPauseMin {
            get { return randomGroupPauseMin; }
            set { randomGroupPauseMin = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPauseMin {
            get { return randomPauseMin; }
            set { randomPauseMin = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPauseMax {
            get { return randomPauseMax; }
            set { randomPauseMax = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public int RandomGroupSoundsMax {
            get { return randomGroupSoundsMax; }
            set { randomGroupSoundsMax = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public int RandomGroupSoundsMin {
            get { return randomGroupSoundsMin; }
            set { randomGroupSoundsMin = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomVolumeMin {
            get { return randomVolumeMin; }
            set { randomVolumeMin = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomVolumeMax {
            get { return randomVolumeMax; }
            set { randomVolumeMax = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPitchMin {
            get { return randomPitchMin; }
            set { randomPitchMin = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPitchMax {
            get { return randomPitchMax; }
            set { randomPitchMax = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPosRangeX {
            get { return randomPosRangeX; }
            set { randomPosRangeX = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPosRangeY {
            get { return randomPosRangeY; }
            set { randomPosRangeY = value; }
        }
        [Category("BehaviourType 20"), Description("These are only saved if \"BehaviourType\" is set to 20.")]
        public float RandomPosRangeZ {
            get { return randomPosRangeZ; }
            set { randomPosRangeZ = value; }
        }

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorSoundEntityPlayType PlayFlags {
            get { return playFlags; }
            set { playFlags = value; }
        }
        public string[] RandomWaves {
            get { return randomWaves; }
            set { randomWaves = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 15 or above.")]
        public float Near {
            get { return near; }
            set { near = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 15 or above.")]
        public float Far {
            get { return far; }
            set { far = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 15 or above.")]
        public int MonoDistance {
            get { return monoDistance; }
            set { monoDistance = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 20 or above.")]
        public int CurveID {
            get { return curveID; }
            set { curveID = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 30 or above.")]
        public float InnerAngle {
            get { return innerAngle; }
            set { innerAngle = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is 30 or above.")]
        public float OuterAngle {
            get { return outerAngle; }
            set { outerAngle = value; }
        }
        [Category("AudioType"), Description("These are only saved if \"AudioType\" is  30 or above.")]
        public float OuterVolume {
            get { return outerVolume; }
            set { outerVolume = value; }
        }

        public ActorSoundEntity(MemoryStream reader, bool isBigEndian)
        {
            randomWaves = new string[5];
            ReadFromFile(reader, isBigEndian);
        }

        public int GetSize()
        {
            return 592;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            behFlags = (ActorSoundEntityBehaviourFlags)reader.ReadInt32(isBigEndian);
            audioType = reader.ReadInt32(isBigEndian);
            behaviourType = reader.ReadInt32(isBigEndian);
            volume = reader.ReadSingle(isBigEndian);
            pitch = reader.ReadSingle(isBigEndian);
            file = reader.ReadStringBuffer(80);
            if(behaviourType == 20)
            {
                int seek = 0x21C;
                reader.Seek(seek, SeekOrigin.Begin);
                randomPauseMin = reader.ReadSingle(isBigEndian);
                randomPauseMax = reader.ReadSingle(isBigEndian);
                randomGroupPauseMin = reader.ReadSingle(isBigEndian);
                randomGroupPauseMax = reader.ReadSingle(isBigEndian);
                randomGroupSoundsMin = reader.ReadInt32(isBigEndian);
                randomGroupSoundsMax = reader.ReadInt32(isBigEndian);
                randomVolumeMin = reader.ReadSingle(isBigEndian);
                randomVolumeMax = reader.ReadSingle(isBigEndian);
                randomPitchMin = reader.ReadSingle(isBigEndian);
                randomPitchMax = reader.ReadSingle(isBigEndian);
                randomPosRangeX = reader.ReadSingle(isBigEndian);
                randomPosRangeY = reader.ReadSingle(isBigEndian);
                randomPosRangeZ = reader.ReadSingle(isBigEndian);

                seek = 0x84;
                reader.Seek(seek, SeekOrigin.Begin);
                playFlags = (ActorSoundEntityPlayType)reader.ReadByte();
                randomWaves = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    randomWaves[i] = reader.ReadStringBuffer(80).TrimEnd('\0');
                    reader.ReadByte();
                }
            }
            reader.Seek(100, SeekOrigin.Begin);
            reader.ReadInt32(isBigEndian);
            switch (audioType)
            {
                case 20:
                    near = reader.ReadSingle(isBigEndian);
                    far = reader.ReadSingle(isBigEndian);
                    monoDistance = reader.ReadInt32(isBigEndian);
                    curveID = reader.ReadInt32(isBigEndian);
                    break;
                case 15:
                    near = reader.ReadSingle(isBigEndian);
                    far = reader.ReadSingle(isBigEndian);
                    curveID = reader.ReadInt32(isBigEndian);
                    break;
                case 30:
                    near = reader.ReadSingle(isBigEndian);
                    far = reader.ReadSingle(isBigEndian);
                    curveID = reader.ReadInt32(isBigEndian);
                    reader.Seek(120, SeekOrigin.Begin);
                    innerAngle = reader.ReadSingle(isBigEndian);
                    outerAngle = reader.ReadSingle(isBigEndian);
                    outerVolume = reader.ReadSingle(isBigEndian);
                    break;
                case 10:
                    break;
                default:
                    break;
            }
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(new byte[592]);
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write((int)behFlags, isBigEndian);
            writer.Write(audioType, isBigEndian);
            writer.Write(behaviourType, isBigEndian);
            writer.Write(volume, isBigEndian);
            writer.Write(pitch, isBigEndian);
            writer.WriteStringBuffer(80, file, '\0');
            if (behaviourType == 20)
            {
                writer.Seek(0x21C, SeekOrigin.Begin);
                writer.Write(randomPauseMin, isBigEndian);
                writer.Write(randomPauseMax, isBigEndian);
                writer.Write(randomGroupPauseMin, isBigEndian);
                writer.Write(randomGroupPauseMax, isBigEndian);
                writer.Write(randomGroupSoundsMin, isBigEndian);
                writer.Write(randomGroupSoundsMax, isBigEndian);
                writer.Write(randomVolumeMin, isBigEndian);
                writer.Write(randomVolumeMax, isBigEndian);
                writer.Write(randomPitchMin, isBigEndian);
                writer.Write(randomPitchMax, isBigEndian);
                writer.Write(randomPosRangeX, isBigEndian);
                writer.Write(randomPosRangeY, isBigEndian);
                writer.Write(randomPosRangeZ, isBigEndian);
                writer.Seek(0x84, SeekOrigin.Begin);
                writer.WriteByte((byte)playFlags);
                for (int i = 0; i < 5; i++)
                {
                    string wave = string.IsNullOrEmpty(randomWaves[i]) == true ? new string(new char[] { '\0' }) : randomWaves[i];
                    writer.WriteStringBuffer(80, wave, '\0');
                    writer.WriteByte(0);
                }
            }
            writer.Seek(100, SeekOrigin.Begin);
            writer.Write(0, isBigEndian);
            switch (audioType)
            {
                case 20:
                    writer.Write(near, isBigEndian);
                    writer.Write(far, isBigEndian);
                    writer.Write(monoDistance, isBigEndian);
                    writer.Write(curveID, isBigEndian);
                    break;
                case 15:
                    writer.Write(near, isBigEndian);
                    writer.Write(far, isBigEndian);
                    writer.Write(curveID, isBigEndian);
                    break;
                case 30:
                    writer.Write(near, isBigEndian);
                    writer.Write(far, isBigEndian);
                    writer.Write(curveID, isBigEndian);
                    writer.Seek(120, SeekOrigin.Begin);
                    writer.Write(innerAngle, isBigEndian);
                    writer.Write(outerAngle, isBigEndian);
                    writer.Write(outerVolume, isBigEndian);
                    break;
                case 10:
                    break;
                default:
                    break;
            }
        }
    }

    public class ActorSpikeStrip : IActorExtraDataInterface
    {
        float length;

        public float Length {
            get { return length; }
            set { length = value; }
        }

        public ActorSpikeStrip(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            length = reader.ReadSingle(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(length, isBigEndian);
        }

        public int GetSize()
        {
            return 4;
        }
    }


    public class ActorAircraft : IActorExtraDataInterface
    {
        int soundMotorID;

        public int SoundMotorID {
            get { return soundMotorID; }
            set { soundMotorID = value; }
        }

        public ActorAircraft(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            soundMotorID = reader.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(soundMotorID, isBigEndian);
        }

        public int GetSize()
        {
            return 4;
        }
    }

    public class ActorItem : IActorExtraDataInterface
    {
        public interface IItem { }
        public class ItemScript : IItem
        {
            string scriptEvent;
            int textID;
            int sentTestAction;

            public string ScriptEvent {
                get { return scriptEvent; }
                set { scriptEvent = value; }
            }
            public int TextID {
                get { return textID; }
                set { textID = value; }
            }
            public int SentTestAction {
                get { return sentTestAction; }
                set { sentTestAction = value; }
            }
        }
        public class Type0
        {
            int tableID;
            int textID;
            int ammo;
            int ammoAUX;

            public int TableID {
                get { return tableID; }
                set { tableID = value; }
            }
            public int TextID {
                get { return textID; }
                set { textID = value; }
            }
            public int Ammo {
                get { return ammo; }
                set { ammo = value; }
            }
            public int AmmoAUX {
                get { return ammoAUX; }
                set { ammoAUX = value; }
            }
        }

        ActorItemFlags flags;
        int type;
        float respawnTime;
        int testPrimitive;
        float range;
        ItemScript scriptEvent;
        Type0 type0Data;
        Vector3 unk1;
        Vector3 unk2;

        public ActorItemFlags Flags {
            get { return flags; }
            set { flags = value; }
        }
        public int Type {
            get { return type; }
            set { type = value; }
        }
        public float RespawnTime {
            get { return respawnTime; }
            set { respawnTime = value; }
        }
        public int TestPrimitive {
            get { return testPrimitive; }
            set { testPrimitive = value; }
        }
        public float Range {
            get { return range; }
            set { range = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ItemScript ScriptEvent {
            get { return scriptEvent; }
            set { scriptEvent = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Type0 Type0Data {
            get { return type0Data; }
            set { type0Data = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Unk1 {
            get { return unk1; }
            set { unk1 = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Unk2 {
            get { return unk2; }
            set { unk2 = value; }
        }

        public ActorItem(MemoryStream reader, bool isBigEndian)
        {
            ReadFromFile(reader, isBigEndian);
        }

        public int GetSize()
        {
            return 152;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            reader.ReadInt16(isBigEndian);
            flags = (ActorItemFlags)reader.ReadUInt16(isBigEndian);
            type = reader.ReadInt32(isBigEndian);
            respawnTime = reader.ReadSingle(isBigEndian);

            switch(type)
            {
                case 0:
                    type0Data = new Type0();
                    type0Data.TableID = reader.ReadInt32(isBigEndian);
                    type0Data.TextID = reader.ReadInt32(isBigEndian);
                    type0Data.Ammo = reader.ReadInt32(isBigEndian);
                    type0Data.AmmoAUX = reader.ReadInt32(isBigEndian);
                    reader.Seek(92, SeekOrigin.Current);
                    break;
                case 2:
                    scriptEvent = new ItemScript();
                    scriptEvent.TextID = reader.ReadInt32(isBigEndian);
                    scriptEvent.SentTestAction = reader.ReadInt32(isBigEndian);
                    reader.Seek(36, SeekOrigin.Current);
                    scriptEvent.ScriptEvent = reader.ReadStringBuffer(64).TrimEnd('\0');
                    break;
                case 3:
                    throw new Exception();
                    break;
                case 7:
                    throw new Exception();
                    break;
                case 8:
                    throw new Exception();
                    break;
                case 9:
                    reader.Seek(108, SeekOrigin.Current);
                    break;
                default:
                    throw new Exception();
                    break;
            }

            testPrimitive = reader.ReadInt32(isBigEndian);
            range = reader.ReadSingle(isBigEndian);
            unk1 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
            unk2 = Vector3Extenders.ReadFromFile(reader, isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write((ushort)0, isBigEndian);
            writer.Write((ushort)flags, isBigEndian);
            writer.Write(type, isBigEndian);
            writer.Write(respawnTime, isBigEndian);

            switch (type)
            {
                case 0:
                    writer.Write(type0Data.TableID, isBigEndian);
                    writer.Write(type0Data.TextID, isBigEndian);
                    writer.Write(type0Data.Ammo, isBigEndian);
                    writer.Write(type0Data.AmmoAUX, isBigEndian);
                    writer.Write(new byte[92]);
                    break;
                case 2:
                    writer.Write(scriptEvent.TextID, isBigEndian);
                    writer.Write(scriptEvent.SentTestAction, isBigEndian);
                    writer.Write(new byte[36]);
                    writer.WriteStringBuffer(64, scriptEvent.ScriptEvent);
                    break;
                case 3:
                    throw new Exception();
                    break;
                case 7:
                    throw new Exception();
                    break;
                case 8:
                    throw new Exception();
                    break;
                case 9:
                    writer.Write(new byte[108]);
                    break;
                default:
                    throw new Exception();
                    break;
            }

            writer.Write(testPrimitive, isBigEndian);
            writer.Write(range, isBigEndian);
            Vector3Extenders.WriteToFile(unk1, writer, isBigEndian);
            Vector3Extenders.WriteToFile(unk2, writer, isBigEndian);
        }
    }

    public class ActorPinup : IActorExtraDataInterface
    {
        int pinupNum;

        public int PinupNum {
            get { return pinupNum; }
            set { pinupNum = value; }
        }

        public ActorPinup(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            pinupNum = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(pinupNum, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.Pinup, pinupNum);
        }

        public int GetSize()
        {
            return 4;
        }
    }

    public class ActorScriptEntity : IActorExtraDataInterface
    {
        string scriptName;
        int unk01;

        public string ScriptName {
            get { return scriptName; }
            set { scriptName = value; }
        }
        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        public ActorScriptEntity(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            scriptName = stream.ReadStringBuffer(96).TrimEnd('\0');
            unk01 = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.WriteStringBuffer(96, scriptName, '\0');
            stream.Write(unk01, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.ScriptEntity, scriptName);
        }

        public int GetSize()
        {
            return 100;
        }
    }
}
