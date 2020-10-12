using System.IO;
using System.Windows.Forms;

namespace ResourceTypes.M3.XBin
{
    public interface BaseTable
    {
        // Serialization/Deserialization from binary.
        void ReadFromFile(BinaryReader reader);
        void WriteToFile(XBinWriter writer);

        // Serialization/Deserialization from XML.
        void ReadFromXML(string file);
        void WriteToXML(string file);

        // Util functions to interact with the editor.
        TreeNode GetAsTreeNodes();
        void SetFromTreeNodes(TreeNode Root);
    }
}
