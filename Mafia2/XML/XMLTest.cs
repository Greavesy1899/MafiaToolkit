using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Mafia2
{
    public class XMLBin
    {
        XMLHeader header;
        XMLSection section;

        public string content;


        public void ReadFromFile(BinaryReader reader)
        {
            header = new XMLHeader();
            header.ReadFromFile(reader);

            if(header.unk3 == false)
            {
                section = new XMLSection();
                section.ReadFromFile(reader);
            }
        }

        public override string ToString()
        {
            return "";
        }

        private struct XMLHeader
        {
            string tag;
            bool unk1;
            string name;
            public bool unk3;

            public void ReadFromFile(BinaryReader reader)
            {
                int tagSize = reader.ReadInt32();
                tag = new string(reader.ReadChars(tagSize));

                unk1 = reader.ReadBoolean();

                int nameSize = reader.ReadInt32();
                name = new string(reader.ReadChars(nameSize));

                unk3 = reader.ReadBoolean();
            }

        }
        private struct XMLSection
        {
            int count;
            int size;
            byte[] data;
            List<XEntry> nodes;

            public void ReadFromFile(BinaryReader reader)
            {
                if(reader.ReadByte() != 4)
                    throw new FormatException();

                count = reader.ReadInt32();
                size = reader.ReadInt32();
                data = reader.ReadBytes(size);

                nodes = new List<XEntry>();

                for(int i = 0; i != nodes.Count; i++)
                {
                    XEntry node = new XEntry();

                }

            }
        }
        private class XEntry
        {
            public string name;
            public int value;
            public uint id;
            public List<uint> children = new List<uint>();
            public List<AEntry> attributes = new List<AEntry>();
        }
        private class AEntry
        {
            public string name;
            public string value;
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
                xmlBin.ReadFromFile(reader);
            }
            bins.Add(xmlBin);
        }
    }
}
