using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FreeIDE.Tags;
using FreeIDE.Components;

namespace FreeIDE.Forms
{
    public partial class EditorForm : BorderLessFormController
    {
        public EditorForm()
        {
            this.InitializeComponent();
            this.InitializeTags();

            ThemeMaster.ApplyTheme(this);
        }

        private void InitializeTags()
        {
            this.button1.Tag = new ButtonTag(0, 0);
        }
    }
}
