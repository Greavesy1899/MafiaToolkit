using System;
using System.IO;
using Utils.StringHelpers;

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
            Enum.TryParse(StringHelpers.ReadString32(reader), out emitterType);
            radius = reader.ReadSingle();
            hemisphere = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            string type = Enum.GetName(typeof(EmitterType), emitterType);
            StringHelpers.WriteString32(writer, type);
            writer.Write(radius);
            writer.Write(hemisphere);
        }

        public override string ToString()
        {
            return "EmitterGeomSphereParams";
        }
    }
}
