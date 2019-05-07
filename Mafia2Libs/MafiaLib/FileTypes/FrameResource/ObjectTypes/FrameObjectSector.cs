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
        BoundingBox unkBounds;
        Vector3 unk_13_vector3;
        Vector3 unk_14_vector3;
        Hash unk_15_hash;

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
        public BoundingBox Unk11 {
            get { return unkBounds ; }
            set { unkBounds = value; }
        }
        public Vector3 Unk13 {
            get { return unk_13_vector3; }
            set { unk_13_vector3 = value; }
        }
        public Vector3 Unk14 {
            get { return unk_14_vector3; }
            set { unk_14_vector3 = value; }
        }
        public Hash Unk15 {
            get { return unk_15_hash; }
            set { unk_15_hash = value; }
        }

        public float BoundsMinimumX {
            get { return unkBounds.Minimum.X; }
            set { unkBounds.Minimum.X = value; }
        }
        public float BoundsMinimumY {
            get { return unkBounds.Minimum.Y; }
            set { unkBounds.Minimum.Y = value; }
        }
        public float BoundsMinimumZ {
            get { return unkBounds.Minimum.Z; }
            set { unkBounds.Minimum.Z = value; }
        }
        public float BoundsMaximumX {
            get { return unkBounds.Maximum.X; }
            set { unkBounds.Maximum.X = value; }
        }
        public float BoundsMaximumY {
            get { return unkBounds.Maximum.Y; }
            set { unkBounds.Maximum.Y = value; }
        }
        public float BoundsMaximumZ {
            get { return unkBounds.Maximum.Z; }
            set { unkBounds.Maximum.Z = value; }
        }

        public FrameObjectSector() : base()
        {
            unkBounds = new BoundingBox();
            unk_13_vector3 = new Vector3(0);
            unk_14_vector3 = new Vector3(0);
            unk_15_hash = new Hash();
        }

        public FrameObjectSector(FrameObjectSector other) : base(other)
        {
            unkBounds = other.unkBounds;
            unk_13_vector3 = other.unk_13_vector3;
            unk_14_vector3 = other.unk_14_vector3;
            unk_15_hash = new Hash(unk_15_hash.String);
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

            unkBounds = BoundingBoxExtenders.ReadFromFile(reader);
            unk_13_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            unk_14_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            unk_15_hash = new Hash(reader);

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

            unkBounds.WriteToFile(writer);
            unk_13_vector3.WriteToFile(writer);
            unk_14_vector3.WriteToFile(writer);
            unk_15_hash.WriteToFile(writer);
        }
    }
}
