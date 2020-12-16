using System.Diagnostics;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeScriptWrapper : AnimEntityWrapper
    {
        public AeScriptWrapper() : base()
        {
            AnimEntityData = new AeScriptData();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeScript;
        }
    }

    public class AeScriptData : AeBaseData
    {
        public string ScriptName { get; set; }
        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Debug.Assert(stream.Position != stream.Length, "I've read the parent class data, although i've hit the eof!");

            ScriptName = stream.ReadString16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.WriteString16(ScriptName, isBigEndian);
        }
    }
}
