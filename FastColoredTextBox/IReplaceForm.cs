using System;

namespace FastColoredTextBoxNS
{
    public interface IReplaceForm : IDisposable
    {
        void TbFind_SetText(string text);
        void TbFind_SelectAll();
        string TbFind_GetText();

        void TbReplace_SetText(string text);
        void TbReplace_SelectAll();
        string TbReplace_GetText();

        void Show();
        void Focus();

        System.Windows.Forms.Form GetForm();
    }
}
