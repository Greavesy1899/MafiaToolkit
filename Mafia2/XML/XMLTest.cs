using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2
{
    public class XMLBin
    {
        public int unk0_int;
        public string unk1_string;

        public int unk2_int;
        public string unk3_string;

        public byte unk4_byte;
        public int unkcount5_int;

        public int unk8_size;
        public long unk9_rand_long;
        public long unk10_rand_long;

        public string unkstring1;
        public string unkstring2;

        public override string ToString()
        {
            return string.Format("{0}, {1}", unk1_string, unk3_string);
        }
    }

    public class XMLTest
    {
        string path = Directory.GetCurrentDirectory() + "/XML/";
        List<XMLBin> bins = new List<XMLBin>();

        public XMLTest()
        {
            DetectFiles();
        }

        public void DetectFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            FileInfo[] files = dirInfo.GetFiles();

            List<string> ItemDesc = new List<string>();

            foreach (FileInfo file in files)
            {
                if (file.Name.Contains("XML_"))
                    ParseXMLBin(file.Name);
            }
        }

        public void ParseXMLBin(string name)
        {
            XMLBin xmlBin = new XMLBin();

            using (BinaryReader reader = new BinaryReader(File.Open(path + name, FileMode.Open)))
            {
                xmlBin.unk0_int = reader.ReadInt32();
                xmlBin.unk1_string = new string(reader.ReadChars(xmlBin.unk0_int));

                reader.ReadByte();

                xmlBin.unk2_int = reader.ReadInt32();
                xmlBin.unk3_string = new string(reader.ReadChars(xmlBin.unk2_int));

                reader.ReadByte();

                xmlBin.unk4_byte = reader.ReadByte();
                xmlBin.unkcount5_int = reader.ReadInt32();
                xmlBin.unk8_size = reader.ReadInt32();
                xmlBin.unk9_rand_long = reader.ReadInt64();
                reader.ReadByte();
                xmlBin.unk10_rand_long = reader.ReadInt64();

                xmlBin.unkstring1 = new string(reader.ReadChars(19));

                reader.ReadBytes(9);

                xmlBin.unkstring2 = new string(reader.ReadChars(13));


            }
            bins.Add(xmlBin);
        }
    }
}
