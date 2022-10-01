using System;
using System.Collections.Generic;

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
        bool Find(string pattern);
        List<Range> FindAll(string pattern);

        System.Windows.Forms.Form GetForm();
    }
}
