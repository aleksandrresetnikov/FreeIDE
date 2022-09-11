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
using FreeIDE.Common;
using FreeIDE.Common.Utils;

namespace FreeIDE.Forms
{
    public partial class EditorForm : BorderLessFormController
    {
        private protected Solution _OpenSolution;
        private protected Solution OpenSolution 
        {
            get => this._OpenSolution;
            set => this.SetOpenSolutionValue(value);
        }

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

        private void SetOpenSolutionValue(Solution solutionValue)
        {
            this._OpenSolution = solutionValue;
            if (this.solutionFileTreeView == null) return;
            this.solutionFileTreeView.Open(this.OpenSolution != null ? this.OpenSolution.SolutionDirectory :
                new DirectoryInfo(DirectoriesHelper.PathToDocumentsDirectory));
        }

        private void CreateSolutionFile(FileInfo fileInfo)
        {
            string newSolutionDirectoryPath = fileInfo.FullName.RemoveExtension();
            string newSolutionFilePath = $@"{newSolutionDirectoryPath}\{fileInfo.Name}";

            Directory.CreateDirectory(newSolutionDirectoryPath);
            File.Create(newSolutionFilePath).Close();

            this.OpenSolution = new Solution(newSolutionFilePath.GetFileInfo());
        }

        private void OpenSolutionFile(FileInfo fileInfo)
        {
            this.OpenSolution = new Solution(fileInfo);
        }

        private void newSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = DirectoriesHelper.PathToSoftDocumentsDirectory;
            saveFileDialog1.Filter = "Solution Files(*.fis;)|*.fis;";
            saveFileDialog1.Title = "Create new Solution *the solution will be created in the selected folder";

            DialogResult dialog = saveFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
                this.CreateSolutionFile(saveFileDialog1.FileName.GetFileInfo());
        }

        private void solutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = DirectoriesHelper.PathToSoftDocumentsDirectory;
            openFileDialog1.Filter = "Solution Files(*.fis;)|*.fis;";
            openFileDialog1.Title = "Oprn Solution";

            DialogResult dialog = openFileDialog1.ShowDialog();
            if (dialog == DialogResult.OK)
                OpenSolutionFile(openFileDialog1.FileName.GetFileInfo());
        }
    }
}
