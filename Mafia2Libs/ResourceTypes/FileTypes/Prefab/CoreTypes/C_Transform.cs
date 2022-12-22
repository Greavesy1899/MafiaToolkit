using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class C_Transform
    {
        public C_Vector3 Translation { get; set; }
        public C_Vector3 Row0 { get; set; }
        public C_Vector3 Row1 { get; set; }
        public C_Vector3 Row2 { get; set; }

        public C_Transform()
        {
            Row0 = new C_Vector3(1.0f, 0.0f, 0.0f);
            Row1 = new C_Vector3(0.0f, 1.0f, 0.0f);
            Row2 = new C_Vector3(0.0f, 0.0f, 1.0f);
            Translation = new C_Vector3();
        }

        public void Load(BitStream MemStream)
        {
            Translation.Load(MemStream);
            Row0.Load(MemStream);
            Row1.Load(MemStream);
            Row2.Load(MemStream);
        }

        public void Save(BitStream MemStream)
        {
            Translation.Save(MemStream);
            Row0.Save(MemStream);
            Row1.Save(MemStream);
            Row2.Save(MemStream);
        }
    }
}
