using System;
using System.Windows.Forms;
using Utils.Settings;

namespace Toolkit.Forms
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            LabelToolkitName.Text = "Mafia Toolkit v " + ToolkitSettings.Version.ToString().Replace(",", ".");
            ThanksBox.Text = "" +
                "- Oleg @ ZModeler 3\r\n" +
                "- Rick 'Gibbed'\r\n" +
                "- Fireboyd for developing UnluacNET\r\n" +
                "- Hurikejnis and Zeuvera for Slovenčina localization\r\n" +
                "\r\n" +
                "Thanks to fellow contributors:\r\n" +
                " - ModdingCode\r\n" +
                " - RoadTrain\r\n" +
                " - Kamzik123\r\n" +
                "\r\n" +
                "Also, a very special thanks to PayPal donators:\r\n" +
                "- Inlife\r\n" +
                "- T3mas1\r\n" +
                "- Jaqub\r\n" +
                "- xEptun\r\n" +
                "- L//oO//nyRider\r\n" +
                "- Nemesis7675\r\n" +
                "- Foxite\r\n" +
                "- MafiaGameVideo\r\n" +
                "- Kamzik123\r\n" +
                "And Patreons:\r\n" +
                "- HamAndRock\r\n" +
                "- Melber\r\n";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProjectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(ProjectLink.Text);
        }
    }
}
