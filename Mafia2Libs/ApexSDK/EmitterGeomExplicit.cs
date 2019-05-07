using System;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

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

            for(int i = 0; i != size; i++)
                position[i] = Vector3Extenders.ReadFromFile(reader);

            size = reader.ReadInt32();
            sphere = new Vector3[size];

            for (int i = 0; i != size; i++)
                sphere[i] = Vector3Extenders.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("EmitterGeomExplicitParams");
        }
    }
}
