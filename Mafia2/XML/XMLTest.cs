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
            List<NodeEntry> nodes;

            public void ReadFromFile(BinaryReader reader)
            {
                if(reader.ReadByte() != 4)
                    throw new FormatException();

                count = reader.ReadInt32();
                size = reader.ReadInt32();
                nodes = new List<NodeEntry>();

                int NodeDataPos = (int)reader.BaseStream.Position;
                data = reader.ReadBytes(size);
                int ExtraDataPos = (int)reader.BaseStream.Position;

                reader.BaseStream.Seek(NodeDataPos, SeekOrigin.Begin);
                for(int i = 0; i != count; i++)
                {
                    NodeEntry node = new NodeEntry(reader);

                    //uint childCount = reader.ReadUInt32();
                    //for(uint x = 0; x != childCount; x++)
                    //{
                    //    node.Children.Add(reader.ReadUInt32());
                    //}
                    //for (uint x = 0; x != childCount; x++)
                    //{
                    //    AttributeEntry attribute = new AttributeEntry();
                    //    attribute.Name.ReadFromFile(reader);
                    //    attribute.Value.ReadFromFile(reader);
                    //    node.Attributes.Add(attribute);
                    //}
                    nodes.Add(node);
                }

            }
        }
        private class NodeEntry
        {
            DataValue name;
            DataValue value;
            uint id;
            List<uint> children = new List<uint>();
            List<AttributeEntry> attributes = new List<AttributeEntry>();

            public List<uint> Children {
                get { return children; }
                set { children = value; }
            }
            public List<AttributeEntry> Attributes {
                get { return attributes; }
                set { attributes = value; }
            }

            public NodeEntry(BinaryReader reader)
            {
                name = new DataValue(reader);
                value = new DataValue(reader);
                this.id = reader.ReadUInt32();
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", name.Value, value.Value);
            }
        }

        private class AttributeEntry
        {
            DataValue name;
            DataValue value;

            public DataValue Name {
                get { return name; }
                set { name = value; }
            }
            public DataValue Value {
                get { return value; }
                set { this.value = value; }
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}", name.Value, value.Value);
            }
        }

        private class DataValue
        {
            XMLDataType type;
            object value;

            public XMLDataType Type {
                get { return type; }
                set { type = value; }
            }
            public object Value {
                get { return value; }
                set { this.value = value; }
            }

            public DataValue(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                type = (XMLDataType)reader.ReadUInt32();

                if (type == XMLDataType.Special)
                    throw new NotImplementedException();
                else if (type == XMLDataType.Boolean)
                    throw new NotImplementedException();
                else if (type == XMLDataType.Float)
                    throw new NotImplementedException();
                else if (type == XMLDataType.String)
                    value = ReadString(reader);
                else if (type == XMLDataType.Integer)
                    throw new NotImplementedException();
                else
                    throw new Exception("Found unknown type: " + type);

            }

            public string ReadString(BinaryReader reader)
            {
                if (reader.ReadInt32() != 0)
                    throw new FormatException();

                bool isWhiteChar = false;
                string rstring = "";

                while(!isWhiteChar)
                {
                    if (reader.PeekChar() == '\0')
                        isWhiteChar = true;
                    else
                        rstring += reader.ReadChar();
                }

                reader.ReadChar();
                return rstring;
            }

            public override string ToString()
            {
                return value.ToString();
            }
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
