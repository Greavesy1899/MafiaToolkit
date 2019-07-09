using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace ResourceTypes.FrameResource
{
    public class ItemDescResource
    {
        public const int Version = 3;
        public const string TypeName = "ItemDesc";
        public string Name = "";
    }

    public class MemFileResource
    {
        public const int Version = 2;
        public const string TypeName = "MemFile";
        public string Name = "";
    }

    public class ScriptResource
    {
        public const int Version = 2;
        public const string TypeName = "String";
        public string Name = "";
        public string[] Scripts = null;

        public void ReadResourceEntry(XPathNodeIterator reader)
        {
            reader.Current.MoveToNext();
            int numScripts = reader.Current.ValueAsInt;
            Scripts = new string[numScripts];
            for(int i = 0; i < numScripts; i++)
            {
                reader.MoveNext();
                Scripts[i] = reader.Current.Value;
            }
        }
    }

    public class TextureResource
    {
        public const int Version = 2;
        public const string TypeName = "Texture";
        public string Name = "";
        public int HasMIP;

        public void ReadResourceEntry(XPathNodeIterator reader)
        {
            reader.Current.MoveToNext();
            HasMIP = reader.Current.ValueAsInt;
        }

        public void WriteResourceEntry(XmlWriter writer)
        {
            writer.WriteElementString("Type", TypeName);
            writer.WriteElementString("File", Name);
            writer.WriteElementString("HasMIP", HasMIP.ToString());
            writer.WriteElementString("Version", Version.ToString());
        }
    }
}
