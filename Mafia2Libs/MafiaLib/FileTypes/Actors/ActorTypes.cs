using SharpDX;
using System;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Actors
{
    public interface IActorExtraDataInterface
    {
        void ReadFromFile(BinaryReader reader);
        void WriteToFile(BinaryWriter writer);

        int GetSize();
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

        public int GetSize()
        {
            return 240;
        }

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
        Vector3 unk03;
        Vector3 unk04;
        Vector3 unk05;
        Vector3 unk06;
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
        Vector3 unkVector1;
        Vector3 unkVector2;
        byte unk_byte3;
        Vector3 unkVector3;
        Vector3 unkVector4;
        Vector3 unkVector5;
        Vector3 unkVector6;
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
        public Vector3 Unk3 {
            get { return unk03; }
            set { unk03 = value; }
        }
        public Vector3 Unk4 {
            get { return unk04; }
            set { unk04 = value; }
        }
        public Vector3 Unk5 {
            get { return unk05; }
            set { unk05 = value; }
        }
        public Vector3 Unk6 {
            get { return unk06; }
            set { unk06 = value; }
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
        public Vector3 UnkVector1 {
            get { return unkVector1; }
            set { unkVector1 = value; }
        }
        public Vector3 UnkVector2 {
            get { return unkVector2; }
            set { unkVector2 = value; }
        }
        public byte UnkByte3 {
            get { return unk_byte3; }
            set { unk_byte3 = value; }
        }
        public Vector3 UnkVector3 {
            get { return unkVector3; }
            set { unkVector3 = value; }
        }
        public Vector3 UnkVector4 {
            get { return unkVector4; }
            set { unkVector4 = value; }
        }
        public Vector3 UnkVector5 {
            get { return unkVector5; }
            set { unkVector5 = value; }
        }
        public Vector3 UnkVector6 {
            get { return unkVector6; }
            set { unkVector6 = value; }
        }
        public int Instanced {
            get { return instanced; }
            set { instanced = value; }
        }
        public int Type {
            get { return type; }
            set { type = value; }
        }
        public ActorLight(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            size = reader.ReadInt32();
            if (size < 2305)
            {
                padding = reader.ReadBytes(9);
                unk01 = reader.ReadInt32();
                unk02 = reader.ReadByte();
                unk03 = Vector3Extenders.ReadFromFile(reader);
                unk04 = Vector3Extenders.ReadFromFile(reader);
                unk05 = Vector3Extenders.ReadFromFile(reader);
                unk06 = Vector3Extenders.ReadFromFile(reader);
                unk07 = reader.ReadInt32();
                unk08 = reader.ReadInt32();
                unk09 = reader.ReadInt32();
                count = reader.ReadByte();
                unk10 = reader.ReadInt32();

                frameLinks = new Hash[count];
                sceneLinks = new Hash[count];
                frameIdxLinks = new int[count];
                for (int i = 0; i < count; i++)
                {
                    sceneLinks[i] = new Hash(reader);
                    frameLinks[i] = new Hash(reader);
                    frameIdxLinks[i] = reader.ReadInt32();
                }

                //flags = reader.ReadInt32();

                for (int i = 0; i < 7; i++)
                    unkFloat1[i] = reader.ReadSingle();

                unk_int = reader.ReadInt32();

                for (int i = 0; i < 5; i++)
                    unkFloat2[i] = reader.ReadSingle();

                unk_byte1 = reader.ReadByte();

                for (int i = 0; i < 17; i++)
                    unkFloat3[i] = reader.ReadSingle();

                unk_byte2 = reader.ReadByte();

                for (int i = 0; i < 5; i++)
                    unkFloat4[i] = reader.ReadSingle();

                nameLight = new Hash(reader);

                unk_int2 = reader.ReadInt32();

                for (int i = 0; i < 20; i++)
                    unkFloat5[i] = reader.ReadSingle();

                for (int i = 0; i < 4; i++)
                    names[i] = new Hash(reader);

                unkVector1 = Vector3Extenders.ReadFromFile(reader);
                unkVector2 = Vector3Extenders.ReadFromFile(reader);
                unk_byte3 = reader.ReadByte();
                unkVector3 = Vector3Extenders.ReadFromFile(reader);
                unkVector4 = Vector3Extenders.ReadFromFile(reader);
                unkVector5 = Vector3Extenders.ReadFromFile(reader);
                unkVector6 = Vector3Extenders.ReadFromFile(reader);
            }
            int skip = 2304 - size;
            reader.BaseStream.Seek(skip, SeekOrigin.Current);
            instanced = reader.ReadInt32();
            type = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(size);
            writer.Write(this.padding);
            writer.Write(unk01);
            writer.Write(unk02);
            Vector3Extenders.WriteToFile(unk03, writer);
            Vector3Extenders.WriteToFile(unk04, writer);
            Vector3Extenders.WriteToFile(unk05, writer);
            Vector3Extenders.WriteToFile(unk06, writer);
            writer.Write(unk07);
            writer.Write(unk08);
            writer.Write(unk09);
            writer.Write(count);
            writer.Write(unk10);

            for (int i = 0; i < count; i++)
            {
                sceneLinks[i].WriteToFile(writer);
                frameLinks[i].WriteToFile(writer);
                writer.Write(frameIdxLinks[i]);
            }

            for (int i = 0; i < 7; i++)
                writer.Write(unkFloat1[i]);

            writer.Write(unk_int);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat2[i]);

            writer.Write(unk_byte1);

            for (int i = 0; i < 17; i++)
                writer.Write(unkFloat3[i]);

            writer.Write(unk_byte2);

            for (int i = 0; i < 5; i++)
                writer.Write(unkFloat4[i]);

            nameLight.WriteToFile(writer);

            writer.Write(unk_int2);

            for (int i = 0; i != 20; i++)
                writer.Write(unkFloat5[i]);

            for (int i = 0; i != 4; i++)
                names[i].WriteToFile(writer);

            unkVector1.WriteToFile(writer);
            unkVector2.WriteToFile(writer);
            writer.Write(unk_byte3);
            unkVector3.WriteToFile(writer);
            unkVector4.WriteToFile(writer);
            unkVector5.WriteToFile(writer);
            unkVector6.WriteToFile(writer);
            byte[] padding = new byte[2304 - size];
            writer.Write(padding);
            writer.Write(instanced);
            writer.Write(type);
        }

        public int GetSize()
        {
            return 2316;
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

        public int GetSize()
        {
            return 364;
        }
    }

    public class ActorSoundEntity : IActorExtraDataInterface
    {
        ActorSoundEntityBehaviourFlags behFlags;
        int type;
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
        float monoDistance;
        int curveID;
        float innerAngle;
        float outerAngle;
        float outerVolume;

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ActorSoundEntityBehaviourFlags BehaviourFlags {
            get { return behFlags; }
            set { behFlags = value; }
        }
        public int Type {
            get { return type; }
            set { type = value; }
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
        public float RandomGroupPauseMax {
            get { return randomGroupPauseMax; }
            set { randomGroupPauseMax = value; }
        }
        public float RandomGroupPauseMin {
            get { return randomGroupPauseMin; }
            set { randomGroupPauseMin = value; }
        }
        public float RandomPauseMin {
            get { return randomPauseMin; }
            set { randomPauseMin = value; }
        }
        public float RandomPauseMax {
            get { return randomPauseMax; }
            set { randomPauseMax = value; }
        }
        public int RandomGroupSoundsMax {
            get { return randomGroupSoundsMax; }
            set { randomGroupSoundsMax = value; }
        }
        public int RandomGroupSoundsMin {
            get { return randomGroupSoundsMin; }
            set { randomGroupSoundsMin = value; }
        }
        public float RandomVolumeMin {
            get { return randomVolumeMin; }
            set { randomVolumeMin = value; }
        }
        public float RandomVolumeMax {
            get { return randomVolumeMax; }
            set { randomVolumeMax = value; }
        }
        public float RandomPitchMin {
            get { return randomPitchMin; }
            set { randomPitchMin = value; }
        }
        public float RandomPitchMax {
            get { return randomPitchMax; }
            set { randomPitchMax = value; }
        }
        public float RandomPosRangeX {
            get { return randomPosRangeX; }
            set { randomPosRangeX = value; }
        }
        public float RandomPosRangeY {
            get { return randomPosRangeY; }
            set { randomPosRangeY = value; }
        }
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
        public float Near {
            get { return near; }
            set { near = value; }
        }
        public float Far {
            get { return far; }
            set { far = value; }
        }
        public float MonoDistance {
            get { return monoDistance; }
            set { monoDistance = value; }
        }
        public int CurveID {
            get { return curveID; }
            set { curveID = value; }
        }
        public float InnerAngle {
            get { return innerAngle; }
            set { innerAngle = value; }
        }
        public float OuterAngle {
            get { return outerAngle; }
            set { outerAngle = value; }
        }
        public float OuterVolume {
            get { return outerVolume; }
            set { outerVolume = value; }
        }

        public ActorSoundEntity(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public int GetSize()
        {
            return 592;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            behFlags = (ActorSoundEntityBehaviourFlags)reader.ReadInt32();
            type = reader.ReadInt32();
            behaviourType = reader.ReadInt32();
            volume = reader.ReadSingle();
            pitch = reader.ReadSingle();
            file = new string(reader.ReadChars(80)).TrimEnd('\0');
            ActorSoundEntityBehaviourFlags tempType = (ActorSoundEntityBehaviourFlags)type;
            long position = reader.BaseStream.Position;
            if(behaviourType != 20)
            {
                int seek = 0x21C - 100;
                reader.BaseStream.Seek(seek, SeekOrigin.Current);
                randomPauseMin = reader.ReadSingle();
                randomPauseMax = reader.ReadSingle();
                randomGroupPauseMin = reader.ReadSingle();
                randomGroupPauseMax = reader.ReadSingle();
                randomGroupSoundsMin = reader.ReadInt32();
                randomGroupSoundsMax = reader.ReadInt32();
                randomVolumeMin = reader.ReadSingle();
                randomVolumeMax = reader.ReadSingle();
                randomPitchMin = reader.ReadSingle();
                randomPitchMax = reader.ReadSingle();
                randomPosRangeX = reader.ReadSingle();
                randomPosRangeY = reader.ReadSingle();
                randomPosRangeZ = reader.ReadSingle();
                reader.BaseStream.Seek(position, SeekOrigin.Begin);

                seek = 0x84 - 100;
                reader.BaseStream.Seek(seek, SeekOrigin.Current);
                playFlags = (ActorSoundEntityPlayType)reader.ReadByte();
                randomWaves = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    randomWaves[i] = new string(reader.ReadChars(80)).TrimEnd('\0');
                    reader.ReadByte();
                }
            }
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            reader.ReadInt32();
            switch (type)
            {
                case 20:
                    near = reader.ReadSingle();
                    far = reader.ReadSingle();
                    monoDistance = reader.ReadSingle();
                    break;
                case 15:
                    near = reader.ReadSingle();
                    far = reader.ReadSingle();
                    curveID = reader.ReadInt32();
                    break;
                case 30:
                    near = reader.ReadSingle();
                    far = reader.ReadSingle();
                    curveID = reader.ReadInt32();
                    innerAngle = reader.ReadSingle();
                    outerAngle = reader.ReadSingle();
                    outerVolume = reader.ReadSingle();
                    break;
                case 10:
                    break;
                default:
                    break;
            }
            reader.BaseStream.Seek(position+492, SeekOrigin.Begin);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(new byte[592]);
            writer.Seek(-592, SeekOrigin.Current);
            writer.Write((int)behFlags);
            writer.Write(type);
            writer.Write(behaviourType);
            writer.Write(volume);
            writer.Write(pitch);
            StringHelpers.WriteStringBuffer(writer, 80, file);
            long position = writer.BaseStream.Position;
            if (behaviourType != 20)
            {
                int seek = 0x21C + 100;
                writer.BaseStream.Seek(seek, SeekOrigin.Current);
                writer.Write(randomPauseMin);
                writer.Write(randomPauseMax);
                writer.Write(randomGroupPauseMin);
                writer.Write(randomGroupPauseMax);
                writer.Write(randomGroupSoundsMin);
                writer.Write(randomGroupSoundsMax);
                writer.Write(randomVolumeMin);
                writer.Write(randomVolumeMax);
                writer.Write(randomPitchMin);
                writer.Write(randomPitchMax);
                writer.Write(randomPosRangeX);
                writer.Write(randomPosRangeY);
                writer.Write(randomPosRangeZ);
                writer.BaseStream.Seek(position, SeekOrigin.Begin);

                seek = 0x84 - 100;
                writer.BaseStream.Seek(seek, SeekOrigin.Current);
                writer.Write((byte)playFlags);
                for (int i = 0; i < 5; i++)
                {
                    StringHelpers.WriteStringBuffer(writer, 80, randomWaves[i]);
                    writer.Write((byte)0);
                }
            }
            writer.BaseStream.Seek(position, SeekOrigin.Begin);
            writer.Write(0);
            switch (type)
            {
                case 20:
                    writer.Write(near);
                    writer.Write(far);
                    writer.Write(monoDistance);
                    break;
                case 15:
                    writer.Write(near);
                    writer.Write(far);
                    writer.Write(curveID);
                    break;
                case 30:
                    writer.Write(near);
                    writer.Write(far);
                    writer.Write(curveID);
                    writer.Write(innerAngle);
                    writer.Write(outerAngle);
                    writer.Write(outerVolume);
                    break;
                case 10:
                    break;
                default:
                    break;
            }
            writer.BaseStream.Seek(position+492, SeekOrigin.Begin);
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

        public ActorItem(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public int GetSize()
        {
            return 152;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            reader.ReadInt16();
            flags = (ActorItemFlags)reader.ReadUInt16();
            type = reader.ReadInt32();
            respawnTime = reader.ReadSingle();

            switch(type)
            {
                case 0:
                    type0Data = new Type0();
                    type0Data.TableID = reader.ReadInt32();
                    type0Data.TextID = reader.ReadInt32();
                    type0Data.Ammo = reader.ReadInt32();
                    type0Data.AmmoAUX = reader.ReadInt32();
                    reader.BaseStream.Seek(92, SeekOrigin.Current);
                    break;
                case 2:
                    scriptEvent = new ItemScript();
                    scriptEvent.TextID = reader.ReadInt32();
                    scriptEvent.SentTestAction = reader.ReadInt32();
                    reader.BaseStream.Seek(36, SeekOrigin.Current);
                    scriptEvent.ScriptEvent = StringHelpers.ReadStringBuffer(reader, 64);
                    scriptEvent.ScriptEvent.TrimEnd('\0');
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
                    reader.BaseStream.Seek(108, SeekOrigin.Current);
                    break;
                default:
                    throw new Exception();
                    break;
            }

            testPrimitive = reader.ReadInt32();
            range = reader.ReadSingle();
            unk1 = Vector3Extenders.ReadFromFile(reader);
            unk2 = Vector3Extenders.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((ushort)0);
            writer.Write((ushort)flags);
            writer.Write(type);
            writer.Write(respawnTime);

            switch (type)
            {
                case 0:
                    writer.Write(type0Data.TableID);
                    writer.Write(type0Data.TextID);
                    writer.Write(type0Data.Ammo);
                    writer.Write(type0Data.AmmoAUX);
                    writer.Write(new byte[92]);
                    break;
                case 2:
                    writer.Write(scriptEvent.TextID);
                    writer.Write(scriptEvent.SentTestAction);
                    writer.Write(new byte[36]);
                    StringHelpers.WriteStringBuffer(writer, 64, scriptEvent.ScriptEvent);
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

            writer.Write(testPrimitive);
            writer.Write(range);
            Vector3Extenders.WriteToFile(unk1, writer);
            Vector3Extenders.WriteToFile(unk2, writer);
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

        public int GetSize()
        {
            return 100;
        }
    }
}
