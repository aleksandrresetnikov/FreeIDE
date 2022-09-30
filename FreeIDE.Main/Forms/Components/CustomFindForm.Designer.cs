namespace FreeIDE.Forms.Components
{
    partial class CustomFindForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonClose.Location = new System.Drawing.Point(357, 1);
            // 
            // buttonMaxType
            // 
            this.buttonMaxType.FlatAppearance.BorderSize = 0;
            this.buttonMaxType.Location = new System.Drawing.Point(332, 1);
            // 
            // buttonMinType
            // 
            this.buttonMinType.FlatAppearance.BorderSize = 0;
            this.buttonMinType.Location = new System.Drawing.Point(307, 1);
            // 
            // timer
            // 
            this.timer.Enabled = false;
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClose.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btClose.Location = new System.Drawing.Point(269, 136);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(93, 28);
            this.btClose.TabIndex = 5;
            this.btClose.Text = "Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // btFindNext
            // 
            this.btFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFindNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btFindNext.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btFindNext.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btFindNext.Location = new System.Drawing.Point(269, 102);
            this.btFindNext.Name = "btFindNext";
            this.btFindNext.Size = new System.Drawing.Size(93, 28);
            this.btFindNext.TabIndex = 4;
            this.btFindNext.Text = "Find next";
            this.btFindNext.UseVisualStyleBackColor = true;
            this.btFindNext.Click += new System.EventHandler(this.btFindNext_Click);
            // 
            // tbFind
            // 
            this.tbFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFind.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFind.Location = new System.Drawing.Point(57, 13);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(305, 18);
            this.tbFind.TabIndex = 0;
            this.tbFind.TextChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.tbFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFind_KeyPress);
            // 
            // cbRegex
            // 
            this.cbRegex.AutoSize = true;
            this.cbRegex.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.cbRegex.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbRegex.Location = new System.Drawing.Point(18, 145);
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
            this.cbMatchCase.Location = new System.Drawing.Point(18, 99);
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
            this.label1.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Find: ";
            // 
            // cbWholeWord
            // 
            this.cbWholeWord.AutoSize = true;
            this.cbWholeWord.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.cbWholeWord.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbWholeWord.Location = new System.Drawing.Point(18, 122);
            this.cbWholeWord.Name = "cbWholeWord";
            this.cbWholeWord.Size = new System.Drawing.Size(123, 19);
            this.cbWholeWord.TabIndex = 2;
            this.cbWholeWord.Text = "Match whole word";
            this.cbWholeWord.UseVisualStyleBackColor = true;
            this.cbWholeWord.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbFind);
            this.panel1.Location = new System.Drawing.Point(5, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 43);
            this.panel1.TabIndex = 6;
            // 
            // CustomFindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 200);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbWholeWord);
            this.Controls.Add(this.cbRegex);
            this.Controls.Add(this.btFindNext);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.cbMatchCase);
            this.MinimumSize = new System.Drawing.Size(350, 150);
            this.Name = "CustomFindForm";
            this.Resizeable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindForm_FormClosing);
            this.Controls.SetChildIndex(this.cbMatchCase, 0);
            this.Controls.SetChildIndex(this.btClose, 0);
            this.Controls.SetChildIndex(this.btFindNext, 0);
            this.Controls.SetChildIndex(this.cbRegex, 0);
            this.Controls.SetChildIndex(this.cbWholeWord, 0);
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

        public System.Windows.Forms.Button btClose;
        public System.Windows.Forms.Button btFindNext;
        public System.Windows.Forms.CheckBox cbRegex;
        public System.Windows.Forms.CheckBox cbMatchCase;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox cbWholeWord;
        public System.Windows.Forms.TextBox tbFind;
        private System.Windows.Forms.Panel panel1;
    }
}