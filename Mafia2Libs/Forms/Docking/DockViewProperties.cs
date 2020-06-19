using Utils.Language;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockViewProperties : DockContent
    {
        public bool[] VisibleProperties;

        public DockViewProperties()
        {
            InitializeComponent();
            Localise();
            Init();
        }

        private void Localise()
        {
            label1.Text = Language.GetString("$TOGGLE_MODELS");
            label2.Text = Language.GetString("$TOGGLE_COLLISIONS");
            label3.Text = Language.GetString("$TOGGLE_BOXES");
            ModelComboBox.Items[0] = Language.GetString("$VISIBLE");
            ModelComboBox.Items[1] = Language.GetString("$HIDDEN");
            CollisionComboBox.Items[0] = Language.GetString("$VISIBLE");
            CollisionComboBox.Items[1] = Language.GetString("$HIDDEN");
            BoxComboBox.Items[0] = Language.GetString("$VISIBLE");
            BoxComboBox.Items[1] = Language.GetString("$HIDDEN");
        }

        private void Init()
        {
            VisibleProperties = new bool[3];
            VisibleProperties[0] = true;
            ModelComboBox.SelectedIndex = 0;
            VisibleProperties[1] = true;
            CollisionComboBox.SelectedIndex = 0;
            VisibleProperties[2] = true;
            BoxComboBox.SelectedIndex = 0;
        }

        private void OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            VisibleProperties[0] = (ModelComboBox.SelectedIndex == 0) ? true : false;
            VisibleProperties[1] = (CollisionComboBox.SelectedIndex == 0) ? true : false;
            VisibleProperties[2] = (BoxComboBox.SelectedIndex == 0) ? true : false;
        }

        private void OnResize(object sender, System.EventArgs e)
        {
            //small idea
            //Size size = ModelComboBox.Size;
            //size.Width = Size.Width - 10;
            //ModelComboBox.Size = size;
            //CollisionComboBox.Size = size;
        }
    }
}
