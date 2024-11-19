﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;
using Utils.Extensions;
using Utils.Language;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Mafia2Tool
{
    public partial class TableEditor : Form
    {
        private FileInfo file;
        private TableData data;
        private Dictionary<uint, string> columnNames = new Dictionary<uint, string>();
        private ushort Version;

        private bool bIsFileEdited = false;

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

                foreach (var hash in hashes)
                {
                    uint key = FNV32.Hash(hash);
                    columnNames.TryAdd(key, hash);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Missing hashes.txt, No column names will be present.", "Toolkit", MessageBoxButtons.OK);
                columnNames = new Dictionary<uint, string>();
            }

            // Load custom hashes. This is optional. Expects format like [uint32] [string]
            FileInfo CustomHashesFile = new FileInfo(Path.Combine("Resources", "custom_hashes.txt"));
            if (CustomHashesFile.Exists)
            {
                string[] CustomHashes = File.ReadAllLines(CustomHashesFile.FullName);

                foreach (string Line in CustomHashes)
                {
                    string[] values = Line.Split(" ");
                    uint hash = uint.Parse(values[0]);
                    columnNames.TryAdd(hash, values[1]);
                }
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
                Version = (ushort)reader.ReadInt32();
                data.Deserialize(Version, reader.BaseStream, Endian.Little);
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

            Label_Version.Text = string.Format("Version: {0}", Version);

            Text = Language.GetString("$TABLE_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void SaveTableData()
        {
            // TODO: This *really* sucks. Can't we just remove the rows/columns and repopulate them?
            // Instead of doing the whole TableData structure...
            TableData newData = new TableData();
            newData.NameHash = data.NameHash;
            newData.Name = data.Name;
            newData.PatchedName = data.PatchedName;
            newData.PatchedNameHash = data.PatchedNameHash;
            newData.PatchedUnk1 = data.PatchedUnk1;
            newData.PatchedUnk2 = data.PatchedUnk2;
            newData.Unk1 = data.Unk1;
            newData.Unk2 = data.Unk2;

            for (int i = 0; i < DataGrid.ColumnCount; i++)
            {
                TableData.Column column = new TableData.Column();
                MTableColumn col = (DataGrid.Columns[i] as MTableColumn);
                column.Type = col.TypeM2;
                column.Unknown2 = col.Unk2;
                column.Unknown3 = col.Unk3;
                column.NameHash = col.NameHash;
                newData.Columns.Add(column);
            }

            for (int i = 0; i < DataGrid.RowCount; i++)
            {
                TableData.Row row = new TableData.Row();
                for (int x = 0; x < DataGrid.ColumnCount; x++)
                {
                    row.Values.Add(DataGrid.Rows[i].Cells[x].Value);
                }

                newData.Rows.Add(row);
            }

            // Don't save the file if we fail to validate
            if (!newData.Validate())
            {
                MessageBox.Show("Failed to validate. Not saving data.", "Toolkit", MessageBoxButtons.OK);
                return;
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                writer.Write((int)Version);
                newData.Serialize(Version, writer.BaseStream, Endian.Little);
            }

            data = newData;

            Text = Language.GetString("$TABLE_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void GetCellProperties(int r, int c)
        {
            RowIndexLabel.Text = string.Format("[Row Index: {0}]", r);
            ColumnIndexLabel.Text = string.Format("[Column Index: {0}]", c);
            Label_DataType.Text = string.Format("[Data Type: {0}]", data.Columns[c].Type);
            Label_ValueDataType.Text = string.Format("[Value Type: {0}]", DataGrid.Rows[r].Cells[c].Value.GetType());
        }

        private void ExitButtonOnClick(object sender, EventArgs e) => Close();
        private void ReloadOnClick(object sender, EventArgs e) => LoadTableData();
        private void SaveOnClick(object sender, EventArgs e) => SaveTableData();

        private void AddRowOnClick(object sender, EventArgs e)
        {
            List<object> data = new List<object>();

            foreach (MTableColumn column in DataGrid.Columns)
            {
                Type DataType = TableData.GetValueTypeForColumnType(column.TypeM2);
                switch (column.TypeM2)
                {
                    case TableData.ColumnType.Boolean:
                    case TableData.ColumnType.Unsigned32:
                    case TableData.ColumnType.Signed32:
                    case TableData.ColumnType.Hash64:
                    case TableData.ColumnType.Float32:
                    case TableData.ColumnType.Flags32:
                        data.Add(Activator.CreateInstance(DataType));
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

            Text = Language.GetString("$TABLE_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnSelectedChange(object sender, EventArgs e)
        {
            if (DataGrid.SelectedCells.Count > 0)
            {
                GetCellProperties(DataGrid.SelectedCells[0].RowIndex, DataGrid.SelectedCells[0].ColumnIndex);
            }
        }

        private void CellContent_Changed(object sender, DataGridViewCellEventArgs e)
        {
            MTableColumn Column = (DataGrid.Columns[e.ColumnIndex] as MTableColumn);
            DataGridViewCell Cell = DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (Cell.Value.GetType() != TableData.GetValueTypeForColumnType(Column.TypeM2))
            {
                object Output = Convert.ChangeType(Cell.Value, TableData.GetValueTypeForColumnType(Column.TypeM2));
                Cell.Value = Output;
            }
            Text = Language.GetString("$TABLE_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void DeleteRowOnClick(object sender, EventArgs e)
        {
            if (DataGrid.SelectedRows.Count > 0)
            {
                DataGrid.Rows.Remove(DataGrid.SelectedRows[0]);

                Text = Language.GetString("$TABLE_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void TableEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", MessageBoxButton.YesNoCancel);

                if (SaveChanges == MessageBoxResult.Yes)
                {
                    SaveTableData();
                }
                else if (SaveChanges == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
