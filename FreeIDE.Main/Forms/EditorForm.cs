using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using FreeIDE.Tags;
using FreeIDE.Components;
using FreeIDE.Common.Utils;

namespace FreeIDE.Forms
{
    public partial class EditorForm : BorderLessFormController
    {
        private protected Solution OpenSolution;

        public EditorForm()
        {
            this.OpenSolution = null;

            this.ClassInit();
        }

        public EditorForm(FileInfo SolutionFile)
        {
            this.OpenSolution = new Solution(SolutionFile);

            this.ClassInit();
        }

        private protected void ClassInit()
        {
            this.InitializeComponent();
            this.InitializeTags();

            ThemeMaster.ApplyTheme(this);
        }

        private void InitializeTags()
        {
            this.mainMenuStrip.Tag = new MenuStripTag(1, 0);
        }
    }
}
