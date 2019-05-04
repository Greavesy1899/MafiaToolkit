using WeifenLuo.WinFormsUI.Docking;

namespace Mafia2Tool.Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        public DockPropertyGrid()
        {
            InitializeComponent();
        }

        public void SetObjectOnPropertyGrid(object obj)
        {
            propertyGrid1.SelectedObject = obj;
        }
    }
}
