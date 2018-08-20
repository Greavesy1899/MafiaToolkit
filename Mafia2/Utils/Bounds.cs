using System.Collections.Generic;
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

        /// <summary>
        /// Calculate bounds from passed vertexes.
        /// </summary>
        public void CalculateBounds(List<Vertex[]> data)
        {
            Min = new Vector3(0);
            Max = new Vector3(0);

            for (int p = 0; p != data.Count; p++)
            {
                for (int i = 0; i != data[p].Length; i++)
                {
                    Vector3 pos = data[p][i].Position;

                    if (pos.X < Min.X)
                        Min.X = pos.X;

                    if (pos.X > Max.X)
                        Max.X = pos.X;

                    if (pos.Y < Min.Y)
                        Min.Y = pos.Y;

                    if (pos.Y > Max.Y)
                        Max.Y = pos.Y;

                    if (pos.Z < Min.Z)
                        Min.Z = pos.Z;

                    if (pos.Z > Max.Z)
                        Max.Z = pos.Z;
                }
            }
        }

        /// <summary>
        /// Calculate bounds from passed vertices.
        /// </summary>
        public void CalculateBounds(Vector3[] data)
        {
            Min = new Vector3(0);
            Max = new Vector3(0);

            for (int i = 0; i != data.Length; i++)
            {
                Vector3 pos = data[i];

                if (pos.X < Min.X)
                    Min.X = pos.X;

                if (pos.X > Max.X)
                    Max.X = pos.X;

                if (pos.Y < Min.Y)
                    Min.Y = pos.Y;

                if (pos.Y > Max.Y)
                    Max.Y = pos.Y;

                if (pos.Z < Min.Z)
                    Min.Z = pos.Z;

                if (pos.Z > Max.Z)
                    Max.Z = pos.Z;
            }
        }

        public override string ToString()
        {
            return $"{Min}, {Max}";
        }
    }
}

