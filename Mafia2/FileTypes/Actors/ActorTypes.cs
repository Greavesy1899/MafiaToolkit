using System;
using System.IO;

namespace Mafia2
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
            file = Functions.ReadString(reader);
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
