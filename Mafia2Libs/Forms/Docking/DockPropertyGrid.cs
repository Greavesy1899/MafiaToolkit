using WeifenLuo.WinFormsUI.Docking;

namespace Forms.Docking
{
    public partial class DockPropertyGrid : DockContent
    {
        public DockPropertyGrid()
        {
            InitializeComponent();
        }

        public void SetObjectOnPropertyGrid(object obj)
        {
            PropertyGrid.SelectedObject = obj;
        }
    }
}
