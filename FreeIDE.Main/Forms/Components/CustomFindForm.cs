using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using FreeIDE.Tags;
using FreeIDE.Components;

using FastColoredTextBoxNS;

namespace FreeIDE.Forms.Components
{
    public partial class CustomFindForm : BorderLessFormController, IFindForm
    {
        public static bool ShowNotFoundMessageBox = true;
        public static bool ShowExceptionMessageBox = true;

        public delegate void FindFormEvent(object sender);
        public delegate void FindFormExceptionEvent(object sender, Exception ex);
        public delegate void FindFormClosingEvent(object sender, FormClosingEventArgs e);
        public delegate void FindFormKeyPressEvent(object sender, KeyPressEventArgs e);

        public event FindFormEvent ShowNotFoundMessageBoxEvent;
        public event FindFormExceptionEvent ShowExceptionMessageBoxEvent;
        public event FindFormEvent FindNextEvent;
        public event FindFormClosingEvent FormClosingEvent;
        public event FindFormEvent LibsRemasteringEvent;
        public event FindFormKeyPressEvent TbKeyPressEvent;

        public bool firstSearch = true;
        public Place startPlace;
        public FastColoredTextBox tb;

        public CustomFindForm(FastColoredTextBox tb)
        {
            this.tb = tb;

            this.InitializeComponent();
            this.InitializeTags();
            this.BaseApplyTheme();

            this.TitleLabel.Text = this.Text;
        }

        private void InitializeTags()
        {
            this.btClose.Tag = new ButtonTag(1, 0);
            this.btFindNext.Tag = new ButtonTag(1, 0);

            this.panel1.Tag = new FreeTag(1, 0);
            this.label1.Tag = new FreeTag(1, 0);
            this.tbFind.Tag = new FreeTag(0, 0);

            this.cbMatchCase.Tag = new FreeTag(0, 0);
            this.cbRegex.Tag = new FreeTag(0, 0);
            this.cbWholeWord.Tag = new FreeTag(0, 0);
        }

        private void BaseApplyTheme()
        {
            ThemeMaster.ApplyTheme(this);
            ThemeMaster.ApplyTheme(this.tbFind);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            FindNext(tbFind.Text);
        }

        public virtual void FindNext(string pattern)
        {
            if (FindNextEvent != null)
                FindNextEvent.Invoke(this);

            try
            {
                RegexOptions opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
                if (!cbRegex.Checked)
                    pattern = Regex.Escape(pattern);
                if (cbWholeWord.Checked)
                    pattern = "\\b" + pattern + "\\b";
                //
                Range range = tb.Selection.Clone();
                range.Normalize();
                //
                if (firstSearch)
                {
                    startPlace = range.Start;
                    firstSearch = false;
                }
                //
                range.Start = range.End;
                if (range.Start >= startPlace)
                    range.End = new Place(tb.GetLineLength(tb.LinesCount - 1), tb.LinesCount - 1);
                else
                    range.End = startPlace;
                //
                foreach (var r in range.GetRangesByLines(pattern, opt))
                {
                    tb.Selection = r;
                    tb.DoSelectionVisible();
                    tb.Invalidate();
                    return;
                }
                //
                if (range.Start >= startPlace && startPlace > Place.Empty)
                {
                    tb.Selection.Start = new Place(0, 0);
                    FindNext(pattern);
                    return;
                }

                if (ShowNotFoundMessageBox)
                    MessageBox.Show("Not found");
                if (ShowNotFoundMessageBoxEvent != null)
                    ShowNotFoundMessageBoxEvent.Invoke(this);
            }
            catch (Exception ex)
            {
                if (ShowExceptionMessageBox)
                    MessageBox.Show(ex.Message);
                if (ShowExceptionMessageBoxEvent != null)
                    ShowExceptionMessageBoxEvent.Invoke(this, ex);
            }
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (TbKeyPressEvent != null)
                TbKeyPressEvent.Invoke(this, e);

            if (e.KeyChar == '\r')
            {
                btFindNext.PerformClick();
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '\x1b')
            {
                Hide();
                e.Handled = true;
                return;
            }
        }

        private void FindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FormClosingEvent != null)
                FormClosingEvent.Invoke(this, e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            this.tb.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnActivated(EventArgs e)
        {
            tbFind.Focus();
            ResetSerach();
        }

        void ResetSerach()
        {
            firstSearch = true;
        }

        private void cbMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            ResetSerach();
        }

        public void TbFind_SetText(string text)
        {
            this.tbFind.Text = text;
        }

        public void TbFind_SelectAll()
        {
            this.tbFind.SelectAll();
        }

        void IFindForm.Focus()
        {
            this.Focus();
        }

        public string TbFind_GetText()
        {
            return this.tbFind.Text;
        }

        public Form GetForm()
        {
            return this;
        }
    }
}
