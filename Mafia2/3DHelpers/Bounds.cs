using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Bounds
    {
        Vector3 min;
        Vector3 max;

        public Vector3 Min {
            get { return min; }
            set { min = value; }
        }

        public Vector3 Max {
            get { return max; }
            set { max = value; }
        }

        public Bounds(BinaryReader reader)
        {
            min = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            max = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
        public Bounds()
        {
            min = new Vector3(0);
            max = new Vector3(0);
        }

        public Bounds(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            min.WriteToFile(writer);
            max.WriteToFile(writer);
        }
    }
}

