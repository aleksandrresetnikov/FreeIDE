namespace FreeIDE.Forms.Components
{
    partial class CustomGoToForm
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
            this.label = new System.Windows.Forms.Label();
            this.tbLineNumber = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F);
            this.TitleLabel.Location = new System.Drawing.Point(5, 6);
            this.TitleLabel.Size = new System.Drawing.Size(161, 17);
            // 
            // buttonMaxType
            // 
            this.buttonMaxType.FlatAppearance.BorderSize = 0;
            // 
            // buttonMinType
            // 
            this.buttonMinType.FlatAppearance.BorderSize = 0;
            // 
            // timer
            // 
            this.timer.Enabled = false;
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.label.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label.Location = new System.Drawing.Point(12, 37);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(100, 15);
            this.label.TabIndex = 0;
            this.label.Text = "Line Number (1/1):";
            // 
            // tbLineNumber
            // 
            this.tbLineNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLineNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLineNumber.Font = new System.Drawing.Font("Consolas", 11.25F);
            this.tbLineNumber.Location = new System.Drawing.Point(15, 55);
            this.tbLineNumber.Name = "tbLineNumber";
            this.tbLineNumber.Size = new System.Drawing.Size(321, 18);
            this.tbLineNumber.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btnOk.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOk.Location = new System.Drawing.Point(122, 95);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 28);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft New Tai Lue", 8.25F);
            this.btnCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancel.Location = new System.Drawing.Point(232, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CustomGoToForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(350, 150);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbLineNumber);
            this.Controls.Add(this.label);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 150);
            this.Name = "CustomGoToForm";
            this.Resizeable = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go To Line";
            this.TopMost = true;
            this.Controls.SetChildIndex(this.label, 0);
            this.Controls.SetChildIndex(this.tbLineNumber, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.buttonMaxType, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonMinType, 0);
            this.Controls.SetChildIndex(this.TitleLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox tbLineNumber;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}