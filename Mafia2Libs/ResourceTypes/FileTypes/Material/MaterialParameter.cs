using System;
using System.IO;

namespace ResourceTypes.Materials
{
    public class MaterialParameter
    {
        string id;
        float[] paramaters;

        public string ID {
            get { return id; }
            set { id = value; }
        }
        public float[] Paramaters {
            get { return paramaters; }
            set { paramaters = value; }
        }

        public MaterialParameter()
        {
            id = "";
            paramaters = new float[0];
        }

        public MaterialParameter(MaterialParameter OtherParameter)
        {
            ID = OtherParameter.ID;
            Paramaters = OtherParameter.paramaters;
        }

        public MaterialParameter(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            id = new string(reader.ReadChars(4));
            int paramCount = reader.ReadInt32() / 4;
            paramaters = new float[paramCount];
            for (int i = 0; i != paramCount; i++)
            {
                paramaters[i] = reader.ReadSingle();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(id.ToCharArray());
            writer.Write(paramaters.Length * 4);
            for (int i = 0; i != paramaters.Length; i++)
            {
                writer.Write(paramaters[i]);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", id, paramaters.Length);
        }
    }
}
