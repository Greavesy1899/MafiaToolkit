using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Actors
{
    public class ActorActionPointScript : IActorExtraDataInterface
    {
        public float WalkRange { get; set; }
        public string ScriptEntity { get; set; }
        public int NumActors { get; set; }
        public int Group { get; set; }
        public int ForceSpawnFlags { get; set; }
        public int ForceSpawnFilter { get; set; }
        public int CarSpawnFlags { get; set; }
        public bool CrewInside { get; set; }
        public bool Summer { get; set; }
        public bool Winter { get; set; }

        public ActorActionPointScript()
        {
            ScriptEntity = string.Empty;
        }

        public int GetSize()
        {
            return 228;
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            WalkRange = reader.ReadSingle(isBigEndian);
            ScriptEntity = reader.ReadStringBuffer(128);
            NumActors = reader.ReadInt32(isBigEndian);
            Group = reader.ReadInt32(isBigEndian);
            ForceSpawnFlags = reader.ReadInt32(isBigEndian);
            reader.ReadBytes(24);
            ForceSpawnFilter = reader.ReadInt32(isBigEndian);
            reader.ReadBytes(24);
            CarSpawnFlags = reader.ReadInt32(isBigEndian);
            reader.ReadBytes(24);
            CrewInside = reader.ReadBoolean();
            Summer = reader.ReadBoolean();
            Winter = reader.ReadBoolean();
            reader.ReadByte();
        }

        public void WriteToFile(MemoryStream writer, bool isBigEndian)
        {
            writer.Write(WalkRange, isBigEndian);
            writer.WriteStringBuffer(128, ScriptEntity);
            writer.Write(NumActors, isBigEndian);
            writer.Write(Group, isBigEndian);
            writer.Write(ForceSpawnFlags, isBigEndian);
            writer.Write(new byte[24]);
            writer.Write(ForceSpawnFilter, isBigEndian);
            writer.Write(new byte[24]);
            writer.Write(CarSpawnFlags, isBigEndian);
            writer.Write(new byte[24]);
            writer.Write(CrewInside);
            writer.Write(Summer);
            writer.Write(Winter);
            writer.Write(false);
        }
    }
}
