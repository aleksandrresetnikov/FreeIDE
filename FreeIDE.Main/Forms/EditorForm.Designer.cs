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
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Узел1");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Узел2");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Узел6");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Узел9");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Узел10");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Узел11");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("Узел12");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Узел13");
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("Узел7", new System.Windows.Forms.TreeNode[] {
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49,
            treeNode50});
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("Узел8");
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("Узел3", new System.Windows.Forms.TreeNode[] {
            treeNode45,
            treeNode51,
            treeNode52});
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("Узел4");
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("Узел5");
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("Stack", new System.Windows.Forms.TreeNode[] {
            treeNode43,
            treeNode44,
            treeNode53,
            treeNode54,
            treeNode55});
            this.fileTreeView1 = new FreeIDE.Controls.FileTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
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
            this.TitleLabel.Size = new System.Drawing.Size(56, 15);
            this.TitleLabel.Text = "FreeIDE";
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
            // fileTreeView1
            // 
            this.fileTreeView1.Location = new System.Drawing.Point(81, 155);
            this.fileTreeView1.Name = "fileTreeView1";
            treeNode43.Name = "Узел1";
            treeNode43.Text = "Узел1";
            treeNode44.Name = "Узел2";
            treeNode44.Text = "Узел2";
            treeNode45.Name = "Узел6";
            treeNode45.Text = "Узел6";
            treeNode46.Name = "Узел9";
            treeNode46.Text = "Узел9";
            treeNode47.Name = "Узел10";
            treeNode47.Text = "Узел10";
            treeNode48.Name = "Узел11";
            treeNode48.Text = "Узел11";
            treeNode49.Name = "Узел12";
            treeNode49.Text = "Узел12";
            treeNode50.Name = "Узел13";
            treeNode50.Text = "Узел13";
            treeNode51.Name = "Узел7";
            treeNode51.Text = "Узел7";
            treeNode52.Name = "Узел8";
            treeNode52.Text = "Узел8";
            treeNode53.Name = "Узел3";
            treeNode53.Text = "Узел3";
            treeNode54.Name = "Узел4";
            treeNode54.Text = "Узел4";
            treeNode55.Name = "Узел5";
            treeNode55.Text = "Узел5";
            treeNode56.Name = "Stack";
            treeNode56.Text = "Stack";
            this.fileTreeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode56});
            this.fileTreeView1.Size = new System.Drawing.Size(199, 188);
            this.fileTreeView1.TabIndex = 7;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.fileTreeView1);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.Controls.SetChildIndex(this.fileTreeView1, 0);
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.FileTreeView fileTreeView1;
    }
}