using System;
using System.IO;
using Mafia2;

namespace ApexSDK
{
    public class EmitterGeomSphere
    {
        private EmitterType emitterType;
        private float radius;
        private float hemisphere;

        public EmitterType EmitterType {
            get { return emitterType; }
            set { emitterType = value; }
        }
        public float Radius {
            get { return radius; }
            set { radius = value; }
        }
        public float Hemisphere {
            get { return hemisphere; }
            set { hemisphere = value; }
        }

        public EmitterGeomSphere(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Enum.TryParse(Functions.ReadString32(reader), out emitterType);
            radius = reader.ReadSingle();
            hemisphere = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("EmitterGeomSphereParams");
        }
    }
}
