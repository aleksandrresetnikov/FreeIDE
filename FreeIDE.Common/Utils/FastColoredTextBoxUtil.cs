namespace FreeIDE.Common.Utils
{
    public static class FastColoredTextBoxUtil
    {
        public static FastColoredTextBoxNS.Language GetLanguage(string path)
        {
            switch (new System.IO.FileInfo(path).Extension.ToLower())
            {
                case ".java": return FastColoredTextBoxNS.Language.Custom;
                case ".cs": return FastColoredTextBoxNS.Language.CSharp;
                case ".json": return FastColoredTextBoxNS.Language.JSON;
                case ".xml": return FastColoredTextBoxNS.Language.XML;
                case ".lua": return FastColoredTextBoxNS.Language.Lua;
                case ".vb": return FastColoredTextBoxNS.Language.VB;
                case ".html": return FastColoredTextBoxNS.Language.HTML;
                case ".js": return FastColoredTextBoxNS.Language.JS;
                default: return FastColoredTextBoxNS.Language.Custom;
            }
        }
    }
}
