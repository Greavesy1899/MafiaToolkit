using System.IO;
using Gibbed.Mafia2.ResourceFormats;
using System.Windows.Forms;
using System;
using Utils.Language;
using Utils.Extensions;
using System.Collections.Generic;
using Gibbed.Illusion.FileFormats.Hashing;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;
        private TableData data;
        private Dictionary<uint, string> columnNames = new Dictionary<uint, string>();

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
            AddRowButton.Text = Language.GetString("$TABLE_ADD_ROW");
        }

        public void Initialise()
        {
            ReadExternalHashes();
            LoadTableData();
            GetCellProperties(0, 0);
        }

        private void ReadExternalHashes()
        {
            try
            {
                string[] hashes = File.ReadAllLines(Path.Combine("Resources", "hashes.txt"));

                foreach(var hash in hashes)
                {
                    uint key = FNV32.Hash(hash);
                    columnNames.TryAdd(key, hash);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Missing hashes.txt, No column names will be present.", "Toolkit", MessageBoxButtons.OK);
                columnNames = new Dictionary<uint, string>();
            }
        }

        private string GetColumnName(uint hash)
        {
            if (columnNames.ContainsKey(hash))
            {
                return columnNames[hash];
            }

            return hash.ToString("X8");
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
                newCol.HeaderText = GetColumnName(newCol.NameHash);
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
                {
                    row.Values.Add(DataGrid.Rows[i].Cells[x].Value);
                }

                newData.Rows.Add(row);
            }

            // Don't save the file if we fail to validate
            if(!newData.Validate())
            {
                MessageBox.Show("Failed to validate. Not saving data.", "Toolkit", MessageBoxButtons.OK);
                return;
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                newData.Serialize(writer);
            }

            data = newData;
        }

        private void GetCellProperties(int r, int c)
        {
            RowIndexLabel.Text = "Row Index: " + r.ToString();
            ColumnIndexLabel.Text = "Column Index: " + c.ToString();
            DataTypeLabel.Text = "Data Type: " + data.Columns[c].Type;
        }

        private void ExitButtonOnClick(object sender, EventArgs e) => Close();
        private void ReloadOnClick(object sender, EventArgs e) => LoadTableData();
        private void SaveOnClick(object sender, EventArgs e) => SaveTableData();

        private void AddRowOnClick(object sender, EventArgs e)
        {
            List<object> data = new List<object>();

            foreach (MTableColumn column in DataGrid.Columns)
            {
                switch(column.TypeM2)
                {
                    case TableData.ColumnType.Boolean:
                    case TableData.ColumnType.Unsigned32:
                    case TableData.ColumnType.Signed32:
                    case TableData.ColumnType.Hash64:
                    case TableData.ColumnType.Float32:
                    case TableData.ColumnType.Flags32:
                        data.Add(0);
                        break;
                    case TableData.ColumnType.Color:
                        data.Add("255 255 255");
                        break;
                    default:
                        data.Add("");
                        break;
                }
            }
            DataGrid.Rows.Add(data.ToArray());
        }

        private void OnSelectedChange(object sender, EventArgs e)
        {
            if (DataGrid.SelectedCells.Count > 0)
            {
                GetCellProperties(DataGrid.SelectedCells[0].RowIndex, DataGrid.SelectedCells[0].ColumnIndex);
            }
        }

        private void DeleteRowOnClick(object sender, EventArgs e)
        {
            if(DataGrid.SelectedRows.Count > 0)
            {
                DataGrid.Rows.Remove(DataGrid.SelectedRows[0]);
            }
        }
    }
}
