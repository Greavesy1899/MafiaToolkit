using System.IO;
using Gibbed.Mafia2.ResourceFormats;
using System.Windows.Forms;
using System;
using Utils.Lang;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;

        public TableEditor(FileInfo file)
        {
            InitializeComponent();
            this.file = file;
            Localise();
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
                data.Deserialize(0, reader.BaseStream, Gibbed.IO.Endian.Little);
            }

            foreach(TableData.Column column in data.Columns)
            {
                DataGridViewColumn newCol = new DataGridViewColumn();
                newCol.HeaderText = column.NameHash.ToString("X8");

                switch(column.Type)
                {
                    case TableData.ColumnType.Boolean:
                        newCol.CellTemplate = new DataGridViewCheckBoxCell();
                        break;
                    case TableData.ColumnType.String16:
                    case TableData.ColumnType.String32:
                    case TableData.ColumnType.String64:
                        newCol.CellTemplate = new DataGridViewTextBoxCell();
                        break;
                    default:
                        newCol.CellTemplate = new DataGridViewTextBoxCell();
                        break;
                }
                //newCol.CellTemplate = new DataGridViewTextBoxCell();
                DataGrid.Columns.Add(newCol);
            }

            foreach(TableData.Row row in data.Rows)
            {
                DataGrid.Rows.Add(row.Values.ToArray());
            }

            ShowDialog();
        }
    }
}
