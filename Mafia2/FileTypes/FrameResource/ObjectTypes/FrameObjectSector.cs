using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    public class FrameObjectSector : FrameObjectJoint
    {

        int unk_08_int;
        int unk_09_int;
        float[] unk_10_floats;
        BoundingBox unk_11_bounds;
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
            get { return unk_11_bounds ; }
            set { unk_11_bounds = value; }
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

        public FrameObjectSector() : base()
        {

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

            unk_11_bounds = new BoundingBox(reader);
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

            unk_11_bounds.WriteToFile(writer);
            unk_13_vector3.WriteToFile(writer);
            unk_14_vector3.WriteToFile(writer);
            unk_15_hash.WriteToFile(writer);
        }
    }
}
