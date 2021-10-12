using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MafiaToolkit.Controls
{
    public sealed class MTreeView : TreeView
    {
        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        //fix from: (gotta love stack overflow!)
        //https://stackoverflow.com/questions/14647216/c-sharp-treeview-ignore-double-click-only-at-checkbox
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0203 && CheckBoxes)
            {
                var localPos = this.PointToClient(Cursor.Position);
                var hitTestInfo = this.HitTest(localPos);
                if (hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Msg = 0x0201;
                }
            }
            base.WndProc(ref m);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            SendMessage(this.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
            base.OnHandleCreated(e);
        }
    }
}
