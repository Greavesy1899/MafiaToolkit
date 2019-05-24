using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectSector : FrameObjectJoint
    {
        int unk_08_int;
        int unk_09_int;
        float[] unk_10_floats;
        BoundingBox bounds;
        Vector3 unk_13_vector3;
        Vector3 unk_14_vector3;
        Hash sectorName;

        public int Unk08 {
            get { return unk_08_int; }
            set { unk_08_int = value; }
        }
        public int Unk09 {
            get { return unk_09_int; }
            set { unk_09_int = value; }
        }
        public float[] Unk10 {
            get { return unk_10_floats; }
            set { unk_10_floats = value; }
        }
        public BoundingBox Bounds {
            get { return bounds ; }
            set { bounds = value; }
        }
        public Vector3 Unk13 {
            get { return unk_13_vector3; }
            set { unk_13_vector3 = value; }
        }
        public Vector3 Unk14 {
            get { return unk_14_vector3; }
            set { unk_14_vector3 = value; }
        }
        public Hash SectorName {
            get { return sectorName; }
            set { sectorName = value; }
        }

        public float BoundsMinimumX {
            get { return bounds.Minimum.X; }
            set { bounds.Minimum.X = value; }
        }
        public float BoundsMinimumY {
            get { return bounds.Minimum.Y; }
            set { bounds.Minimum.Y = value; }
        }
        public float BoundsMinimumZ {
            get { return bounds.Minimum.Z; }
            set { bounds.Minimum.Z = value; }
        }
        public float BoundsMaximumX {
            get { return bounds.Maximum.X; }
            set { bounds.Maximum.X = value; }
        }
        public float BoundsMaximumY {
            get { return bounds.Maximum.Y; }
            set { bounds.Maximum.Y = value; }
        }
        public float BoundsMaximumZ {
            get { return bounds.Maximum.Z; }
            set { bounds.Maximum.Z = value; }
        }

        public FrameObjectSector() : base()
        {
            bounds = new BoundingBox();
            unk_13_vector3 = new Vector3(0);
            unk_14_vector3 = new Vector3(0);
            sectorName = new Hash();
        }

        public FrameObjectSector(FrameObjectSector other) : base(other)
        {
            bounds = other.bounds;
            unk_13_vector3 = other.unk_13_vector3;
            unk_14_vector3 = other.unk_14_vector3;
            sectorName = new Hash(sectorName.String);
        }

        public FrameObjectSector(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk_08_int = reader.ReadInt32();
            unk_09_int = reader.ReadInt32();

            unk_10_floats = new float[unk_09_int * 4];
            for (int i = 0; i != unk_10_floats.Length; i++)
            {
                unk_10_floats[i] = reader.ReadSingle();
            }

            bounds = BoundingBoxExtenders.ReadFromFile(reader);
            unk_13_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            unk_14_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            sectorName = new Hash(reader);

        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk_08_int);
            writer.Write(unk_09_int);

            for(int i = 0; i != unk_10_floats.Length; i++)
            {
                writer.Write(unk_10_floats[i]);
            }

            bounds.WriteToFile(writer);
            unk_13_vector3.WriteToFile(writer);
            unk_14_vector3.WriteToFile(writer);
            sectorName.WriteToFile(writer);
        }
    }
}
