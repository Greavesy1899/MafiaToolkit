using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MafiaToolkit.Controls
{
    class MListView : ListView
    {
        public MListView()
        {
            this.DoubleBuffered = true;
        }
    }
}
