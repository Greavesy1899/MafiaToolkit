using System;
using System.IO;
using Utils.Types;

namespace ResourceTypes.Materials
{
    public class MaterialTexture
    {
        public string ID { get; set; }
        public Hash TextureName { get; set; }

        public MaterialTexture()
        {
            ID = "";
            TextureName = new Hash();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            ID = new string(reader.ReadChars(4));
            TextureName.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(ID.ToCharArray());
            TextureName.WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", ID, TextureName.ToString());
        }
    }
}
