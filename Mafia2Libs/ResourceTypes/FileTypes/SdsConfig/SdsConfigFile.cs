using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.Logging;

namespace ResourceTypes.SDSConfig
{
    public class SdsConfigFile
    {
        public static uint Magic = 0x73647370; // 'psds'
        public static int Version = 2;
        [PropertyIgnoreByReflector]
        public StringTable StringTable { get; set; } = new();
        public Template[] Template { get; set; } = new Template[0];
        public SdsConfigFile()
        {

        }

        public SdsConfigFile(FileInfo file)
        {
            ReadFromFile(file);
        }

        public SdsConfigFile(string fileName)
        {
            ReadFromFile(fileName);
        }

        public void ReadFromFile(FileInfo file)
        {
            ReadFromFile(file.FullName);
        }

        public void ReadFromFile(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            uint magic = br.ReadUInt32();
            int version = br.ReadInt32();

            if (version != Version || magic != Magic)
                throw new FormatException("Unsupported SDS config version");

            StringTable = new(br);

            short Count = br.ReadInt16();
            Template = new Template[Count];

            for (int i = 0; i < Template.Length; i++)
            {
                Template[i] = new(br, this);
            }

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, "SdsConfig - Failed to reach EOF.");
        }

        public void WriteToFile(FileInfo file)
        {
            WriteToFile(file.FullName);
        }

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                Write(ms);

                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(Stream s)
        {
            using (BinaryWriter bw = new(s))
            {
                Write(bw);
            }
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Magic);
            bw.Write(Version);

            List<string> strings = new();

            foreach (var data in Template)
            {
                strings.AddRange(data.Strings);
            }

            var stringTable = StringTable.BuildFromStrings(strings);

            bw.Write(stringTable.Length);
            bw.Write(stringTable);

            bw.Write((short)Template.Length);

            foreach (var val in Template)
            {
                val.Write(bw, this);
            }
        }

        public void ConvertToXML(string Filename)
        {
            XElement Root = ReflectionHelpers.ConvertPropertyToXML(this);
            Root.Save(Filename);
        }

        public void ConvertFromXML(string Filename)
        {
            XElement LoadedDoc = XElement.Load(Filename);
            SdsConfigFile FileContents = ReflectionHelpers.ConvertToPropertyFromXML<SdsConfigFile>(LoadedDoc);

            // Copy data taken from loaded XML
            Template = FileContents.Template;
        }
    }
}