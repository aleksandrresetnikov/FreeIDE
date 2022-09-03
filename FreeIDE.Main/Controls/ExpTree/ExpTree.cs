using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

using Microsoft.VisualBasic;

using FreeIDE.Common;
using static FreeIDE.Common.CShItem;

namespace FreeIDE.Controls.ExpTree 
{
    [DefaultProperty("StartUpDirectory")]
    [DefaultEvent("StartUpDirectoryChanged")]
    public partial class ExpTree : UserControl
    {
        private TreeNode Root;
        public event StartUpDirectoryChangedEventHandler StartUpDirectoryChanged;
        public delegate void StartUpDirectoryChangedEventHandler(StartDir newVal);
        public event ExpTreeNodeSelectedEventHandler ExpTreeNodeSelected;
        public delegate void ExpTreeNodeSelectedEventHandler(string SelPath, CShItem Item);
        private bool EnableEventPost = true; // flag to supress ExpTreeNodeSelected raising during refresh and 

        private TVDragWrapper _DragDropHandler;
        private TVDragWrapper DragDropHandler
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _DragDropHandler;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_DragDropHandler != null)
                {
                    _DragDropHandler.ShDragEnter -= DragWrapper_ShDragEnter;
                    _DragDropHandler.ShDragLeave -= DragWrapper_ShDragLeave;
                    _DragDropHandler.ShDragOver -= DragWrapper_ShDragOver;
                    _DragDropHandler.ShDragDrop -= DragWrapper_ShDragDrop;
                }

                _DragDropHandler = value;
                if (_DragDropHandler != null)
                {
                    _DragDropHandler.ShDragEnter += DragWrapper_ShDragEnter;
                    _DragDropHandler.ShDragLeave += DragWrapper_ShDragLeave;
                    _DragDropHandler.ShDragOver += DragWrapper_ShDragOver;
                    _DragDropHandler.ShDragDrop += DragWrapper_ShDragDrop;
                }
            }
        }

        private bool m_showHiddenFolders = true;

        public ExpTree() : base()
        {
            // expandNodeTimer is used to expand a node that is hovered over, with a delay
            expandNodeTimer = new System.Windows.Forms.Timer();

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call
            // setting the imagelist here allows many good things to happen, but
            // also one bad thing -- the "tooltip" like display of selectednode.text
            // is made invisible.  This remains a problem to be solved.
            SystemImageListManager.SetTreeViewImageList(tv1, false);

            StartUpDirectoryChanged += OnStartUpDirectoryChanged;

            OnStartUpDirectoryChanged(m_StartUpDirectory);

        }

        // ExpTree overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) 
                components.Dispose();
            base.Dispose(disposing);
        }

        // Required by the Windows Form Designer
        private IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        private TreeView _tv1;

        internal virtual TreeView tv1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get => _tv1;

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tv1 != null)
                {
                    _tv1.BeforeExpand -= tv1_BeforeExpand;
                    _tv1.AfterSelect -= tv1_AfterSelect;
                    _tv1.VisibleChanged -= tv1_VisibleChanged;
                    _tv1.BeforeCollapse -= tv1_BeforeCollapse;
                    _tv1.HandleCreated -= tv1_HandleCreated;
                    _tv1.HandleDestroyed -= tv1_HandleDestroyed;
                    _tv1.ItemDrag -= tv1_ItemDrag;
                }

                _tv1 = value;
                if (_tv1 != null)
                {
                    _tv1.BeforeExpand += tv1_BeforeExpand;
                    _tv1.AfterSelect += tv1_AfterSelect;
                    _tv1.VisibleChanged += tv1_VisibleChanged;
                    _tv1.BeforeCollapse += tv1_BeforeCollapse;
                    _tv1.HandleCreated += tv1_HandleCreated;
                    _tv1.HandleDestroyed += tv1_HandleDestroyed;
                    _tv1.ItemDrag += tv1_ItemDrag;
                }
            }
        }
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            _tv1 = new TreeView();
            _tv1.BeforeExpand += tv1_BeforeExpand;
            _tv1.AfterSelect += tv1_AfterSelect;
            _tv1.VisibleChanged += tv1_VisibleChanged;
            _tv1.BeforeCollapse += tv1_BeforeCollapse;
            _tv1.HandleCreated += tv1_HandleCreated;
            _tv1.HandleDestroyed += tv1_HandleDestroyed;
            _tv1.ItemDrag += tv1_ItemDrag;
            this.SuspendLayout();
            // 
            // tv1
            // 
            _tv1.Dock = DockStyle.Fill;
            _tv1.HideSelection = false;
            _tv1.Location = new Point(0, 0);
            _tv1.Name = "_tv1";
            _tv1.ShowRootLines = false;
            _tv1.Size = new Size(200, 264);
            _tv1.TabIndex = 0;
            // 
            // ExpTree
            // 
            this.Controls.Add(_tv1);
            this.Name = "ExpTree";
            this.Size = new Size(200, 264);
            this.ResumeLayout(false);

        }

        public override bool AllowDrop
        {
            get
            {
                return !(DragDropHandler == null);
            }
            set
            {
                if (value)
                {
                    if (DragDropHandler == null)
                    {
                        if (tv1.IsHandleCreated)
                        {
                            if (Application.OleRequired() == ApartmentState.STA)
                            {
                                DragDropHandler = new TVDragWrapper(tv1);
                                int res;
                                res = ShellDll.RegisterDragDrop(tv1.Handle, DragDropHandler);
                                if (!(res == 0) | res == -2147221247)
                                {
                                    DragDropHandler.Dispose();
                                    DragDropHandler = default;
                                    Marshal.ThrowExceptionForHR(res);
                                    // Throw New Exception("Failed to Register DragDrop for " & Me.Name)
                                }
                            }
                            else
                            {
                                throw new ThreadStateException("ThreadMustBeSTA");
                            }
                        }
                    }
                }
                else if (!(DragDropHandler == null))
                {
                    int res;
                    res = ShellDll.RevokeDragDrop(tv1.Handle);
                    if (res != 0) Debug.WriteLine("RevokeDragDrop returned " + res);
                    DragDropHandler.Dispose();
                    DragDropHandler = default;
                }
            }
        }

        // <Summary>
        // RootItem is a Run-Time only Property
        // Setting this Item via an External call results in
        // re-setting the entire tree to be rooted in the 
        // input CShItem
        // The new CShItem must be a valid CShItem of some kind
        // of Folder (File Folder or System Folder)
        // Attempts to set it using a non-Folder CShItem are ignored
        // </Summary>
        [Browsable(false)]
        public CShItem RootItem
        {
            get
            {
                return (Root.Tag as CShItem);
            }
            set
            {
                if (value.IsFolder)
                {
                    if (!(Root == null)) ClearTree();

                    Root = new TreeNode(value.DisplayName);
                    BuildTree(value.GetDirectories());
                    Root.ImageIndex = SystemImageListManager.GetIconIndex(ref value, false);
                    Root.SelectedImageIndex = Root.ImageIndex;
                    Root.Tag = value;
                    tv1.Nodes.Add(Root);
                    Root.Expand();
                    tv1.SelectedNode = Root;
                }
            }
        }

        [Browsable(false)]
        public CShItem SelectedItem
        {
            get
            {
                if (!(tv1.SelectedNode == null)) return (tv1.SelectedNode.Tag as CShItem);
                else return default;
            }
        }

        [Category("Options")]
        [Description("Show Hidden Directories.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool ShowHiddenFolders
        {
            get => m_showHiddenFolders;
            set => m_showHiddenFolders = value;
        }

        [Category("Options")]
        [Description("Allow Collapse of Root Item.")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool ShowRootLines
        {
            get
            {
                return tv1.ShowRootLines;
            }
            set
            {
                if (!(value == tv1.ShowRootLines))
                {
                    tv1.ShowRootLines = value;
                    tv1.Refresh();
                }
            }
        }

        public enum StartDir : int
        {
            Desktop = 0x0,
            Programs = 0x2,
            Controls = 0x3,
            Printers = 0x4,
            Personal = 0x5,
            Favorites = 0x6,
            Startup = 0x7,
            Recent = 0x8,
            SendTo = 0x9,
            StartMenu = 0xB,
            MyDocuments = 0xC,
            // MyMusic = &HD
            // MyVideo = &HE
            DesktopDirectory = 0x10,
            MyComputer = 0x11,
            My_Network_Places = 0x12,
            // NETHOOD = &H13
            // FONTS = &H14
            ApplicatationData = 0x1A,
            // PRINTHOOD = &H1B
            Internet_Cache = 0x20,
            Cookies = 0x21,
            History = 0x22,
            Windows = 0x24,
            System = 0x25,
            Program_Files = 0x26,
            MyPictures = 0x27,
            Profile = 0x28,
            Systemx86 = 0x29,
            AdminTools = 0x30
        }

        private StartDir m_StartUpDirectory = StartDir.Desktop;

        [Category("Options")]
        [Description("Sets the Initial Directory of the Tree")]
        [DefaultValue(StartDir.Desktop)]
        [Browsable(true)]
        public StartDir StartUpDirectory
        {
            get
            {
                return m_StartUpDirectory;
            }
            set
            {
                if (Array.IndexOf(Enum.GetValues(value.GetType()), value) >= 0)
                {
                    m_StartUpDirectory = value;
                    StartUpDirectoryChanged?.Invoke(value);
                }
                else
                {
                    throw new ApplicationException("Invalid Initial StartUpDirectory");
                }
            }
        }

        /// <Summary>RefreshTree Method thanks to Calum McLellan</Summary>
        [Description("Refresh the Tree and all nodes through the currently selected item")]
        public void RefreshTree(CShItem rootCSI = default)
        {
            // Modified to use ExpandANode(CShItem) rather than ExpandANode(path)
            // Set refresh variable for BeforeExpand method
            EnableEventPost = false;
            // Begin Calum's change -- With some modification
            TreeNode Selnode;
            if (tv1.SelectedNode == null) Selnode = Root;
            else Selnode = tv1.SelectedNode;
            // End Calum's change
            try
            {
                tv1.BeginUpdate();
                CShItem SelCSI = (Selnode.Tag as CShItem);

                // Set root node
                if (rootCSI == null) RootItem = RootItem;
                else RootItem = rootCSI;
                // Try to expand the node
                if (!ExpandANode(SelCSI))
                {
                    var nodeList = new ArrayList();
                    while (!(Selnode.Parent == null))
                    {
                        nodeList.Add(Selnode.Parent);
                        Selnode = Selnode.Parent;
                    }

                    foreach (TreeNode currentSelnode in nodeList)
                    {
                        Selnode = currentSelnode;
                        if (ExpandANode((CShItem)Selnode.Tag)) break;
                    }
                }
            }
            // Reset refresh variable for BeforeExpand method
            finally
            {
                tv1.EndUpdate();
            }
            EnableEventPost = true;
            // We suppressed EventPosting during refresh, so give it one now
            tv1_AfterSelect(this, new TreeViewEventArgs(tv1.SelectedNode));
        }

        public bool ExpandANode(string newPath)
        {
            bool ExpandANodeRet = default;
            ExpandANodeRet = false;     // assume failure
            CShItem newItem;
            try
            {
                newItem = CShItem.GetCShItem(newPath);
                if (newItem is null) return ExpandANodeRet;
                if (!newItem.IsFolder) return ExpandANodeRet;
            }
            catch
            {
                return default;
            }
            return ExpandANode(newItem);
        }

        public bool ExpandANode(CShItem newItem)
        {
            bool ExpandANodeRet = default;
            ExpandANodeRet = false;     // assume failure
            var baseNode = Root;
            tv1.BeginUpdate();
            // do the drill down -- Node to expand must be included in tree
            baseNode.Expand(); // Ensure base is filled in
            int lim = CShItem.PidlCount(newItem.PIDL) - CShItem.PidlCount((baseNode.Tag as CShItem).PIDL);
            // TODO: Test ExpandARow again on XP to ensure that the CP problem ix fixed
            while (lim > 0)
            {
                foreach (TreeNode testNode in baseNode.Nodes)
                {
                    if (CShItem.IsAncestorOf((testNode.Tag as CShItem), newItem, false))
                    {
                        baseNode = testNode;
                        RefreshNode(baseNode);   // ensure up-to-date
                        baseNode.Expand();
                        lim -= 1;
                        goto NEXLEV;
                    }
                }
                goto XIT;     // on falling thru For, we can't find it, so get out
                NEXLEV:
                ;
            }
            // after falling thru here, we have found & expanded the node
            tv1.HideSelection = false;
            this.Select();
            tv1.SelectedNode = baseNode;
            ExpandANodeRet = true;
            XIT:
            ;
            tv1.EndUpdate();
            return ExpandANodeRet;
        }

        private void OnStartUpDirectoryChanged(StartDir newVal)
        {
            if (!(Root == null)) ClearTree();

            CShItem special;
            special = CShItem.GetCShItem((ShellDll.CSIDL)Conversion.Val(m_StartUpDirectory));
            Root = new TreeNode(special.DisplayName);
            Root.ImageIndex = SystemImageListManager.GetIconIndex(ref special, false);
            Root.SelectedImageIndex = Root.ImageIndex;
            Root.Tag = special;
            BuildTree(special.GetDirectories());
            tv1.Nodes.Add(Root);
            Root.Expand();
        }

        private void BuildTree(ArrayList L1)
        {
            L1.Sort();
            foreach (CShItem CSI in L1)
                if (!(CSI.IsHidden & !m_showHiddenFolders))
                    Root.Nodes.Add(MakeNode(CSI));
        }

        /// <summary>
        /// Creates a TreeNode whose .Text is the DisplayName of the CShItem.<br />
        /// Sets the IconIndexes for that TreeNode from the CShItem.<br />
        /// Sets the Tag of the TreeNode to the CShItem<br />
        /// If the CShItem (a Folder) has or may have sub-Folders (see Remarks), adds a Dummy node to
        ///   the TreeNode's .Nodes collection. This is always done if the input CShItem represents a Removable device. Checking
        ///   further on such devices may cause unacceptable delays.
        /// Returns the complete TreeNode.
        /// </summary>
        /// <param name="item">The CShItem to make a TreeNode to represent.</param>
        /// <returns>A TreeNode set up to represent the CShItem.</returns>
        /// <remarks>
        /// This routine will not be called if the CShItem (a Folder) is Hidden and ExpTree's ShowHidden Property is False.<br />
        /// If the Folder is Hidden and ShowHidden is True, then this routine will be called.<br />
        /// If the Folder is Hidden and it only contains Hidden Folders (files are not considered here), then, 
        /// the HasSubFolders attribute may be returned False even though Hidden Folders exist. In that case, we 
        /// must make an extra check to ensure that the TreeNode is expandable.<br />
        /// 
        /// There are additional complication with HasSubFolders. 
        /// <ul>
        /// <li>
        /// On XP and earlier systems, HasSubFolders was always
        /// returned True if the Folder was on a Remote system. On Vista and above, the OS would check and return an 
        /// accurate value. This extra check can take a long time on Remote systems - approximately the same amount of time as checking
        /// item.GetDirectories.Count. Versions 2.12 and above of ExpTreeLib have a modified HasSubFolders Property which will always
        /// return True if the Folder is on a Remote system, restoring XP behavior.</li>
        /// <li>
        /// On XP and earlier systems, compressed files (.zip, .cab, etc) were treated as files. On Vista and above, they are treated
        /// as Folders. ExpTreeLib continues to treat such files as files. The HasSubFolder attribute will report a Folder which
        /// contains only compressed files as True. In MakeNode, I simply accept the Vista and above interpretation, setting a dummy
        /// node in such a Folder. An attempt to expand such a TreeNode will just turn off the expansion marker.
        /// </li>
        /// </ul>
        /// </remarks>
        private TreeNode MakeNode(CShItem item)
        {
            var newNode = new TreeNode(item.DisplayName);
            newNode.Tag = item;
            newNode.ImageIndex = SystemImageListManager.GetIconIndex(ref item, false);
            newNode.SelectedImageIndex = SystemImageListManager.GetIconIndex(ref item, true);

            if (item.IsRemovable) newNode.Nodes.Add(new TreeNode(" : "));
            else if (item.HasSubFolders) newNode.Nodes.Add(new TreeNode(" : "));
            else if (item.IsHidden && item.GetDirectories().Count > 0) newNode.Nodes.Add(new TreeNode(" : "));

            return newNode;
        }

        private void ClearTree()
        {
            tv1.Nodes.Clear();
            Root = null;
        }

        private void tv1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            Cursor oldCursor = Cursor;
            Cursor = Cursors.WaitCursor;
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text.Equals(" : "))
            {
                e.Node.Nodes.Clear();
                CShItem CSI = (e.Node.Tag as CShItem);
                var StTime = DateTime.Now;
                ArrayList D = CSI.GetDirectories();

                if (D.Count > 0)
                {
                    D.Sort();    // uses the class comparer
                    foreach (CShItem item in D)
                        if (!(item.IsHidden & !m_showHiddenFolders))
                            e.Node.Nodes.Add(MakeNode(item));
                }
                Debug.WriteLine("Expanding " + e.Node.Text + " " + DateTime.Now.Subtract(StTime).TotalMilliseconds.ToString() + "ms");
            }
            else    // Ensure content is accurate
            {
                RefreshNode(e.Node);
            }
            Cursor = oldCursor;
        }

        private void tv1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            CShItem CSI = (e.Node.Tag as CShItem);
            if (object.ReferenceEquals(CSI, Root.Tag) && !tv1.ShowRootLines)
            {
                {
                    var withBlock = tv1;
                    try
                    {
                        withBlock.BeginUpdate();
                        withBlock.ShowRootLines = true;
                        RefreshNode(node);
                        withBlock.ShowRootLines = false;
                    }
                    finally
                    {
                        withBlock.EndUpdate();
                    }
                }
            }
            else
            {
                RefreshNode(node);
            }

            if (EnableEventPost) // turned off during RefreshTree
            {
                if (CSI.Path.StartsWith(":")) ExpTreeNodeSelected?.Invoke(CSI.DisplayName, CSI);
                else ExpTreeNodeSelected?.Invoke(CSI.Path, CSI);
            }
        }

        private void RefreshNode(TreeNode thisRoot)
        {
            if (!(thisRoot.Nodes.Count == 1 && thisRoot.Nodes[0].Text.Equals(" : ")))
            {
                CShItem thisItem = (thisRoot.Tag as CShItem);
                if (thisItem.RefreshDirectories())   // RefreshDirectories True = the contained list of Directories has changed
                {
                    ArrayList curDirs = thisItem.GetDirectories(false); // suppress 2nd refresh
                    var delNodes = new ArrayList();
                    TreeNode node;
                    foreach (TreeNode currentNode in thisRoot.Nodes)
                    {
                        node = currentNode; // this is the old node contents
                        int i;
                        var loopTo = curDirs.Count - 1;
                        for (i = 0; i <= loopTo; i++)
                        {
                            if (((CShItem)curDirs[i]).Equals(node.Tag))
                            {
                                curDirs.RemoveAt(i);   // found it, don't compare again
                                goto NXTOLD;
                            }
                        }
                        // fall thru = node no longer here
                        delNodes.Add(node);
                        NXTOLD:;
                    }
                    if (delNodes.Count + curDirs.Count > 0)  // had changes
                    {
                        try
                        {
                            tv1.BeginUpdate();
                            foreach (TreeNode currentNode1 in delNodes)
                            {
                                node = currentNode1; // dir not here anymore, delete node
                                thisRoot.Nodes.Remove(node);
                                // any CShItems remaining in curDirs is a new dir under thisRoot
                            }

                            foreach (CShItem csi in curDirs)
                                if (!(csi.IsHidden & !m_showHiddenFolders)) 
                                    thisRoot.Nodes.Add(MakeNode(csi));
                            // we only need to resort if we added
                            // sort is based on CShItem in .Tag
                            if (curDirs.Count > 0)
                            {
                                var tmpA = new TreeNode[thisRoot.Nodes.Count];
                                thisRoot.Nodes.CopyTo(tmpA, 0);
                                Array.Sort(tmpA, new TagComparer());
                                thisRoot.Nodes.Clear();
                                thisRoot.Nodes.AddRange(tmpA);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Error in RefreshNode -- " + ex.ToString() + Constants.vbCrLf + ex.StackTrace);
                        }
                        finally
                        {
                            tv1.EndUpdate();
                        }
                    }
                }
            }
        }

        /// <Summary>When a form containing this control is Hidden and then re-Shown,
        /// the association to the SystemImageList is lost.  Also lost is the
        /// Expanded state of the various TreeNodes. 
        /// The VisibleChanged Event occurs when the form is re-shown (and other times
        /// as well).  
        /// We re-establish the SystemImageList as the ImageList for the TreeView and
        /// restore at least some of the Expansion.</Summary>
        private void tv1_VisibleChanged(object sender, EventArgs e)
        {
            if (tv1.Visible)
            {
                SystemImageListManager.SetTreeViewImageList(tv1, false);
                if (Root != null)
                {
                    Root.Expand();
                    if (!(tv1.SelectedNode == null)) tv1.SelectedNode.Expand();
                    else tv1.SelectedNode = Root;
                }
            }
        }

        /// <Summary>Should never occur since if the condition tested for is True,
        /// the user should never be able to Collapse the node. However, it is
        /// theoretically possible for the code to request a collapse of this node
        /// If it occurs, cancel it</Summary>
        private void tv1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!tv1.ShowRootLines && object.ReferenceEquals(e.Node, Root)) e.Cancel = true;
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private void tv1_HandleCreated(object sender, EventArgs e) => 
            SetWindowTheme(tv1.Handle, "explorer", default);

        private void tv1_HandleDestroyed(object sender, EventArgs e) =>
            AllowDrop = false;

        /// <Summary>Given a CShItem, find the TreeNode that belongs to the
        /// equivalent (matching PIDL) CShItem's most immediate surviving ancestor.
        /// Note: referential comparison might not work since there is no guarantee
        /// that the exact same CShItem is stored in the tree.</Summary>
        /// <returns> Me.Root if not found, otherwise the Treenode whose .Tag is
        /// equivalent to the input CShItem's most immediate surviving ancestor </returns>
        private TreeNode FindAncestorNode(CShItem CSI)
        {
            TreeNode FindAncestorNodeRet = default;
            FindAncestorNodeRet = default;
            if (!CSI.IsFolder) return FindAncestorNodeRet; // only folders in tree
            var baseNode = Root;
            int lim = PidlCount(CSI.PIDL) - PidlCount((baseNode.Tag as CShItem).PIDL);
            while (lim > 1)
            {
                foreach (TreeNode testNode in baseNode.Nodes)
                {
                    if (CShItem.IsAncestorOf((testNode.Tag as CShItem), CSI, false))
                    {
                        baseNode = testNode;
                        baseNode.Expand();
                        lim -= 1;
                        goto NEXTLEV;
                    }
                }
                // CSI's Ancestor may have moved or been deleted, return the last one
                // found (if none, will return Me.Root)
                return baseNode;
                NEXTLEV:;
            }
            // on fall thru, we have it
            return baseNode;
        }

        private void tv1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Primary (internal) data type
            var toDrag = new ArrayList();
            CShItem csi = (((TreeNode)e.Item).Tag as CShItem);
            toDrag.Add(csi);

            // also need Shell IDList Array
            MemoryStream MS;
            MS = CProcDataObject.MakeShellIDArray(toDrag);

            // Fairly universal data type (must be an array)
            var strD = new string[1];
            strD[0] = csi.Path;

            // Build data to drag
            var dataObj = new DataObject();
            dataObj.SetData(toDrag);
            if (!(MS == null)) dataObj.SetData("Shell IDList Array", true, MS);
            dataObj.SetData("FileDrop", true, strD);

            // Do drag, allowing Copy and Move
            DragDropEffects ddeff;
            ddeff = tv1.DoDragDrop(dataObj, DragDropEffects.Copy | DragDropEffects.Move);

            // the following line commented out, since we can't depend on ddeff
            // If ddeff = DragDropEffects.None Then Exit Sub 'nothing happened
            RefreshNode(FindAncestorNode(csi));
        }

        // dropNode is the TreeNode that most recently was DraggedOver or
        // Dropped onto.  
        private TreeNode dropNode;
        private System.Windows.Forms.Timer expandNodeTimer;

        private void expandNodeTimer_Tick(object sender, EventArgs e)
        {
            expandNodeTimer.Stop();
            if (!(dropNode == null))
            {
                this.DragDropHandler.ShDragOver -= DragWrapper_ShDragOver;
                try
                {
                    tv1.BeginUpdate();
                    dropNode.Expand();
                    dropNode.EnsureVisible();
                }
                finally
                {
                    tv1.EndUpdate();
                }
                this.DragDropHandler.ShDragOver += DragWrapper_ShDragOver;
            }
        }

        /// <Summary>ShDragEnter does nothing. It is here for debug tracking</Summary>
        private void DragWrapper_ShDragEnter(ArrayList Draglist, IntPtr pDataObj, int grfKeyState, int pdwEffect)
        {
        }

        /// <Summary>Drag has left the control. Cleanup what we have to</Summary>
        private void DragWrapper_ShDragLeave()
        {
            expandNodeTimer.Stop();    // shut off the dragging over nodes timer
            if (!(dropNode == null)) ResetTreeviewNodeColor(dropNode);
            dropNode = null;
        }

        /// <Summary>ShDragOver manages the appearance of the TreeView.  Management of
        /// the underlying FolderItem is done in DragWrapper
        /// Credit to Cory Smith for TreeView colorizing technique and code,
        /// at http://addressof.com/blog/archive/2004/10/01/955.aspx
        /// Node expansion based on expandNodeTimer added by me.
        /// </Summary>
        private void DragWrapper_ShDragOver(object Node, Point pt, int grfKeyState, int pdwEffect)
        {
            if (Node == null)  // clean up node stuff & fix color. Leave Draginfo alone-cleaned up on DragLeave
            {
                expandNodeTimer.Stop();
                if (dropNode != null)
                {
                    //ResetTreeviewNodeColor(dropNode);
                    dropNode = null;
                }
            }
            else  // Drag is Over a node - fix color & DragDropEffects
            {
                if (object.ReferenceEquals(Node, dropNode)) return; // we've already done it all

                expandNodeTimer.Stop(); // not over previous node anymore
                try
                {
                    tv1.BeginUpdate();
                    int delta = tv1.Height - pt.Y;
                    if (delta < tv1.Height / 2 & delta > 0)
                    {
                        if (!(Node == null) && (Node as TreeNode).NextVisibleNode != null) 
                            (Node as TreeNode).NextVisibleNode.EnsureVisible();
                    }
                    if (delta > tv1.Height / 2 & delta < tv1.Height)
                    {
                        if (!((Node as TreeNode) == null) && (Node as TreeNode).PrevVisibleNode != null)
                            (Node as TreeNode).PrevVisibleNode.EnsureVisible();
                    }
                    if (!(Node as TreeNode).BackColor.Equals(SystemColors.Highlight))
                    {
                        ResetTreeviewNodeColor(tv1.Nodes[0]);
                        (Node as TreeNode).BackColor = SystemColors.Highlight;
                        (Node as TreeNode).ForeColor = SystemColors.HighlightText;
                    }
                }
                finally
                {
                    tv1.EndUpdate();
                }

                dropNode = (Node as TreeNode); // dropNode is the Saved Global version of Node
                if (!dropNode.IsExpanded)
                {
                    expandNodeTimer.Interval = 1200;
                    expandNodeTimer.Start();
                }
            }
        }

        private void DragWrapper_ShDragDrop(ArrayList DragList, object Node, int grfKeyState, int pdwEffect)
        {
            expandNodeTimer.Stop();

            if (!(dropNode == null))
            {
                ResetTreeviewNodeColor(dropNode);
            }
            else
            {
                ResetTreeviewNodeColor(tv1.Nodes[0]);
                // If Directories were Moved, we must find and update the DragSource TreeNodes
                // of course, it is possible that the Drag was external to the App and 
                // the DragSource TreeNode might not exist in the Tree
                // All of this is somewhat chancy since we can't count on pdwEffect or
                // on a Move having actually started, let alone finished
            }      // that is what is in DragList

            foreach (CShItem CSI in DragList)
                if (CSI.IsFolder) RefreshNode(FindAncestorNode(CSI)); // only care about Folders
            if (object.ReferenceEquals(tv1.SelectedNode, dropNode))   // Fake a reselect
            {
                var e = new TreeViewEventArgs(tv1.SelectedNode, TreeViewAction.Unknown);
                tv1_AfterSelect(tv1, e);      // will do a RefreshNode and raise AfterNodeSelect Event
            }
            else
            {
                RefreshNode(dropNode);        // Otherwise, just refresh the Target
                if (pdwEffect != (int)DragDropEffects.Copy && pdwEffect != (int)DragDropEffects.Link)
                {
                    // it may have been a move. if so need to do an AfterSelect on the DragSource if it is SelectedNode
                    if (DragList.Count > 0)     // can't happen but check
                    {
                        if (!(tv1.SelectedNode == null))     // ditto
                        {
                            CShItem csiSel = (tv1.SelectedNode.Tag as CShItem);
                            CShItem csiSource = (DragList[0] as CShItem);  // assume all from same dir
                            if (CShItem.IsAncestorOf(csiSel, csiSource)) // also true for equality
                            {
                                var e = new TreeViewEventArgs(tv1.SelectedNode, TreeViewAction.Unknown);
                                tv1_AfterSelect(tv1, e);      // will do a RefreshNode and raise AfterNodeSelect Event
                            }
                        }
                    }
                }
            }
            dropNode = null;
        }

        private void ResetTreeviewNodeColor(TreeNode node)
        {
            if (!node.BackColor.Equals(Color.Empty))
            {
                node.BackColor = Color.Empty;
                node.ForeColor = Color.Empty;
            }
            if (node.FirstNode != null && node.IsExpanded)
            {
                foreach (TreeNode child in node.Nodes)
                {
                    if (!child.BackColor.Equals(Color.Empty))
                    {
                        child.BackColor = Color.Empty;
                        child.ForeColor = Color.Empty;
                    }
                    if (child.FirstNode != null && child.IsExpanded)
                    {
                        ResetTreeviewNodeColor(child);
                    }
                }
            }
        }
    }
}