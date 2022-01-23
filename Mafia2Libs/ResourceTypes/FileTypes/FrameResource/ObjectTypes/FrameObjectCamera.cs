using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCamera : FrameObjectJoint
    {
        int numLens;
        LensData[] lens;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LensData[] Lens {
            get { return lens; }
            set { lens = value; }
        }

        public FrameObjectCamera(FrameResource OwningResource) : base(OwningResource)
        {
            numLens = 0;
        }

        public FrameObjectCamera(FrameObjectCamera other) : base(other)
        {
            numLens = other.numLens;
            lens = other.lens;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            numLens = reader.ReadInt32(isBigEndian);

            lens = new LensData[numLens];

            for (int i = 0; i != numLens; i++)
                lens[i] = new LensData(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(numLens);

            if (numLens <= 0)
                return;

            lens = new LensData[numLens];

            for (int i = 0; i != numLens; i++)
                lens[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("Camera Block");
        }

        public class LensData
        {
            float[] unkFloats;
            HashName unkHash;

            public float[] UnkFloats {
                get { return unkFloats; }
                set { unkFloats = value; }
            }
            public HashName UnkHash {
                get { return unkHash; }
                set { unkHash = value; }
            }

            public LensData(MemoryStream reader, bool isBigEndian)
            {
                unkFloats = new float[5];

                for (int i = 0; i != 5; i++)
                    unkFloats[i] = reader.ReadSingle(isBigEndian);

                unkHash = new HashName(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                for (int i = 0; i != 5; i++)
                    writer.Write(unkFloats[i]);

                unkHash.WriteToFile(writer);
            }
        }
    }
}
