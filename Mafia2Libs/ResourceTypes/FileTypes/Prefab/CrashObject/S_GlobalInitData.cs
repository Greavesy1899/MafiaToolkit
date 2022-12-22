using BitStreams;
using System.ComponentModel;
using System.Diagnostics;
using Utils.Logging;

namespace ResourceTypes.Prefab.CrashObject
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class S_GlobalInitData
    {
        private uint PrefabVersion;

        public virtual void Load(BitStream MemStream)
        {
            // Should be 4
            PrefabVersion = MemStream.ReadUInt32();
            ToolkitAssert.Ensure(PrefabVersion == 4, "Prefab version should always equal 4.");
        }

        public virtual void Save(BitStream MemStream)
        {
            // Should store 4
            MemStream.WriteUInt32(4);
        }
    }
}
