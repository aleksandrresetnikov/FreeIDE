using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;

namespace FastColoredTextBoxNS
{
    public partial class ReplaceForm : Form
    {
        public static string FormTitleText = "Find and replace";
        public static string label1Text = "Find:";
        public static string label2Text = "Replace:";
        public static bool cbMatchCaseCheckedStatys = false;
        public static bool cbWholeWordCheckedStatys = false;
        public static bool cbRegexCheckedStatys = false;
        public static string cbMatchCaseText = "Match case";
        public static string cbWholeWordText = "Match whole word";
        public static string cbRegexText = "Regex";
        public static Color backFormColor = SystemColors.Control;
        public static Color containPanelColor = SystemColors.Control;
        public static Color backTextBoxColor = SystemColors.Control;
        public static Color foreTextBoxColor = SystemColors.WindowText;
        public static Color backButtonsColor = SystemColors.Control;
        public static Color foreButtonsColor = SystemColors.WindowText;
        public static Font textBoxFont = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        public static Point textBoxFindLocation = new Point(62, 16);
        public static Point textBoxReplaceLocation = new Point(62, 42);
        public static FlatStyle buttonsStyle = FlatStyle.Standard;
        public static string btFindNextText = "Find next";
        public static string btCloseText = "Close";
        public static string btReplaceText = "Replace";
        public static string btReplaceAllText = "Replace all";



        FastColoredTextBox tb;
        bool firstSearch = true;
        Place startPlace;

        public ReplaceForm(FastColoredTextBox tb)
        {
            InitializeComponent();
            LPI_LibsRemaster();
            this.tb = tb;
        }

        private void LPI_LibsRemaster()
        {
            //  This method is not copyright infringement.
            //This method only allows me to slightly edit the form
            //(its colors to suit my program) and also translate the library honestly.

            this.Text = FormTitleText;
            this.BackColor = backFormColor;
            panel1.BackColor = containPanelColor;

            label1.Text = label1Text;
            label1.ForeColor = foreButtonsColor;
            label2.Text = label2Text;
            label2.ForeColor = foreButtonsColor;

            tbFind.Font = textBoxFont;
            tbFind.BackColor = backTextBoxColor;
            tbFind.ForeColor = foreTextBoxColor;
            tbFind.Location = textBoxFindLocation;
            tbReplace.Font = textBoxFont;
            tbReplace.BackColor = backTextBoxColor;
            tbReplace.ForeColor = foreTextBoxColor;
            tbReplace.Location = textBoxReplaceLocation;
            
            cbMatchCase.Checked = cbMatchCaseCheckedStatys;
            cbWholeWord.Checked = cbWholeWordCheckedStatys;
            cbRegex.Checked = cbRegexCheckedStatys;
            cbMatchCase.Text = cbMatchCaseText;
            cbWholeWord.Text = cbWholeWordText;
            cbRegex.Text = cbRegexText;
            cbMatchCase.ForeColor = foreButtonsColor;
            cbWholeWord.ForeColor = foreButtonsColor;
            cbRegex.ForeColor = foreButtonsColor;

            btFindNext.BackColor = backButtonsColor;
            btClose.BackColor = backButtonsColor;
            btReplace.BackColor = backButtonsColor;
            btReplaceAll.BackColor = backButtonsColor;

            btFindNext.ForeColor = foreButtonsColor;
            btClose.ForeColor = foreButtonsColor;
            btReplace.ForeColor = foreButtonsColor;
            btReplaceAll.ForeColor = foreButtonsColor;

            btFindNext.FlatStyle = buttonsStyle;
            btClose.FlatStyle = buttonsStyle;
            btReplace.FlatStyle = buttonsStyle;
            btReplaceAll.FlatStyle = buttonsStyle;

            btFindNext.Text = btFindNextText;
            btClose.Text = btCloseText;
            btReplace.Text = btReplaceText;
            btReplaceAll.Text = btReplaceAllText;
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
            var range = tb.Selection.IsEmpty? tb.Range.Clone() : tb.Selection.Clone();
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
    }
}
