namespace FreeIDE.Forms.Components
{
    partial class CustomReplaceForm
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
            this.btClose = new System.Windows.Forms.Button();
            this.btFindNext = new System.Windows.Forms.Button();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.cbRegex = new System.Windows.Forms.CheckBox();
            this.cbMatchCase = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbWholeWord = new System.Windows.Forms.CheckBox();
            this.btReplace = new System.Windows.Forms.Button();
            this.btReplaceAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbReplace = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonClose.Location = new System.Drawing.Point(335, 1);
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(5, 6);
            this.TitleLabel.Size = new System.Drawing.Size(161, 17);
            // 
            // buttonMaxType
            // 
            this.buttonMaxType.FlatAppearance.BorderSize = 0;
            this.buttonMaxType.Location = new System.Drawing.Point(310, 1);
            // 
            // buttonMinType
            // 
            this.buttonMinType.FlatAppearance.BorderSize = 0;
            this.buttonMinType.Location = new System.Drawing.Point(285, 1);
            // 
            // timer
            // 
            this.timer.Enabled = false;
            // 
            // btClose
            // 
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btClose.Location = new System.Drawing.Point(236, 219);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(104, 28);
            this.btClose.TabIndex = 8;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btFindNext
            // 
            this.btFindNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btFindNext.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btFindNext.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btFindNext.Location = new System.Drawing.Point(236, 117);
            this.btFindNext.Name = "btFindNext";
            this.btFindNext.Size = new System.Drawing.Size(104, 28);
            this.btFindNext.TabIndex = 5;
            this.btFindNext.Text = "Find next";
            this.btFindNext.UseVisualStyleBackColor = true;
            this.btFindNext.Click += new System.EventHandler(this.btFindNext_Click);
            // 
            // tbFind
            // 
            this.tbFind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFind.Font = new System.Drawing.Font("Consolas", 11.25F);
            this.tbFind.Location = new System.Drawing.Point(68, 12);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(273, 18);
            this.tbFind.TabIndex = 0;
            this.tbFind.TextChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.tbFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFind_KeyPress);
            // 
            // cbRegex
            // 
            this.cbRegex.AutoSize = true;
            this.cbRegex.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.cbRegex.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbRegex.Location = new System.Drawing.Point(20, 167);
            this.cbRegex.Name = "cbRegex";
            this.cbRegex.Size = new System.Drawing.Size(57, 19);
            this.cbRegex.TabIndex = 3;
            this.cbRegex.Text = "Regex";
            this.cbRegex.UseVisualStyleBackColor = true;
            this.cbRegex.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            // 
            // cbMatchCase
            // 
            this.cbMatchCase.AutoSize = true;
            this.cbMatchCase.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.cbMatchCase.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbMatchCase.Location = new System.Drawing.Point(20, 121);
            this.cbMatchCase.Name = "cbMatchCase";
            this.cbMatchCase.Size = new System.Drawing.Size(83, 19);
            this.cbMatchCase.TabIndex = 1;
            this.cbMatchCase.Text = "Match case";
            this.cbMatchCase.UseVisualStyleBackColor = true;
            this.cbMatchCase.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Find: ";
            // 
            // cbWholeWord
            // 
            this.cbWholeWord.AutoSize = true;
            this.cbWholeWord.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.cbWholeWord.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbWholeWord.Location = new System.Drawing.Point(20, 144);
            this.cbWholeWord.Name = "cbWholeWord";
            this.cbWholeWord.Size = new System.Drawing.Size(123, 19);
            this.cbWholeWord.TabIndex = 2;
            this.cbWholeWord.Text = "Match whole word";
            this.cbWholeWord.UseVisualStyleBackColor = true;
            this.cbWholeWord.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            // 
            // btReplace
            // 
            this.btReplace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReplace.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btReplace.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btReplace.Location = new System.Drawing.Point(236, 151);
            this.btReplace.Name = "btReplace";
            this.btReplace.Size = new System.Drawing.Size(104, 28);
            this.btReplace.TabIndex = 6;
            this.btReplace.Text = "Replace";
            this.btReplace.UseVisualStyleBackColor = true;
            this.btReplace.Click += new System.EventHandler(this.btReplace_Click);
            // 
            // btReplaceAll
            // 
            this.btReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btReplaceAll.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btReplaceAll.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btReplaceAll.Location = new System.Drawing.Point(236, 185);
            this.btReplaceAll.Name = "btReplaceAll";
            this.btReplaceAll.Size = new System.Drawing.Size(104, 28);
            this.btReplaceAll.TabIndex = 7;
            this.btReplaceAll.Text = "Replace all";
            this.btReplaceAll.UseVisualStyleBackColor = true;
            this.btReplaceAll.Click += new System.EventHandler(this.btReplaceAll_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(10, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Replace:";
            // 
            // tbReplace
            // 
            this.tbReplace.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbReplace.Font = new System.Drawing.Font("Consolas", 11.25F);
            this.tbReplace.Location = new System.Drawing.Point(68, 40);
            this.tbReplace.Name = "tbReplace";
            this.tbReplace.Size = new System.Drawing.Size(273, 18);
            this.tbReplace.TabIndex = 0;
            this.tbReplace.TextChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.tbReplace.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFind_KeyPress);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tbFind);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tbReplace);
            this.panel1.Location = new System.Drawing.Point(5, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 68);
            this.panel1.TabIndex = 10;
            // 
            // CustomReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 270);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btReplaceAll);
            this.Controls.Add(this.btReplace);
            this.Controls.Add(this.cbWholeWord);
            this.Controls.Add(this.cbMatchCase);
            this.Controls.Add(this.cbRegex);
            this.Controls.Add(this.btFindNext);
            this.Controls.Add(this.btClose);
            this.Name = "CustomReplaceForm";
            this.Resizeable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find and replace";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReplaceForm_FormClosing);
            this.Controls.SetChildIndex(this.btClose, 0);
            this.Controls.SetChildIndex(this.btFindNext, 0);
            this.Controls.SetChildIndex(this.cbRegex, 0);
            this.Controls.SetChildIndex(this.cbMatchCase, 0);
            this.Controls.SetChildIndex(this.cbWholeWord, 0);
            this.Controls.SetChildIndex(this.btReplace, 0);
            this.Controls.SetChildIndex(this.btReplaceAll, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button btFindNext;
        private System.Windows.Forms.CheckBox cbRegex;
        private System.Windows.Forms.CheckBox cbMatchCase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbWholeWord;
        private System.Windows.Forms.Button btReplace;
        private System.Windows.Forms.Button btReplaceAll;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbFind;
        public System.Windows.Forms.TextBox tbReplace;
        private System.Windows.Forms.Panel panel1;
    }
}