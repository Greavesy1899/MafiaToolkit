using System.Diagnostics;
using System.IO;

namespace Mafia2
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int unk02;
        Float4[] unkFloats;
        Bounds unkBounds;

        //-1 means invert the float, eg: 25.459 would be -25.459
        //data[0] = top face			 1
        //data[1] = right face		 1
        //data[2] = front face		-1
        //data[3] = back face			 1
        //data[4] = bottom face		-1
        //data[5] = left face			-1

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public int Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public Float4[] UnkFloats {
            get { return unkFloats; }
            set { unkFloats = value; }
        }
        public Bounds Bounds {
            get { return unkBounds; }
            set { unkBounds = value; }
        }
        public FrameObjectArea(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            unk02 = reader.ReadInt32();
            unkFloats = new Float4[unk02];

            for (int i = 0; i != unkFloats.Length; i++)
                unkFloats[i] = new Float4(reader);

            unkBounds = new Bounds(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(unk02);

            for (int i = 0; i != unkFloats.Length; i++)
                unkFloats[i].WriteToFile(writer);

            unkBounds.WriteToFile(writer);
        }

        public void WriteARAFile(BinaryWriter writer)
        {
            Vector3[] verts = new Vector3[8];

            try
            {
                verts[0] = new Vector3(unkFloats[5].Data[3], unkFloats[4].Data[3], unkFloats[3].Data[3]);
                verts[1] = new Vector3(unkFloats[0].Data[3], unkFloats[4].Data[3], unkFloats[3].Data[3]);
                verts[2] = new Vector3(unkFloats[5].Data[3], unkFloats[1].Data[3], unkFloats[3].Data[3]);
                verts[3] = new Vector3(unkFloats[0].Data[3], unkFloats[1].Data[3], unkFloats[3].Data[3]);
                verts[4] = new Vector3(unkFloats[5].Data[3], unkFloats[4].Data[3], -unkFloats[2].Data[3]);
                verts[5] = new Vector3(unkFloats[0].Data[3], unkFloats[4].Data[3], -unkFloats[2].Data[3]);
                verts[6] = new Vector3(unkFloats[5].Data[3], unkFloats[1].Data[3], -unkFloats[2].Data[3]);
                verts[7] = new Vector3(unkFloats[0].Data[3], unkFloats[1].Data[3], -unkFloats[2].Data[3]);

                for (int i = 0; i != verts.Length; i++)
                    verts[i].WriteToFile(writer);

                Matrix.Position.WriteToFile(writer);
            }
            catch
            {
                Debug.WriteLine("ERROR WRITING ARA: {0}", Name.String);

                for (int i = 0; i != verts.Length; i++)
                    verts[i].WriteToFile(writer);

                Matrix.Position.WriteToFile(writer);
            }
        }

        public override string ToString()
        {
            return Name.String;
        }
    }
}
