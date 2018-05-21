using System.IO;

namespace Mafia2 {
    public class FrameObjectSector : FrameObjectJoint {

        int unk_08_int;
        int unk_09_int;
        float[] unk_10_floats;
        Bounds unk_11_bounds;
        Vector3 unk_13_vector3;
        Vector3 unk_14_vector3;
        Hash unk_15_hash;

        public FrameObjectSector(BinaryReader reader) : base() {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader) {
            base.ReadFromFile(reader);
            unk_08_int = reader.ReadInt32();
            unk_09_int = reader.ReadInt32();

            unk_10_floats = new float[unk_09_int * 4];
            for(int i = 0; i != unk_10_floats.Length; i++) {
                unk_10_floats[i] = reader.ReadSingle();
            }

            unk_11_bounds = new Bounds(reader);
            unk_13_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            unk_14_vector3 = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            unk_15_hash = new Hash(reader);

        }
    }
}
