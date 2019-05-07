using System.Collections.Generic;
using System.IO;
using Utils.Models;
using SharpDX;

namespace Utils.SharpDXExtensions
{
    public static class BoundingBoxExtenders
    {
        public static BoundingBox ReadFromFile(BinaryReader reader)
        {
            BoundingBox bbox = new BoundingBox();
            bbox.Minimum = Vector3Extenders.ReadFromFile(reader);
            bbox.Maximum = Vector3Extenders.ReadFromFile(reader);
            return bbox;
        }

        public static void WriteToFile(this BoundingBox bbox, BinaryWriter writer)
        {
            bbox.Minimum.WriteToFile(writer);
            bbox.Maximum.WriteToFile(writer);
        }

        public static BoundingBox CalculateBounds(List<Vertex[]> data)
        {
            BoundingBox bbox = new BoundingBox
            {
                Minimum = new Vector3(0),
                Maximum = new Vector3(0)
            };

            for (int p = 0; p != data.Count; p++)
            {
                for (int i = 0; i != data[p].Length; i++)
                {
                    Vector3 pos = data[p][i].Position;

                    if (pos.X < bbox.Minimum.X)
                        bbox.Minimum.X = pos.X;

                    if (pos.X > bbox.Maximum.X)
                        bbox.Maximum.X = pos.X;

                    if (pos.Y < bbox.Minimum.Y)
                        bbox.Minimum.Y = pos.Y;

                    if (pos.Y > bbox.Maximum.Y)
                        bbox.Maximum.Y = pos.Y;

                    if (pos.Z < bbox.Minimum.Z)
                        bbox.Minimum.Z = pos.Z;

                    if (pos.Z > bbox.Maximum.Z)
                        bbox.Maximum.Z = pos.Z;
                }
            }
            return bbox;
        }

        public static BoundingBox CalculateBounds(Vector3[] data)
        {
            BoundingBox bbox = new BoundingBox
            {
                Minimum = new Vector3(0),
                Maximum = new Vector3(0)
            };

            for (int i = 0; i != data.Length; i++)
            {
                Vector3 pos = data[i];

                if (pos.X < bbox.Minimum.X)
                    bbox.Minimum.X = pos.X;

                if (pos.X > bbox.Maximum.X)
                    bbox.Maximum.X = pos.X;

                if (pos.Y < bbox.Minimum.Y)
                    bbox.Minimum.Y = pos.Y;

                if (pos.Y > bbox.Maximum.Y)
                    bbox.Maximum.Y = pos.Y;

                if (pos.Z < bbox.Minimum.Z)
                    bbox.Minimum.Z = pos.Z;

                if (pos.Z > bbox.Maximum.Z)
                    bbox.Maximum.Z = pos.Z;
            }
            return bbox;
        }
    }

    public static class BoundingSphereExtenders
    {
        public static BoundingSphere ReadFromFile(BinaryReader reader)
        {
            BoundingSphere bSphere = new BoundingSphere
            {
                Center = Vector3Extenders.ReadFromFile(reader),
                Radius = reader.ReadSingle()
            };
            return bSphere;
        }

        public static void WriteToFile(this BoundingSphere bSphere, BinaryWriter writer)
        {
            bSphere.Center.WriteToFile(writer);
            writer.Write(bSphere.Radius);
        }

        public static BoundingSphere CalculateFromBBox(BoundingBox bBox)
        {
            BoundingSphere bSphere = new BoundingSphere();
            bSphere.Center = new Vector3((bBox.Minimum.X + bBox.Maximum.X) / 2.0f, (bBox.Minimum.Y + bBox.Maximum.Y) / 2.0f, (bBox.Minimum.Z + bBox.Maximum.Z) / 2.0f);
            float m_Radius = 0.0f;
            Vector3.Distance(ref bSphere.Center, ref bBox.Maximum, out m_Radius);
            bSphere.Radius = m_Radius;
            return bSphere;
        }
    }
}

