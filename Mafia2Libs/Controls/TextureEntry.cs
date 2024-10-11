using Rendering.Graphics;
using ResourceTypes.Materials;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mafia2Tool
{
    public partial class TextureEntry : UserControl
    {
        public SceneData SceneData;
        private bool isSelected;
        private Color defaultColour;
        private IMaterial material;

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

        public void SetMaterial(IMaterial material)
        {
            if (material != null)
            {
                MaterialName.Text = material.GetMaterialName();
                TextureImage.Image = TextureLoader.LoadThumbnail(material);
                this.material = material;
            }
        }

        public IMaterial GetMaterial()
        {
            return material;
        }

        public event EventHandler<EventArgs> OnEntrySingularClick;
        public event EventHandler<EventArgs> OnEntryDoubleClick;

        private void OnSingularClick(object sender, EventArgs e)
        {
            var SingularClick = OnEntrySingularClick;
            if (SingularClick != null)
            {
                OnEntrySingularClick(this, EventArgs.Empty);
            }
            IsSelected = true;
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            var DoubleClick = OnEntryDoubleClick;
            if(DoubleClick != null)
            {
                OnEntryDoubleClick(this, EventArgs.Empty);
            }
        }

        private void RecurseMouseClick(ControlCollection Controls)
        {
            foreach (Control control in Controls)
            {
                control.MouseClick += OnSingularClick;
                control.MouseDoubleClick += OnDoubleClick;
                RecurseMouseClick(control.Controls);
            }
        }
        private void OnLoad(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                control.MouseClick += OnSingularClick;
                control.MouseDoubleClick += OnDoubleClick;
                RecurseMouseClick(control.Controls);
            }
        }
    }
}
