using System;
using System.Windows.Forms;

using FreeIDE.Tags;
using FreeIDE.Components;

using FastColoredTextBoxNS;

namespace FreeIDE.Forms.Components
{
    public partial class CustomGoToForm : BorderLessFormController, IGoToForm
    {
        public string LabelText = "Line number ";

        public int SelectedLineNumber { get; set; }
        public int TotalLineCount { get; set; }

        public CustomGoToForm()
        {
            this.InitializeComponent();
            this.InitializeTags();
            this.BaseApplyTheme();
            this.CenterToScreen();

            this.TitleLabel.Text = this.Text;
        }

        private void InitializeTags()
        {
            this.label.Tag = new FreeTag(0, 0);
            this.tbLineNumber.Tag = new FreeTag(1, 0);

            this.btnCancel.Tag = new ButtonTag(1, 0);
            this.btnOk.Tag = new ButtonTag(1, 0);
        }

        private void BaseApplyTheme()
        {
            ThemeMaster.ApplyTheme(this);
            ThemeMaster.ApplyTheme(this.tbLineNumber);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbLineNumber.Text = this.SelectedLineNumber.ToString();

            this.label.Text = String.Format($"{this.LabelText}(1 - {0}):", this.TotalLineCount);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            this.tbLineNumber.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int enteredLine;
            if (int.TryParse(this.tbLineNumber.Text, out enteredLine))
            {
                enteredLine = Math.Min(enteredLine, this.TotalLineCount);
                enteredLine = Math.Max(1, enteredLine);

                this.SelectedLineNumber = enteredLine;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public int GetSelectedLineNumber()
        {
            return this.SelectedLineNumber;
        }

        public void SetTotalLineCount(int totalLineCount)
        {
            this.TotalLineCount = totalLineCount;
        }

        public void SetSelectedLineNumber(int selectedLineNumber)
        {
            this.SelectedLineNumber = selectedLineNumber;
        }

        public Form GetForm()
        {
            return this;
        }
    }
}
