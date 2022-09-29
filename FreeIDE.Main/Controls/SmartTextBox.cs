using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FastColoredTextBoxNS;

namespace FreeIDE.Controls
{
    internal class SmartTextBox : FastColoredTextBox
    {
        public void Do_Cut()
        {
            this.Cut(); 
        }        
        public void Do_Copy()
        {
            this.Copy();
        }
        public void Do_Paste()
        {
            this.Paste();
        }
        public void Do_Delete()
        {
            this.SelectedText = null;
        }
    }
}
