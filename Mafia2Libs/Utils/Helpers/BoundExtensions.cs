using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Vortice.Mathematics;
using Utils.Models;

namespace Utils.VorticeUtils
{
    public static class BoundingBoxExtenders
    {
        public static float GetWidth(this BoundingBox BBox)
        {
            return (BBox.Maximum.X - BBox.Minimum.X);
        }

        public static float GetHeight(this BoundingBox BBox)
        {
            return (BBox.Maximum.Y - BBox.Minimum.Y);
        }

        public static float GetDepth(this BoundingBox BBox)
        {
            return (BBox.Maximum.Z - BBox.Minimum.Z);
        }

        public static void SetMinimum(ref this BoundingBox BBox, Vector3 min)
        {
            BBox = new BoundingBox(min, BBox.Maximum);
        }

        public static void SetMaximum(ref this BoundingBox BBox, Vector3 max)
        {
            BBox = new BoundingBox(BBox.Minimum, max);
        }

        public static BoundingBox ReadFromFile(BinaryReader reader)
        {
            Vector3 Min = Vector3Utils.ReadFromFile(reader);
            Vector3 Max = Vector3Utils.ReadFromFile(reader);
            BoundingBox bbox = new BoundingBox(Min, Max);

            return bbox;
        }

        public static BoundingBox ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            Vector3 Min = Vector3Utils.ReadFromFile(reader, isBigEndian);
            Vector3 Max = Vector3Utils.ReadFromFile(reader, isBigEndian);
            BoundingBox bbox = new BoundingBox(Min, Max);

            return bbox;
        }

        public static void WriteToFile(this BoundingBox bbox, BinaryWriter writer)
        {
            bbox.Minimum.WriteToFile(writer);
            bbox.Maximum.WriteToFile(writer);
        }

        public static void WriteToFile(this BoundingBox bbox, MemoryStream writer, bool isBigEndian)
        {
            bbox.Minimum.WriteToFile(writer, isBigEndian);
            bbox.Maximum.WriteToFile(writer, isBigEndian);
        }

        public static BoundingBox Swap(this BoundingBox box)
        {
            box = new BoundingBox(Vector3Utils.Swap(box.Minimum), Vector3Utils.Swap(box.Maximum));
            return box;
        }

        public static BoundingBox CalculateBounds(List<Vertex[]> data)
        {
            Vector3 Min = Vector3.Zero;
            Vector3 Max = Vector3.Zero;

            for (int p = 0; p != data.Count; p++)
            {
                for (int i = 0; i != data[p].Length; i++)
                {
                    Vector3 pos = data[p][i].Position;

                    if (pos.X < Min.X)
                    {
                        Min.X = pos.X;
                    }

                    if (pos.X > Max.X)
                    {
                        Max.X = pos.X;
                    }

                    if (pos.Y < Min.Y)
                    {
                        Min.Y = pos.Y;
                    }

                    if (pos.Y > Max.Y)
                    {
                        Max.Y = pos.Y;
                    }

                    if (pos.Z < Min.Z)
                    {
                        Min.Z = pos.Z;
                    }

                    if (pos.Z > Max.Z)
                    {
                        Max.Z = pos.Z;
                    }
                }
            }

            BoundingBox bbox = new BoundingBox(Min, Max);
            return bbox;
        }
    }
}
