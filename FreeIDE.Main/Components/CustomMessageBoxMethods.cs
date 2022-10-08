using System.Windows.Forms;

using FreeIDE.Forms.Components;

namespace FreeIDE.Components
{
    internal class CustomMessageBoxMethods
    {
        public static DialogResult Show(string Message) =>
            new CustomMessageBox(Message).ShowDialog();

        public static DialogResult Show(string Caption, string Message) =>
            new CustomMessageBox(Caption, Message).ShowDialog();

        public static DialogResult Show(string Caption, string Message, MessageBoxButtons Buttons) =>
            new CustomMessageBox(Caption, Message, Buttons).ShowDialog();

        public static DialogResult Show(string Caption, string Message, CustomMessageBoxIcon MessageIcon) =>
            new CustomMessageBox(Caption, Message, MessageIcon).ShowDialog();

        public static DialogResult Show(string Message, MessageBoxButtons Buttons) =>
            new CustomMessageBox(Message, Buttons).ShowDialog();

        public static DialogResult Show(string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon) =>
            new CustomMessageBox(Message, Buttons, MessageIcon).ShowDialog();

        public static DialogResult Show(string Message, CustomMessageBoxIcon MessageIcon) =>
            new CustomMessageBox(Message, MessageIcon).ShowDialog();

        public static DialogResult Show(string Caption, string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon) =>
            new CustomMessageBox(Caption, Message, Buttons, MessageIcon).ShowDialog();


        public static DialogResult Show(string Message, bool WordWrap) =>
            new CustomMessageBox(Message) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Caption, string Message, bool WordWrap) =>
            new CustomMessageBox(Caption, Message) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Caption, string Message, MessageBoxButtons Buttons, bool WordWrap) =>
            new CustomMessageBox(Caption, Message, Buttons) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Caption, string Message, CustomMessageBoxIcon MessageIcon, bool WordWrap) =>
            new CustomMessageBox(Caption, Message, MessageIcon) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Message, MessageBoxButtons Buttons, bool WordWrap) =>
            new CustomMessageBox(Message, Buttons) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon, bool WordWrap) =>
            new CustomMessageBox(Message, Buttons, MessageIcon) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Message, CustomMessageBoxIcon MessageIcon, bool WordWrap) =>
            new CustomMessageBox(Message, MessageIcon) { WordWrap = WordWrap }.ShowDialog();

        public static DialogResult Show(string Caption, string Message, MessageBoxButtons Buttons, CustomMessageBoxIcon MessageIcon, bool WordWrap) =>
            new CustomMessageBox(Caption, Message, Buttons, MessageIcon) { WordWrap = WordWrap }.ShowDialog();
    }
}
