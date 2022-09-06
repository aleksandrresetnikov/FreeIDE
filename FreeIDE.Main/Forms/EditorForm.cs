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
using FreeIDE.Common;

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

            this.solutionFileTreeView.Open(this.OpenSolution != null ? this.OpenSolution.SolutionDirectory :
                new DirectoryInfo(DirectoriesHelper.PathToDocumentsDirectory));

            ThemeMaster.ApplyTheme(this);
        }

        private void InitializeTags()
        {
            this.mainMenuStrip.Tag = new MenuStripTag(1, 0);
            this.solutionFileTreeView.Tag = new FileTreeViewTag(1, 0);
        }
    }
}
