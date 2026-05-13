using System;
using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.M3.XBin
{
    // Placeholder for XBin tables whose internal struct has not yet
    // been reverse engineered. Reads the entire remaining file body
    // into a byte buffer and writes it back verbatim so editing
    // workflows that round-trip an unknown file do not corrupt it.
    //
    // Each unsupported hash should still be registered explicitly in
    // XBinFactory with its own switch case pointing at this class
    // (rather than serving as a silent default), so the list of
    // "RE pending" hashes stays visible to maintainers.
    public class RawXBinTable : BaseTable
    {
        public byte[] RawBody { get; set; }
        public string DisplayName { get; set; }

        public RawXBinTable() : this("Unparsed XBin") { }

        public RawXBinTable(string displayName)
        {
            RawBody = Array.Empty<byte>();
            DisplayName = displayName;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            long remaining = reader.BaseStream.Length - reader.BaseStream.Position;
            RawBody = reader.ReadBytes((int)remaining);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(RawBody);
        }

        public void ReadFromXML(string file)
        {
            throw new NotSupportedException(DisplayName + ": full struct RE is pending; XML import is not available yet.");
        }

        public void WriteToXML(string file)
        {
            throw new NotSupportedException(DisplayName + ": full struct RE is pending; XML export is not available yet.");
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode(DisplayName + " (raw, " + RawBody.Length + " bytes)");
            Root.Tag = this;
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
        }
    }
}
