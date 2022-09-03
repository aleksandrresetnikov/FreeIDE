using System;
using System.Drawing;
using System.Windows.Forms;

using FreeIDE.Tags;

namespace FreeIDE.Components
{
    public partial class BorderLessFormController : BorderLessForm
    {
        //public bool fullScreen = false;
        private protected Size saveMinSize;
        private protected Point saveMinPos;

        public BorderLessFormController()
        {
            this.InitializeComponent();
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMoveMethod(e);
        }

        private void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDownMethod(e);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonMaxType_Click(object sender, EventArgs e)
        {
            SetMaxSize();
        }

        private void buttonMinType_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void SetMaxSize(bool invert = true)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Location = saveMinPos;
                this.Size = saveMinSize;
                buttonMaxType.Text = "🗖";
            }
            else
            {
                saveMinPos = this.Location;
                saveMinSize = this.Size;
                this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width,
                                     Screen.PrimaryScreen.WorkingArea.Height);
                this.Location = new Point(0, 0);
                buttonMaxType.Text = "❒";
            }

            if (invert)
            {
                this.WindowState = (this.WindowState == FormWindowState.Maximized ?
                    FormWindowState.Normal : FormWindowState.Maximized);
            }
        }

        private protected override void OnHeaderMouseMove(MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Location = saveMinPos;
                this.Size = saveMinSize;
                this.WindowState = FormWindowState.Normal;
                buttonMaxType.Text = "🗖";
            }
            base.OnHeaderMouseMove(e);
        }

        private protected override void OnHeaderMouseDoubleClick(MouseEventArgs e)
        {
            SetMaxSize();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.Clean();
        }

        public override void ApplyTheme(ThemeData themeData)
        {
            base.ApplyTheme(themeData);
            this.OnBasedApplyTheme(themeData);
            this.OnBasedApplyThemeWithTags(themeData);
        }

        private protected virtual void OnBasedApplyTheme(ThemeData themeData)
        {
            // Window state buttons:
            this.buttonClose.BackColor = themeData.HeaderBackColor;
            this.buttonClose.FlatAppearance.MouseOverBackColor = themeData.ButtonClose_MouseOverBackColor;
            this.buttonMaxType.BackColor = themeData.HeaderBackColor;
            this.buttonMinType.BackColor = themeData.HeaderBackColor;

            this.buttonClose.ForeColor = themeData.WindowStateButtonsForeColor;
            this.buttonMaxType.ForeColor = themeData.WindowStateButtonsForeColor;
            this.buttonMinType.ForeColor = themeData.WindowStateButtonsForeColor;

            this.buttonClose.Location = new Point(this.buttonClose.Location.X - (themeData.BorderHeight - 1),
                this.buttonClose.Location.Y + (themeData.BorderHeight - 1));
            this.buttonMaxType.Location = new Point(this.buttonMaxType.Location.X - (themeData.BorderHeight - 1),
                this.buttonMaxType.Location.Y + (themeData.BorderHeight - 1));
            this.buttonMinType.Location = new Point(this.buttonMinType.Location.X - (themeData.BorderHeight - 1),
                this.buttonMinType.Location.Y + (themeData.BorderHeight - 1));

            // Title label:
            this.TitleLabel.BackColor = themeData.HeaderBackColor;
            this.TitleLabel.ForeColor = themeData.TitleLabelForeColor;
        }

        private protected virtual void OnBasedApplyThemeWithTags(ThemeData themeData)
        {
            foreach (Control controlItem in base.Controls)
            {
                if (controlItem.Tag is IThemeTag) ApplyThemeForControl(controlItem);
            }
        }

        private protected virtual void ApplyThemeForControl(Control control)
        {
            IThemeTag themeTag = (control.Tag as IThemeTag);

        }
    }
}
