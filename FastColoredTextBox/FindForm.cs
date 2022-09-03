using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;

namespace FastColoredTextBoxNS
{
    public partial class FindForm : Form
    {
        // For LPI_LibsRemaster :
        public static string FormTitleText = "Find";
        public static string label1Text = "Find";
        public static string textBoxText = "Find";
        public static bool cbMatchCaseCheckedStatys = false;
        public static bool cbWholeWordCheckedStatys = false;
        public static bool cbRegexCheckedStatys = false;
        public static string cbMatchCaseText = "Match case";
        public static string cbWholeWordText = "Match whole word";
        public static string cbRegexText = "Regex";
        public static string btFindNextText = "Find next";
        public static string btCloseText = "Close";
        public static Color backFormColor = SystemColors.Control;
        public static Color containPanelColor = SystemColors.Control;
        public static Color backTextBoxColor = SystemColors.Control;
        public static Color foreTextBoxColor = SystemColors.WindowText;
        public static Color backButtonsColor = SystemColors.Control;
        public static Color foreButtonsColor = SystemColors.WindowText;
        public static FlatStyle buttonsStyle = FlatStyle.Standard;
        public static Font textBoxFont = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        public static Point textBoxLocation = new Point(61, 14);

        public static bool ShowNotFoundMessageBox = true;
        public static bool ShowExceptionMessageBox = true;

        public delegate void FindFormEvent(object sender);
        public delegate void FindFormExceptionEvent(object sender, Exception ex);
        public delegate void FindFormClosingEvent(object sender, FormClosingEventArgs e);
        public delegate void FindFormKeyPressEvent(object sender, KeyPressEventArgs e);

        public static event FindFormEvent ShowNotFoundMessageBoxEvent;
        public static event FindFormExceptionEvent ShowExceptionMessageBoxEvent;
        public static event FindFormEvent FindNextEvent;
        public static event FindFormClosingEvent FormClosingEvent;
        public static event FindFormEvent LibsRemasteringEvent;
        public static event FindFormKeyPressEvent TbKeyPressEvent;



        public bool firstSearch = true;
        public Place startPlace;
        public FastColoredTextBox tb;

        public FindForm(FastColoredTextBox tb)
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

            if (LibsRemasteringEvent != null)
                LibsRemasteringEvent.Invoke(this);

            this.Text               = FormTitleText;
            this.BackColor          = backFormColor;
            panel1.BackColor        = containPanelColor;

            label1.Text             = label1Text;
            label1.ForeColor        = foreButtonsColor;

            tbFind.Text             = textBoxText;
            tbFind.Font             = textBoxFont;
            tbFind.BackColor        = backTextBoxColor;
            tbFind.ForeColor        = foreTextBoxColor;
            tbFind.Location         = textBoxLocation;

            btFindNext.BackColor    = backButtonsColor;
            btClose.BackColor       = backButtonsColor;
            btFindNext.ForeColor    = foreButtonsColor;
            btClose.ForeColor       = foreButtonsColor;
            btFindNext.FlatStyle    = buttonsStyle;
            btClose.FlatStyle       = buttonsStyle;
            btFindNext.Text         = btFindNextText;
            btClose.Text            = btCloseText;

            cbMatchCase.Checked     = cbMatchCaseCheckedStatys;
            cbWholeWord.Checked     = cbWholeWordCheckedStatys;
            cbRegex.Checked         = cbRegexCheckedStatys;
            cbMatchCase.Text        = cbMatchCaseText;
            cbWholeWord.Text        = cbWholeWordText;
            cbRegex.Text            = cbRegexText;
            cbMatchCase.ForeColor   = foreButtonsColor;
            cbWholeWord.ForeColor   = foreButtonsColor;
            cbRegex.ForeColor       = foreButtonsColor;
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
    }
}
