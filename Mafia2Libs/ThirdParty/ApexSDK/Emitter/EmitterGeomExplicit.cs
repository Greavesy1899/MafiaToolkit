using System;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ApexSDK
{
    public class EmitterGeomExplicit
    {
        private Vector3[] position;
        private Vector3[] sphere;

        public EmitterGeomExplicit(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            position = new Vector3[size];

            for (int i = 0; i < size; i++)
            {
                position[i] = Vector3Utils.ReadFromFile(reader);
            }

            size = reader.ReadInt32();
            sphere = new Vector3[size];

            for (int i = 0; i < size; i++)
            {
                sphere[i] = Vector3Utils.ReadFromFile(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(position.Length);
            for(int i = 0; i < position.Length; i++)
            {
                Vector3Utils.WriteToFile(position[i], writer);
            }
            writer.Write(sphere.Length);
            for (int i = 0; i < sphere.Length; i++)
            {
                Vector3Utils.WriteToFile(sphere[i], writer);
            }
        }

        public override string ToString()
        {
            return "EmitterGeomExplicitParams";
        }
    }
}
