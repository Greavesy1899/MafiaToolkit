/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class XmlResource1
    {
        public static void Serialize(Stream output, string content, Endian endian)
        {
            // Serialize header. It is BX with a 1 after.
            output.WriteValueU16(0x5842, endian);
            output.WriteValueU16(1, endian);

            // Read the XMLDocument.
            XmlDocument document = new XmlDocument();
            document.LoadXml(File.ReadAllText(content));
            XmlNodeList ChildNodes = document.ChildNodes;
            SerializeNodeEntry(output, ChildNodes, endian);
        }

        public static string Deserialize(Stream input, Endian endian)
        {
            if (input.ReadValueU16(endian) != 0x5842) // 'BX'
            {
                throw new FormatException();
            }

            if (input.ReadValueU16(endian) > 1)
            {
                throw new FormatException();
            }

            var root = (NodeEntry)DeserializeNodeEntry(input, endian);

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var output = new StringBuilder();
            var writer = XmlWriter.Create(output, settings);

            writer.WriteStartDocument();
            WriteXmlNode(writer, root);
            writer.WriteEndDocument();

            writer.Flush();
            return output.ToString();
        }

        private static void WriteXmlNode(XmlWriter writer, NodeEntry node)
        {
            writer.WriteStartElement(node.Name);

            foreach (var attribute in node.Attributes)
            {
                writer.WriteStartAttribute(attribute.Name);
                writer.WriteValue(attribute.Value == null
                                      ? ""
                                      : attribute.Value.ToString());
                writer.WriteEndAttribute();
            }

            foreach (var child in node.Children)
            {
                WriteXmlNode(writer, child);
            }

            if (node.Value != null)
            {
                writer.WriteValue(node.Value.ToString());
            }

            writer.WriteEndElement();
        }

        private static void SerializeNodeEntry(Stream output, XmlNodeList ChildNodes, Endian endian)
        {
            foreach(XmlNode ChildNode in ChildNodes)
            {
                if(ChildNode.NodeType == XmlNodeType.Text && ChildNode.Value != null)
                {
                    output.WriteValueU16((ushort)(ChildNode.Value.Length + 1), endian);
                    output.WriteValueU8(4);
                    output.WriteValueU8(0);
                    output.WriteValueU16((ushort)ChildNode.Value.Length, endian);
                    output.WriteStringZ(ChildNode.Value);
                }
                if (ChildNode.NodeType == XmlNodeType.Element)
                {
                    string name = ChildNode.Name;
                    int NumChildren = ChildNode.ChildNodes != null ? ChildNode.ChildNodes.Count : 0;
                    int NumAttributes = ChildNode.Attributes != null ? ChildNode.Attributes.Count : 0;
                    int NumBytes = name.Length + 1 + 16 * NumChildren + 16 * NumAttributes;
                    output.WriteValueU16((ushort)NumBytes, endian);
                    output.WriteValueU8(1);
                    output.WriteValueU8((byte)name.Length);
                    output.WriteValueU16((ushort)NumChildren, endian);
                    output.WriteValueU8((byte)NumAttributes);
                    output.WriteStringZ(name);

                    if(ChildNode.HasChildNodes)
                    {
                        SerializeNodeEntry(output, ChildNode.ChildNodes, endian);
                    }

                    foreach(XmlAttribute AttributeNode in ChildNode.Attributes)
                    {
                        if(AttributeNode.Name != null)
                        {
                            output.WriteValueU16((ushort)(AttributeNode.Name.Length + 1), endian);
                            output.WriteValueU8(5);
                            output.WriteValueU8((byte)AttributeNode.Name.Length);
                            output.WriteStringZ(AttributeNode.Name);
                        }
                        if (AttributeNode.Value != null)
                        {
                            output.WriteValueU16((ushort)(AttributeNode.Value.Length + 1), endian);
                            output.WriteValueU8(4);
                            output.WriteValueU8(0);
                            output.WriteValueU16((ushort)AttributeNode.Value.Length);
                            output.WriteStringZ(AttributeNode.Value);
                        }
                    }
                }
            }
        }

        private static object DeserializeNodeEntry(Stream input, Endian endian)
        {
            var unk1 = input.ReadValueU16(endian);
            var nodeType = input.ReadValueU8();

            switch (nodeType)
            {
                case 1:
                {
                    var nameLength = input.ReadValueU8();
                    var childCount = input.ReadValueU16(endian);
                    var attributeCount = input.ReadValueU8();

                    var name = input.ReadString(nameLength + 1, true, Encoding.UTF8);

                    var node = new NodeEntry()
                    {
                        Name = name,
                    };

                    var children = new List<object>();
                    for (ushort i = 0; i < childCount; i++)
                    {
                        children.Add(DeserializeNodeEntry(input, endian));
                    }

                    if (children.Count == 1 && children[0] is DataValue)
                    {
                        node.Value = (DataValue)children[0];
                    }
                    else
                    {
                        foreach (var child in children)
                        {
                            node.Children.Add((NodeEntry)child);
                        }
                    }

                    for (byte i = 0; i < attributeCount; i++)
                    {
                        var child = DeserializeNodeEntry(input, endian);

                        if (child is NodeEntry)
                        {
                            var data = (NodeEntry)child;

                            if (data.Children.Count != 0 || data.Attributes.Count != 0)
                            {
                                throw new FormatException();
                            }

                            var attribute = new AttributeEntry()
                            {
                                Name = data.Name,
                                Value = data.Value,
                            };
                            node.Attributes.Add(attribute);
                        }
                        else
                        {
                            node.Attributes.Add((AttributeEntry)child);
                        }
                    }

                    return node;
                }

                case 4:
                {
                    var valueType = input.ReadValueU8();
                    if (valueType == 0)
                    {
                        var valueLength = input.ReadValueU16(endian);
                        var value = input.ReadString(valueLength + 1, true, Encoding.UTF8);
                        return new DataValue(DataType.String, value);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                case 5:
                {
                    var nameLength = input.ReadValueU8();
                    var name = input.ReadString(nameLength + 1, true, Encoding.UTF8);

                    var attribute = new NodeEntry()
                    {
                        Name = name,
                    };

                    attribute.Value = (DataValue)DeserializeNodeEntry(input, endian);
                    return attribute;
                }

                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private class NodeEntry
        {
            public string Name;
            public DataValue Value;
            public List<NodeEntry> Children = new List<NodeEntry>();
            public List<AttributeEntry> Attributes = new List<AttributeEntry>();
        }

        private class AttributeEntry
        {
            public string Name;
            public DataValue Value;
        }

        private enum DataType
        {
            String = 0,
        }

        private class DataValue
        {
            public DataType Type;
            public object Value;

            public DataValue(DataType type, object value)
            {
                this.Type = type;
                this.Value = value;
            }

            public override string ToString()
            {
                return this.Value.ToString();
            }
        }
    }
}
