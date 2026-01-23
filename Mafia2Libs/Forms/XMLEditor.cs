using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Utils.Settings;

using Language = Utils.Language.Language;

namespace Mafia2Tool
{
    public partial class XMLEditor : Form
    {
        private FileInfo xmlFile;
        private bool bIsFileEdited = false;
        private Encoding fileEncoding = Encoding.UTF8;

        public XMLEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            xmlFile = file;
            LoadFile();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the XML editor.");
        }

        private void Localise()
        {
            Text = Language.GetString("$XML_EDITOR_TITLE");
            Button_File.Text = Language.GetString("$FILE");
            Button_Save.Text = Language.GetString("$SAVE");
            Button_Reload.Text = Language.GetString("$RELOAD");
            Button_Exit.Text = Language.GetString("$EXIT");
            Button_Edit.Text = Language.GetString("$EDIT");
            Button_Format.Text = Language.GetString("$XML_FORMAT");
            Button_Validate.Text = Language.GetString("$XML_VALIDATE");
            Button_Find.Text = Language.GetString("$FIND");
            Button_Replace.Text = Language.GetString("$REPLACE");
            Button_GoToLine.Text = Language.GetString("$GO_TO_LINE");
        }

        private void LoadFile()
        {
            try
            {
                using (var reader = new StreamReader(xmlFile.FullName, true))
                {
                    TextBox_XML.Text = reader.ReadToEnd();
                    fileEncoding = reader.CurrentEncoding;
                }

                FileIsNotEdited();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save()
        {
            try
            {
                File.WriteAllText(xmlFile.FullName, TextBox_XML.Text, fileEncoding);
                FileIsNotEdited();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reload()
        {
            if (bIsFileEdited)
            {
                var result = MessageBox.Show(Language.GetString("$RELOAD_CONFIRM"), "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            LoadFile();
        }

        private void FormatXML()
        {
            try
            {
                var doc = XDocument.Parse(TextBox_XML.Text);
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineChars = Environment.NewLine,
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = doc.Declaration == null
                };

                using (var sw = new StringWriter())
                {
                    using (var writer = XmlWriter.Create(sw, settings))
                    {
                        doc.Save(writer);
                    }

                    string result = sw.ToString();
                    if (doc.Declaration != null)
                    {
                        result = doc.Declaration.ToString() + Environment.NewLine + result;
                    }
                    TextBox_XML.Text = result;
                }

                FileIsEdited();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error formatting XML: {ex.Message}", "Format Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidateXML()
        {
            try
            {
                XDocument.Parse(TextBox_XML.Text);
                MessageBox.Show(Language.GetString("$XML_VALID"), Language.GetString("$XML_VALIDATION"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (XmlException ex)
            {
                MessageBox.Show($"{Language.GetString("$XML_INVALID")}\n\nLine {ex.LineNumber}, Position {ex.LinePosition}:\n{ex.Message}",
                    Language.GetString("$XML_VALIDATION"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Navigate to error position
                if (ex.LineNumber > 0)
                {
                    GoToLine(ex.LineNumber);
                }
            }
        }

        private void ShowFindDialog()
        {
            string searchText = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter text to find:", "Find", "", -1, -1);

            if (!string.IsNullOrEmpty(searchText))
            {
                int startIndex = TextBox_XML.SelectionStart + TextBox_XML.SelectionLength;
                int index = TextBox_XML.Text.IndexOf(searchText, startIndex, StringComparison.OrdinalIgnoreCase);

                if (index == -1 && startIndex > 0)
                {
                    // Wrap around
                    index = TextBox_XML.Text.IndexOf(searchText, 0, StringComparison.OrdinalIgnoreCase);
                }

                if (index >= 0)
                {
                    TextBox_XML.Select(index, searchText.Length);
                    TextBox_XML.ScrollToCaret();
                    TextBox_XML.Focus();
                }
                else
                {
                    MessageBox.Show("Text not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowReplaceDialog()
        {
            using (var replaceForm = new Form())
            {
                replaceForm.Text = "Find and Replace";
                replaceForm.Size = new Size(400, 180);
                replaceForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                replaceForm.StartPosition = FormStartPosition.CenterParent;
                replaceForm.MaximizeBox = false;
                replaceForm.MinimizeBox = false;

                var lblFind = new Label { Text = "Find:", Location = new Point(10, 15), AutoSize = true };
                var txtFind = new TextBox { Location = new Point(80, 12), Width = 290 };
                var lblReplace = new Label { Text = "Replace:", Location = new Point(10, 45), AutoSize = true };
                var txtReplace = new TextBox { Location = new Point(80, 42), Width = 290 };

                var btnFindNext = new Button { Text = "Find Next", Location = new Point(80, 80), Width = 90 };
                var btnReplace = new Button { Text = "Replace", Location = new Point(180, 80), Width = 90 };
                var btnReplaceAll = new Button { Text = "Replace All", Location = new Point(280, 80), Width = 90 };

                btnFindNext.Click += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(txtFind.Text))
                    {
                        int startIndex = TextBox_XML.SelectionStart + TextBox_XML.SelectionLength;
                        int index = TextBox_XML.Text.IndexOf(txtFind.Text, startIndex, StringComparison.OrdinalIgnoreCase);
                        if (index == -1 && startIndex > 0)
                            index = TextBox_XML.Text.IndexOf(txtFind.Text, 0, StringComparison.OrdinalIgnoreCase);

                        if (index >= 0)
                        {
                            TextBox_XML.Select(index, txtFind.Text.Length);
                            TextBox_XML.ScrollToCaret();
                        }
                        else
                        {
                            MessageBox.Show("Text not found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };

                btnReplace.Click += (s, e) =>
                {
                    if (TextBox_XML.SelectedText.Equals(txtFind.Text, StringComparison.OrdinalIgnoreCase))
                    {
                        TextBox_XML.SelectedText = txtReplace.Text;
                    }
                    btnFindNext.PerformClick();
                };

                btnReplaceAll.Click += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(txtFind.Text))
                    {
                        int count = 0;
                        string newText = TextBox_XML.Text;
                        int index;
                        while ((index = newText.IndexOf(txtFind.Text, StringComparison.OrdinalIgnoreCase)) >= 0)
                        {
                            newText = newText.Remove(index, txtFind.Text.Length).Insert(index, txtReplace.Text);
                            count++;
                        }
                        TextBox_XML.Text = newText;
                        MessageBox.Show($"Replaced {count} occurrence(s).", "Replace All", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };

                replaceForm.Controls.AddRange(new Control[] { lblFind, txtFind, lblReplace, txtReplace, btnFindNext, btnReplace, btnReplaceAll });
                replaceForm.ShowDialog(this);
            }
        }

        private void ShowGoToLineDialog()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter line number:", "Go to Line", "1", -1, -1);

            if (int.TryParse(input, out int lineNumber))
            {
                GoToLine(lineNumber);
            }
        }

        private void GoToLine(int lineNumber)
        {
            if (lineNumber < 1) lineNumber = 1;

            int currentLine = 1;
            int position = 0;

            for (int i = 0; i < TextBox_XML.Text.Length && currentLine < lineNumber; i++)
            {
                if (TextBox_XML.Text[i] == '\n')
                {
                    currentLine++;
                    position = i + 1;
                }
            }

            if (position < TextBox_XML.Text.Length)
            {
                TextBox_XML.SelectionStart = position;
                TextBox_XML.SelectionLength = 0;
                TextBox_XML.ScrollToCaret();
                TextBox_XML.Focus();
            }
        }

        private void UpdateStatusBar()
        {
            int line = TextBox_XML.GetLineFromCharIndex(TextBox_XML.SelectionStart) + 1;
            int col = TextBox_XML.SelectionStart - TextBox_XML.GetFirstCharIndexOfCurrentLine() + 1;
            int totalLines = TextBox_XML.Lines.Length;
            StatusLabel_Position.Text = $"Line: {line}, Column: {col} | Total Lines: {totalLines}";
        }

        private void FileIsEdited()
        {
            if (!bIsFileEdited)
            {
                Text = Language.GetString("$XML_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void FileIsNotEdited()
        {
            Text = Language.GetString("$XML_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void TextBox_XML_TextChanged(object sender, EventArgs e)
        {
            FileIsEdited();
            UpdateStatusBar();
        }

        private void TextBox_XML_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        private void XMLEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                var result = System.Windows.MessageBox.Show(
                    Language.GetString("$SAVE_PROMPT"),
                    "Toolkit",
                    System.Windows.MessageBoxButton.YesNoCancel);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (result == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Button_Save_Click(object sender, EventArgs e) => Save();
        private void Button_Reload_Click(object sender, EventArgs e) => Reload();
        private void Button_Exit_Click(object sender, EventArgs e) => Close();
        private void Button_Format_Click(object sender, EventArgs e) => FormatXML();
        private void Button_Validate_Click(object sender, EventArgs e) => ValidateXML();
        private void Button_Find_Click(object sender, EventArgs e) => ShowFindDialog();
        private void Button_Replace_Click(object sender, EventArgs e) => ShowReplaceDialog();
        private void Button_GoToLine_Click(object sender, EventArgs e) => ShowGoToLineDialog();
    }
}
