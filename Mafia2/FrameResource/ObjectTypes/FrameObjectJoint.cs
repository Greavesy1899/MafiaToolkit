using System.IO;

namespace Mafia2 {
    public class FrameObjectJoint : FrameObjectBase {

        byte unknown_07_byte;
        unk_struct1[] unk_07_array;

        public byte Unk_07_Byte {
            get { return unknown_07_byte; }
            set { unknown_07_byte = value; }
        }
        public unk_struct1[] Unk_07_Array {
            get { return unk_07_array; }
            set { unk_07_array = value; }
        }

        public FrameObjectJoint() : base() {

        }

        public FrameObjectJoint(BinaryReader reader) : base() {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader) {
            base.ReadFromFile(reader);
            unknown_07_byte = reader.ReadByte();
            unk_07_array = new unk_struct1[unknown_07_byte];

            for(int i = 0; i != unknown_07_byte; i++) {
                unk_07_array[i] = new unk_struct1(reader);
            }
        }

        public override string ToString()
        {
            return string.Format("Frame Block");
        }

        public struct unk_struct1 {
            
            int unknown_01;
            Hash unknown_02_hash;
            Hash unknown_03_hash;

            public int Unk_01 {
                get { return unknown_01; }
                set { unknown_01 = value; }
            }
            public Hash Unk_02_Hash {
                get { return unknown_02_hash; }
                set { unknown_02_hash = value; }
            }
            public Hash Unk_03_Hash {
                get { return unknown_03_hash; }
                set { unknown_03_hash = value; }
            }

            public unk_struct1(BinaryReader reader) {
                unknown_01 = reader.ReadInt32();
                unknown_02_hash = new Hash(reader);
                unknown_03_hash = new Hash(reader);
            }
        }

    }

}
