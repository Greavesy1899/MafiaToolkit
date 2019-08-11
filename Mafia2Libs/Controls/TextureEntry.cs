using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class TextureEntry : UserControl
    {
        private bool isSelected;
        private Color defaultColour;

        public bool IsSelected {
            get { return isSelected; }
            set {
                isSelected = value;
                MaterialName.BackColor = (value == true ? Color.LightBlue : defaultColour);
            }
        }

        public TextureEntry()
        {
            InitializeComponent();
            defaultColour = MaterialName.BackColor;
        }

        public event EventHandler<EventArgs> WasClicked;

        public void SetMaterialName(string value)
        {
            MaterialName.Text = value;
        }

        public void SetMaterialTexture(Image image)
        {
            TextureImage.Image = image;
        }

        private void OnDoubleClick(object sender, System.EventArgs e)
        {
            var wasClicked = WasClicked;
            if (wasClicked != null)
            {
                WasClicked(this, EventArgs.Empty);
            }
            IsSelected = true;
        }

        private void RecurseMouseClick(ControlCollection Controls)
        {
            foreach (Control control in Controls)
            {
                control.MouseClick += OnDoubleClick;
                RecurseMouseClick(control.Controls);
            }
        }
        private void OnLoad(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                control.MouseClick += OnDoubleClick;
                RecurseMouseClick(control.Controls);
            }
        }
    }
}
