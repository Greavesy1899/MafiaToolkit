using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Prop
    {
        public int ID { get; set; }
        public uint Value { get; set; }
        public Prop(int iID)
        {
            ID = iID;
        }

        public Prop()
        {
            ID = 0;
            Value = 0;
        }
    }
}
