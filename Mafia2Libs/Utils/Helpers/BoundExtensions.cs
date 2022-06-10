using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Vortice.Mathematics;
using Utils.Models;
using System;

namespace Utils.VorticeUtils
{
    public static class BoundingBoxExtenders
    {
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
            bbox.Min.WriteToFile(writer);
            bbox.Max.WriteToFile(writer);
        }

        public static void WriteToFile(this BoundingBox bbox, MemoryStream writer, bool isBigEndian)
        {
            bbox.Min.WriteToFile(writer, isBigEndian);
            bbox.Max.WriteToFile(writer, isBigEndian);
        }

        public static BoundingBox Swap(this BoundingBox box)
        {
            box = new BoundingBox(Vector3Utils.Swap(box.Min), Vector3Utils.Swap(box.Max));
            return box;
        }

        public static BoundingBox CalculateBounds(List<Vertex[]> data)
        {
            Vector3 Min = new Vector3(1.0f / float.MinValue);
            Vector3 Max = new Vector3(1.0f / float.MaxValue);

            for (int p = 0; p < data.Count; p++)
            {
                for (int i = 0; i < data[p].Length; i++)
                {
                    Vector3 pos = data[p][i].Position;
                    Min.X = MathF.Min(pos.X, Min.X);
                    Max.X = MathF.Max(pos.X, Max.X);
                    Min.Y = MathF.Min(pos.Y, Min.Y);
                    Max.Y = MathF.Max(pos.Y, Max.Y);
                    Min.Z = MathF.Min(pos.Z, Min.Z);
                    Max.Z = MathF.Max(pos.Z, Max.Z);
                }
            }

            return new BoundingBox(Min, Max);
        }
    }
}
