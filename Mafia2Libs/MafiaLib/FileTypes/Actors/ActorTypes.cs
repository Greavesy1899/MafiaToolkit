using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Actors
{
    public interface IActorExtraDataInterface
    {
        void ReadFromFile(BinaryReader reader);
        void WriteToFile(BinaryWriter writer);
    }

    public class ActorUnkBase : IActorExtraDataInterface
    {
        /*
         * signed int __usercall sub_3F9D50@<eax>(long double a1@<st0>, int a2, UnkPropData *a3)
{
  int v3; // edi@1
  int v4; // esi@1
  int v5; // edi@3
  signed int result; // eax@3
  __int64 v7; // [sp+38h] [bp-B0h]@1
  int v8; // [sp+40h] [bp-A8h]@1
  __int64 v9; // [sp+48h] [bp-A0h]@1
  int v10; // [sp+50h] [bp-98h]@1
  char v11; // [sp+58h] [bp-90h]@2
  int v12; // [sp+D8h] [bp-10h]@1

  v3 = a2;
  v12 = __stack_chk_guard;
  a3->byte0 = (*(int (__stdcall **)(int))(*(_DWORD *)a2 + 28))(a2);
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v3 + 12))(v3, 31894661);
  a3->float4 = a1;
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v3 + 12))(v3, 31894677);
  a3->floatC = a1;
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v3 + 12))(v3, (char *)&loc_1A70F49 + 4169058);
  a3->float8 = a1;
  a3->dword10 = (*(int (__cdecl **)(int, signed int))(*(_DWORD *)v3 + 8))(v3, 31894709);
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v3 + 12))(v3, (char *)&loc_1A687A6 + 4169057);
  a3->float14 = a1;
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v3 + 12))(v3, (char *)&loc_1A70F5F + 4169057);
  a3->float18 = a1;
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v3 + 12))(v3, 31859990);
  a3->float1C = a1;
  a3->dwordD4 = (*(int (__cdecl **)(int, char *))(*(_DWORD *)v3 + 8))(v3, (char *)&loc_1A70F6F + 4169057);
  (*(void (__cdecl **)(__int64 *, int, char *))(*(_DWORD *)v3 + 24))(&v9, v3, (char *)&loc_1A70F76 + 4169057);
  a3->dwordE0 = v10;
  a3->qwordD8 = v9;
  (*(void (__stdcall **)(__int64 *))(*(_DWORD *)a2 + 24))(&v7);
  a3->dwordEC = v8;
  a3->qwordE4 = v7;
  v4 = 0;
  do
  {
    sprintf(&v11, "Hit%dId", v4 + 3);
    *(_DWORD *)&a3->gap20[4 * v4 + 8] = (*(int (__cdecl **)(int, char *))(*(_DWORD *)a2 + 8))(a2, &v11);
    sprintf(&v11, (const char *)&loc_1A70F8B + 4169057, v4 + 3);
    (*(void (__cdecl **)(int, char *))(*(_DWORD *)a2 + 12))(a2, &v11);
    *(float *)&a3->gap20[4 * v4 + 20] = a1;
    sprintf(&v11, (const char *)&loc_1A70F98 + 4169058, v4 + 3);
    (*(void (__cdecl **)(int, char *))(*(_DWORD *)a2 + 12))(a2, &v11);
    *(float *)&a3->gap20[4 * v4 + 32] = a1;
    sprintf(&v11, (const char *)&loc_1A70FA8 + 4169059, v4 + 3);
    *(_DWORD *)&a3->gap20[4 * v4 + 44] = (*(int (__cdecl **)(int, char *))(*(_DWORD *)a2 + 8))(a2, &v11);
    sprintf(&v11, (const char *)&unk_1A70FB6 + 4169057, v4 + 3);
    *(_DWORD *)&a3->gap20[4 * v4 + 56] = (*(int (__cdecl **)(int, char *))(*(_DWORD *)a2 + 8))(a2, &v11);
    sprintf(&v11, "Hit%dFreqLow", v4 + 3);
    (*(void (__cdecl **)(int, char *))(*(_DWORD *)a2 + 12))(a2, &v11);
    *(float *)&a3->gap20[4 * v4 + 68] = a1;
    sprintf(&v11, "Hit%dFreqHi", v4 + 3);
    (*(void (__cdecl **)(int, char *))(*(_DWORD *)a2 + 12))(a2, &v11);
    *(float *)&a3->gap20[4 * v4-- + 80] = a1;
  }
  while ( v4 != -3 );
  v5 = a2;
  a3->dword74 = (*(int (__cdecl **)(int, char *))(*(_DWORD *)a2 + 8))(a2, (char *)&locret_1A70FDB + 4169057);
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v5 + 12))(v5, (char *)&loc_1A70FE3 + 4169057);
  a3->float78 = a1;
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 12))(v5, 31894866);
  a3->float7C = a1;
  a3->dword80 = (*(int (__cdecl **)(int, char *))(*(_DWORD *)v5 + 8))(v5, (char *)&loc_1A71002 + 4169057);
  a3->dword84 = (*(int (__cdecl **)(int, char *))(*(_DWORD *)v5 + 8))(v5, (char *)&loc_1A7100E + 4169057);
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 12))(v5, 31894907);
  a3->float88 = a1;
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 12))(v5, 31894920);
  a3->float8C = a1;
  a3->dword90 = (*(int (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 8))(v5, 31894932);
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v5 + 12))(v5, (char *)&loc_1A7103A + 4169057);
  a3->float94 = a1;
  (*(void (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 12))(v5, 31894952);
  a3->float98 = a1;
  a3->dword9C = (*(int (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 8))(v5, 31894968);
  a3->dwordA0 = (*(int (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 8))(v5, 31894979);
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v5 + 12))(v5, (char *)&loc_1A7106D + 4169057);
  a3->floatA4 = a1;
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v5 + 12))(v5, (char *)&loc_1A71079 + 4169057);
  a3->floatA8 = a1;
  a3->dwordAC = (*(int (__cdecl **)(int, signed int))(*(_DWORD *)v5 + 8))(v5, 31895013);
  a3->dwordB0 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895030);
  a3->dwordB4 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895041);
  a3->dwordB8 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895052);
  a3->dwordBC = (*(int (__cdecl **)(int, char *))(*(_DWORD *)v5 + 8))(v5, (char *)&loc_1A710B9 + 4169057);
  a3->dwordC0 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895080);
  a3->dwordC4 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895096);
  a3->dwordC8 = (*(int (__cdecl **)(_DWORD, signed int))(*(_DWORD *)v5 + 8))(v5, 31895118);
  (*(void (__cdecl **)(int, char *))(*(_DWORD *)v5 + 12))(v5, (char *)&locret_1A710FD + 4169057);
  a3->floatD0 = a1;
  a3->dwordCC = (*(int (__cdecl **)(int, char *))(*(_DWORD *)v5 + 8))(v5, (char *)&loc_1A7110F + 4169059);
  result = __stack_chk_guard;
  if ( __stack_chk_guard == v12 )
    result = 1;
  return result;
}*/
        //float float4;
        //float float8;
        //float floatC;
        //int dword10;
        //float float10;
        //float float14;
        //float float18;
        //float float1C;
        //int dwordD4;
        //int dwordD0;
        //long qwordD8;
        //int dwordEC;
        //int qwordE4;

        byte[] data;

        public void ReadFromFile(BinaryReader reader)
        {
            //float4 = reader.ReadSingle();
            //float8 = reader.ReadSingle();
            //floatC = reader.ReadSingle();
            //dword10 = Convert.ToInt32(reader.ReadSingle());
            //float14 = reader.ReadSingle();
            //float18 = reader.ReadSingle();
            //float1C = reader.ReadSingle();
            //dwordD4 = Convert.ToInt32(reader.ReadSingle());

            data = reader.ReadBytes(240);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(data);
        }
    }

    public class ActorRadio : ActorUnkBase
    {
        int flags;
        float range;
        float nearRange;
        float volume;
        int curveID;
        string program;
        string playlist;
        string station;

        public ActorRadio(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public new void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            flags = reader.ReadInt32();
            range = reader.ReadSingle();
            nearRange = reader.ReadSingle();
            volume = reader.ReadSingle();
            curveID = reader.ReadInt32();
            program = new string(reader.ReadChars(256));
            playlist = new string(reader.ReadChars(256));
            station = new string(reader.ReadChars(256));
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
        public ActorTrafficCar(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            type = reader.ReadInt32();
            bbox = BoundingBoxExtenders.ReadFromFile(reader);
            unk0 = reader.ReadSingle();
            unk1 = reader.ReadSingle();
            unk2 = reader.ReadSingle();
            unk3 = reader.ReadSingle();
            maxElements = reader.ReadInt32();
            pie = reader.ReadInt32();
            tableName = new string(reader.ReadChars(32)).TrimEnd('\0');
            areaName = new string(reader.ReadChars(64)).TrimEnd('\0');
            carUnk4 = reader.ReadSingle();
            carUnk5 = reader.ReadSingle();
            carUnk6 = reader.ReadInt32();
            spawnedParking = reader.ReadInt32();
            parking = reader.ReadInt32();
            crewGenerator = new string(reader.ReadChars(32)).TrimEnd('\0');
            dirtyMin = reader.ReadInt32();
            dirtyMax = reader.ReadInt32();
            damageMin = reader.ReadInt32();
            damageMax = reader.ReadInt32();
            zero = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(type);
            BoundingBoxExtenders.WriteToFile(bbox, writer);
            writer.Write(unk0);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(maxElements);
            writer.Write(pie);
            StringHelpers.WriteStringBuffer(writer, 32, tableName, '\0');
            StringHelpers.WriteStringBuffer(writer, 64, areaName, '\0');
            writer.Write(carUnk4);
            writer.Write(carUnk5);
            writer.Write(carUnk6);
            writer.Write(spawnedParking);
            writer.Write(parking);
            StringHelpers.WriteStringBuffer(writer, 32, crewGenerator, '\0');
            writer.Write(dirtyMin);
            writer.Write(dirtyMax);
            writer.Write(damageMin);
            writer.Write(damageMax);
            writer.Write(zero);
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

        public ActorTrafficHuman(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            type = reader.ReadInt32();
            bbox = BoundingBoxExtenders.ReadFromFile(reader);
            unk0 = reader.ReadSingle();
            unk1 = reader.ReadSingle();
            unk2 = reader.ReadSingle();
            unk3 = reader.ReadSingle();
            maxElements = reader.ReadInt32();
            pie = reader.ReadInt32();
            tableName = new string(reader.ReadChars(32)).TrimEnd('\0');
            areaName = new string(reader.ReadChars(64)).TrimEnd('\0');
            zDistance = reader.ReadSingle();
            agregationRange = reader.ReadSingle();
            agregationCount = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(type);
            BoundingBoxExtenders.WriteToFile(bbox, writer);
            writer.Write(unk0);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(maxElements);
            writer.Write(pie);
            StringHelpers.WriteStringBuffer(writer, 32, tableName, '\0');
            StringHelpers.WriteStringBuffer(writer, 64, areaName, '\0');
            writer.Write(zDistance);
            writer.Write(agregationRange);
            writer.Write(agregationCount);
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

        public ActorTrafficTrain(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            type = reader.ReadInt32();
            bbox = BoundingBoxExtenders.ReadFromFile(reader);
            unk0 = reader.ReadSingle();
            unk1 = reader.ReadSingle();
            unk2 = reader.ReadSingle();
            unk3 = reader.ReadSingle();
            maxElements = reader.ReadInt32();
            pie = reader.ReadInt32();
            tableName = new string(reader.ReadChars(32)).TrimEnd('\0');
            areaName = new string(reader.ReadChars(64)).TrimEnd('\0');
            crewGenerator = new string(reader.ReadChars(32)).TrimEnd('\0');
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(type);
            BoundingBoxExtenders.WriteToFile(bbox, writer);
            writer.Write(unk0);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(maxElements);
            writer.Write(pie);
            StringHelpers.WriteStringBuffer(writer, 32, tableName, '\0');
            StringHelpers.WriteStringBuffer(writer, 64, areaName, '\0');
            StringHelpers.WriteStringBuffer(writer, 32, crewGenerator, '\0');
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

        public ActorWardrobe(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            cameraPos = reader.ReadBytes(72);
            doorName = new string(reader.ReadChars(32)).TrimEnd('\0');
            soundName = new string(reader.ReadChars(32)).TrimEnd('\0');
            humanAnimationName = new string(reader.ReadChars(32)).TrimEnd('\0');
            textID = reader.ReadInt32();
            unk0 = reader.ReadSingle();
            testPrimitive = reader.ReadInt32();
            posData = reader.ReadBytes(28);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(cameraPos);
            StringHelpers.WriteStringBuffer(writer, 32, doorName, '\0');
            StringHelpers.WriteStringBuffer(writer, 32, soundName, '\0');
            StringHelpers.WriteStringBuffer(writer, 32, humanAnimationName, '\0');
            writer.Write(textID);
            writer.Write(unk0);
            writer.Write(testPrimitive);
            writer.Write(posData);
        }
    }

    public class ActorDoor : IActorExtraDataInterface
    {
        byte[] data;
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

        public byte[] Data { get { return data; } set { data = value; } }
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

        public ActorDoor(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            data = reader.ReadBytes(240);
            disabled = reader.ReadByte();
            closesPortals = reader.ReadByte();
            physicalOpen = reader.ReadByte();
            physicalClose = reader.ReadByte();
            closedMagnitude = reader.ReadSingle();
            automaticOpen = reader.ReadByte();
            automaticClose = reader.ReadByte();
            automaticCloseDelay = reader.ReadInt16();
            unk0 = reader.ReadInt32();
            unk1 = reader.ReadSingle();
            locked = reader.ReadInt32();
            lockedSoundID = reader.ReadInt32();
            lockTime = reader.ReadInt32();
            unlockTime = reader.ReadInt32();
            lockSoundID = reader.ReadInt32();
            unlockSoundID = reader.ReadInt32();
            lockpickSoundID = reader.ReadInt32();
            lockpickClicksCount = reader.ReadInt32();
            openingSoundID = reader.ReadInt32();
            closingSoundID = reader.ReadInt32();
            closingSound2ID = reader.ReadInt32();
            closingSound3ID = reader.ReadInt32();
            closingSoundMagnitude = reader.ReadInt32();
            closingSound2Magnitude = reader.ReadInt32();
            closingSound3Magnitude = reader.ReadInt32();
            movingSoundID = reader.ReadInt32();
            kickingSoundID = reader.ReadInt32();
            kickable = reader.ReadInt32();
            closedSoundID = reader.ReadInt32();
            openHint = reader.ReadInt32();
            closeHint = reader.ReadInt32();
            lockpickHint = reader.ReadInt32();
            kickHint = reader.ReadInt32();
            actorActionsEnabled = reader.ReadInt32();
            pushAwayMode = reader.ReadInt32();
            pushAwayReaction = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(data);
            writer.Write(disabled);
            writer.Write(closesPortals);
            writer.Write(physicalOpen);
            writer.Write(physicalClose);
            writer.Write(closedMagnitude);
            writer.Write(automaticOpen);
            writer.Write(automaticClose);
            writer.Write(automaticCloseDelay);
            writer.Write(unk0);
            writer.Write(unk1);
            writer.Write(locked);
            writer.Write(lockedSoundID);
            writer.Write(lockTime);
            writer.Write(unlockTime);
            writer.Write(lockSoundID);
            writer.Write(unlockSoundID);
            writer.Write(lockpickSoundID);
            writer.Write(lockpickClicksCount);
            writer.Write(openingSoundID);
            writer.Write(closingSoundID);
            writer.Write(closingSound2ID);
            writer.Write(closingSound3ID);
            writer.Write(closingSoundMagnitude);
            writer.Write(closingSound2Magnitude);
            writer.Write(closingSound3Magnitude);
            writer.Write(movingSoundID);
            writer.Write(kickingSoundID);
            writer.Write(kickable);
            writer.Write(closedSoundID);
            writer.Write(openHint);
            writer.Write(closeHint);
            writer.Write(lockpickHint);
            writer.Write(kickHint);
            writer.Write(actorActionsEnabled);
            writer.Write(pushAwayMode);
            writer.Write(pushAwayReaction);
        }
    }

    public class ActorSoundEntity : IActorExtraDataInterface
    {
        int type;
        int behaviourType;
        float volume;
        float pitch;
        string file;

        public ActorSoundEntity(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            type = reader.ReadInt32();
            behaviourType = reader.ReadInt32();
            volume = reader.ReadSingle();
            pitch = reader.ReadSingle();  
            file = StringHelpers.ReadString(reader);
            ActorSoundEntityFlags tempType = (ActorSoundEntityFlags)type;

            Console.WriteLine("Sound: Name {0}", file);
            Console.WriteLine("Sound: Has Flag PlayInWinter {0}", tempType.HasFlag(ActorSoundEntityFlags.PlayInWinter));
            Console.WriteLine("Sound: Has Flag Loop {0}", tempType.HasFlag(ActorSoundEntityFlags.Loop));
            Console.WriteLine("Sound: Has Flag UseAdvancedScene {0}", tempType.HasFlag(ActorSoundEntityFlags.UseAdvancedScene));
            Console.WriteLine("Sound: Has Flag SectorRestricted {0}", tempType.HasFlag(ActorSoundEntityFlags.SectorRestricted));
            Console.WriteLine("Sound: Has Flag PlayInDay {0}", tempType.HasFlag(ActorSoundEntityFlags.PlayInDay));
            Console.WriteLine("Sound: Has Flag PlayInNight {0}", tempType.HasFlag(ActorSoundEntityFlags.PlayInNight));
            Console.WriteLine("Sound: Has Flag PlayInRain {0}", tempType.HasFlag(ActorSoundEntityFlags.PlayInRain));
            Console.WriteLine("Sound: Has Flag PlayInSummer {0}", tempType.HasFlag(ActorSoundEntityFlags.PlayInSummer));
            //if(behaviourType == 20)
            //{
            //    throw new NotImplementedException();
            //}

            //if(behaviourType > 19)
            //{
            //    if(behaviourType == 30)
            //    {
            //        throw new NotImplementedException();
            //    }

            //    if(behaviourType == 20)
            //    {
            //        throw new NotImplementedException();
            //    }
            //}

            //if(behaviourType != 10)
            //{
            //    if(behaviourType == 15)
            //    {
            //        throw new NotImplementedException();
            //    }
            //}
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class ActorSpikeStrip : IActorExtraDataInterface
    {
        float length;

        public float Length {
            get { return length; }
            set { length = value; }
        }

        public ActorSpikeStrip(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            length = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(length);
        }
    }


    public class ActorAircraft : IActorExtraDataInterface
    {
        int soundMotorID;

        public int SoundMotorID {
            get { return soundMotorID; }
            set { soundMotorID = value; }
        }

        public ActorAircraft(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            soundMotorID = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(soundMotorID);
        }
    }

    public class ActorPinup : IActorExtraDataInterface
    {
        int pinupNum;

        public int PinupNum {
            get { return pinupNum; }
            set { pinupNum = value; }
        }

        public ActorPinup(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            pinupNum = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(pinupNum);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.Pinup, pinupNum);
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

        public ActorScriptEntity(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            scriptName = new string(reader.ReadChars(96)).TrimEnd('\0');
            unk01 = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(scriptName.ToCharArray());
            for (int i = 0; i != 96 - scriptName.Length; i++)
                writer.Write('\0');
            writer.Write(unk01);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ActorTypes.ScriptEntity, scriptName);
        }
    }
}
