using System.IO;
using Gibbed.Mafia2.ResourceFormats;
using System.Windows.Forms;
using System;
using Utils.Lang;
using Utils.Extensions;
using System.Collections.Generic;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;
        private TableData data;

        public TableEditor(FileInfo file)
        {
            InitializeComponent();
            this.file = file;
            Localise();
            Initialise();
            Show();
        }

        public void Localise()
        {
            Text = Language.GetString("$TABLE_EDITOR_TITLE");
            FileButton.Text = Language.GetString("$FILE");
            EditButton.Text = Language.GetString("$EDIT");
            SaveButton.Text = Language.GetString("$SAVE");
            ExitButton.Text = Language.GetString("$EXIT");
            ReloadButton.Text = Language.GetString("$RELOAD");
            AddColumnButton.Text = Language.GetString("$TABLE_ADD_COLUMN");
            AddRowButton.Text = Language.GetString("$TABLE_ADD_ROW");
        }

        public void Initialise()
        {
            AddColumnButton.Enabled = false;
            LoadTableData();
            RowIndexLabel.Text = "Row Index: 0";
            ColumnIndexLabel.Text = "Column Index: 0";
        }

        private void LoadTableData()
        {
            DataGrid.Rows.Clear();
            DataGrid.Columns.Clear();

            data = new TableData();
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                data.Deserialize(0, reader.BaseStream, Gibbed.IO.Endian.Little);
            }

            foreach (TableData.Column column in data.Columns)
            {
                MTableColumn newCol = new MTableColumn();
                newCol.NameHash = column.NameHash;
                newCol.HeaderText = column.NameHash.ToString("X8");
                newCol.Unk2 = column.Unknown2;
                newCol.Unk3 = column.Unknown3;
                newCol.TypeM2 = column.Type;

                switch (newCol.TypeM2)
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
                DataGrid.Columns.Add(newCol);
            }

            foreach (TableData.Row row in data.Rows)
            {
                DataGrid.Rows.Add(row.Values.ToArray());
            }
        }

        private void SaveTableData()
        {
            TableData newData = new TableData();
            newData.NameHash = data.NameHash;
            newData.Name = data.Name;
            newData.Unk1 = data.Unk1;
            newData.Unk2 = data.Unk2;
            
            for(int i = 0; i != DataGrid.ColumnCount; i++)
            {
                TableData.Column column = new TableData.Column();
                MTableColumn col = (DataGrid.Columns[i] as MTableColumn);
                column.Type = col.TypeM2;
                column.Unknown2 = col.Unk2;
                column.Unknown3 = col.Unk3;
                column.NameHash = col.NameHash;
                newData.Columns.Add(column);
            }

            for (int i = 0; i != DataGrid.RowCount; i++)
            {
                TableData.Row row = new TableData.Row();
                for (int x = 0; x != DataGrid.ColumnCount; x++)
                    row.Values.Add(DataGrid.Rows[i].Cells[x].Value);
                newData.Rows.Add(row);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                newData.Serialize(writer);
            }

            data = newData;
        }

        private void ExitButtonOnClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ReloadOnClick(object sender, EventArgs e)
        {
            LoadTableData();
        }

        private void SaveOnClick(object sender, EventArgs e)
        {
            SaveTableData();
        }

        private void AddRowOnClick(object sender, EventArgs e)
        {
            List<object> data = new List<object>();

            foreach (MTableColumn column in DataGrid.Columns)
            {
                if (column.TypeM2 == TableData.ColumnType.Boolean)
                    data.Add(0);
                else
                    data.Add("");
            }
            DataGrid.Rows.Add(data.ToArray());
        }

        private void AddColumnOnClick(object sender, EventArgs e)
        {

        }

        private void OnCellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowIndexLabel.Text = "Row Index: " + e.RowIndex.ToString();
            ColumnIndexLabel.Text = "Column Index: " + e.ColumnIndex.ToString();
            DataTypeLabel.Text = "Data Type: " + data.Columns[e.ColumnIndex].Type;
        }
    }
}
