﻿using FreeIDE.Components;

namespace FreeIDE.Tags
{
    internal class ButtonTag : IThemeTag
    {
        private int[] themeTags = new int[] { 1,1 };

        public object? FreeTag { get; set; } = null;

        public ButtonTag(int themeTag) => this.themeTags[0] = themeTag;
        public ButtonTag(params int[] themeTags) => this.themeTags = themeTags;

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
