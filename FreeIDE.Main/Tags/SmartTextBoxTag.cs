using FreeIDE.Components;
using FreeIDE.Syntax;

namespace FreeIDE.Tags
{
    internal class SmartTextBoxTag : IThemeTag
    {
        private int[] themeTags = new int[] { 0, 0 };

        public object FreeTag { get; set; } = null;
        public SmartTbInfo SmartTextBoxInfo { get; set; }

        public SmartTextBoxTag(int themeTag) => this.themeTags[0] = themeTag;
        public SmartTextBoxTag(int themeTag1, int themeTag2) => this.themeTags = new int[] { themeTag1, themeTag2 };
        public SmartTextBoxTag(params int[] themeTags) => this.themeTags = themeTags;

        public SmartTextBoxTag(SmartTbInfo SmartTextBoxInfo) => this.SmartTextBoxInfo = SmartTextBoxInfo;
        public SmartTextBoxTag(int themeTag, SmartTbInfo SmartTextBoxInfo) :
            this(themeTag) => this.SmartTextBoxInfo = SmartTextBoxInfo;
        public SmartTextBoxTag(int themeTag1, int themeTag2, SmartTbInfo SmartTextBoxInfo) :
            this(themeTag1, themeTag2) => this.SmartTextBoxInfo = SmartTextBoxInfo;
        public SmartTextBoxTag(SmartTbInfo SmartTextBoxInfo, params int[] themeTags) :
            this(themeTags) => this.SmartTextBoxInfo = SmartTextBoxInfo;

        #region IThemeTag
        public void SetThemeTags(params int[] themeTags) => this.themeTags = themeTags;
        public void SetThemeTag1(int themeTag) => this.themeTags[0] = themeTag;
        public void SetThemeTag2(int themeTag) => this.themeTags[1] = themeTag;
        public int GetThemeTag1() => this.themeTags[0];
        public int GetThemeTag2() => this.themeTags[1];
        public int[] GetThemeTags() => this.themeTags;
        #endregion

        public static SmartTextBoxTag CreateSmartTextBoxTagInstance(SmartTbInfo smartTbInfo) =>
            new SmartTextBoxTag(0, 0, smartTbInfo);
    }
}
