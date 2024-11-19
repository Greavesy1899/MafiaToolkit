using System.ComponentModel;
using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxCompiledFaceGraphLink
    {
        public uint NodeIndex { get; set; } // This links to FxCompiledFaceGraphNode
        public FxLinkFnType LinkFnType { get; set; }
        public float[] Parameters { get; set; }

        public FxCompiledFaceGraphLink()
        {
            Parameters = new float[0];
        }

        public void ReadFromFile(FxArchive Owner, BinaryReader reader)
        {
            NodeIndex = reader.ReadUInt32();
            LinkFnType = (FxLinkFnType)reader.ReadUInt32();

            // Read parameters
            uint NumParameters = reader.ReadUInt32();
            Parameters = new float[NumParameters];
            for (int i = 0; i < NumParameters; i++)
            {
                Parameters[i] = reader.ReadSingle();
            }
        }

        public void WriteToFile(FxArchive Owner, BinaryWriter writer)
        {
            writer.Write(NodeIndex);
            writer.Write((uint)LinkFnType);

            // Write parameters
            writer.Write(Parameters.Length);
            foreach (float Parameter in Parameters)
            {
                writer.Write(Parameter);
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FxCompiledFaceGraphNode
    {
        public FxCompiledFaceGraphNodeType NodeType { get; set; }
        public FxName Name { get; set; }
        public float NodeMin { get; set; }
        public float OneOverNodeMin { get; set; }
        public float NodeMax { get; set; }
        public float OneOverNodeMax { get; set; }
        public FxInputOp InputOperation { get; set; }
        public uint ArrayUnk1 { get; set; }
        public FxCompiledFaceGraphLink[] InputLinks { get; set; }

        public FxCompiledFaceGraphNode()
        {
            Name = new FxName();
            InputLinks = new FxCompiledFaceGraphLink[0];
        }

        public void ReadFromFile(FxArchive OwningArchive, BinaryReader reader)
        {
            NodeType = (FxCompiledFaceGraphNodeType)reader.ReadUInt32();
            Name.ReadFromFile(OwningArchive, reader);
            NodeMin = reader.ReadSingle();
            OneOverNodeMin = reader.ReadSingle();
            NodeMax = reader.ReadSingle();
            OneOverNodeMax = reader.ReadSingle();
            InputOperation = (FxInputOp)reader.ReadUInt32();

            // Read Links
            uint ArrayUnk0 = reader.ReadUInt32();
            InputLinks = new FxCompiledFaceGraphLink[ArrayUnk0];
            for (int i = 0; i < ArrayUnk0; i++)
            {
                FxCompiledFaceGraphLink LinkObject = new FxCompiledFaceGraphLink();
                LinkObject.ReadFromFile(OwningArchive, reader);
                InputLinks[i] = LinkObject;
            }

            ArrayUnk1 = reader.ReadUInt32();
        }

        public void WriteToFile(FxArchive OwningArchive, BinaryWriter writer)
        {
            writer.Write((uint)NodeType);
            Name.WriteToFile(OwningArchive, writer);
            writer.Write(NodeMin);
            writer.Write(OneOverNodeMin);
            writer.Write(NodeMax);
            writer.Write(OneOverNodeMax);
            writer.Write((uint)InputOperation);

            // Write Links
            writer.Write(InputLinks.Length);
            foreach (FxCompiledFaceGraphLink GraphLink in InputLinks)
            {
                GraphLink.WriteToFile(OwningArchive, writer);
            }

            writer.Write(ArrayUnk1);
        }

        public void PopulateStringTable(FxArchive Owner)
        {
            Name.AddToStringTable(Owner);
        }

        public override string ToString()
        {
            return string.Format("{0} - Links: {1}", Name, InputLinks.Length);
        }
    }
}
