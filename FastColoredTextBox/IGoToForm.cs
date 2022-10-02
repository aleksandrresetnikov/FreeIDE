using System;

namespace FastColoredTextBoxNS
{
    public interface IGoToForm : IDisposable
    {
        System.Windows.Forms.DialogResult ShowDialog();
        void SetTotalLineCount(int totalLineCount);
        void SetSelectedLineNumber(int selectedLineNumber);
        int GetSelectedLineNumber();

        System.Windows.Forms.Form GetForm();
    }
}
