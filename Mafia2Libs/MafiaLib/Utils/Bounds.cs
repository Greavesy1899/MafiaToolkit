using System.Collections.Generic;
using System.IO;
using SharpDX;

//updates since 16/01/19
//Converted to SharpDX classes. Added Extenders and removed my classes.

//TODO:
//When vectors are removed, I need to update readers and writers.

namespace Mafia2
{
    public static class BoundingBoxExtenders
    {
        public static BoundingBox ReadFromFile(BinaryReader reader)
        {
            BoundingBox bbox = new BoundingBox();
            bbox.Minimum.X = reader.ReadSingle();
            bbox.Minimum.Y = reader.ReadSingle();
            bbox.Minimum.Z = reader.ReadSingle();
            bbox.Maximum.X = reader.ReadSingle();
            bbox.Maximum.Y = reader.ReadSingle();
            bbox.Maximum.Z = reader.ReadSingle();
            return bbox;
        }

        public static void WriteToFile(this BoundingBox bbox, BinaryWriter writer)
        {
            writer.Write(bbox.Minimum.X);
            writer.Write(bbox.Minimum.Y);
            writer.Write(bbox.Minimum.Z);
            writer.Write(bbox.Maximum.X);
            writer.Write(bbox.Maximum.Y);
            writer.Write(bbox.Maximum.Z);
        }

        public static BoundingBox CalculateBounds(List<Vertex[]> data)
        {
            BoundingBox bbox = new BoundingBox();
            bbox.Minimum = new SharpDX.Vector3(0);
            bbox.Maximum = new SharpDX.Vector3(0);

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

        /// <summary>
        /// Calculate bounds from passed vertices.
        /// </summary>
        public static BoundingBox CalculateBounds(Vector3[] data)
        {
            BoundingBox bbox = new BoundingBox();
            bbox.Minimum = new SharpDX.Vector3(0);
            bbox.Maximum = new SharpDX.Vector3(0);

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
            BoundingSphere bSphere = new BoundingSphere();
            bSphere.Center.X = reader.ReadSingle();
            bSphere.Center.Y = reader.ReadSingle();
            bSphere.Center.Z = reader.ReadSingle();
            bSphere.Radius = reader.ReadSingle();
            return bSphere;
        }

        public static void WriteToFile(this BoundingSphere bSphere, BinaryWriter writer)
        {
            writer.Write(bSphere.Center.X);
            writer.Write(bSphere.Center.Y);
            writer.Write(bSphere.Center.Z);
            writer.Write(bSphere.Radius);
        }

        public static BoundingSphere CalculateFromBBox(BoundingBox bBox)
        {
            BoundingSphere bSphere = new BoundingSphere();
            bSphere.Center = new SharpDX.Vector3((bBox.Minimum.X + bBox.Maximum.X) / 2.0f, (bBox.Minimum.Y + bBox.Maximum.Y) / 2.0f, (bBox.Minimum.Z + bBox.Maximum.Z) / 2.0f);

            //find radius
            float m_Radius = 0.0f;
            SharpDX.Vector3.Distance(ref bSphere.Center, ref bBox.Maximum, out m_Radius);
            bSphere.Radius = m_Radius;
            return bSphere;
        }
    }
}

