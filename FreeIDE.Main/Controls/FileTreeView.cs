using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using FreeIDE.Common.Utils;
using FreeIDE.Common.Pathes;
using FreeIDE.Common;

namespace FreeIDE.Controls
{
    public delegate void FileTreeViewFileEvent(object sender, FileTreeViewFileEventArgs e);

    public class FileTreeViewFileEventArgs
    {
        public PathsCollectorItem Paths;
    }

    internal class FileTreeView : TreeView
    {
        private string fileType_LastFileName = "";
        private FileType fileType_RenameNode = FileType.File;
        private DirectoryInfo _OpenDirectory;

        public PathsCollector PathsHistory { get; private protected set; }
        public string SelectedPath { get; private protected set; }
        public DirectoryInfo OpenDirectory { get => _OpenDirectory; private protected set => Update(value); }
        public PathItem SelectedPathItem => new PathItem(this.SelectedNode.Tag.ToString());

        public event FileTreeViewFileEvent OpenFile;

        public event FileTreeViewFileEvent RemoveFile;
        public event FileTreeViewFileEvent DeleteFile;
        public event FileTreeViewFileEvent CutFile;
        public event FileTreeViewFileEvent CopyFile;
        public event FileTreeViewFileEvent PasteFile;
        public event FileTreeViewFileEvent PasteFiles;

        public event FileTreeViewFileEvent RemoveDirectory;
        public event FileTreeViewFileEvent DeleteDirectory;
        public event FileTreeViewFileEvent CutDirectory;
        public event FileTreeViewFileEvent CopyDirectory;
        public event FileTreeViewFileEvent PasteDirectory;
        public event FileTreeViewFileEvent PasteDirectories;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public FileTreeView() : base()
        {
            this.PathsHistory = new PathsCollector();
            this.LabelEdit = true;

            this.BeforeExpand += FileTreeView_BeforeExpand;
            this.BeforeSelect += FileTreeView_BeforeSelect;
            this.ItemDrag += FileTreeView_ItemDrag;
            this.DragEnter += FileTreeView_DragEnter;
            this.DragOver += FileTreeView_DragOver;
            this.DragDrop += FileTreeView_DragDrop;
            this.BeforeLabelEdit += FileTreeView_BeforeLabelEdit;
            this.AfterLabelEdit += FileTreeView_AfterLabelEdit;
            this.DoubleClick += FileTreeView_DoubleClick;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null);
        }

        public void Open(DirectoryInfo Directory)
        {
            this.OpenDirectory = Directory;
            this.SelectedPath = Directory.FullName;
        }
        public void Update(DirectoryInfo _OpenDirectory)
        {
            this._OpenDirectory = _OpenDirectory;

            this.Nodes.Clear();
            TreeNode rootNode = new TreeNode(this.OpenDirectory.Name);
            TreeNode[] treeFiles = new TreeNode[Directory.GetFiles(OpenDirectory.FullName).Length +
                Directory.GetDirectories(OpenDirectory.FullName).Length];

            int numNode = 0;
            foreach (string path in Directory.GetDirectories(OpenDirectory.FullName))
            {
                treeFiles[numNode] = new TreeNode(new DirectoryInfo(path).Name);
                treeFiles[numNode].Name = new FileInfo(path).Name;
                treeFiles[numNode].Nodes.Add(new TreeNode("..."));
                treeFiles[numNode].Tag = path;
                rootNode.Nodes.Add(treeFiles[numNode]);
                numNode++;
            }
            foreach (string path in Directory.GetFiles(OpenDirectory.FullName))
            {
                treeFiles[numNode] = new TreeNode(new FileInfo(path).Name);
                treeFiles[numNode].Name = new FileInfo(path).Name;
                treeFiles[numNode].Tag = path;
                rootNode.Nodes.Add(treeFiles[numNode]);
                numNode++;
            }

            rootNode.Tag = OpenDirectory.FullName;
            rootNode.ImageIndex = 0;

            this.Nodes.Add(rootNode);
        }

        private bool CheckSelectedNode()
        {
            if (this.SelectedNode == null) return false;

            if (new FileInfo(this.SelectedNode.Tag.ToString()).Exists) return true; // OK
            else if (new DirectoryInfo(this.SelectedNode.Tag.ToString()).Exists) return true; // OK
            else { this.SelectedNode.Remove(); return false; } // Error: The file or folder does not exist in the file system
        }

        private void AddHistory(PathItem PathItemFrom)
        {
            this.PathsHistory.Add(new PathsCollectorItem(PathItemFrom));
        }

        private void AddHistory(PathItem PathItemFrom, PathItem PathItemTo)
        {
            this.PathsHistory.Add(new PathsCollectorItem(PathItemFrom, PathItemTo));
        }

        private void FileTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (CheckSelectedNode() && this.OpenFile != null)
            {
                PathItem pathItem = new PathItem(this.SelectedNode.Tag.ToString());
                AddHistory(pathItem);

                OpenFile.Invoke(this, new FileTreeViewFileEventArgs { 
                    Paths = new PathsCollectorItem(pathItem) 
                });
            }
        }
        private void FileTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == fileType_LastFileName || e.Label == null || e.Label == "") return;

            string lastText = e.Node.Text;
            e.Node.Name = e.Label;
            e.Node.Text = e.Label;

            if (fileType_RenameNode == FileType.File)
            {
                FileInfo lastFileInfo = new FileInfo(e.Node.Tag.ToString());
                if (lastFileInfo.Exists)
                {
                    string lastTag = e.Node.Tag.ToString();
                    e.Node.Tag = lastFileInfo.DirectoryName + $"/" + e.Label;
                    e.Node.ImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(e.Node.Tag.ToString()).Extension);
                    e.Node.SelectedImageIndex = e.Node.ImageIndex;
                    if (File.Exists(e.Node.Tag.ToString()))
                    {
                        MessageBox.Show("Файл с таким именем\nуже существует", "Проблема");
                        var invoke = BeginInvoke((Action)delegate
                        {
                            e.Node.Tag = lastTag;
                            e.Node.Text = lastText;
                            e.Node.Name = lastText;
                            e.Node.ImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(e.Node.Tag.ToString()).Extension);
                        });
                        return;
                    }

                    File.Move(lastFileInfo.FullName, e.Node.Tag.ToString());
                    AddHistory(new PathItem(lastFileInfo), new PathItem(e.Node.Tag.ToString()));
                }
            }
            else if (fileType_RenameNode == FileType.Dir)
            {
                DirectoryInfo lastDirInfo = new DirectoryInfo(e.Node.Tag.ToString());
                if (lastDirInfo.Exists)
                {
                    string lastTag = e.Node.Tag.ToString();
                    e.Node.Tag = lastDirInfo.Parent.FullName + $"/" + e.Label;
                    e.Node.ImageIndex = 0;
                    e.Node.SelectedImageIndex = 0;
                    if (Directory.Exists(e.Node.Tag.ToString()))
                    {
                        MessageBox.Show("Папка с таким именем\nуже существует", "Проблема");
                        var invoke = BeginInvoke((Action)delegate
                        {
                            e.Node.Tag = lastTag;
                            e.Node.Text = lastText;
                            e.Node.Name = lastText;
                        });
                        return;
                    }

                    lastDirInfo.Delete();
                    Directory.CreateDirectory(e.Node.Tag.ToString());
                    AddHistory(new PathItem(lastDirInfo), new PathItem(e.Node.Tag.ToString()));
                }
            }
        }
        private void FileTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (CheckSelectedNode())
            {
                if (this.SelectedPathItem.IsFile)
                {
                    fileType_RenameNode = FileType.File;
                    fileType_LastFileName = this.SelectedPathItem.GetFileInfo.Name;
                }
                else if (this.SelectedPathItem.IsDirectory)
                {
                    fileType_RenameNode = FileType.Dir;
                    fileType_LastFileName = this.SelectedPathItem.GetDirectoryInfo.Name;
                }
            }
        }
        private void FileTreeView_DragDrop(object sender, DragEventArgs e)
        {

        }
        private void FileTreeView_DragOver(object sender, DragEventArgs e)
        {

        }
        private void FileTreeView_DragEnter(object sender, DragEventArgs e)
        {

        }
        private void FileTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }
        private void FileTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }
        private void FileTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (e.Node.Tag == null) return;
                e.Node.Nodes.Clear();

                TreeNode[] treeFiles = new TreeNode[Directory.GetFiles(e.Node.Tag.ToString()).Length + Directory.GetDirectories(e.Node.Tag.ToString()).Length];

                int numNode = 0;
                foreach (string path in Directory.GetDirectories(e.Node.Tag.ToString()))
                {
                    treeFiles[numNode] = new TreeNode(new DirectoryInfo(path).Name);
                    treeFiles[numNode].Name = new FileInfo(path).Name;
                    treeFiles[numNode].Nodes.Add(new TreeNode());
                    treeFiles[numNode].Tag = path;
                    treeFiles[numNode].ImageIndex = 0;
                    treeFiles[numNode].SelectedImageIndex = 0;
                    e.Node.Nodes.Add(treeFiles[numNode]);
                    numNode++;
                }
                foreach (string path in Directory.GetFiles(e.Node.Tag.ToString()))
                {
                    treeFiles[numNode] = new TreeNode(new FileInfo(path).Name);
                    treeFiles[numNode].Name = new FileInfo(path).Name;
                    treeFiles[numNode].Tag = path;
                    treeFiles[numNode].ImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(path).Extension);
                    treeFiles[numNode].SelectedImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(path).Extension);
                    e.Node.Nodes.Add(treeFiles[numNode]);
                    numNode++;
                }
            }
            catch (Exception ex)
            {
                Logger.AddNewLog(@"\Error", "FileTreeView_BeforeExpand\n" + ex);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
