using System;

namespace FastColoredTextBoxNS
{
    public interface IFindForm : IDisposable
    {
        void TbFind_SetText(string text);
        void TbFind_SelectAll();
        string TbFind_GetText();

        void Show();
        void Focus();
        void FindNext(string pattern);

        System.Windows.Forms.Form GetForm();
    }
}
