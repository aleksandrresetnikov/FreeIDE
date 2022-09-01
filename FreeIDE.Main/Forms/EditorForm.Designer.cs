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
            this.smartSyntaxTextBox1 = new FreeIDE.Controls.SmartSyntaxTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smartSyntaxTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonClose.Location = new System.Drawing.Point(775, 1);
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
            // smartSyntaxTextBox1
            // 
            this.smartSyntaxTextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.smartSyntaxTextBox1.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.smartSyntaxTextBox1.BackBrush = null;
            this.smartSyntaxTextBox1.CharHeight = 14;
            this.smartSyntaxTextBox1.CharWidth = 8;
            this.smartSyntaxTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.smartSyntaxTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.smartSyntaxTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.smartSyntaxTextBox1.IsReplaceMode = false;
            this.smartSyntaxTextBox1.Location = new System.Drawing.Point(52, 57);
            this.smartSyntaxTextBox1.Name = "smartSyntaxTextBox1";
            this.smartSyntaxTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.smartSyntaxTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.smartSyntaxTextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("smartSyntaxTextBox1.ServiceColors")));
            this.smartSyntaxTextBox1.Size = new System.Drawing.Size(648, 345);
            this.smartSyntaxTextBox1.TabIndex = 0;
            this.smartSyntaxTextBox1.Text = "smartSyntaxTextBox1";
            this.smartSyntaxTextBox1.Zoom = 100;
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.smartSyntaxTextBox1);
            this.Name = "EditorForm";
            this.Text = "EditorForm";
            this.Controls.SetChildIndex(this.smartSyntaxTextBox1, 0);
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smartSyntaxTextBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.SmartSyntaxTextBox smartSyntaxTextBox1;
    }
}