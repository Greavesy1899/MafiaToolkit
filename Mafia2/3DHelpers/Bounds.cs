using System.IO;

namespace Mafia2 {
    public struct Bounds {
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

        public Bounds(BinaryReader reader) {
            min = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            max = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}

