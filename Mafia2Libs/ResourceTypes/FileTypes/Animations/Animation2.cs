using System.IO;
using System.Numerics;

namespace ResourceTypes.Animation2
{
    public class Animation2Loader
    {
        private int animSetID; //usually in the name. Different types. If not using skeleton, it's 0xFFFF
        private byte version; //Potentially Version 2, Because why not, it is Animation2. :)
        private int magic; //IDA says 0xFA5612BC
        private short unk0;
        private short unk1;
        private Vector4 unk2;
        private Vector3 unk3;
        private Vector3 unk4;
        private ulong hash;
        private byte unk5;
        private float unk6;
        private short unk7;
        private byte unk8;
        private ulong boneID;
        private byte unk9;


        //private TransformMatrix matrix; //matrix of the root?
        //private ulong hash; //hash of the animation?
        //private byte unk0; //usually 0? right after hash.
        //private Half[] bonePos = new Half[3]; //potential bone positionings.
        //private byte unk1; //usually 3? right after the position.
        //private long traceBone; //Detected trace (1000), or 0
        //private byte unk2; //usually 1? after the traceBone, and before counts?
        //private short unkS1; //usually 0xFF?
        //private short unkS2; //count1? timeline count? (Number of frames?)
        //private short unkS3; //count2? Bone count.
        //private TransformMatrix boneMatrix; //matrix of the bone?

        public void ReadFromFile(BinaryReader reader)
        {
            animSetID = reader.ReadInt32();
            version = reader.ReadByte();
            magic = reader.ReadInt32();
            //matrix = new TransformMatrix(reader);
            //hash = reader.ReadUInt64();
            //unk0 = reader.ReadByte();
            //bonePos[0] = Half.ToHalf(reader.ReadBytes(2), 0);
            //bonePos[1] = Half.ToHalf(reader.ReadBytes(2), 0);
            //bonePos[2] = Half.ToHalf(reader.ReadBytes(2), 0);
            //unk1 = reader.ReadByte(); //usually 3
            //traceBone = reader.ReadInt64();

            ////if (traceBone != 0 || traceBone != 1000)
            ////    throw new FormatException("Error, traceBone isn't 0 or 1000");

            //unkS1 = reader.ReadInt16();
            //reader.ReadBytes(3); //usually 1 0 0;
            ////if unkS2 and unkS3 is 0, then thats EOF.
            //unkS2 = reader.ReadInt16(); //must be counts
            //unkS3 = reader.ReadInt16(); //must be counts
            ////boneMatrix = new TransformMatrix(reader);

            //long boneID = reader.ReadInt64();

        }
    }
}
