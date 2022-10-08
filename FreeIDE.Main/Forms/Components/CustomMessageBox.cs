using System;
using System.Drawing;
using System.Windows.Forms;

using FreeIDE.Components;

namespace FreeIDE.Forms.Components
{
    public partial class CustomMessageBox : BorderLessForm
    {
        public bool CheckBoxResult { get => this.checkBox.Checked; }

        public MessageBoxButtons Buttons;
        public CustomMessageBoxIcon MessageIcon;
        public string Caption;
        public string Message;
        public bool ShowCheckBox = false;
        public bool WordWrap = false;
        public bool OverAllWindows = true;
        public string CheckBoxText = "Не показывать снова";

        public CustomMessageBox(string Message)
        {
            this.Buttons = MessageBoxButtons.OK;
            this.Message = Message;

            Init();
        }
        public CustomMessageBox(string Caption, string Message)
        {
            this.Caption = Caption;
            this.Buttons = MessageBoxButtons.OK;
            this.Message = Message;

            Init();
        }
        public CustomMessageBox(string Caption, string Message, MessageBoxButtons Buttons)
        {
            this.Caption = Caption;
            this.Buttons = Buttons;
            this.Message = Message;

            Init();
        }
        public CustomMessageBox(string Caption, string Message, CustomMessageBoxIcon MessageIcon)
        {
            this.Caption = Caption;
            this.MessageIcon = MessageIcon;
            this.Message = Message;

            Init();
        }
        public CustomMessageBox(string Message, MessageBoxButtons Buttons)
        {
            this.Buttons = Buttons;
            this.Message = Message;

            Init();
        }
        public CustomMessageBox(string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon)
        {
            this.Buttons = Buttons;
            this.Message = Message;
            this.MessageIcon = MessageIcon;

            Init();
        }
        public CustomMessageBox(string Message, CustomMessageBoxIcon MessageIcon)
        {
            this.Buttons = MessageBoxButtons.OK;
            this.Message = Message;
            this.MessageIcon = MessageIcon;

            Init();
        }
        public CustomMessageBox(string Caption, string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon)
        {
            this.Caption = Caption;
            this.Buttons = Buttons;
            this.Message = Message;
            this.MessageIcon = MessageIcon;

            Init();
        }

        private void Init()
        {
            this.InitializeComponent();
            this.HideAllButtons();
            this.SetButtons();
            this.ApplyDataSettings();
            this.SetMessageIcon();
            this.CenterToScreen();
        }

        private void ApplyDataSettings()
        {
            this.label1.Text = this.Caption != null ? this.Caption : "";
            this.richTextBox1.Text = this.Message;

            this.checkBox.Visible = this.ShowCheckBox;
            this.checkBox.Text = this.CheckBoxText != null ? this.CheckBoxText : "";

            this.TopMost = this.OverAllWindows;

            /*if (this.ShowCheckBox)
            {
                this.buttonOK.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonCancel.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonAbort.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonYes.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonNo.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonRetry.Location = new Point(this.buttonOK.Location.X, 5);
                this.buttonIgnore.Location = new Point(this.buttonOK.Location.X, 5);
            }*/
        }

        private void LPI_MessageBox_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            this.checkBox.Visible = this.ShowCheckBox;
            this.checkBox.Text = this.CheckBoxText != null ? this.CheckBoxText : "";
            this.richTextBox1.WordWrap = this.WordWrap;

            this.UpdateSize();

            //MessageSounds.MessageSound.Play();
        }
        private void LPI_MessageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer.Enabled = false;
            this.Dispose();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SetButtons()
        {
            switch (this.Buttons)
            {
                case MessageBoxButtons.OK:
                    this.buttonOK.Visible = true;

                    this.buttonOK.Location = this.buttonCancel.Location;
                    return;
                case MessageBoxButtons.OKCancel:
                    this.buttonOK.Visible = true;
                    this.buttonCancel.Visible = true;
                    return;
                case MessageBoxButtons.AbortRetryIgnore:
                    this.buttonAbort.Visible = true;
                    this.buttonRetry.Visible = true;
                    this.buttonIgnore.Visible = true;
                    return;
                case MessageBoxButtons.YesNoCancel:
                    this.buttonYes.Visible = true;
                    this.buttonNo.Visible = true;
                    this.buttonCancel.Visible = true;
                    return;
                case MessageBoxButtons.YesNo:
                    this.buttonYes.Visible = true;
                    this.buttonNo.Visible = true;

                    this.buttonYes.Location = this.buttonNo.Location;
                    this.buttonNo.Location = this.buttonCancel.Location;
                    return;
                case MessageBoxButtons.RetryCancel:
                    this.buttonRetry.Visible = true;
                    this.buttonCancel.Visible = true;
                    return;
            }
        }

        private void HideAllButtons()
        {
            this.buttonOK.Visible = false;
            this.buttonCancel.Visible = false;
            this.buttonAbort.Visible = false;
            this.buttonYes.Visible = false;
            this.buttonNo.Visible = false;
            this.buttonRetry.Visible = false;
            this.buttonIgnore.Visible = false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void buttonRetry_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void richTextBox1_Resize(object sender, EventArgs e)
        {
            //UpdateSize();
        }

        private void UpdateSize()
        {
            SizeF stringSize = Graphics.FromImage(new Bitmap(1, 1)).MeasureString("A", richTextBox1.Font);
            int maxLineLength = 0;

            foreach (string item in richTextBox1.Lines)
                if (item.Length > maxLineLength)
                    maxLineLength = item.Length;

            int calculate = 84 + (int)(maxLineLength / 2.2F * (stringSize.Height));
            this.Width = maxLineLength < 36 ? 341 : (calculate <= 600 ? /*maxLineLength*/calculate : 600);
            this.Height = (int)((float)(200 + (this.ShowCheckBox ? 25 : 10) + 26 +
                (stringSize.Width * (float)richTextBox1.Lines.Length * 1.3F)));

            Console.WriteLine(calculate);
            //GetLineFromCharIndex 
        }

        public void SetMessageIcon()
        {
            switch (this.MessageIcon)
            {
                case CustomMessageBoxIcon.None:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.message_icon_4X;
                    return;
                case CustomMessageBoxIcon.Hand:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.hand_icon_4X;
                    return;
                case CustomMessageBoxIcon.Stop:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.stop_icon_4X;
                    return;
                case CustomMessageBoxIcon.Error:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.error_icon_4X;
                    return;
                case CustomMessageBoxIcon.Question:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.question_icon_4X;
                    return;
                case CustomMessageBoxIcon.Exclamation:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.exclamation_icon_4X;
                    return;
                case CustomMessageBoxIcon.Warning:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.warning_icon_4X;
                    return;
                case CustomMessageBoxIcon.Asterisk:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.asterisk_icon_4X;
                    return;
                case CustomMessageBoxIcon.Information:
                    this.PictureBoxMessageIcon.Image = MessageBoxResource.info_icon_4X;
                    return;
            }
        }

        private void PictureBoxMessageIcon_Click(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMoveMethod(e);
        }
    }
}
