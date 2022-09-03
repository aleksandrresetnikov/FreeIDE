namespace FreeIDE.Components
{
    partial class BorderLessFormController
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.buttonMaxType = new System.Windows.Forms.Button();
            this.buttonMinType = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = false;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(85)))));
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Tomato;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonClose.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonClose.Location = new System.Drawing.Point(325, 1);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(24, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(85)))));
            this.TitleLabel.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.TitleLabel.Location = new System.Drawing.Point(5, 5);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(203, 15);
            this.TitleLabel.TabIndex = 4;
            this.TitleLabel.Text = "LPI_BorderLessFormController";
            this.TitleLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleLabel_MouseMove);
            // 
            // buttonMaxType
            // 
            this.buttonMaxType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMaxType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(85)))));
            this.buttonMaxType.FlatAppearance.BorderSize = 0;
            this.buttonMaxType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMaxType.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMaxType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonMaxType.Location = new System.Drawing.Point(300, 1);
            this.buttonMaxType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonMaxType.Name = "buttonMaxType";
            this.buttonMaxType.Size = new System.Drawing.Size(24, 23);
            this.buttonMaxType.TabIndex = 5;
            this.buttonMaxType.Text = "🗖";
            this.buttonMaxType.UseVisualStyleBackColor = false;
            this.buttonMaxType.Click += new System.EventHandler(this.buttonMaxType_Click);
            // 
            // buttonMinType
            // 
            this.buttonMinType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMinType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(85)))));
            this.buttonMinType.FlatAppearance.BorderSize = 0;
            this.buttonMinType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMinType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMinType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonMinType.Location = new System.Drawing.Point(275, 1);
            this.buttonMinType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonMinType.Name = "buttonMinType";
            this.buttonMinType.Size = new System.Drawing.Size(24, 23);
            this.buttonMinType.TabIndex = 6;
            this.buttonMinType.Text = "—";
            this.buttonMinType.UseVisualStyleBackColor = false;
            this.buttonMinType.Click += new System.EventHandler(this.buttonMinType_Click);
            // 
            // BorderLessFormController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(38)))), ((int)(((byte)(63)))));
            this.BorderColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 200);
            this.ColorHeaderUnderline = System.Drawing.Color.White;
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.buttonMinType);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonMaxType);
            this.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(85)))));
            this.HeaderHeight = 26;
            this.IconPadding = 2;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MinimumSize = new System.Drawing.Size(350, 200);
            this.Name = "BorderLessFormController";
            this.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ShowHeaderUnderline = true;
            this.ShowIcon = false;
            this.Text = "Добавить зависимость";
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Label TitleLabel;
        public System.Windows.Forms.Button buttonMaxType;
        public System.Windows.Forms.Button buttonMinType;
    }
}