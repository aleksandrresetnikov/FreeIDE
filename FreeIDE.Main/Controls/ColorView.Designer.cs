namespace FreeIDE.Controls
{
    partial class ColorView
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.colorViever = new System.Windows.Forms.Panel();
            this.timerHideToolTip = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // colorViever
            // 
            this.colorViever.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colorViever.BackColor = System.Drawing.Color.DimGray;
            this.colorViever.Location = new System.Drawing.Point(1, 1);
            this.colorViever.Name = "colorViever";
            this.colorViever.Size = new System.Drawing.Size(98, 98);
            this.colorViever.TabIndex = 0;
            this.colorViever.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorViever_MouseDown);
            // 
            // timerHideToolTip
            // 
            this.timerHideToolTip.Interval = 1000;
            this.timerHideToolTip.Tick += new System.EventHandler(this.timerHideToolTip_Tick);
            // 
            // ColorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.colorViever);
            this.Name = "ColorView";
            this.Size = new System.Drawing.Size(100, 100);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LPI_ColorView_MouseDown);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel colorViever;
        private System.Windows.Forms.Timer timerHideToolTip;
    }
}
