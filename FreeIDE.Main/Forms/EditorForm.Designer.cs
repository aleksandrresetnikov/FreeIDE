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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Узел1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Узел2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Узел6");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Узел9");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Узел10");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Узел11");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Узел12");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Узел13");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Узел7", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Узел8");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Узел3", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode9,
            treeNode10});
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Узел4");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Узел5");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Stack", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode11,
            treeNode12,
            treeNode13});
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
            this.fileTreeView1.Location = new System.Drawing.Point(157, 67);
            this.fileTreeView1.Name = "fileTreeView1";
            treeNode1.Name = "Узел1";
            treeNode1.Text = "Узел1";
            treeNode2.Name = "Узел2";
            treeNode2.Text = "Узел2";
            treeNode3.Name = "Узел6";
            treeNode3.Text = "Узел6";
            treeNode4.Name = "Узел9";
            treeNode4.Text = "Узел9";
            treeNode5.Name = "Узел10";
            treeNode5.Text = "Узел10";
            treeNode6.Name = "Узел11";
            treeNode6.Text = "Узел11";
            treeNode7.Name = "Узел12";
            treeNode7.Text = "Узел12";
            treeNode8.Name = "Узел13";
            treeNode8.Text = "Узел13";
            treeNode9.Name = "Узел7";
            treeNode9.Text = "Узел7";
            treeNode10.Name = "Узел8";
            treeNode10.Text = "Узел8";
            treeNode11.Name = "Узел3";
            treeNode11.Text = "Узел3";
            treeNode12.Name = "Узел4";
            treeNode12.Text = "Узел4";
            treeNode13.Name = "Узел5";
            treeNode13.Text = "Узел5";
            treeNode14.Name = "Stack";
            treeNode14.Text = "Stack";
            this.fileTreeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode14});
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
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            this.Controls.SetChildIndex(this.fileTreeView1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.FileTreeView fileTreeView1;
    }
}