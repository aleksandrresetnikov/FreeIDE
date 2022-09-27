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
using FreeIDE.Common;
using FreeIDE.Common.Utils;
using FreeIDE.Common.Pathes;
using FreeIDE.Controls;
using FreeIDE.Components;
using FreeIDE.Syntax.Syntaxs;
using FreeIDE.Syntax;

using FastColoredTextBoxNS;

namespace FreeIDE.Forms
{
    public partial class EditorForm : BorderLessFormController
    {
        private FastColoredTextBox tb;
        private static bool zoomInvokeStatys = true;
        private static SmartTextBox GetSmartTextBoxInTab(TabPage tabPage) => (tabPage.Controls[0] as SmartTextBox);
        private SmartTextBox SelectSmartTextBox => GetSmartTextBoxInTab(mainTabControl.SelectedTab);

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
            this.InitializeTheme();
            this.InitializeSolutionFileTreeView();
        }

        private void InitializeSolutionFileTreeView()
        {
            this.solutionFileTreeView.Open(this.OpenSolution != null ? this.OpenSolution.SolutionDirectory :
                new DirectoryInfo(DirectoriesHelper.PathToDocumentsDirectory));
            this.solutionFileTreeView.MoveDirectory += SolutionFileTreeView_MoveDirectory;
            this.solutionFileTreeView.OpenFile += SolutionFileTreeView_OpenFile;
        }

        private void SolutionFileTreeView_OpenFile(object sender, FileTreeViewFileEventArgs e)
        {
            Console.WriteLine(e.Paths.PathItemFrom.ToString());

            switch (e.Paths.PathItemFrom.GetFileExtension.ToLower())
            {
                case ".txt":
                    this.CodePanel_createTab(null);
                    this.CodePanel_open(e.Paths.PathItemFrom.ToString());
                    return;
                case ".cs":
                    this.CodePanel_createTab(null);
                    this.CodePanel_open(e.Paths.PathItemFrom.ToString());
                    return;
            }
        }

        private void CodePanel_createTab(string path)
        {
            //try
            //{
                tb = new SmartTextBox();
                tb.Name = "tb";
                tb.Visible = true;
                tb.Font = new Font("Consolas", 12);
                tb.ContextMenuStrip = CodePanel_contextMenuStrip;
                tb.Dock = DockStyle.Fill;
                tb.BorderStyle = BorderStyle.None;
                tb.VirtualSpace = false;
                tb.LeftPadding = 17;
                tb.Language = Language.Custom;
                tb.AddStyle(CSharpSyntax.sameWordsStyle);
                //tb.BackColor = GetTextEditorSettings.getTextEditorTextBackground();
                //tb.ForeColor = GetTextEditorSettings.getTextEditorTextForeround();
                tb.ShowFoldingLines = true;
                tb.ShowLineNumbers = true;
                tb.ShowCaretWhenInactive = false;
                tb.HighlightFoldingIndicator = true;
                tb.WideCaret = false;
                tb.IsReplaceMode = false;
                tb.LineNumberStartValue = (uint)1;
                tb.PreferredLineWidth = 0;
                tb.LineNumberColor = Color.Teal;
                tb.AutoCompleteBrackets = false;
                tb.LineInterval = 0;
                tb.CaretBlinking = true;
                tb.AutoIndent = true;
                tb.DefaultMarkerSize = 0;
                //tb.Zoom = GetTextEditorSettings.getZoom();
                tb.BookmarkColor = Color.PowderBlue;

                TabPage tab = new TabPage(path != null ? Path.GetFileName(path) : "[new]");
                tab.Controls.Add(tb);
                tab.Tag = path;
                if (path != null) tb.OpenFile(path);
                tb.Tag = new SmartTbInfo();
                (tb.Tag as SmartTbInfo).filePath = path;

                this.mainTabControl.TabPages.Add(tab);
                this.mainTabControl.SelectedTab = tab;

                tb.Focus();
                tb.DelayedTextChangedInterval = 100;
                tb.DelayedEventsInterval = 100;

                tb.TextChanged += CodePanel_ctb_TextChanged;

                tb.ZoomChanged += CodePanel_Tb_ZoomChanged;
                tb.ChangedLineColor = CSharpSyntax.changedLineColor;
                tb.HighlightingRangeType = HighlightingRangeType.VisibleRange;

                /*codePanel_foldLines();
                codePanel_highlightCurrentLine();
                codePanel_dokumentMap();
                codePanel_showInvisibleChars();*/
            /*}
            catch (Exception ex)
            {
                MessageBox.Show($"\n\tStackTrace : \n{ex.Message}", $"Error: {ex.Message}", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
        private void CodePanel_ctb_TextChanged(object sender, TextChangedEventArgs e)
        {
            CodePanel_syntaxHighlight(e);
        }
        private void CodePanel_syntaxHighlight(TextChangedEventArgs e)
        {
            //SmartTextBox item = (this.mainTabControl.SelectedTab.Container.Components[0] as SmartTextBox);

            
        }
        private void CodePanel_Tb_ZoomChanged(object sender, EventArgs e)
        {
            try
            {
                if (!zoomInvokeStatys ||
                        this.mainTabControl.TabPages.Count <= 0 ||
                        this.mainTabControl.TabPages == null ||
                        this.mainTabControl.SelectedTab == null) return;

                zoomInvokeStatys = false;
                int zoomValue = SelectSmartTextBox.Zoom;
                foreach (TabPage tab in this.mainTabControl.TabPages)
                    GetSmartTextBoxInTab(tab).Zoom = zoomValue;
                zoomInvokeStatys = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + $"\n\tStackTrace : \n{ex.StackTrace}", $"Error: {ex.Message}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CodePanel_open(string path)
        {
            //  !!!Attention!!! This method does not add a new file to the code editor,
            //but opens the file in an already open file in the code editor !
            //
            //  Be careful and careful when using this method in this API! Also,
            //be careful when creating plugins and read detailed instructions on
            //FastLPI_Api && Plug-ings_Api (when developing plugins).

            if (this.mainTabControl.TabPages.Count <= 0 ||
                this.mainTabControl.TabPages == null) return;

            var item = this.SelectSmartTextBox;
            this.mainTabControl.SelectedTab.Tag = path;
            item.Name = new FileInfo(path).Name;
            this.mainTabControl.SelectedTab.Text = new FileInfo(path).Name;

            try
            {
                item.Language = FastColoredTextBoxUtil.GetLanguage(path);

                //(item.Tag as SmartTbInfo).popupMenu = GetAutocompleteMenu(item, path);

                var t = Task.Run(() =>
                {
                    (item.Tag as SmartTbInfo).filePath = path;

                    StreamReader reader = new StreamReader(path);
                    string text = reader.ReadToEnd();
                    var Invoke = BeginInvoke((Action)delegate {
                        item.Text = text;

                        //(item.Bookmarks as Bookmarks).items = openProject.CurentUserProjectData.GetBookmarks(path, tb);
                    });

                    reader.Close();
                    GC.Collect();
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void SolutionFileTreeView_MoveDirectory(object sender, FileTreeViewFileEventArgs e)
        {
            Console.WriteLine($"Move: \'{e.Paths.PathItemFrom}\'; To: \'{e.Paths.PathItemTo}\'");
        }

        private void InitializeTags()
        {
            this.mainMenuStrip.Tag = new MenuStripTag(1, 0);
            this.solutionFileTreeView.Tag = new FileTreeViewTag(1, 0);
            this.mainTabControl.Tag = new FlatTabControlTag(0, 0);
            this.CodePanel_contextMenuStrip.Tag = new MenuStripTag(1, 0);
        }

        private void InitializeTheme()
        {
            ThemeMaster.ApplyTheme(this);

            ThemeMaster.ApplyTheme(this.solutionFileTreeView);
            ThemeMaster.ApplyTheme(this.mainTabControl);
            ThemeMaster.ApplyTheme(this.CodePanel_contextMenuStrip);
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

        private void solutionFileTreeView_OpenFile(object sender, FileTreeViewFileEventArgs e)
        {
            if (e.Paths.PathItemFrom.IsFile) OpenFile(e.Paths.PathItemFrom); 
        }

        private void OpenFile(PathItem pathItem)
        {
            Console.WriteLine(pathItem);
        }
    }
}
