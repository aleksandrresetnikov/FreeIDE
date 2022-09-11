namespace FreeIDE.Forms
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.solutionFileTreeView = new FreeIDE.Controls.FileTreeView();
            this.solutionFileTreeView_ImageList = new System.Windows.Forms.ImageList(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.existingProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.initialWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lastFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAmongAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amongAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.findFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadOpenFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAllFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overAllWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.theCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.constructorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.solutionTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terminalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainContainerPanel = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.mainContainerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonClose.Location = new System.Drawing.Point(775, 1);
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.TitleLabel.Location = new System.Drawing.Point(5, 6);
            this.TitleLabel.Size = new System.Drawing.Size(58, 15);
            this.TitleLabel.Text = "Free IDE";
            // 
            // buttonMaxType
            // 
            this.buttonMaxType.FlatAppearance.BorderSize = 0;
            this.buttonMaxType.Location = new System.Drawing.Point(751, 1);
            // 
            // buttonMinType
            // 
            this.buttonMinType.FlatAppearance.BorderSize = 0;
            this.buttonMinType.Location = new System.Drawing.Point(727, 1);
            // 
            // timer
            // 
            this.timer.Enabled = false;
            // 
            // solutionFileTreeView
            // 
            this.solutionFileTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.solutionFileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionFileTreeView.ImageIndex = 0;
            this.solutionFileTreeView.ImageList = this.solutionFileTreeView_ImageList;
            this.solutionFileTreeView.Location = new System.Drawing.Point(0, 0);
            this.solutionFileTreeView.Name = "solutionFileTreeView";
            this.solutionFileTreeView.SelectedImageIndex = 0;
            this.solutionFileTreeView.ShowLines = false;
            this.solutionFileTreeView.Size = new System.Drawing.Size(257, 387);
            this.solutionFileTreeView.StateImageList = this.solutionFileTreeView_ImageList;
            this.solutionFileTreeView.TabIndex = 7;
            // 
            // solutionFileTreeView_ImageList
            // 
            this.solutionFileTreeView_ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("solutionFileTreeView_ImageList.ImageStream")));
            this.solutionFileTreeView_ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.solutionFileTreeView_ImageList.Images.SetKeyName(0, "Folder");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(1, "OpenFolder");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(2, "File");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(3, "Textfile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(4, "XMLFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(5, "Image");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(6, "XAML");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(7, "Map");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(8, "HTML");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(9, "CSS");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(10, "CPPFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(11, "CSharpFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(12, "FSharpFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(13, "VBFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(14, "PYFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(15, "FontFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(16, "ObjectFile");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(17, "Assembly");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(18, "Frame");
            this.solutionFileTreeView_ImageList.Images.SetKeyName(19, "Soundfile");
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.mainMenuStrip);
            this.mainPanel.Location = new System.Drawing.Point(5, 30);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(790, 414);
            this.mainPanel.TabIndex = 8;
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.projectToolStripMenuItem3});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(790, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.openToolStripMenuItem,
            this.addToolStripMenuItem,
            this.toolStripSeparator1,
            this.initialWindowToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator3,
            this.printingToolStripMenuItem,
            this.toolStripSeparator4,
            this.lastFilesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newSolutionToolStripMenuItem,
            this.projectToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // newSolutionToolStripMenuItem
            // 
            this.newSolutionToolStripMenuItem.Name = "newSolutionToolStripMenuItem";
            this.newSolutionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newSolutionToolStripMenuItem.Text = "New Solution";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.projectToolStripMenuItem.Text = "New Project";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionToolStripMenuItem,
            this.projectToolStripMenuItem1,
            this.folderToolStripMenuItem});
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // solutionToolStripMenuItem
            // 
            this.solutionToolStripMenuItem.Name = "solutionToolStripMenuItem";
            this.solutionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.solutionToolStripMenuItem.Text = "Solution";
            // 
            // projectToolStripMenuItem1
            // 
            this.projectToolStripMenuItem1.Name = "projectToolStripMenuItem1";
            this.projectToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.projectToolStripMenuItem1.Text = "Project";
            // 
            // folderToolStripMenuItem
            // 
            this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            this.folderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.folderToolStripMenuItem.Text = "Folder";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem2,
            this.existingProjectToolStripMenuItem});
            this.addToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addToolStripMenuItem.Image")));
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // projectToolStripMenuItem2
            // 
            this.projectToolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("projectToolStripMenuItem2.Image")));
            this.projectToolStripMenuItem2.Name = "projectToolStripMenuItem2";
            this.projectToolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.projectToolStripMenuItem2.Text = "New Project";
            // 
            // existingProjectToolStripMenuItem
            // 
            this.existingProjectToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("existingProjectToolStripMenuItem.Image")));
            this.existingProjectToolStripMenuItem.Name = "existingProjectToolStripMenuItem";
            this.existingProjectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.existingProjectToolStripMenuItem.Text = "Existing Project";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // initialWindowToolStripMenuItem
            // 
            this.initialWindowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("initialWindowToolStripMenuItem.Image")));
            this.initialWindowToolStripMenuItem.Name = "initialWindowToolStripMenuItem";
            this.initialWindowToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.initialWindowToolStripMenuItem.Text = "Initial Window";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAllToolStripMenuItem.Image")));
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // printingToolStripMenuItem
            // 
            this.printingToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printingToolStripMenuItem.Image")));
            this.printingToolStripMenuItem.Name = "printingToolStripMenuItem";
            this.printingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.printingToolStripMenuItem.Text = "Printing";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // lastFilesToolStripMenuItem
            // 
            this.lastFilesToolStripMenuItem.Name = "lastFilesToolStripMenuItem";
            this.lastFilesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.lastFilesToolStripMenuItem.Text = "Last Files";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gotoToolStripMenuItem,
            this.toolStripSeparator5,
            this.findToolStripMenuItem,
            this.findAmongAllFilesToolStripMenuItem,
            this.searchAndReplaceToolStripMenuItem,
            this.amongAllFilesToolStripMenuItem,
            this.toolStripSeparator6,
            this.findFileToolStripMenuItem,
            this.toolStripSeparator7,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator8,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator9,
            this.selectAllToolStripMenuItem,
            this.toolStripSeparator10,
            this.reloadOpenFilesToolStripMenuItem,
            this.reloadAllFilesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.gotoToolStripMenuItem.Text = "Goto";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(257, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // findAmongAllFilesToolStripMenuItem
            // 
            this.findAmongAllFilesToolStripMenuItem.Name = "findAmongAllFilesToolStripMenuItem";
            this.findAmongAllFilesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.findAmongAllFilesToolStripMenuItem.Text = "Find among all files";
            // 
            // searchAndReplaceToolStripMenuItem
            // 
            this.searchAndReplaceToolStripMenuItem.Name = "searchAndReplaceToolStripMenuItem";
            this.searchAndReplaceToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.searchAndReplaceToolStripMenuItem.Text = "Find and Replace";
            // 
            // amongAllFilesToolStripMenuItem
            // 
            this.amongAllFilesToolStripMenuItem.Name = "amongAllFilesToolStripMenuItem";
            this.amongAllFilesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.amongAllFilesToolStripMenuItem.Text = "Find and Replace among all files";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(257, 6);
            // 
            // findFileToolStripMenuItem
            // 
            this.findFileToolStripMenuItem.Name = "findFileToolStripMenuItem";
            this.findFileToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.findFileToolStripMenuItem.Text = "Find file";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(257, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(257, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(257, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(257, 6);
            // 
            // reloadOpenFilesToolStripMenuItem
            // 
            this.reloadOpenFilesToolStripMenuItem.Name = "reloadOpenFilesToolStripMenuItem";
            this.reloadOpenFilesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.reloadOpenFilesToolStripMenuItem.Text = "Reload Open Files";
            // 
            // reloadAllFilesToolStripMenuItem
            // 
            this.reloadAllFilesToolStripMenuItem.Name = "reloadAllFilesToolStripMenuItem";
            this.reloadAllFilesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.reloadAllFilesToolStripMenuItem.Text = "Reload All Files";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overAllWindowsToolStripMenuItem,
            this.toolStripSeparator12,
            this.theCodeToolStripMenuItem,
            this.constructorToolStripMenuItem,
            this.toolStripSeparator11,
            this.solutionTreeToolStripMenuItem,
            this.propertiesPanelToolStripMenuItem,
            this.outputPanelToolStripMenuItem,
            this.terminalToolStripMenuItem,
            this.propertiesWindowToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // overAllWindowsToolStripMenuItem
            // 
            this.overAllWindowsToolStripMenuItem.Name = "overAllWindowsToolStripMenuItem";
            this.overAllWindowsToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.overAllWindowsToolStripMenuItem.Text = "Over All Windows";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(175, 6);
            // 
            // theCodeToolStripMenuItem
            // 
            this.theCodeToolStripMenuItem.Name = "theCodeToolStripMenuItem";
            this.theCodeToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.theCodeToolStripMenuItem.Text = "The code";
            // 
            // constructorToolStripMenuItem
            // 
            this.constructorToolStripMenuItem.Name = "constructorToolStripMenuItem";
            this.constructorToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.constructorToolStripMenuItem.Text = "Constructor";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(175, 6);
            // 
            // solutionTreeToolStripMenuItem
            // 
            this.solutionTreeToolStripMenuItem.Name = "solutionTreeToolStripMenuItem";
            this.solutionTreeToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.solutionTreeToolStripMenuItem.Text = "Solution Tree";
            // 
            // propertiesPanelToolStripMenuItem
            // 
            this.propertiesPanelToolStripMenuItem.Name = "propertiesPanelToolStripMenuItem";
            this.propertiesPanelToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.propertiesPanelToolStripMenuItem.Text = "Properties Panel";
            // 
            // outputPanelToolStripMenuItem
            // 
            this.outputPanelToolStripMenuItem.Name = "outputPanelToolStripMenuItem";
            this.outputPanelToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.outputPanelToolStripMenuItem.Text = "Output Panel";
            // 
            // terminalToolStripMenuItem
            // 
            this.terminalToolStripMenuItem.Name = "terminalToolStripMenuItem";
            this.terminalToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.terminalToolStripMenuItem.Text = "Terminal";
            // 
            // propertiesWindowToolStripMenuItem
            // 
            this.propertiesWindowToolStripMenuItem.Name = "propertiesWindowToolStripMenuItem";
            this.propertiesWindowToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.propertiesWindowToolStripMenuItem.Text = "Project Properties";
            // 
            // projectToolStripMenuItem3
            // 
            this.projectToolStripMenuItem3.Name = "projectToolStripMenuItem3";
            this.projectToolStripMenuItem3.Size = new System.Drawing.Size(60, 20);
            this.projectToolStripMenuItem3.Text = "Project";
            // 
            // mainContainerPanel
            // 
            this.mainContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainContainerPanel.Controls.Add(this.splitContainer1);
            this.mainContainerPanel.Controls.Add(this.splitter1);
            this.mainContainerPanel.Location = new System.Drawing.Point(5, 57);
            this.mainContainerPanel.Name = "mainContainerPanel";
            this.mainContainerPanel.Size = new System.Drawing.Size(790, 387);
            this.mainContainerPanel.TabIndex = 8;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.solutionFileTreeView);
            this.splitContainer1.Size = new System.Drawing.Size(787, 387);
            this.splitContainer1.SplitterDistance = 526;
            this.splitContainer1.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 387);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainContainerPanel);
            this.Controls.Add(this.mainPanel);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.Controls.SetChildIndex(this.mainPanel, 0);
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            this.Controls.SetChildIndex(this.mainContainerPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainContainerPanel.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.FileTreeView solutionFileTreeView;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem existingProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem initialWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem printingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem lastFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAmongAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchAndReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem amongAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem findFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem reloadOpenFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadAllFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem theCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem constructorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem solutionTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terminalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem overAllWindowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem propertiesWindowToolStripMenuItem;
        private System.Windows.Forms.ImageList solutionFileTreeView_ImageList;
        private System.Windows.Forms.Panel mainContainerPanel;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}