﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

using FreeIDE.Common;
using FreeIDE.Common.Utils;
using FreeIDE.Components.Renderers;
using FreeIDE.Controls;
using FreeIDE.Controls.TabControl;

using FastColoredTextBoxNS;

namespace FreeIDE.Components
{
    internal class ThemeMaster
    {
        public static readonly string ThemesFolder = $@"{DirectoryUtil.GetLokationFolder()}\Data\Themes";
        public static XDocument ThemeXDocument;
        public static ThemeData ThemeData;

        // Loading the selected (used) theme
        public static void LoadUseTheme()
        {
            try
            {
                ThemeXDocument = XDocument.Load(GetSelectThemePath());
                ThemeData = ThemeData.ParseThemeData(ThemeXDocument);
            }
            catch (Exception ex)
            {
                Logger.AddNewLog("ThemeMaster.LoadUseTheme", $"Exception: {ex}");

                // In case of error, we apply the default theme
                ThemeData = ThemeData.GetDefaultThemeData();
            }

            ThemeData.UpdateDigests();
            ThemeData.PrintInfo();
        }

        // Applies the selected, preloaded theme to a form of type BorderLessForm
        public static void ApplyTheme(BorderLessForm form)
        {
            form.ApplyTheme(ThemeData);
        }

        // Applies the selected, preloaded theme to a user control
        public static void ApplyTheme(Control control)
        {
            Console.WriteLine($@"#ApplyTheme for: {control.Name}");

            control.BackColor = ThemeData.ColorsDigest[(control.Tag as IThemeTag).GetThemeTag1()];
            control.ForeColor = ThemeData.ForeColorsDigest[(control.Tag as IThemeTag).GetThemeTag1()];

            // Specific to the Button class
            if (control is Button) 
            {
                (control as Button).FlatAppearance.BorderColor =
                    ThemeData.BordersColorDigest[(control.Tag as IThemeTag).GetThemeTag2()];
                (control as Button).FlatAppearance.BorderSize =
                    ThemeData.BordersHeightDigest[(control.Tag as IThemeTag).GetThemeTag2()];
            }

            // Specific to the MenuStrip class
            if (control is MenuStrip)
            {
                (control as MenuStrip).Renderer = new CustomMenuStripRenderer(ThemeData.MenuStrip_TitlebarColor, 
                    ThemeData.MenuStrip_TitlebarSize, ThemeData.MenuStrip_MainColor, ThemeData.MenuStrip_ItemSelectedColor,
                    ThemeData.MenuStrip_ItemForeColor);
            }

            // Specific to the ContextMenuStrip class
            if (control is ContextMenuStrip)
            {
                (control as ContextMenuStrip).Renderer = new CustomMenuStripRenderer(ThemeData.MenuStrip_TitlebarColor,
                    ThemeData.MenuStrip_TitlebarSize, ThemeData.MenuStrip_MainColor, ThemeData.MenuStrip_ItemSelectedColor,
                    ThemeData.MenuStrip_ItemForeColor);
            }

            // Specific to the FlatTabControl class
            if (control is FlatTabControl)
            {
                (control as FlatTabControl).BorderColor =
                    ThemeData.BordersColorDigest[(control.Tag as IThemeTag).GetThemeTag2()];
                (control as FlatTabControl).BorderSize =
                    ThemeData.BordersHeightDigest[(control.Tag as IThemeTag).GetThemeTag2()];
                (control as FlatTabControl).myBackColor =
                    ThemeData.ColorsDigest[(control.Tag as IThemeTag).GetThemeTag1()];
            }

            // Specific to the SmartTextBox class
            if (control is SmartTextBox)
            {
                (control as SmartTextBox).BookmarkColor = ThemeData.TextBox_BookmarkColor;
                (control as SmartTextBox).LineNumberColor = ThemeData.TextBox_LineNumberColor;
                (control as SmartTextBox).IndentBackColor = ThemeData.TextBox_IndentBackColor;
                (control as SmartTextBox).SelectionColor = ThemeData.TextBox_SelectionColor;
                (control as SmartTextBox).PaddingBackColor = ThemeData.TextBox_PaddingBackColor;
                (control as SmartTextBox).ChangedLineColor = ThemeData.TextBox_ChangedLineColor;
                (control as SmartTextBox).CurrentLineColor = ThemeData.TextBox_CurrentLineColor;
                (control as SmartTextBox).TextAreaBorderColor = ThemeData.TextBox_TextAreaBorderColor;
                (control as SmartTextBox).ServiceColors = ThemeData.TextBox_ServiceColors;
            }
        }

        // Returns the path to the xml file with the selected theme
        private static string GetSelectThemePath()
        {
            return $@"{ThemesFolder}\{SettingsMaster.GetThemeName()}.xml";
        }
    }

    public class ThemeData
    {
        public Color[] ColorsDigest;
        public Color[] ForeColorsDigest;
        public Color[] BordersColorDigest;
        public Int32[] BordersHeightDigest;

        public string RootName = "Theme";
        public string ThemeName = "Light";

        public Color Color1 = Color.White;
        public Color Color2 = Color.WhiteSmoke;
        public Color Color3 = Color.Black;
        public Color ForeColor1 = Color.Black;
        public Color ForeColor2 = Color.Black;
        public Color ForeColor3 = Color.WhiteSmoke;

        public Color ButtonClose_MouseOverBackColor = Color.Tomato;
        public Color WindowStateButtonsForeColor = Color.Black;
        public Color TitleLabelForeColor = Color.Black;
        public Color BorderColor = Color.Transparent;
        public Color HeaderBackColor = Color.Transparent;
        public Color ColorHeaderUnderline = Color.Black;

        public Color Borders1Color = Color.Black;
        public Color Borders2Color = Color.Black;
        public Color Borders3Color = Color.Black;

        public Int32 BorderHeight = 1;
        public Int32 HeaderHeight = 20;
        public Int32 IconHeight = 20;
        public Int32 IconPaddingX = 1;
        public Int32 IconPaddingY = 1;
        public Int32 WidthHeaderUnderline = 2;

        public Int32 Borders1Height = 1;
        public Int32 Borders2Height = 0;
        public Int32 Borders3Height = 0;

        public UInt16 MenuStrip_TitlebarSize = 2;
        public Color MenuStrip_TitlebarColor = Color.FromArgb(89, 135, 214);
        public Color MenuStrip_MainColor = Color.FromArgb(39, 40, 34);
        public Color MenuStrip_ItemSelectedColor = Color.FromArgb(24, 25, 19);
        public Color MenuStrip_ItemForeColor = Color.White;

        public Color TextBox_BookmarkColor = Color.PowderBlue;
        public Color TextBox_LineNumberColor = Color.Teal;
        public Color TextBox_IndentBackColor = Color.White;
        public Color TextBox_SelectionColor = Color.DarkGreen;
        public Color TextBox_PaddingBackColor = Color.Transparent;
        public Color TextBox_ChangedLineColor = Color.Transparent;
        public Color TextBox_CurrentLineColor = Color.Transparent;
        public Color TextBox_TextAreaBorderColor = Color.Black;

        public ServiceColors TextBox_ServiceColors;

        public ThemeData()
        {
            UpdateDigests();
        }

        public void UpdateDigests()
        {
            this.ColorsDigest = new Color[] { this.Color1, this.Color2, this.Color3 };
            this.ForeColorsDigest = new Color[] { this.ForeColor1, this.ForeColor2, this.ForeColor3 };
            this.BordersColorDigest = new Color[] { this.Borders1Color, this.Borders2Color, this.Borders3Color };
            this.BordersHeightDigest = new Int32[] { this.Borders1Height, this.Borders2Height, this.Borders3Height };

            foreach (var item in ColorsDigest)
                Console.WriteLine(item);
        }

        public static ThemeData GetDefaultThemeData() => new ThemeData();
        public static ThemeData ParseThemeData(XDocument xDocument) => new ThemeData
        {
            RootName = xDocument.Root.Name.LocalName,
            ThemeName = xDocument.Root.Element("Name").Value,

            Color1 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color1")),
            Color2 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color2")),
            Color3 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color3")),

            ForeColor1 = ParseColorFromXDocumentItem(xDocument.Root.Element("ForeColor1")),
            ForeColor2 = ParseColorFromXDocumentItem(xDocument.Root.Element("ForeColor2")),
            ForeColor3 = ParseColorFromXDocumentItem(xDocument.Root.Element("ForeColor3")),

            Borders1Color = ParseColorFromXDocumentItem(xDocument.Root.Element("Borders1Color")),
            Borders2Color = ParseColorFromXDocumentItem(xDocument.Root.Element("Borders2Color")),
            Borders3Color = ParseColorFromXDocumentItem(xDocument.Root.Element("Borders3Color")),

            ButtonClose_MouseOverBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("ButtonClose_MouseOverBackColor")),
            WindowStateButtonsForeColor = ParseColorFromXDocumentItem(xDocument.Root.Element("WindowStateButtonsForeColor")),
            TitleLabelForeColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TitleLabelForeColor")),
            BorderColor = ParseColorFromXDocumentItem(xDocument.Root.Element("BorderColor")),
            HeaderBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("HeaderBackColor")),
            ColorHeaderUnderline = ParseColorFromXDocumentItem(xDocument.Root.Element("ColorHeaderUnderline")),

            BorderHeight = Convert.ToInt32(xDocument.Root.Element("BorderHeight").Value),
            HeaderHeight = Convert.ToInt32(xDocument.Root.Element("HeaderHeight").Value),
            IconHeight = Convert.ToInt32(xDocument.Root.Element("IconHeight").Value),
            IconPaddingX = Convert.ToInt32(xDocument.Root.Element("IconPaddingX").Value),
            IconPaddingY = Convert.ToInt32(xDocument.Root.Element("IconPaddingY").Value),
            WidthHeaderUnderline = Convert.ToInt32(xDocument.Root.Element("WidthHeaderUnderline").Value),

            Borders1Height = Convert.ToInt32(xDocument.Root.Element("Borders1Height").Value),
            Borders2Height = Convert.ToInt32(xDocument.Root.Element("Borders2Height").Value),
            Borders3Height = Convert.ToInt32(xDocument.Root.Element("Borders3Height").Value),

            MenuStrip_TitlebarSize = (UInt16)Convert.ToInt32(xDocument.Root.Element("MenuStrip_TitlebarSize").Value),
            MenuStrip_TitlebarColor = ParseColorFromXDocumentItem(xDocument.Root.Element("MenuStrip_TitlebarColor")),
            MenuStrip_MainColor = ParseColorFromXDocumentItem(xDocument.Root.Element("MenuStrip_MainColor")),
            MenuStrip_ItemSelectedColor = ParseColorFromXDocumentItem(xDocument.Root.Element("MenuStrip_ItemSelectedColor")),
            MenuStrip_ItemForeColor = ParseColorFromXDocumentItem(xDocument.Root.Element("MenuStrip_ItemForeColor")),

            TextBox_BookmarkColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_BookmarkColor")),
            TextBox_LineNumberColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_LineNumberColor")),
            TextBox_IndentBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_IndentBackColor")),
            TextBox_SelectionColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_SelectionColor")),
            TextBox_PaddingBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_PaddingBackColor")),
            TextBox_ChangedLineColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_ChangedLineColor")),
            TextBox_CurrentLineColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_CurrentLineColor")),
            TextBox_TextAreaBorderColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TextBox_TextAreaBorderColor")),

            TextBox_ServiceColors = ParseServiceColorsFromXDocumentItem(xDocument.Root.Element("TextBox_ServiceColors"))
        };
        public void PrintInfo()
        {
            /*Console.WriteLine("{0,-20} = {1,5}", "RootName",  RootName);
            Console.WriteLine("{0,-20} = {1,5}", "ThemeName", ThemeName);*/

            /*Console.WriteLine("{0,-20} = {1,5}", "Color1", Color1);
            Console.WriteLine("{0,-20} = {1,5}", "Color2", Color2);
            Console.WriteLine("{0,-20} = {1,5}", "Color3", Color3);*/

            /*Console.WriteLine("{0,-20} = {1,5}", "BorderColor", BorderColor);
            Console.WriteLine("{0,-20} = {1,5}", "HeaderBackColor", HeaderBackColor);
            Console.WriteLine("{0,-20} = {1,5}", "ColorHeaderUnderline", ColorHeaderUnderline);

            Console.WriteLine("{0,-20} = {1,5}", "HeaderHeight", HeaderHeight);
            Console.WriteLine("{0,-20} = {1,5}", "IconHeight", IconHeight);
            Console.WriteLine("{0,-20} = {1,5}", "IconPadding", IconPadding);
            Console.WriteLine("{0,-20} = {1,5}", "WidthHeaderUnderline", WidthHeaderUnderline);*/

            Console.WriteLine(TextBox_ServiceColors.ToString());
        }

        private static Color ParseColorFromXDocumentItem(XElement xElement)
        {
            return ColorParser.ParseColor(xElement.Attribute("Type").Value, xElement.Value);
        }

        private static ServiceColors ParseServiceColorsFromXDocumentItem(XElement xElement)
        {
            if (xElement.Attribute("ThemeMasterTag") == null ||
                xElement.Attribute("ThemeMasterTag").Value != "ServiceColors") return null;

            return new ServiceColors
            {
                CollapseMarkerForeColor = ParseColorFromXDocumentItem(xElement.Element("CollapseMarkerForeColor")),
                CollapseMarkerBackColor = ParseColorFromXDocumentItem(xElement.Element("CollapseMarkerBackColor")),
                CollapseMarkerBorderColor = ParseColorFromXDocumentItem(xElement.Element("CollapseMarkerBorderColor")),

                ExpandMarkerForeColor = ParseColorFromXDocumentItem(xElement.Element("ExpandMarkerForeColor")),
                ExpandMarkerBackColor = ParseColorFromXDocumentItem(xElement.Element("ExpandMarkerBackColor")),
                ExpandMarkerBorderColor = ParseColorFromXDocumentItem(xElement.Element("ExpandMarkerBorderColor")),
            };
        }
    }

    public interface IThemeTag
    {
        void SetThemeTags(params int[] themeTags);
        void SetThemeTag1(int themeTag);
        void SetThemeTag2(int themeTag);
        int GetThemeTag1();
        int GetThemeTag2();
        int[] GetThemeTags();
    }
}
