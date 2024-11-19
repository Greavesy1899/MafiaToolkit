using System;
using System.Windows.Forms;
using Utils.Language;

namespace Toolkit.Forms
{
    public partial class ExceptionForm : Form
    {
        public ExceptionForm()
        {
            InitializeComponent();
            Localise();
        }

        private void Localise()
        {
            Button_Continue.Text = Language.GetString("$EXCEPTION_CONTINUE");
            Button_Quit.Text = Language.GetString("$EXCEPTION_QUIT");
            Label_ExceptionMsg.Text = Language.GetString("$EXCEPTION_LABEL");
        }

        public void ShowException(Exception InException)
        {
            string RichTextMessage = string.Format("Message: \n{0} \n\nTarget Site: \n{1} \n\nCallstack: \n{2}", InException.Message, InException.TargetSite.ToString(), InException.StackTrace);
            RichTextBox_StackTrace.Text = RichTextMessage;
        }

        private void Button_Continue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void Button_Quit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
