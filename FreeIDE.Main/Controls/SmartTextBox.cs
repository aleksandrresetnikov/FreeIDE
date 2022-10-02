using System;
using System.IO;
using System.Windows.Forms;

using FreeIDE.Tags;
using FreeIDE.Forms.Components;

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
            this.SetFindDialog(new CustomFindForm(this));
        }
        public void Do_FindAndReplace()
        {
            this.ShowReplaceDialog();
            this.SetReplaceForm(new CustomReplaceForm(this));
        }
        public void Do_Goto()
        {
            this.ShowGoToDialog();
            this.SetGoToForm(new CustomGoToForm());
        }

        public void Do_AddLabel()
        {
            this.BookmarkLine(this.Selection.Start.iLine);
        }
        public void Do_RemoveLabel()
        {
            this.UnbookmarkLine(this.Selection.Start.iLine);
        }

        public void Do_CommentSelected()
        {
            this.CommentSelected();
        }

        public void Do_Save()
        {
            File.Create((this.Tag as SmartTextBoxTag).SmartTextBoxInfo.fileInfo.FullName).Close();
            using (Stream stream = File.OpenWrite((this.Tag as SmartTextBoxTag).SmartTextBoxInfo.fileInfo.FullName))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(this.Text);
                }
                stream.Close();
            }

            GC.Collect();
        }
        public void Do_SaveAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.InitialDirectory = (this.Tag as SmartTextBoxTag).SmartTextBoxInfo.fileInfo.Directory.FullName;
                saveFileDialog.Title = "Save file as ...";
                saveFileDialog.FileName = (this.Tag as SmartTextBoxTag).SmartTextBoxInfo.fileInfo.Name;

                DialogResult dialog = saveFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    File.Create(saveFileDialog.FileName).Close();
                    using (Stream stream = File.OpenWrite(saveFileDialog.FileName))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(this.Text);
                        }
                        stream.Close();
                    }
                }
            }

            GC.Collect();
        }

        public override void ShowGoToDialog()
        {
            if (goToForm == null)
                goToForm = new CustomGoToForm();

            base.ShowGoToDialog();
        }
    }
}
