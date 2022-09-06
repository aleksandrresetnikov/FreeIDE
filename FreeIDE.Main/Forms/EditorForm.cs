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
using FreeIDE.Components.Renderers;

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
            this.mainMenuStrip.Tag = new MenuStripTag(1, 0);
            //this.mainMenuStrip.Renderer = new CustomMenuStripRenderer();
        }
    }
}
