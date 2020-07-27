using Rendering.Graphics;
using ResourceTypes.Materials;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class TextureEntry : UserControl
    {
        private bool isSelected;
        private Color defaultColour;
        private Material material;

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

        public void SetMaterial(Material material)
        {
            if (material != null)
            {
                MaterialName.Text = material.MaterialName;
                TextureImage.Image = TextureLoader.LoadThumbnail(material);
                this.material = material;
            }
        }

        public Material GetMaterial()
        {
            return material;
        }

        public event EventHandler<EventArgs> WasClicked;

        private void OnDoubleClick(object sender, EventArgs e)
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
