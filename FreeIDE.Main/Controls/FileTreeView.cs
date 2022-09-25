using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

using FreeIDE.Common;
using FreeIDE.Common.Utils;
using FreeIDE.Common.Pathes;

namespace FreeIDE.Controls
{
    public delegate void FileTreeViewFileEvent(object sender, FileTreeViewFileEventArgs e);
    public delegate void FileTreeViewPasteEvent(object sender, FileTreeViewPasteEventArgs e);

    public class FileTreeViewFileEventArgs
    {
        public PathsCollectorItem Paths;
    }

    public class FileTreeViewPasteEventArgs
    {
        public PathsCollector Paths;
    }

    internal class FileTreeView : TreeView
    {
        private string fileType_LastFileName = "";
        private FileType fileType_RenameNode = FileType.File;
        private DirectoryInfo _OpenDirectory;
        private System.Threading.Timer checkTimer;
        private object checkTimerCallbackObject = null;

        public PathsCollector PathsHistory { get; private protected set; } = new PathsCollector();
        public string SelectedPath { get; private protected set; }
        public DirectoryInfo OpenDirectory { get => _OpenDirectory; private protected set => Update(value); }
        public PathItem SelectedPathItem => new PathItem(this.SelectedNode == null ? null : this.SelectedNode.Tag.ToString());
        public int CheckTimerInterval { get; set; } = 100;
        public bool CheckTimerDoing { get; set; } = true;

        public event FileTreeViewFileEvent OpenFile;
        public event FileTreeViewPasteEvent Paste;

        public event FileTreeViewFileEvent RenameFile;
        public event FileTreeViewFileEvent MoveFile;
        public event FileTreeViewFileEvent DeleteFile;
        public event FileTreeViewFileEvent CutFile;
        public event FileTreeViewFileEvent CopyFile;
        public event FileTreeViewFileEvent CopyToFile;

        public event FileTreeViewFileEvent RenameDirectory;
        public event FileTreeViewFileEvent MoveDirectory;
        public event FileTreeViewFileEvent DeleteDirectory;
        public event FileTreeViewFileEvent CutDirectory;
        public event FileTreeViewFileEvent CopyDirectory;
        public event FileTreeViewFileEvent CopyToDirectory;

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public FileTreeView() : base()
        {
            if (this.CheckTimerDoing)
                this.checkTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.checkTimerDoing), 
                    this.checkTimerCallbackObject, 0, this.CheckTimerInterval);

            this.LabelEdit = true;
            this.AllowDrop = true;

            this.BeforeExpand += FileTreeView_BeforeExpand;
            this.ItemDrag += FileTreeView_ItemDrag;
            this.DragEnter += FileTreeView_DragEnter;
            this.DragOver += FileTreeView_DragOver;
            this.DragDrop += FileTreeView_DragDrop;
            this.BeforeLabelEdit += FileTreeView_BeforeLabelEdit;
            this.AfterLabelEdit += FileTreeView_AfterLabelEdit;
            this.DoubleClick += FileTreeView_DoubleClick;
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
        public void DoCopyFile()
        {
            if (!CheckSelectedNode()) return;
            Clipboard.SetFileDropList(new StringCollection { this.SelectedPathItem.Path });
            this.PathsHistory.Add(new PathsCollectorItem(new PathItem(this.SelectedPathItem.Path)));

            if (this.CopyFile != null) CopyFile.Invoke(this, new FileTreeViewFileEventArgs
            {
                Paths = new PathsCollectorItem(this.SelectedPathItem.ClonePath())
            });

            Console.WriteLine("Copy");
        }
        public void DoPaste()
        {
            if (!CheckSelectedNode() || !Clipboard.ContainsFileDropList()) { return; }

            StringCollection filesArray = Clipboard.GetFileDropList();
            Dictionary<string, string> filesPairs = new Dictionary<string, string>();
            this.SelectedNode = this.SelectedPathItem.IsFile ? this.SelectedNode.Parent : this.SelectedNode;

            foreach (string item in filesArray)
            {
                if (File.Exists(item))
                {
                    TreeNode newTreeNode = new TreeNode(new FileInfo(item).Name);
                    newTreeNode.ImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(item).Extension);
                    newTreeNode.SelectedImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(item).Extension);
                    newTreeNode.Tag = this.SelectedPathItem.Path + $"/" + new FileInfo(item).Name;
                    if (!File.Exists(newTreeNode.Tag.ToString()))
                    {
                        this.SelectedNode.Nodes.Add(newTreeNode);
                        File.Copy(item, newTreeNode.Tag.ToString());
                    }

                    filesPairs.Add(item, newTreeNode.Tag.ToString());
                }
                else if (Directory.Exists(item))
                {
                    TreeNode newTreeNode = new TreeNode(new DirectoryInfo(item).Name);
                    newTreeNode.Tag = this.SelectedPathItem.Path + $"/" + new DirectoryInfo(item).Name;
                    newTreeNode.ImageIndex = 0;
                    newTreeNode.SelectedImageIndex = 0;
                    newTreeNode.Nodes.Add(new TreeNode());
                    if (!Directory.Exists(newTreeNode.Tag.ToString()))
                    {
                        this.SelectedNode.Nodes.Add(newTreeNode);
                        DirectoryUtil.CopyDir(item, newTreeNode.Tag.ToString());
                    }

                    filesPairs.Add(item, newTreeNode.Tag.ToString());
                }

                Console.WriteLine(item);
            }

            if (this.Paste != null) Paste.Invoke(this, new FileTreeViewPasteEventArgs
            {
                Paths = PathsCollector.Parse(filesPairs)
            });
            this.PathsHistory.AddDictionary(filesPairs);
            Console.WriteLine("Paste");
        }
        public void DoDelete()
        {
            if (!CheckSelectedNode()) { return; }

            if (this.SelectedPathItem.IsFile)
            {
                this.SelectedPathItem.Delete();
            }
            else if (this.SelectedPathItem.IsDirectory)
            {
                DialogResult dialog = MessageBox.Show("Are you sure about deleting the directory and all its contents ?", 
                    "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                    this.SelectedPathItem.Delete();
            }
        }
        public void DoRename()
        {
            if (this.SelectedNode != null)
                this.SelectedNode.BeginEdit();
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            SetWindowTheme(this.Handle, "explorer", null);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.C)) // Copy
            {
                this.DoCopyFile();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.V)) // Paste
            {
                this.DoPaste();
                return true;
            }
            else if (keyData == (Keys.Delete))
            {
                this.DoDelete();
                return true;
            }
            else if (keyData == (Keys.F2))
            {
                this.DoRename();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void checkTimerDoing(Object stateInfo)
        {
            try
            {
                //Console.Clear();
                var invoke = this.BeginInvoke((Action)delegate
                {
                    try
                    {
                        TreeNode treeNode = this.GetRootNode();

                        //Console.WriteLine(/*this.GetRootNode().Text*/this.SelectedNode.Name);
                        this.CheckSelectedNode();
                        this.CheckNodesList(treeNode);
                    }
                    catch (Exception ex) { }
                });
            }
            catch { }
        }

        private bool ContainsFile(TreeNodeCollection treeNodeCollection, FileInfo file)
        {
            foreach (TreeNode node in treeNodeCollection)
                if (node.Tag.ToString() == file.FullName)
                    return true;
            return false;
        }

        private TreeNode GetRootNode()
        {
            if (this.SelectedNode == null) 
                return ((this.Nodes != null && this.Nodes[0] != null) ? this.Nodes[0] : null);

            TreeNode node = this.SelectedNode;
            while (node.Parent != null) node = node.Parent;

            return node;
        }

        private bool CheckSelectedNode()
        {
            if (this.SelectedNode == null) return false;

            if (new FileInfo(this.SelectedNode.Tag.ToString()).Exists) return true; // OK
            else if (new DirectoryInfo(this.SelectedNode.Tag.ToString()).Exists) return true; // OK
            else { this.SelectedNode.Remove(); return false; } // Error: The file or folder does not exist in the file system
        }

        private void CheckNodesList(TreeNode node)
        {
            if (node == null) return;

            foreach (TreeNode _node in node.Nodes)
            {
                if (_node.Tag == null) /*_node.Remove()*/;
                else if (new FileInfo(_node.Tag.ToString()).Exists) continue;
                else if (new DirectoryInfo(_node.Tag.ToString()).Exists) CheckNodesList(_node);
                else _node.Remove();
            }
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

                if (pathItem.IsFile)
                {
                    this.OpenFile.Invoke(this, new FileTreeViewFileEventArgs
                    {
                        Paths = new PathsCollectorItem(pathItem)
                    });
                }
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
                        MessageBox.Show("A file with the same name already exists", "Problem",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        var invoke = BeginInvoke((Action)delegate
                        {
                            e.Node.Tag = lastTag;
                            e.Node.Text = lastText;
                            e.Node.Name = lastText;
                            e.Node.ImageIndex = IconsUtil.GetImageIndexMini(new FileInfo(e.Node.Tag.ToString()).Extension);
                        });
                        return;
                    }

                    if (this.RenameFile != null) this.RenameFile.Invoke(this, new FileTreeViewFileEventArgs 
                    {
                        Paths = new PathsCollectorItem(new PathItem(lastFileInfo), new PathItem(e.Node.Tag.ToString()))
                    });
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
                        MessageBox.Show("A folder with the same name already exists", "Problem",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        var invoke = BeginInvoke((Action)delegate
                        {
                            e.Node.Tag = lastTag;
                            e.Node.Text = lastText;
                            e.Node.Name = lastText;
                        });
                        return;
                    }

                    if (this.RenameDirectory != null) this.RenameFile.Invoke(this, new FileTreeViewFileEventArgs
                    {
                        Paths = new PathsCollectorItem(new PathItem(lastDirInfo), new PathItem(e.Node.Tag.ToString()))
                    });
                    DirectoryUtil.MoveDir(lastDirInfo.FullName, e.Node.Tag.ToString());
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
                    this.fileType_RenameNode = FileType.File;
                    this.fileType_LastFileName = this.SelectedPathItem.GetFileInfo.Name;
                }
                else if (this.SelectedPathItem.IsDirectory)
                {
                    this.fileType_RenameNode = FileType.Dir;
                    this.fileType_LastFileName = this.SelectedPathItem.GetDirectoryInfo.Name;
                }
            }
        }
        private void FileTreeView_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = this.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = this.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (targetNode == null) return;
            if (draggedNode == null) return;

            if (!draggedNode.Equals(targetNode) && !TreeNodeHelper.ContainsNode(draggedNode, targetNode))
            {
                PathItem fromPath = new PathItem(draggedNode.Tag.ToString());
                PathItem toFilePath = new PathItem(targetNode.Tag.ToString() + $"/" + fromPath.GetFileInfo.Name);
                PathItem toDirectoryPath = new PathItem(targetNode.Tag.ToString() + $"/" + fromPath.GetDirectoryInfo.Name);

                if (e.Effect == DragDropEffects.Move && new FileInfo(targetNode.Tag.ToString()).Extension.Replace(" ", "") == "")
                {
                    if (targetNode.ContainsNodeInName(draggedNode))
                    {
                        MessageBox.Show("This file already exists in this directory", "Problem", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (fromPath.IsFile)
                        {
                            if (File.Exists(toFilePath.Path))
                            {
                                MessageBox.Show("This file already exists in this directory", "Problem",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            File.Move(fromPath.Path, toFilePath.Path);
                            draggedNode.Tag = toFilePath;
                            draggedNode.Remove(); 
                            targetNode.Nodes.Add(draggedNode);

                            if (this.MoveFile != null) this.MoveFile.Invoke(this, new FileTreeViewFileEventArgs
                            {
                                Paths = new PathsCollectorItem(new PathItem(draggedNode.Tag.ToString()), new PathItem(toFilePath.Path))
                            });
                            AddHistory(fromPath.ClonePath(), toFilePath.ClonePath());
                        }
                        else if (fromPath.IsDirectory)
                        {
                            if (Directory.Exists(toDirectoryPath.Path))
                            {
                                MessageBox.Show("This directory already exists in this directory", "Problem",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            DirectoryUtil.MoveDir(draggedNode.Tag.ToString(), toDirectoryPath.Path);
                            draggedNode.Tag = toDirectoryPath;
                            draggedNode.Remove(); 
                            targetNode.Nodes.Add(draggedNode);

                            if (this.MoveDirectory != null) this.MoveDirectory.Invoke(this, new FileTreeViewFileEventArgs
                            {
                                Paths = new PathsCollectorItem(new PathItem(draggedNode.Tag.ToString()), new PathItem(toDirectoryPath.Path))
                            });
                            AddHistory(fromPath.ClonePath(), toDirectoryPath.ClonePath());
                        }
                        else 
                        { 
                            MessageBox.Show("The file does not exist", "Problem", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error); 
                            draggedNode.Remove(); 
                        }
                    }
                }
                else if (e.Effect == DragDropEffects.Copy && new FileInfo(targetNode.Tag.ToString()).Extension.Replace(" ", "") == "")
                {
                    if (targetNode.ContainsNodeInName(draggedNode))
                    {
                        MessageBox.Show("This file already exists in this directory", "Problem",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        TreeNode newNode = (TreeNode)draggedNode.Clone(); newNode.Tag = null;
                        Console.WriteLine($@"File path : {draggedNode.Tag}, file copy to (path) : {targetNode.Tag}");
                        if (new FileInfo(draggedNode.Tag.ToString()).Exists)
                        {
                            if (File.Exists(toFilePath.Path))
                            {
                                MessageBox.Show("This file already exists in this directory", "Problem",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            File.Copy(draggedNode.Tag.ToString(), toFilePath.Path);
                            newNode.Tag = toFilePath;

                            if (this.CopyToFile != null) this.CopyToFile.Invoke(this, new FileTreeViewFileEventArgs
                            {
                                Paths = new PathsCollectorItem(new PathItem(draggedNode.Tag.ToString()), new PathItem(toFilePath.Path))
                            });
                            AddHistory(fromPath.ClonePath(), toFilePath.ClonePath());
                        }
                        else if (new DirectoryInfo(draggedNode.Tag.ToString()).Exists)
                        {
                            if (Directory.Exists(toDirectoryPath.Path))
                            {
                                MessageBox.Show("This directory already exists in this directory", "Problem",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            DirectoryUtil.CopyDir(draggedNode.Tag.ToString(), toDirectoryPath.Path);
                            newNode.Tag = toDirectoryPath;

                            if (this.CopyToDirectory != null) this.CopyToDirectory.Invoke(this, new FileTreeViewFileEventArgs
                            {
                                Paths = new PathsCollectorItem(new PathItem(draggedNode.Tag.ToString()),  new PathItem(toDirectoryPath.Path))
                            });
                            AddHistory(fromPath.ClonePath(), toDirectoryPath.ClonePath());
                        }
                        else 
                        { 
                            MessageBox.Show("The file does not exist", "Problem", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error); 
                            draggedNode.Remove(); 
                            newNode.Tag = null; 
                        }

                        if (newNode != null && newNode.Tag != null) targetNode.Nodes.Add(newNode);
                    }
                }

                targetNode.Expand();
            }
        }
        private void FileTreeView_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = this.PointToClient(new Point(e.X, e.Y));
            this.SelectedNode = this.GetNodeAt(targetPoint);
        }
        private void FileTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }
        private void FileTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                DoDragDrop(e.Item, DragDropEffects.Move);
            else if (e.Button == MouseButtons.Right)
                DoDragDrop(e.Item, DragDropEffects.Copy);
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
