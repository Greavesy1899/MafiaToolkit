using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Bounds
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        /// <summary>
        /// Construct bounds by reading data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public Bounds(BinaryReader reader)
        {
            ReadToFile(reader);
        }

        /// <summary>
        /// Construct bounds with data set at 0.
        /// </summary>
        public Bounds() { }

        /// <summary>
        /// Construct bounds with passed vectors.
        /// </summary>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        public Bounds(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadToFile(BinaryReader reader)
        {
            Min = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            Max = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            Min.WriteToFile(writer);
            Max.WriteToFile(writer);
        }

        public override string ToString()
        {
            return $"{Min}, {Max}";
        }
    }
}

