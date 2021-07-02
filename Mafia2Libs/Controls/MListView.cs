using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mafia2Tool.Controls
{
    class MListView : ListView
    {
        public MListView()
        {
            this.DoubleBuffered = true;
        }
    }
}
