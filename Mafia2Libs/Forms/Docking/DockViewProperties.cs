using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockViewProperties : DockContent
    {
        public bool[] VisibleProperties;

        public DockViewProperties()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            VisibleProperties = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                VisibleProperties[i] = true;
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void OnValueChanged(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                VisibleProperties[i] = checkedListBox1.GetItemChecked(i);
        }
    }
}
