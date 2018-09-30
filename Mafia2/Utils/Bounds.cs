using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct BoundingBox
    {
        private Vector3 min;
        private Vector3 max;

        public Vector3 Min {
            get { return min; }
            set { min = value; }
        }
        public Vector3 Max {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// Construct bounds by reading data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public BoundingBox(BinaryReader reader)
        {
            min = new Vector3(0);
            max = new Vector3(0);
            ReadToFile(reader);
        }

        /// <summary>
        /// Construct bounds with passed vectors.
        /// </summary>
        /// <param name="min">Minimum vector.</param>
        /// <param name="max">Maximum vector.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadToFile(BinaryReader reader)
        {
            min = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            max = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            min.WriteToFile(writer);
            max.WriteToFile(writer);
        }

        /// <summary>
        /// Calculate bounds from passed vertexes.
        /// </summary>
        public void CalculateBounds(List<Vertex[]> data)
        {
            min = new Vector3(0);
            max = new Vector3(0);

            for (int p = 0; p != data.Count; p++)
            {
                for (int i = 0; i != data[p].Length; i++)
                {
                    Vector3 pos = data[p][i].Position;

                    if (pos.X < min.X)
                        min.X = pos.X;

                    if (pos.X > max.X)
                        max.X = pos.X;

                    if (pos.Y < min.Y)
                        min.Y = pos.Y;

                    if (pos.Y > max.Y)
                        max.Y = pos.Y;

                    if (pos.Z < min.Z)
                        min.Z = pos.Z;

                    if (pos.Z > max.Z)
                        max.Z = pos.Z;
                }
            }
        }

        /// <summary>
        /// Calculate bounds from passed vertices.
        /// </summary>
        public void CalculateBounds(Vector3[] data)
        {
            min = new Vector3(0);
            max = new Vector3(0);

            for (int i = 0; i != data.Length; i++)
            {
                Vector3 pos = data[i];

                if (pos.X < min.X)
                    min.X = pos.X;

                if (pos.X > max.X)
                    max.X = pos.X;

                if (pos.Y < min.Y)
                    min.Y = pos.Y;

                if (pos.Y > max.Y)
                    max.Y = pos.Y;

                if (pos.Z < min.Z)
                    min.Z = pos.Z;

                if (pos.Z > max.Z)
                    max.Z = pos.Z;
            }
        }

        public override string ToString()
        {
            return string.Format("Min: {0}, Max: {1}", min, max);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public struct BoundingSphere
    {
        private Vector3 center;
        private float radius;

        public Vector3 Center {
            get { return center; }
            set { center = value; }
        }
        public float Radius {
            get { return radius; }
            set { radius = value; }
        }

        public BoundingSphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadToFile(BinaryReader reader)
        {
            center = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            radius = reader.ReadSingle();
        }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            center.WriteToFile(writer);
            writer.Write(radius);
        }

        public void CreateFromBoundingBox(BoundingBox box)
        {
            // Find the center of the box.
            center = new Vector3((box.Min.X + box.Max.X) / 2.0f,
                                         (box.Min.Y + box.Max.Y) / 2.0f,
                                         (box.Min.Z + box.Max.Z) / 2.0f);

            // Find the distance between the center and one of the corners of the box.
            radius = Vector3.Distance(center, box.Max);
        }

        public override string ToString()
        {
            return string.Format("Center: {0}, Radius: {1}", center, radius);
        }
    }
}

