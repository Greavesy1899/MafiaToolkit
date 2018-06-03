using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mafia2
{
    public class FrameObjectLight : FrameObjectBase
    {
        int flags;
        float[] unkFloat1 = new float[7];
        int unk_int;
        float[] unkFloat2 = new float[5];
        byte unk_byte1;
        float[] unkFloat3 = new float[17];
        byte unk_byte2;
        float[] unkFloat4 = new float[5];
        Hash nameLight;
        int unk_int2;
        float[] unkFloat5 = new float[20];
        Hash[] names = new Hash[4];
        Vector3 unkVector1;
        Vector3 unkVector2;
        byte unk_byte3;
        Vector3 unkVector3;
        Vector3 unkVector4;
        Vector3 unkVector5;
        Vector3 unkVector6;

        public FrameObjectLight(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            flags = reader.ReadInt32();

            for (int i = 0; i < 7; i++)
                unkFloat1[i] = reader.ReadSingle();

            unk_int = reader.ReadInt32();

            for (int i = 0; i < 5; i++)
                unkFloat2[i] = reader.ReadSingle();

            unk_byte1 = reader.ReadByte();

            for (int i = 0; i < 17; i++)
                unkFloat3[i] = reader.ReadSingle();

            unk_byte2 = reader.ReadByte();

            for (int i = 0; i < 5; i++)
                unkFloat4[i] = reader.ReadSingle();

            if (flags == 127744)
                reader.ReadByte();

            nameLight = new Hash(reader);


            unk_int2 = reader.ReadInt32();

            for (int i = 0; i < 20; i++)
                unkFloat5[i] = reader.ReadSingle();

            if(flags != 127744)
                reader.ReadByte();

            for (int i = 0; i < 4; i++)
                names[i] = new Hash(reader);

            unkVector1 = new Vector3(reader);
            unkVector2 = new Vector3(reader);
            unk_byte3 = reader.ReadByte();
            unkVector3 = new Vector3(reader);
            unkVector4 = new Vector3(reader);
            unkVector5 = new Vector3(reader);
            unkVector6 = new Vector3(reader);

        }
    }
}
