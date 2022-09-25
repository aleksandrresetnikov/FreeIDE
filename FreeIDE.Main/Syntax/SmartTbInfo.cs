namespace FreeIDE.Syntax
{
    public class SmartTbInfo
    {
        public FastColoredTextBoxNS.AutocompleteMenu popupMenu;
        public string filePath;
        public System.IO.FileInfo fileInfo => new System.IO.FileInfo(this.filePath);
    }
}
