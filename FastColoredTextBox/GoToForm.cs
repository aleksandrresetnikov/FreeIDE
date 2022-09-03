using System;
using System.Windows.Forms;
using System.Drawing;

namespace FastColoredTextBoxNS
{
    public partial class GoToForm : Form
    {
        public static string FormTitleText = "Go To Line";
        public static Color backFormColor = SystemColors.Control;
        public static string LabelText = "Line number ";
        public static Color foreTextBoxColor = SystemColors.WindowText;
        public static Color foreButtonsColor = SystemColors.WindowText;
        public static FlatStyle buttonsStyle = FlatStyle.Standard;
        public static Color backTextBoxColor = SystemColors.Control;
        public static Color backButtonsColor = SystemColors.Control;
        public static Font textBoxFont = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        public static string btnOKText = "OK";
        public static string btnCancelText = "Cancel";



        public int SelectedLineNumber { get; set; }
        public int TotalLineCount { get; set; }

        public GoToForm()
        {
            InitializeComponent();
            LPI_LibsRemaster();
        }

        private void LPI_LibsRemaster()
        {
            //  This method is not copyright infringement.
            //This method only allows me to slightly edit the form
            //(its colors to suit my program) and also translate the library honestly.

            this.Text               = FormTitleText;
            this.BackColor          = backFormColor;

            label.Text              = LabelText + "(1 / 1):";
            label.ForeColor         = foreButtonsColor;

            tbLineNumber.Font       = textBoxFont;
            tbLineNumber.BackColor  = backTextBoxColor;
            tbLineNumber.ForeColor  = foreTextBoxColor;

            btnCancel.BackColor     = backButtonsColor;
            btnOk.BackColor         = backButtonsColor;
            btnCancel.ForeColor     = foreButtonsColor;
            btnOk.ForeColor         = foreButtonsColor;
            btnCancel.FlatStyle     = buttonsStyle;
            btnOk.FlatStyle         = buttonsStyle;
            btnCancel.Text          = btnCancelText;
            btnOk.Text              = btnOKText;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.tbLineNumber.Text = this.SelectedLineNumber.ToString();

            this.label.Text = String.Format($"{LabelText}(1 - {0}):", this.TotalLineCount);
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
    }
}
