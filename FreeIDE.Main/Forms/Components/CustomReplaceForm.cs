using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using FreeIDE.Tags;
using FreeIDE.Components;

using FastColoredTextBoxNS;

namespace FreeIDE.Forms.Components
{
    public partial class CustomReplaceForm : BorderLessFormController, IReplaceForm
    {
        FastColoredTextBox tb;
        bool firstSearch = true;
        Place startPlace;

        public CustomReplaceForm(FastColoredTextBox tb)
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
            this.btReplace.Tag = new ButtonTag(1, 0);
            this.btReplaceAll.Tag = new ButtonTag(1, 0);

            this.panel1.Tag = new FreeTag(1, 0);
            this.label1.Tag = new FreeTag(1, 0);
            this.label2.Tag = new FreeTag(1, 0);

            this.cbMatchCase.Tag = new FreeTag(0, 0);
            this.cbRegex.Tag = new FreeTag(0, 0);
            this.cbWholeWord.Tag = new FreeTag(0, 0);

            this.tbFind.Tag = new FreeTag(0, 0);
            this.tbReplace.Tag = new FreeTag(0, 0);
        }

        private void BaseApplyTheme()
        {
            ThemeMaster.ApplyTheme(this);
            ThemeMaster.ApplyTheme(this.tbFind);
            ThemeMaster.ApplyTheme(this.tbReplace);
            ThemeMaster.ApplyTheme(this.label1);
            ThemeMaster.ApplyTheme(this.label2);
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Find(tbFind.Text))
                    MessageBox.Show("Not found");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Range> FindAll(string pattern)
        {
            var opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (!cbRegex.Checked)
                pattern = Regex.Escape(pattern);
            if (cbWholeWord.Checked)
                pattern = "\\b" + pattern + "\\b";
            //
            var range = tb.Selection.IsEmpty ? tb.Range.Clone() : tb.Selection.Clone();
            //
            var list = new List<Range>();
            foreach (var r in range.GetRangesByLines(pattern, opt))
                list.Add(r);

            return list;
        }

        public bool Find(string pattern)
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
                tb.Selection.Start = r.Start;
                tb.Selection.End = r.End;
                tb.DoSelectionVisible();
                tb.Invalidate();
                return true;
            }
            if (range.Start >= startPlace && startPlace > Place.Empty)
            {
                tb.Selection.Start = new Place(0, 0);
                return Find(pattern);
            }
            return false;
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btFindNext_Click(sender, null);
            if (e.KeyChar == '\x1b')
                Hide();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // David
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ReplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            this.tb.Focus();
        }

        private void btReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb.SelectionLength != 0)
                    if (!tb.Selection.ReadOnly)
                        tb.InsertText(tbReplace.Text);
                btFindNext_Click(sender, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btReplaceAll_Click(object sender, EventArgs e)
        {
            try
            {
                tb.Selection.BeginUpdate();

                //search
                var ranges = FindAll(tbFind.Text);
                //check readonly
                var ro = false;
                foreach (var r in ranges)
                    if (r.ReadOnly)
                    {
                        ro = true;
                        break;
                    }
                //replace
                if (!ro)
                    if (ranges.Count > 0)
                    {
                        tb.TextSource.Manager.ExecuteCommand(new ReplaceTextCommand(tb.TextSource, ranges, tbReplace.Text));
                        tb.Selection.Start = new Place(0, 0);
                    }
                //
                tb.Invalidate();
                MessageBox.Show(ranges.Count + " occurrence(s) replaced");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            tb.Selection.EndUpdate();
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

        public Form GetForm()
        {
            return this;
        }

        public void TbFind_SetText(string text)
        {
            this.tbFind.Text = text;
        }

        public void TbFind_SelectAll()
        {
            this.tbFind.SelectAll();
        }

        public string TbFind_GetText()
        {
            return this.tbFind.Text;
        }

        public void TbReplace_SetText(string text)
        {
            this.tbReplace.Text = text;
        }

        public void TbReplace_SelectAll()
        {
            this.tbReplace.SelectAll();
        }

        public string TbReplace_GetText()
        {
            return this.tbReplace.Text;
        }

        void IReplaceForm.Focus()
        {
            this.Focus();
        }
    }
}
