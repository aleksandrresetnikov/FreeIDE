using FreeIDE.Components;

namespace FreeIDE.Tags
{
    internal class MenuStripTag : IThemeTag
    {
        private int[] themeTags = new int[] { 0, 0 };

        public object FreeTag { get; set; } = null;

        public MenuStripTag(int themeTag) => this.themeTags[0] = themeTag;
        public MenuStripTag(int themeTag1, int themeTag2) => this.themeTags = new int[] { themeTag1, themeTag2 };
        public MenuStripTag(params int[] themeTags) => this.themeTags = themeTags;

        #region IThemeTag
        public void SetThemeTags(params int[] themeTags) => this.themeTags = themeTags;
        public void SetThemeTag1(int themeTag) => this.themeTags[0] = themeTag;
        public void SetThemeTag2(int themeTag) => this.themeTags[1] = themeTag;
        public int GetThemeTag1() => this.themeTags[0];
        public int GetThemeTag2() => this.themeTags[1];
        public int[] GetThemeTags() => this.themeTags;
        #endregion
    }
}
