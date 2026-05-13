using System;
using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.M3.XBin
{
    // Multiplayer xbins shipped with M1:DE that contain no real data —
    // the engine reserves a hash per MP table but ships a placeholder
    // with a fixed 88-byte body (an empty SDB container header).
    // All 12 MP placeholders observed in M1:DE share the same body
    // bytes; this class is a verbatim passthrough so the editor can
    // open them without modification.
    public class EmptyMPPlaceholderTable : BaseTable
    {
        private const int BodySize = 88;

        private static readonly byte[] DefaultBody =
        {
            0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00,
        };

        public byte[] Body { get; set; }

        public EmptyMPPlaceholderTable()
        {
            Body = (byte[])DefaultBody.Clone();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            Body = reader.ReadBytes(BodySize);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(Body);
        }

        public void ReadFromXML(string file)
        {
            throw new NotImplementedException("Empty MP placeholders have no editable data.");
        }

        public void WriteToXML(string file)
        {
            throw new NotImplementedException("Empty MP placeholders have no editable data.");
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode("Empty MP Placeholder");
            Root.Tag = this;
            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
        }
    }
}
