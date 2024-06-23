﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Logging;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Mathematics;

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
        public int StaticFriction { get; set; }
        public float DynamicFriction { get; set; }
        public float Restitution { get; set; }
        public float Unk1C { get; set; }
        public uint Unk20 { get; set; }
        public uint Unk24 { get; set; }
        public uint Unk28 { get; set; }
        public PathObjectTypes POType { get; set; }
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

        public ActorPhysicsBase()
        {
            HitInfo = new HitData[3];
            for (int i = 0; i < HitInfo.Length; i++)
            {
                HitInfo[i] = new HitData();
            }
        }

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
            Unk1C = reader.ReadSingle(isBigEndian);
            Unk20 = reader.ReadUInt32(isBigEndian);
            Unk24 = reader.ReadUInt32(isBigEndian);
            Unk28 = reader.ReadUInt32(isBigEndian);
            reader.Seek(212, SeekOrigin.Begin);
            POType = (PathObjectTypes)reader.ReadInt32(isBigEndian);
            POPos = Vector3Utils.ReadFromFile(reader, isBigEndian);
            POSize = Vector3Utils.ReadFromFile(reader, isBigEndian);
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
            GarbageID = reader.ReadInt32(isBigEndian);
            ParticleHitSpeedMin = reader.ReadSingle(isBigEndian);
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
            writer.Write(Unk1C, isBigEndian);
            writer.Write(Unk20, isBigEndian);
            writer.Write(Unk24, isBigEndian);
            writer.Write(Unk28, isBigEndian);
            writer.Seek(212, SeekOrigin.Begin);
            writer.Write((int)POType, isBigEndian);
            Vector3Utils.WriteToFile(POPos, writer, isBigEndian);
            Vector3Utils.WriteToFile(POSize, writer, isBigEndian);
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

    public class ActorActorDetector : IActorExtraDataInterface
    {
        private int testPrimitive;
        private float range;
        private float sizeX;
        private float sizeY;
        private float sizeZ;

        public int TestPrimitive {
            get { return testPrimitive; }
            set { testPrimitive = value; }
        }
        public float Range {
            get { return range; }
            set { range = value; }
        }
        public float SizeX {
            get { return sizeX; }
            set { sizeX = value; }
        }
        public float SizeY {
            get { return sizeY; }
            set { sizeY = value; }
        }
        public float SizeZ {
            get { return sizeZ; }
            set { sizeZ = value; }
        }
        public int GetSize()
        {
            return 20;
        }

        public ActorActorDetector()
        {
            testPrimitive = 0;
            range = 0.0f;
            sizeX = 0.0f;
            sizeY = 0.0f;
            sizeZ = 0.0f;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            testPrimitive = stream.ReadInt32(isBigEndian);
            range = stream.ReadSingle(isBigEndian);
            sizeX = stream.ReadSingle(isBigEndian);
            sizeY = stream.ReadSingle(isBigEndian);
            sizeZ = stream.ReadSingle(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(testPrimitive, isBigEndian);
            writer.Write(range, isBigEndian);
            writer.Write(sizeX, isBigEndian);
            writer.Write(sizeY, isBigEndian);
            writer.Write(sizeZ, isBigEndian);
        }
    }

    public class ActorCleanEntity : IActorExtraDataInterface
    {
        public float Radius { get; set; }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BBoxSize { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorCleanEntityFlags Flags { get; set; }

        public ActorCleanEntity()
        {
            Radius = 2.0f;
            BBoxSize = new Vector3(1.0f, 1.0f, 1.0f);
            Flags = ActorCleanEntityFlags.ClearOnInit | ActorCleanEntityFlags.BlockTraffic | ActorCleanEntityFlags.BlockPedestrians | ActorCleanEntityFlags.UseBoxObstacle;
        }

        public int GetSize()
        {
            return 20;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Radius = stream.ReadSingle(isBigEndian);
            BBoxSize = Vector3Utils.ReadFromFile(stream, isBigEndian);
            Flags = (ActorCleanEntityFlags)stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(Radius, isBigEndian);
            Vector3Utils.WriteToFile(BBoxSize, writer, isBigEndian);
            writer.Write((int)Flags, isBigEndian);
        }
    }

    public class ActorRadio : ActorPhysicsBase
    {
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorRadioFlags Flags { get; set; }
        public float Range { get; set; }
        public float NearRange { get; set; }
        [Description("The volume of the Radio. It is multiplied by 100 in game.")]
        public float Volume { get; set; }
        public int CurveID { get; set; }
        public string Program { get; set; }
        public string Playlist { get; set; }
        public string Station { get; set; }

        public ActorRadio() : base()
        {
            Program = string.Empty;
            Playlist = string.Empty;
            Station = string.Empty;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            reader.Seek(240, SeekOrigin.Begin);
            Flags = (ActorRadioFlags)reader.ReadInt32(isBigEndian);
            Range = reader.ReadSingle(isBigEndian);
            NearRange = reader.ReadSingle(isBigEndian);
            Volume = reader.ReadSingle(isBigEndian);
            //Volume /= 100.0f;
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
            //Volume *= 100.0f;
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
        Vector3 bboxMin;
        Vector3 bboxMax;
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
        public Vector3 BoundingBoxMinimum { get { return bboxMin; } set { bboxMin = value; } }
        public Vector3 BoundingBoxMaximum { get { return bboxMax; } set { bboxMax = value; } }
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

        public ActorTrafficCar()
        {
            tableName = string.Empty;
            areaName = string.Empty;
            crewGenerator = string.Empty;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bboxMin = Vector3Utils.ReadFromFile(reader, isBigEndian);
            bboxMax = Vector3Utils.ReadFromFile(reader, isBigEndian);
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
            Vector3Utils.WriteToFile(bboxMin, writer, isBigEndian);
            Vector3Utils.WriteToFile(bboxMax, writer, isBigEndian);
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
        Vector3 bboxMin;
        Vector3 bboxMax;
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
        public Vector3 BoundingBoxMinimum { get { return bboxMin; } set { bboxMin = value; } }
        public Vector3 BoundingBoxMaximum { get { return bboxMax; } set { bboxMax = value; } }
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

        public ActorTrafficHuman()
        {
            tableName = string.Empty;
            areaName = string.Empty;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bboxMin = Vector3Utils.ReadFromFile(reader, isBigEndian);
            bboxMax = Vector3Utils.ReadFromFile(reader, isBigEndian);
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
            Vector3Utils.WriteToFile(bboxMin, writer, isBigEndian);
            Vector3Utils.WriteToFile(bboxMax, writer, isBigEndian);
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
        Vector3 bboxMin;
        Vector3 bboxMax;
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
        public Vector3 BoundingBoxMinimum { get { return bboxMin; } set { bboxMin = value; } }
        public Vector3 BoundingBoxMaximum { get { return bboxMax; } set { bboxMax = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public float Unk1 { get { return unk1; } set { unk1 = value; } }
        public float Unk2 { get { return unk2; } set { unk2 = value; } }
        public float Unk3 { get { return unk3; } set { unk3 = value; } }
        public int MaxElements { get { return maxElements; } set { maxElements = value; } }
        public int Pie { get { return pie; } set { pie = value; } }
        public string TableName { get { return tableName; } set { tableName = value; } }
        public string AreaName { get { return areaName; } set { areaName = value; } }
        public string CrewGenerator { get { return crewGenerator; } set { crewGenerator = value; } }

        public ActorTrafficTrain()
        {
            tableName = string.Empty;
            areaName = string.Empty;
            crewGenerator = string.Empty;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            type = reader.ReadInt32(isBigEndian);
            bboxMin = Vector3Utils.ReadFromFile(reader, isBigEndian);
            bboxMax = Vector3Utils.ReadFromFile(reader, isBigEndian);
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
            Vector3Utils.WriteToFile(bboxMin, writer, isBigEndian);
            Vector3Utils.WriteToFile(bboxMax, writer, isBigEndian);
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

        public byte[] CameraPos { get { return cameraPos; } set { cameraPos = value; } }
        public string DoorName { get { return doorName; } set { doorName = value; } }
        public string SoundName { get { return soundName; } set { soundName = value; } }
        public string HumanAnimationName { get { return humanAnimationName; } set { humanAnimationName = value; } }
        public int TextID { get { return textID; } set { textID = value; } }
        public float Unk0 { get { return unk0; } set { unk0 = value; } }
        public int TestPrimitive { get { return testPrimitive; } set { testPrimitive = value; } }
        public byte[] PosData { get { return posData; } set { posData = value; } }

        public ActorWardrobe()
        {
            PosData = new byte[0];
            CameraPos = new byte[0];
            DoorName = string.Empty;
            SoundName = string.Empty;
            HumanAnimationName = string.Empty;
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

    public class
    ActorLight : IActorExtraDataInterface
    {
        byte[] padding;
        int unk01;
        byte unk02;
        Matrix4x4 uMatrix0;
        int unk07;
        int unk08;
        int unk09;
        int unk10;
        HashName[] sceneLinks;
        HashName[] frameLinks;
        int[] frameIdxLinks;
        int flags;
        float[] unkFloat1 = new float[7];
        int unk_int;
        float[] unkFloat2 = new float[5];
        byte unk_byte1;
        float[] unkFloat3 = new float[17];
        byte unk_byte2;
        float[] unkFloat4 = new float[5];
        HashName nameLight;
        int unk_int2;
        float[] unkFloat5 = new float[20];
        HashName[] names = new HashName[4];
        BoundingBox boundingBox;
        byte unk_byte3;
        Matrix4x4 uMatrix1;
        int instanced;
        int type;

        //DO NOT STORE IN ACTOR!
        private Quaternion uMatrix0Quat;
        private Quaternion uMatrix1Quat;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public byte Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Matrix4x4 UnkMatrix0 {
            get { return uMatrix0; }
        }
        public Vector3 UnkMatrix0Translation {
            get { return uMatrix0.Translation; }
            set { uMatrix0.Translation = value; }
        }
        public Quaternion UnkMatrix0Quaternion {
            get { return uMatrix0Quat; }
            set {
                uMatrix0Quat = value;
                var translation = uMatrix0.Translation;
                uMatrix0 = Matrix4x4.CreateFromQuaternion(uMatrix0Quat);
                uMatrix0.Translation = translation;
            }
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
        public int Unk10 {
            get { return unk10; }
            set { unk10 = value; }
        }
        public HashName[] SceneLinks {
            get { return sceneLinks; }
            set { sceneLinks = value; }
        }
        public HashName[] FrameLinks {
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
        public HashName NameLight {
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
        public HashName[] UnkHashes {
            get { return names; }
            set { names = value; }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMinimum {
            get { return boundingBox.Min; }
            set { boundingBox.SetMinimum(value); }
        }
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 BoundaryBoxMaximum {
            get { return boundingBox.Max; }
            set { boundingBox.SetMaximum(value); }
        }
        public byte UnkByte3 {
            get { return unk_byte3; }
            set { unk_byte3 = value; }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Matrix4x4 UnkMatrix1 {
            get { return uMatrix1; }
        }
        public Vector3 UnkMatrix1Translation {
            get { return uMatrix1.Translation; }
            set { uMatrix1.Translation = value; }
        }
        public Quaternion UnkMatrix1Quaternion {
            get { return uMatrix1Quat; }
            set {
                uMatrix1Quat = value;
                var translation = uMatrix1.Translation;
                uMatrix1 = Matrix4x4.CreateFromQuaternion(uMatrix1Quat);
                uMatrix1.Translation = translation;
            }
        }
        public int Instanced {
            get { return instanced; }
            set { instanced = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public ActorLight()
        {
            padding = new byte[10];
            uMatrix0 = Matrix4x4.Identity;
            uMatrix1 = Matrix4x4.Identity;
            frameLinks = new HashName[0];
            sceneLinks = new HashName[0];
            frameIdxLinks = new int[0];
            unkFloat1 = new float[7];
            unkFloat2 = new float[5];
            unkFloat3 = new float[17];
            unkFloat4 = new float[5];
            unkFloat5 = new float[20];
            names = new HashName[4];
            for (int i = 0; i < 4; i++)
            {
                names[i] = new HashName();
            }

            nameLight = new HashName();
            uMatrix0Quat = Quaternion.CreateFromRotationMatrix(uMatrix0);
            uMatrix1Quat = Quaternion.CreateFromRotationMatrix(uMatrix1);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            int size = stream.ReadInt32(isBigEndian);
            if (size < 2305)
            {
                long pos = stream.Position;
                padding = stream.ReadBytes(10);
                unk01 = stream.ReadInt32(isBigEndian);
                uMatrix0 = MatrixUtils.ReadFromFile(stream, isBigEndian);
                unk07 = stream.ReadInt32(isBigEndian);
                unk08 = stream.ReadInt32(isBigEndian);
                unk09 = stream.ReadInt32(isBigEndian);
                uint count = stream.ReadByte8();
                unk10 = stream.ReadInt32(isBigEndian);

                frameLinks = new HashName[count];
                sceneLinks = new HashName[count];
                frameIdxLinks = new int[count];
                for (int i = 0; i < count; i++)
                {
                    sceneLinks[i] = new HashName(stream, isBigEndian);
                    frameLinks[i] = new HashName(stream, isBigEndian);
                    frameIdxLinks[i] = stream.ReadInt32(isBigEndian);
                }

                for (int i = 0; i < 7; i++)
                {
                    unkFloat1[i] = stream.ReadSingle(isBigEndian);
                }

                unk_int = stream.ReadInt32(isBigEndian);

                for (int i = 0; i < 5; i++)
                {
                    unkFloat2[i] = stream.ReadSingle(isBigEndian);
                }

                unk_byte1 = stream.ReadByte8();

                for (int i = 0; i < 17; i++)
                {
                    unkFloat3[i] = stream.ReadSingle(isBigEndian);
                }

                unk_byte2 = stream.ReadByte8();

                for (int i = 0; i < 5; i++)
                {
                    unkFloat4[i] = stream.ReadSingle(isBigEndian);
                }

                nameLight = new HashName(stream, isBigEndian);

                unk_int2 = stream.ReadInt32(isBigEndian);

                for (int i = 0; i < 20; i++)
                {
                    unkFloat5[i] = stream.ReadSingle(isBigEndian);
                }

                for (int i = 0; i < 4; i++)
                {
                    names[i] = new HashName(stream, isBigEndian);
                }

                boundingBox = BoundingBoxExtenders.ReadFromFile(stream, isBigEndian);
                unk_byte3 = stream.ReadByte8();
                uMatrix1 = MatrixUtils.ReadFromFile(stream, isBigEndian);
            }
            stream.Seek(2308, SeekOrigin.Begin);
            instanced = stream.ReadInt32(isBigEndian);
            type = stream.ReadInt32(isBigEndian);

            //PREP 
            uMatrix0Quat = Quaternion.CreateFromRotationMatrix(uMatrix0);
            uMatrix1Quat = Quaternion.CreateFromRotationMatrix(uMatrix1);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            // Sanity check whether the user has got it right
            int Length = sceneLinks.Length;
            ToolkitAssert.Ensure(frameLinks.Length == Length && frameIdxLinks.Length == Length, "SceneLinks, FrameLinks and FrameIDXLinks should be all equal. Fix this problem and resave");

            // Stubbed light, go back to beginning
            writer.Write(new byte[2316]);
            writer.Seek(0, SeekOrigin.Begin);

            // we will come back and fill this in
            writer.Write(-1, isBigEndian);
            long StartPos = writer.Position;

            // begin writing light data
            writer.Write(padding);
            writer.Write(unk01, isBigEndian);
            //writer.WriteByte(unk02);
            uMatrix0.WriteToFile(writer, isBigEndian);
            writer.Write(unk07, isBigEndian);
            writer.Write(unk08, isBigEndian);
            writer.Write(unk09, isBigEndian);
            writer.WriteByte((byte)sceneLinks.Length);
            writer.Write(unk10, isBigEndian);

            for (int i = 0; i < sceneLinks.Length; i++)
            {
                sceneLinks[i].WriteToFile(writer, isBigEndian);
                frameLinks[i].WriteToFile(writer, isBigEndian);
                writer.Write(frameIdxLinks[i], isBigEndian);
            }

            for (int i = 0; i < 7; i++)
            {
                writer.Write(unkFloat1[i], isBigEndian);
            }

            writer.Write(unk_int, isBigEndian);

            for (int i = 0; i < 5; i++)
            {
                writer.Write(unkFloat2[i], isBigEndian);
            }

            writer.WriteByte(unk_byte1);

            for (int i = 0; i < 17; i++)
            {
                writer.Write(unkFloat3[i], isBigEndian);
            }

            writer.WriteByte(unk_byte2);

            for (int i = 0; i < 5; i++)
            {
                writer.Write(unkFloat4[i], isBigEndian);
            }

            nameLight.WriteToFile(writer, isBigEndian);

            writer.Write(unk_int2, isBigEndian);

            for (int i = 0; i != 20; i++)
            {
                writer.Write(unkFloat5[i], isBigEndian);
            }

            for (int i = 0; i != 4; i++)
            {
                names[i].WriteToFile(writer, isBigEndian);
            }

            boundingBox.WriteToFile(writer, isBigEndian);
            writer.WriteByte(unk_byte3);
            uMatrix1.WriteToFile(writer, isBigEndian);

            // fix size
            long Size = writer.Position - StartPos;
            writer.Seek(0, SeekOrigin.Begin);
            writer.Write((int)Size, isBigEndian);

            // move to the end of the stream and fill in Instanced & Type
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

        public ActorDoor() : base()
        {
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

        public ActorSoundEntity()
        {
            randomWaves = new string[5];
            file = string.Empty;
        }

        public ActorSoundEntity(IActorExtraDataInterface extraData)
        {
            ActorSoundEntity other = (extraData as ActorSoundEntity);
            behFlags = other.behFlags;
            audioType = other.audioType;
            behaviourType = other.behaviourType;
            volume = other.volume;
            pitch = other.pitch;
            file = other.file;
            randomPauseMin = other.randomPauseMin;
            randomPauseMax = other.randomPauseMax;
            randomGroupPauseMin = other.randomGroupPauseMin;
            randomGroupPauseMax = other.randomGroupPauseMax;
            randomGroupSoundsMin = other.randomGroupSoundsMin;
            randomGroupSoundsMax = other.randomGroupSoundsMax;
            randomVolumeMin = other.randomVolumeMin;
            randomVolumeMax = other.randomVolumeMax;
            randomPitchMin = other.randomPitchMin;
            randomPitchMax = other.randomPitchMax;
            randomPosRangeX = other.randomPosRangeX;
            randomPosRangeY = other.randomPosRangeY;
            randomPosRangeZ = other.randomPosRangeZ;
            playFlags = other.playFlags;
            randomWaves = other.randomWaves;
            near = other.near;
            far = other.far;
            monoDistance = other.monoDistance;
            curveID = other.curveID;
            innerAngle = other.innerAngle;
            outerAngle = other.outerAngle;
            outerVolume = other.outerVolume;
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
            if (behaviourType == 20)
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

    public class ActorFrameWrapper : IActorExtraDataInterface
    {
        int unk0;

        public int Unk0 {
            get { return unk0; }
            set { unk0 = value; }
        }

        public ActorFrameWrapper()
        {

        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            unk0 = reader.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(unk0, isBigEndian);
        }

        public int GetSize()
        {
            return 4;
        }
    }
    public class ActorAircraft : IActorExtraDataInterface
    {
        public uint SoundMotorID { get; set; }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            SoundMotorID = reader.ReadUInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(SoundMotorID, isBigEndian);
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

            public ItemScript()
            {
                scriptEvent = "";
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

        public ActorItem()
        {
            type0Data = new Type0();
            scriptEvent = new ItemScript();
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

            switch (type)
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
            unk1 = Vector3Utils.ReadFromFile(reader, isBigEndian);
            unk2 = Vector3Utils.ReadFromFile(reader, isBigEndian);
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
            Vector3Utils.WriteToFile(unk1, writer, isBigEndian);
            Vector3Utils.WriteToFile(unk2, writer, isBigEndian);
        }
    }

    public class ActorPinup : IActorExtraDataInterface
    {
        int pinupNum;

        public int PinupNum {
            get { return pinupNum; }
            set { pinupNum = value; }
        }

        public ActorPinup()
        {
            pinupNum = 0;
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
            return string.Format("{0}, {1}", ActorTypes.C_Pinup, pinupNum);
        }

        public int GetSize()
        {
            return 4;
        }
    }

    public class ActorStaticEntity : IActorExtraDataInterface
    {
        int unk01;

        public ActorStaticEntity()
        {
            unk01 = 0;
        }

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        public int GetSize()
        {
            return 4;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            unk01 = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(unk01, isBigEndian);
        }
    }

    public class ActorStaticParticle : IActorExtraDataInterface
    {
        private uint particleUID;
        private bool optimalize;
        private bool loop;
        private float loopDelay;
        private float loopDelayRnd;

        public uint ParticleUID {
            get { return particleUID; }
            set { particleUID = value; }
        }
        public bool Optimalize {
            get { return optimalize; }
            set { optimalize = value; }
        }
        public bool Loop {
            get { return loop; }
            set { loop = value; }
        }
        public float LoopDelay {
            get { return loopDelay; }
            set { loopDelay = value; }
        }
        public float LoopDelayRnd {
            get { return loopDelayRnd; }
            set { loopDelayRnd = value; }
        }

        public ActorStaticParticle()
        {

        }

        public int GetSize()
        {
            return 16;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            particleUID = stream.ReadUInt32(isBigEndian);
            optimalize = stream.ReadBoolean();
            loop = stream.ReadBoolean();
            //extra 2 bytes here
            stream.Position += 2;
            loopDelay = stream.ReadSingle(isBigEndian);
            loopDelayRnd = stream.ReadSingle(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(particleUID, isBigEndian);
            writer.WriteByte(Convert.ToByte(optimalize));
            writer.WriteByte(Convert.ToByte(loop));
            writer.Position += 2;
            writer.Write(loopDelay, isBigEndian);
            writer.Write(loopDelay, isBigEndian);
        }
    }

    public class ActorHuman : IActorExtraDataInterface
    {
        public float HealthMax { get; set; }
        public int Human2_Unk1 { get; set; }
        public HumanAIType HumanType { get; set; }
        public HumanAggressiveFlags Aggressiveness { get; set; }
        public HumanCourageFlags Courage { get; set; }
        public int PanicOnEvent { get; set; }
        public int PanicOnHP { get; set; }
        public float Sight { get; set; }
        public float VisionAngle { get; set; }
        public int Hearing { get; set; }
        public int UseSoundScene { get; set; }
        public int FightingSkill { get; set; }
        public int ReactionOnSounds { get; set; }
        public int RecogniseTime { get; set; }
        public int RecogniseRange { get; set; }
        public int PreferMelee { get; set; }
        public float MeleeAttackLevel { get; set; }
        public float MeleeBlockLevel { get; set; }
        public float MeleeAggressiveness { get; set; }
        public float MeleeCounterBlockSkill { get; set; }
        public int HumanVoices { get; set; }
        public float ShootDispXY { get; set; }
        public float ShootDispZ { get; set; }
        public float ShootDispCorrectionTimeMin { get; set; }
        public float ShootDispCorrectionTimeMax { get; set; }
        public float Unk0 { get; set; }
        public float Unk1 { get; set; }
        public ulong Archetype { get; set; }
        public int Unk2 { get; set; }
        public float Unk3 { get; set; }
        public int Unk4 { get; set; }

        public ActorHuman()
        {
        }

        public int GetSize()
        {
            return 128;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            HealthMax = stream.ReadSingle(isBigEndian);
            Human2_Unk1 = stream.ReadInt32(isBigEndian);
            HumanType = (HumanAIType)stream.ReadInt32(isBigEndian);
            Aggressiveness = (HumanAggressiveFlags)stream.ReadInt32(isBigEndian);
            Courage = (HumanCourageFlags)stream.ReadInt32(isBigEndian);
            PanicOnEvent = stream.ReadInt32(isBigEndian);
            PanicOnHP = stream.ReadInt32(isBigEndian);
            Sight = stream.ReadSingle(isBigEndian);
            VisionAngle = MathHelper.ToDegrees(stream.ReadSingle(isBigEndian));
            Hearing = stream.ReadInt32(isBigEndian);
            UseSoundScene = stream.ReadInt32(isBigEndian);
            FightingSkill = stream.ReadInt32(isBigEndian);
            ReactionOnSounds = stream.ReadInt32(isBigEndian);
            RecogniseTime = stream.ReadInt32(isBigEndian);
            RecogniseRange = stream.ReadInt32(isBigEndian);
            PreferMelee = stream.ReadInt32(isBigEndian);
            MeleeAttackLevel = stream.ReadSingle(isBigEndian);
            MeleeBlockLevel = stream.ReadSingle(isBigEndian);
            MeleeAggressiveness = stream.ReadSingle(isBigEndian);
            MeleeCounterBlockSkill = stream.ReadSingle(isBigEndian);
            HumanVoices = stream.ReadInt32(isBigEndian);
            ShootDispXY = stream.ReadSingle(isBigEndian);
            ShootDispZ = stream.ReadSingle(isBigEndian);
            ShootDispCorrectionTimeMin = stream.ReadSingle(isBigEndian);
            ShootDispCorrectionTimeMax = stream.ReadSingle(isBigEndian);
            Unk1 = stream.ReadSingle(isBigEndian);
            Archetype = stream.ReadUInt64(isBigEndian);
            Unk2 = stream.ReadInt32(isBigEndian);
            Unk0 = stream.ReadSingle(isBigEndian);
            Unk3 = stream.ReadSingle(isBigEndian);
            Unk4 = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(HealthMax, isBigEndian);
            writer.Write(Human2_Unk1, isBigEndian);
            writer.Write((int)HumanType, isBigEndian);
            writer.Write((int)Aggressiveness, isBigEndian);
            writer.Write((int)Courage, isBigEndian);
            writer.Write(PanicOnEvent, isBigEndian);
            writer.Write(PanicOnHP, isBigEndian);
            writer.Write(Sight, isBigEndian);
            writer.Write(MathHelper.ToRadians(VisionAngle), isBigEndian);
            writer.Write(Hearing, isBigEndian);
            writer.Write(UseSoundScene, isBigEndian);
            writer.Write(FightingSkill, isBigEndian);
            writer.Write(ReactionOnSounds, isBigEndian);
            writer.Write(RecogniseTime, isBigEndian);
            writer.Write(RecogniseRange, isBigEndian);
            writer.Write(PreferMelee, isBigEndian);
            writer.Write(MeleeAttackLevel, isBigEndian);
            writer.Write(MeleeBlockLevel, isBigEndian);
            writer.Write(MeleeAggressiveness, isBigEndian);
            writer.Write(MeleeCounterBlockSkill, isBigEndian);
            writer.Write(HumanVoices, isBigEndian);
            writer.Write(ShootDispXY, isBigEndian);
            writer.Write(ShootDispZ, isBigEndian);
            writer.Write(ShootDispCorrectionTimeMin, isBigEndian);
            writer.Write(ShootDispCorrectionTimeMax, isBigEndian);
            writer.Write(Unk1, isBigEndian);
            writer.Write(Archetype, isBigEndian);
            writer.Write(Unk2, isBigEndian);
            writer.Write(Unk0, isBigEndian);
            writer.Write(Unk3, isBigEndian);
            writer.Write(Unk4, isBigEndian);
        }
    }

    public class ActorPlayer2 : ActorHuman
    {
        // Empty because ActorHuman contains the data
        // Only used to attempt to distinguish between the two types at a later date
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

        public ActorScriptEntity()
        {
            scriptName = string.Empty;
            unk01 = 0;
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
            return string.Format("{0}, {1}", ActorTypes.C_ScriptEntity, scriptName);
        }

        public int GetSize()
        {
            return 100;
        }
    }

    public class ActorTree : IActorExtraDataInterface
    {
        public int GarbageID { get; set; }
        public int SummerLeavesID { get; set; }
        public int WinterLeavesID { get; set; }

        public ActorTree()
        {

        }

        public int GetSize()
        {
            return 12;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            GarbageID = stream.ReadInt32(isBigEndian);
            SummerLeavesID = stream.ReadInt32(isBigEndian);
            WinterLeavesID = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(GarbageID, isBigEndian);
            writer.Write(SummerLeavesID, isBigEndian);
            writer.Write(WinterLeavesID, isBigEndian);
        }
    }

    public class ActorCutscene : IActorExtraDataInterface
    {
        public string Name { get; set; }
        public int Looped { get; set; }
        public int NumOfLoops { get; set; }

        public ActorCutscene()
        {
            Name = "";
        }

        public int GetSize()
        {
            return 136;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Name = stream.ReadStringBuffer(128);
            Looped = stream.ReadInt32(isBigEndian);
            NumOfLoops = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.WriteStringBuffer(128, Name);
            writer.Write(Looped, isBigEndian);
            writer.Write(NumOfLoops, isBigEndian);
        }
    }

    public class ActorActionPoint : IActorExtraDataInterface
    {
        public float AttractionCoeff { get; set; }
        public float AttractionRadius { get; set; }
        public float WalkRange { get; set; }
        public int SensorType { get; set; }
        public float Radius { get; set; }
        public Vector3 BBox { get; set; }
        public string ScriptEntity { get; set; }
        public byte[] Data { get; set; }
        public int Type { get; set; }
        public int NumActors { get; set; }

        public int GetSize()
        {
            return 2216;
        }

        public ActorActionPoint() : base()
        {
            Data = new byte[2048];
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            AttractionCoeff = stream.ReadSingle(isBigEndian);
            AttractionRadius = stream.ReadSingle(isBigEndian);
            WalkRange = stream.ReadSingle(isBigEndian);
            SensorType = stream.ReadInt32(isBigEndian);
            Radius = stream.ReadSingle(isBigEndian);
            BBox = Vector3Utils.ReadFromFile(stream, isBigEndian);
            ScriptEntity = stream.ReadStringBuffer(128);
            Data = stream.ReadBytes(2048);

            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] != 0)
                {
                    System.Windows.MessageBox.Show("Detected ActionPoint actor with extra data! Please mention on the Mafia: Toolkit discord!", "Toolkit", System.Windows.MessageBoxButton.OK);
                }
            }

            Type = stream.ReadInt32(isBigEndian);
            NumActors = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(AttractionCoeff, isBigEndian);
            writer.Write(AttractionRadius, isBigEndian);
            writer.Write(WalkRange, isBigEndian);
            writer.Write(SensorType, isBigEndian);
            writer.Write(Radius, isBigEndian);
            Vector3Utils.WriteToFile(BBox, writer, isBigEndian);
            writer.WriteStringBuffer(128, ScriptEntity);
            writer.Write(Data);
            writer.Write(Type, isBigEndian);
            writer.Write(NumActors, isBigEndian);
        }
    }

    public class ActorGarage : IActorExtraDataInterface
    {
        public string DoorName { get; set; }
        public byte RestrictedCars { get; set; }
        public int Unk01 { get; set; }
        public Vector3 CameraPos { get; set; }
        public Vector3 CameraTarget { get; set; }
        public Vector3 StagePos { get; set; }
        public Vector3 SpawnPos { get; set; }
        public Vector3 Human1SpawnPos { get; set; }
        public Vector3 Human2SpawnPos { get; set; }
        public Vector3 Human3SpawnPos { get; set; }
        public Vector3 Human4SpawnPos { get; set; }

        public ActorGarage()
        {
            DoorName = "";
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            DoorName = stream.ReadStringBuffer(31);
            RestrictedCars = stream.ReadByte8();
            Unk01 = stream.ReadInt32(isBigEndian);
            CameraPos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            CameraTarget = Vector3Utils.ReadFromFile(stream, isBigEndian);
            StagePos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            SpawnPos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            Human1SpawnPos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            Human2SpawnPos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            Human3SpawnPos = Vector3Utils.ReadFromFile(stream, isBigEndian);
            Human4SpawnPos = Vector3Utils.ReadFromFile(stream, isBigEndian);

            ToolkitAssert.Ensure(RestrictedCars == 0, "Restricted Cars is not 0. Please inform Greavesy.");
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.WriteStringBuffer(31, DoorName);
            writer.WriteByte(RestrictedCars);
            writer.Write(Unk01, isBigEndian);
            CameraPos.WriteToFile(writer, isBigEndian);
            CameraTarget.WriteToFile(writer, isBigEndian);
            StagePos.WriteToFile(writer, isBigEndian);
            SpawnPos.WriteToFile(writer, isBigEndian);
            Human1SpawnPos.WriteToFile(writer, isBigEndian);
            Human2SpawnPos.WriteToFile(writer, isBigEndian);
            Human3SpawnPos.WriteToFile(writer, isBigEndian);
            Human4SpawnPos.WriteToFile(writer, isBigEndian);
        }

        public int GetSize()
        {
            return 132;
        }
    }

    public class ActorBlocker : IActorExtraDataInterface
    {
        public bool BlockPlayer { get; set; }
        public bool BlockHuman { get; set; }
        public bool BlockVehicle { get; set; }
        public Vector3 BBox { get; set; }

        public ActorBlocker()
        {
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            BlockPlayer = stream.ReadBoolean();
            BlockHuman = stream.ReadBoolean();
            BlockVehicle = stream.ReadBoolean();
            stream.ReadBoolean(); // Padding?
            BBox = Vector3Utils.ReadFromFile(stream, isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(BlockPlayer);
            writer.Write(BlockHuman);
            writer.Write(BlockVehicle);
            writer.Write(false); // Padding?
            BBox.WriteToFile(writer, isBigEndian);
        }

        public int GetSize()
        {
            return 16;
        }
    }

    public class ActorActionPointSearch : IActorExtraDataInterface
    {
        public float WalkRange { get; set; }
        public string ScriptEntity { get; set; }

        public ActorActionPointSearch()
        {
            ScriptEntity = "";
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            WalkRange = stream.ReadSingle(isBigEndian);
            ScriptEntity = stream.ReadStringBuffer(128);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(WalkRange, isBigEndian);
            writer.WriteStringBuffer(128, ScriptEntity);
        }

        public int GetSize()
        {
            return 132;
        }
    }

    public class ActorStaticWeapon : IActorExtraDataInterface
    {
        public int Unk01 { get; set; }

        public ActorStaticWeapon() { }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            Unk01 = stream.ReadInt32(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(Unk01, isBigEndian);
        }

        public int GetSize()
        {
            return 4;
        }
    }

    public class ActorCrashObject : ActorPhysicsBase
    {
        public class ActorCrashObjectEntry
        {
            public ulong Hash { get; set; }
            public float Unk01 { get; set; }
            public int Unk02 { get; set; }
        }

        public float RemoveDistance { get; set; }
        public int RemoveTime { get; set; }
        public int RemoveRule { get; set; }
        public int MainPartRule { get; set; }
        public int DropEndTime { get; set; }
        public int HitPlayerFilter { get; set; }
        public int HumanRule { get; set; }
        public bool CameraCollision { get; set; }
        public bool BlinkingAnimation { get; set; }
        public bool DisableLight { get; set; }
        public bool ActiveInDayOnly { get; set; }
        public int NumEntries { get; set; }
        public int Unk03 { get; set; }
        public ActorCrashObjectEntry[] Entries { get; set; }
        public int Unk01 { get; set; }
        public int Unk02 { get; set; }

        public override int GetSize()
        {
            return 416;
        }

        public ActorCrashObject() : base()
        {
            Entries = new ActorCrashObjectEntry[8];
            for (int i = 0; i < 8; i++)
            {
                Entries[i] = new ActorCrashObjectEntry();
            }
        }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            stream.Seek(240, SeekOrigin.Begin);

            RemoveDistance = stream.ReadSingle(isBigEndian);
            RemoveTime = stream.ReadInt32(isBigEndian);
            RemoveRule = stream.ReadInt32(isBigEndian);
            MainPartRule = stream.ReadInt32(isBigEndian);
            DropEndTime = stream.ReadInt32(isBigEndian);
            HitPlayerFilter = stream.ReadInt32(isBigEndian);
            HumanRule = stream.ReadInt32(isBigEndian);
            CameraCollision = stream.ReadBoolean();
            BlinkingAnimation = stream.ReadBoolean();
            DisableLight = stream.ReadBoolean();
            ActiveInDayOnly = stream.ReadBoolean();

            NumEntries = stream.ReadInt32(isBigEndian);

            Unk03 = stream.ReadInt32(isBigEndian);

            Entries = new ActorCrashObjectEntry[8];
            for (int i = 0; i < 8; i++)
            {
                ActorCrashObjectEntry Entry = new ActorCrashObjectEntry();
                Entry.Hash = stream.ReadUInt64(isBigEndian);
                Entry.Unk01 = stream.ReadSingle(isBigEndian);
                Entry.Unk02 = stream.ReadInt32(isBigEndian);
                Entries[i] = Entry;
            }

            Unk01 = stream.ReadInt32(isBigEndian);
            Unk02 = stream.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);
            writer.Seek(240, SeekOrigin.Begin);

            writer.Write(RemoveDistance, isBigEndian);
            writer.Write(RemoveTime, isBigEndian);
            writer.Write(RemoveRule, isBigEndian);
            writer.Write(MainPartRule, isBigEndian);
            writer.Write(DropEndTime, isBigEndian);
            writer.Write(HitPlayerFilter, isBigEndian);
            writer.Write(HumanRule, isBigEndian);
            writer.Write(CameraCollision);
            writer.Write(BlinkingAnimation);
            writer.Write(DisableLight);
            writer.Write(ActiveInDayOnly);

            writer.Write(NumEntries, isBigEndian);

            writer.Write(Unk03, isBigEndian);

            for (int i = 0; i < 8; i++)
            {
                writer.Write(Entries[i].Hash, isBigEndian);
                writer.Write(Entries[i].Unk01, isBigEndian);
                writer.Write(Entries[i].Unk02, isBigEndian);
            }

            writer.Write(Unk01, isBigEndian);
            writer.Write(Unk02, isBigEndian);
        }
    }

    public class ActorJukebox : ActorPhysicsBase
    {
        public string SoundCoin { get; set; }
        public float Volume { get; set; }
        public float Range { get; set; }
        public uint CurveID { get; set; }
        public float NearRange { get; set; }

        public override int GetSize()
        {
            return 744;
        }

        public ActorJukebox() : base()
        {
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            reader.Position = 645;
            SoundCoin = reader.ReadStringBuffer(83);
            Volume = reader.ReadSingle(isBigEndian);
            Range = reader.ReadSingle(isBigEndian);
            NearRange = reader.ReadSingle(isBigEndian);
            CurveID = reader.ReadUInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);

            // Write padding
            writer.Write(new byte[GetSize() - writer.Length]);
            writer.Position = 645;
            writer.WriteStringBuffer(83, SoundCoin);
            writer.Write(Volume, isBigEndian);
            writer.Write(Range, isBigEndian);
            writer.Write(NearRange, isBigEndian);
            writer.Write(CurveID, isBigEndian);
        }
    }

    public class ActorPhysicsScene : IActorExtraDataInterface
    {
        public Vector3 BBox { get; set; }
        public uint SceneID { get; set; }
        public bool CarSimulated { get; set; }
        public bool CarFarScene { get; set; }
        public ushort Unk01 { get; set; }

        public ActorPhysicsScene() : base()
        {
            BBox = Vector3.Zero;
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            BBox = Vector3Utils.ReadFromFile(stream, isBigEndian);
            SceneID = stream.ReadUInt32(isBigEndian);
            CarSimulated = stream.ReadBoolean();
            CarFarScene = stream.ReadBoolean();
            Unk01 = stream.ReadUInt16(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            BBox.WriteToFile(writer, isBigEndian);
            writer.Write(SceneID, isBigEndian);
            writer.Write(CarSimulated);
            writer.Write(CarFarScene);
            writer.Write(Unk01, isBigEndian);
        }

        public int GetSize()
        {
            return 20;
        }
    }

    public class ActorBoat : ActorPhysicsBase
    {
        public float Thrust { get; set; }
        public float AreaRudder { get; set; }
        public float Sink { get; set; }
        public float Wobling { get; set; }
        public int MoveOnRoad { get; set; }
        public int ExhaustID { get; set; }
        public int SoundMotorID { get; set; }
        public int SoundHornID { get; set; }
        public float SimplePhysicsDist { get; set; }
        public Vector3 WheelPos { get; set; }

        public ActorBoat() : base() { }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            reader.Seek(240, SeekOrigin.Begin);

            Thrust = reader.ReadSingle(isBigEndian);
            AreaRudder = reader.ReadSingle(isBigEndian);
            Sink = reader.ReadSingle(isBigEndian);
            Wobling = reader.ReadSingle(isBigEndian);
            MoveOnRoad = reader.ReadInt32(isBigEndian);
            ExhaustID = reader.ReadInt32(isBigEndian);
            SoundMotorID = reader.ReadInt32(isBigEndian);
            SoundHornID = reader.ReadInt32(isBigEndian);
            SimplePhysicsDist = reader.ReadSingle(isBigEndian);
            WheelPos = Vector3Utils.ReadFromFile(reader, isBigEndian);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);

            // Write padding
            writer.Write(new byte[GetSize() - writer.Length]);
            writer.Position = 240;

            writer.Write(Thrust, isBigEndian);
            writer.Write(AreaRudder, isBigEndian);
            writer.Write(Sink, isBigEndian);
            writer.Write(Wobling, isBigEndian);
            writer.Write(MoveOnRoad, isBigEndian);
            writer.Write(ExhaustID, isBigEndian);
            writer.Write(SoundMotorID, isBigEndian);
            writer.Write(SoundHornID, isBigEndian);
            writer.Write(SimplePhysicsDist, isBigEndian);
            WheelPos.WriteToFile(writer, isBigEndian);
        }

        public override int GetSize()
        {
            return 288;
        }
    }

    public class ActorTelephone : ActorCrashObject
    {
        [Category("C_Telephone")]
        public bool bEnableForPlayer { get; set; }
        [Category("C_Telephone")]
        public bool bEnableForNPC { get; set; }
        [Category("C_Telephone")]
        public int RingToneSoundID { get; set; }
        [Category("C_Telephone")]
        public int PickSoundID { get; set; }
        [Category("C_Telephone")]
        public int HangSoundID { get; set; }
        [Category("C_Telephone")]
        public int DialSoundID { get; set; }
        [Category("C_Telephone")]
        public int DialToneSoundID { get; set; }
        [Category("C_Telephone")]
        public int IdleToneSoundID { get; set; }
        [Category("C_Telephone")]
        public TelephoneType PhoneTypeID { get; set; }

        public ActorTelephone() : base() { }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            bEnableForPlayer = reader.ReadBoolean();
            bEnableForNPC = reader.ReadBoolean();
            reader.Seek(2, SeekOrigin.Current); // padding
            RingToneSoundID = reader.ReadInt32(isBigEndian);
            PickSoundID = reader.ReadInt32(isBigEndian);
            HangSoundID = reader.ReadInt32(isBigEndian);
            DialSoundID = reader.ReadInt32(isBigEndian);
            DialToneSoundID = reader.ReadInt32(isBigEndian);
            IdleToneSoundID = reader.ReadInt32(isBigEndian);
            PhoneTypeID = (TelephoneType)reader.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);

            writer.Write(bEnableForPlayer);
            writer.Write(bEnableForNPC);
            writer.Write((ushort)0, isBigEndian); // padding
            writer.Write(RingToneSoundID, isBigEndian);
            writer.Write(PickSoundID, isBigEndian);
            writer.Write(HangSoundID, isBigEndian);
            writer.Write(DialSoundID, isBigEndian);
            writer.Write(DialToneSoundID, isBigEndian);
            writer.Write(IdleToneSoundID, isBigEndian);
            writer.Write((int)PhoneTypeID, isBigEndian);
        }

        public override int GetSize()
        {
            return 448;
        }
    }

    public class ActorFramesController : IActorExtraDataInterface
    {
        public class FramesControllerEntry
        {
            public ulong SceneNameHash { get; set; }
            public ulong FrameNameHash { get; set; }

            public override string ToString()
            {
                return string.Format("Scene: {0} Frame: {1}", SceneNameHash, FrameNameHash);
            }
        }

        public FramesControllerEntry[] Entries { get; set; }
        public ulong Unk0 { get; set; }

        private const uint NUM_ENTRIES = 20;

        public ActorFramesController() : base()
        {
            Entries = new FramesControllerEntry[NUM_ENTRIES];
            for(ulong i = 0; i < NUM_ENTRIES; i++)
            {
                Entries[i] = new FramesControllerEntry();
            }
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            for (ulong i = 0; i < NUM_ENTRIES; i++)
            {
                Entries[i].SceneNameHash = stream.ReadUInt64(isBigEndian);
                Entries[i].FrameNameHash = stream.ReadUInt64(isBigEndian);
            }

            Unk0 = stream.ReadUInt64(isBigEndian);
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            for (ulong i = 0; i < NUM_ENTRIES; i++)
            {
                writer.Write(Entries[i].SceneNameHash, isBigEndian);
                writer.Write(Entries[i].FrameNameHash, isBigEndian);
            }

            writer.Write(Unk0, isBigEndian);
        }

        public int GetSize()
        {
            return 328;
        }
    }

    public class ActorLift : ActorPhysicsBase
    {
        [Category("C_Lift")]
        public int InitialFloor { get; set; }
        [Category("C_Lift")]
        public float MotionSpeed { get; set; }
        [Category("C_Lift")]
        public float AcceleratingDistance { get; set; }
        [Category("C_Lift")]
        public float BreakingDistance { get; set; }
        [Category("C_Lift")]
        public string LightFrameName { get; set; } // limited to 32
        [Category("C_Lift")]
        public string LinkerPrefix { get; set; } // limited to 32
        [Category("C_Lift")]
        public int SoundCategoryId { get; set; }
        [Category("C_Lift")]
        public int AcceleratingSoundId { get; set; }
        [Category("C_Lift")]
        public int BreakingSoundId { get; set; }
        [Category("C_Lift")]
        public int MovingSoundId { get; set; }
        [Category("C_Lift")]
        public int ButtonSoundId { get; set; }
        [Category("C_Lift")]
        public bool bAutomaticDoorOpen { get; set; }
        [Category("C_Lift")]
        public string Door1Name { get; set; }
        [Category("C_Lift")]
        public string Door2Name { get; set; }
        [Category("C_Lift")]
        public bool bScriptControlled { get; set; }
        [Category("C_Lift")]
        public int InnerSpaceGraphIndex { get; set; }
        [Category("C_Lift")]
        public int OverrideFloors { get; set; }
        [Category("C_Lift")]
        public int SplineType { get; set; }
        [Category("C_Lift")]
        public float SplineInRange { get; set; }
        [Category("C_Lift")]
        public int SplineNavigation { get; set; }
        [Category("C_Lift")]
        public ulong SplineLock0 { get; set; }
        [Category("C_Lift")]
        public ulong SplineLock1 { get; set; }
        [Category("C_Lift")]
        public float Unk0 { get; set; }
        [Category("C_Lift")]
        public float Unk1 { get; set; }
        [Category("C_Lift")]
        public float Unk2 { get; set; }


        public ActorLift()
        {
            LightFrameName = string.Empty;
            LinkerPrefix = string.Empty;
            Door1Name = string.Empty;
            Door2Name = string.Empty;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);

            reader.Seek(240, SeekOrigin.Begin);

            InitialFloor = reader.ReadInt32(isBigEndian);
            MotionSpeed = reader.ReadSingle(isBigEndian);
            AcceleratingDistance = reader.ReadSingle(isBigEndian);
            BreakingDistance = reader.ReadSingle(isBigEndian);
            LightFrameName = reader.ReadStringBuffer(32);
            LinkerPrefix = reader.ReadStringBuffer(32);
            SoundCategoryId = reader.ReadInt32(isBigEndian);
            AcceleratingSoundId = reader.ReadInt32(isBigEndian);
            BreakingSoundId = reader.ReadInt32(isBigEndian);
            MovingSoundId = reader.ReadInt32(isBigEndian);
            ButtonSoundId = reader.ReadInt32(isBigEndian);
            bAutomaticDoorOpen = Convert.ToBoolean(reader.ReadByte8());
            Door1Name = reader.ReadStringBuffer(64);
            Door2Name = reader.ReadStringBuffer(64);
            bScriptControlled = Convert.ToBoolean(reader.ReadByte8());
            reader.Seek(2, SeekOrigin.Current); // padding?
            InnerSpaceGraphIndex = reader.ReadInt32(isBigEndian);
            OverrideFloors = reader.ReadInt32(isBigEndian);
            SplineType = reader.ReadInt32(isBigEndian);
            SplineInRange = reader.ReadSingle(isBigEndian);
            SplineNavigation = reader.ReadInt32(isBigEndian);
            SplineLock0 = reader.ReadUInt64(isBigEndian);
            SplineLock1 = reader.ReadUInt64(isBigEndian);
            Unk0 = reader.ReadSingle(isBigEndian);
            Unk1 = reader.ReadSingle(isBigEndian);
            Unk2 = reader.ReadSingle(isBigEndian);

        }

        public override void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            base.WriteToFile(writer, isBigEndian);

            // Write padding
            writer.Write(new byte[GetSize() - writer.Length]);
            writer.Position = 240;

            writer.Write(InitialFloor, isBigEndian);
            writer.Write(MotionSpeed, isBigEndian);
            writer.Write(AcceleratingDistance, isBigEndian);
            writer.Write(BreakingDistance, isBigEndian);
            writer.WriteStringBuffer(32, LightFrameName);
            writer.WriteStringBuffer(32, LinkerPrefix);
            writer.Write(SoundCategoryId, isBigEndian);
            writer.Write(AcceleratingSoundId, isBigEndian);
            writer.Write(BreakingSoundId, isBigEndian);
            writer.Write(MovingSoundId, isBigEndian);
            writer.Write(ButtonSoundId, isBigEndian);
            writer.WriteByte(Convert.ToByte(bAutomaticDoorOpen));
            writer.WriteStringBuffer(64, Door1Name);
            writer.WriteStringBuffer(64, Door2Name);
            writer.WriteByte(Convert.ToByte(bScriptControlled));
            writer.Write((ushort)0, isBigEndian); // padding
            writer.Write(InnerSpaceGraphIndex, isBigEndian);
            writer.Write(OverrideFloors, isBigEndian);
            writer.Write(SplineType, isBigEndian);
            writer.Write(SplineInRange, isBigEndian);
            writer.Write(SplineNavigation, isBigEndian);
            writer.Write(SplineLock0, isBigEndian);
            writer.Write(SplineLock1, isBigEndian);
            writer.Write(Unk0, isBigEndian);
            writer.Write(Unk1, isBigEndian);
            writer.Write(Unk2, isBigEndian);
        }

        public override int GetSize()
        {
            return 520;
        }
    }

    public class ActorSoundMixer : IActorExtraDataInterface
    {
        public class Mixer
        {
            public class FadePoint
            {
                public int MixValue { get; set; }
                public float MixVolume { get; set; }

                public override string ToString()
                {
                    return string.Format("{0} -> {1}", MixValue, MixVolume);
                }
            }

            public uint SoundType { get; set; }
            public float SoundVolume { get; set; }
            public float SoundPitch { get; set; }
            public string SoundWave { get; set; }
            public int Unk0 { get; set; }
            public float Near { get; set; }
            public float Far { get; set; }
            public int CurveId { get; set; }

            // This may only be present if SoundType == 30
            public float MonoDistance { get; set; }
            public float InnerAngle { get; set; } // stored in rad
            public float OuterAngle { get; set; } // stored in rad
            public float OuterVolume { get; set; }
            public FadePoint[] FadePoints { get; set; }

            private const uint MAX_FADE_POINTS = 5;

            public Mixer()
            {
                SoundWave = string.Empty;
                FadePoints = new FadePoint[MAX_FADE_POINTS];
                for(int i = 0; i < MAX_FADE_POINTS; i++)
                {
                    FadePoints[i] = new FadePoint();
                }
            }

            public void Read(MemoryStream InStream, bool bIsBigEndian)
            {
                SoundType = InStream.ReadUInt32(bIsBigEndian);
                SoundVolume = InStream.ReadSingle(bIsBigEndian);
                SoundPitch = InStream.ReadSingle(bIsBigEndian);
                SoundWave = InStream.ReadStringBuffer(80);
                Unk0 = InStream.ReadInt32(bIsBigEndian); // always zero?
                Near = InStream.ReadSingle(bIsBigEndian);
                Far = InStream.ReadSingle(bIsBigEndian);
                CurveId = InStream.ReadInt32(bIsBigEndian);
                MonoDistance = InStream.ReadSingle(bIsBigEndian);
                InnerAngle = InStream.ReadSingle(bIsBigEndian);
                OuterAngle = InStream.ReadSingle(bIsBigEndian);
                OuterVolume = InStream.ReadSingle(bIsBigEndian);

                for (int i = 0; i < MAX_FADE_POINTS; i++)
                {
                    FadePoints[i].MixValue = InStream.ReadInt32(bIsBigEndian);
                    FadePoints[i].MixVolume = InStream.ReadSingle(bIsBigEndian);
                }
            }

            public void Write(MemoryStream InStream, bool bIsBigEndian)
            {
                InStream.Write(SoundType, bIsBigEndian);
                InStream.Write(SoundVolume, bIsBigEndian);
                InStream.Write(SoundPitch, bIsBigEndian);
                InStream.WriteStringBuffer(80, SoundWave);
                InStream.Write(Unk0, bIsBigEndian);
                InStream.Write(Near, bIsBigEndian);
                InStream.Write(Far, bIsBigEndian);
                InStream.Write(CurveId, bIsBigEndian);
                InStream.Write(MonoDistance, bIsBigEndian);
                InStream.Write(InnerAngle, bIsBigEndian);
                InStream.Write(OuterAngle, bIsBigEndian);
                InStream.Write(OuterVolume, bIsBigEndian);

                for (int i = 0; i < MAX_FADE_POINTS; i++)
                {
                    InStream.Write(FadePoints[i].MixValue, bIsBigEndian);
                    InStream.Write(FadePoints[i].MixVolume, bIsBigEndian);
                }
            }

            public override string ToString()
            {
                string ActualSoundWave = (SoundWave == string.Empty ? "[UNSET]" :  SoundWave);
                return string.Format("Type: {0} Sound: {1}", SoundType, ActualSoundWave);
            }
        }

        public int Unk0 { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorSoundMixerFlags Flags { get; set; }
        public Mixer[] Mixers { get; set; }

        private const uint MAX_MIXERS = 3;

        public ActorSoundMixer()
        {
            // Initialise array
            Mixers = new Mixer[MAX_MIXERS];
            for (int i = 0; i < MAX_MIXERS; i++)
            {
                Mixers[i] = new Mixer();
            }
        }

        public void ReadFromFile(MemoryStream InStream, bool bIsBigEndian)
        {
            Unk0 = InStream.ReadInt32(bIsBigEndian);
            Flags = (ActorSoundMixerFlags)InStream.ReadInt32(bIsBigEndian);

            for(int i = 0; i < MAX_MIXERS; i++)
            {
                Mixers[i] = new Mixer();
                Mixers[i].Read(InStream, bIsBigEndian);
            }
        }

        public void WriteToFile(MemoryStream InStream, bool bIsBigEndian)
        {
            InStream.Write(Unk0, bIsBigEndian);
            InStream.Write((int)Flags, bIsBigEndian);

            for(int i = 0; i < MAX_MIXERS; i++)
            {
                Mixers[i].Write(InStream, bIsBigEndian);
            }
        }

        public int GetSize()
        {
            return 500;
        }
    }

    public class ActorFireTarget : IActorExtraDataInterface
    {
        public int FireTargetType { get; set; }
        public float Timeout { get; set; }
        public float Range { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }
        public Vector3 BoxExtents { get; set; }
        public int ParticleId { get; set; }
        public float ParticleScale { get; set; }
        public int Probability { get; set; }
        public int SoundId { get; set; }

        public void ReadFromFile(MemoryStream InStream, bool bIsBigEndian)
        {
            FireTargetType = InStream.ReadInt32(bIsBigEndian);
            Timeout = InStream.ReadSingle(bIsBigEndian);
            Range = InStream.ReadSingle(bIsBigEndian);
            Radius = InStream.ReadSingle(bIsBigEndian);
            Height = InStream.ReadSingle(bIsBigEndian);
            BoxExtents = Vector3Utils.ReadFromFile(InStream, bIsBigEndian);
            ParticleId = InStream.ReadInt32(bIsBigEndian);
            ParticleScale = InStream.ReadSingle(bIsBigEndian);
            Probability = InStream.ReadInt32(bIsBigEndian);
            SoundId = InStream.ReadInt32(bIsBigEndian);
        }

        public void WriteToFile(MemoryStream InStream, bool bIsBigEndian)
        {
            InStream.Write(FireTargetType, bIsBigEndian);
            InStream.Write(Timeout, bIsBigEndian);
            InStream.Write(Range, bIsBigEndian);
            InStream.Write(Radius, bIsBigEndian);
            InStream.Write(Height, bIsBigEndian);
            BoxExtents.WriteToFile(InStream, bIsBigEndian);
            InStream.Write(ParticleId, bIsBigEndian);
            InStream.Write(ParticleScale, bIsBigEndian);
            InStream.Write(Probability, bIsBigEndian);
            InStream.Write(SoundId, bIsBigEndian);
        }

        public int GetSize()
        {
            return 48;
        }
    }

}
