using System;
using System.Windows.Forms;
using Utils.Language;

namespace MafiaToolkit
{
    public partial class NewObjectForm : Form
    {
        public Control control;

        public NewObjectForm(bool nameEnabled)
        {
            InitializeComponent();
            Localise();

           textBox1.Enabled = nameEnabled;
        }

        private void Localise()
        {
            buttonCancel.Text = Language.GetString("$CANCEL");
            buttonContinue.Text = Language.GetString("$CONTINUE");
            Text = Language.GetString("$NEWOBJFORM_TITLE");
        }

        public void LoadOption(Control desiredControl)
        {
            control = desiredControl;
            panel1.Controls.Add(control);          
        }

        public void SetLabel(string text)
        {
            label.Text = text;
        }

        public string GetInputText()
        {
            return textBox1.Text;
        }

        public void OnButtonClickContinue(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public void OnButtonClickCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
