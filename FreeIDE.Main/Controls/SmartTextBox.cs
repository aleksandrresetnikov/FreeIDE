using FastColoredTextBoxNS;

namespace FreeIDE.Controls
{
    internal class SmartTextBox : FastColoredTextBox
    {
        public void Do_Cut()
        {
            this.Cut(); 
        }        
        public void Do_Copy()
        {
            this.Copy();
        }
        public void Do_Paste()
        {
            this.Paste();
        }
        public void Do_Delete()
        {
            this.SelectedText = null;
        }
        public void Do_SelectAll()
        {
            this.SelectAll();
        }
        public void Do_CutAll()
        {
            this.SelectAll();
            this.Cut();
        }
        public void Do_DeleteAll()
        {
            this.SelectAll();
            this.SelectedText = null;
        }

        public void Do_Undo()
        {
            this.Undo();
        }
        public void Do_Redo()
        {
            this.Redo();
        }
        public void Do_ClearUndoBufer()
        {
            this.ClearUndo();
        }

        public void Do_Find()
        {
            this.ShowFindDialog();
        }
        public void Do_FindAndReplace()
        {
            this.ShowReplaceDialog();
        }
        public void Do_Goto()
        {
            this.ShowGoToDialog();
        }
    }
}
