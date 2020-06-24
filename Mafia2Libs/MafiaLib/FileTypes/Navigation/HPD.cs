using SharpDX;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    public class HPDData
    {
        int unk0;
        int unk1;
        byte[] remainingHeader; //132
        public unkStruct[] unkData;
        string unk2;
        int unk3;
        int unk4;

        public class unkStruct
        {
            public int id;
            public Vector3 unk0;
            public Vector3 unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
            public int unk6;

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", id, unk0.ToString(), unk1.ToString());
            }
        }

        public HPDData(BinaryReader reader)
        {
            StreamWriter writer = new StreamWriter("NAV_HPD_DATA content.txt");
            unk0 = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            writer.WriteLine(unk0);
            writer.WriteLine(unk1);
            remainingHeader = reader.ReadBytes(132);

            writer.WriteLine("");
            unkData = new unkStruct[unk1];

            for (int i = 0; i != unkData.Length; i++)
            {
                unkStruct data = new unkStruct();
                data.id = reader.ReadInt32();
                data.unk0 = Vector3Extenders.ReadFromFile(reader);
                data.unk1 = Vector3Extenders.ReadFromFile(reader);

                //Vector3 pos = data.unk0;
                //float y = pos.Y;
                //pos.Y = -pos.Z;
                //pos.Z = y;
                //data.unk0 = pos;

                //pos = data.unk1;
                //y = pos.Y;
                //pos.Y = -pos.Z;
                //pos.Z = y;
                //data.unk1 = pos;

                data.unk2 = reader.ReadInt32();
                data.unk3 = reader.ReadInt32();
                data.unk4 = reader.ReadInt32();
                data.unk5 = reader.ReadInt32();
                data.unk6 = reader.ReadInt32();
                writer.WriteLine(data.id);
                writer.WriteLine(data.unk1);
                writer.WriteLine(data.unk0);
                writer.WriteLine(data.unk2);
                writer.WriteLine(data.unk3);
                writer.WriteLine(data.unk4);
                writer.WriteLine(data.unk5);
                writer.WriteLine(data.unk6);
                unkData[i] = data;
                writer.WriteLine("");
            }
            writer.WriteLine("");
            unk2 = StringHelpers.ReadString(reader);
            writer.WriteLine(unk2);
            unk3 = reader.ReadInt32();
            writer.WriteLine(unk3);
            unk4 = reader.ReadInt32();
            writer.WriteLine(unk4);
            writer.Close();
        }
    }
}
