using System;
using System.Drawing;
using System.Windows.Forms;

namespace FreeIDE.Components
{
    public partial class BorderLessFormController : BorderLessForm
    {
        public bool fullScreen = false;
        protected Size saveMinSize;
        protected Point saveMinPos;

        public BorderLessFormController()
        {
            this.InitializeComponent();
        }

        private void TitleLabel_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMoveMethod(e);
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
            if (fullScreen)
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
            if (invert) fullScreen = !fullScreen;
        }

        private protected override void OnHeaderMouseMove(MouseEventArgs e)
        {
            if (fullScreen)
            {
                this.Location = saveMinPos;
                this.Size = saveMinSize;
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
    }
}
