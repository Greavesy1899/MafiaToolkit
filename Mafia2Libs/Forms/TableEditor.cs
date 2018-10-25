using System.IO;
using Gibbed.Mafia2.ResourceFormats;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;

        public TableEditor(FileInfo file)
        {
            InitializeComponent();
            this.file = file;
            Initialise();
        }

        public void Localise()
        {
            Text = Language.GetString("$TABLE_EDITOR_TITLE");
        }

        public void Initialise()
        {
            TableData data = new TableData();
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                data.Deserialize(reader);
            }
        }
    }
}
