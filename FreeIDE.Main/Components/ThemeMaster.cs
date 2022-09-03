﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using FreeIDE.Common;
using FreeIDE.Common.Utils;

namespace FreeIDE.Components
{
    internal class ThemeMaster
    {
        public static readonly string ThemesFolder = $@"{DirectoryUtil.GetLokationFolder()}\Data\Themes";
        public static XDocument ThemeXDocument;
        public static ThemeData ThemeData;

        public static void LoadUseTheme()
        {
            try
            {
                ThemeXDocument = XDocument.Load(GetSelectThemePath());
                ThemeData = ThemeData.ParseThemeData(ThemeXDocument);

                ThemeData.PrintInfo();
            }
            catch (Exception ex)
            {
                Logger.AddNewLog("ThemeMaster.LoadUseTheme", $"Exception: {ex}");
            }
        }

        public static void ApplyTheme(BorderLessForm form)
        {
            form.ApplyTheme(ThemeData);
        }

        private static string GetSelectThemePath()
        {
            return $@"{ThemesFolder}\{SettingsMaster.GetThemeName()}.xml";
        }
    }

    public class ThemeData
    {
        public string RootName = "Theme";
        public string ThemeName = "Light";

        public Color Color1;
        public Color Color2;
        public Color Color3;

        public Color ButtonClose_MouseOverBackColor = Color.Tomato;
        public Color WindowStateButtonsForeColor = Color.Black;
        public Color TitleLabelForeColor = Color.Black;
        public Color BorderColor = Color.Transparent;
        public Color HeaderBackColor = Color.Transparent;
        public Color ColorHeaderUnderline = Color.Black;

        public Int32 HeaderHeight = 20;
        public Int32 IconHeight = 20;
        public Int32 IconPadding = 1;
        public Int32 WidthHeaderUnderline = 2;

        public static ThemeData ParseThemeData(XDocument xDocument) => new ThemeData
        {
            RootName = xDocument.Root.Name.LocalName,
            ThemeName = xDocument.Root.Element("Name").Value,

            Color1 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color1")),
            Color2 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color2")),
            Color3 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color3")),

            ButtonClose_MouseOverBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("ButtonClose_MouseOverBackColor")),
            WindowStateButtonsForeColor = ParseColorFromXDocumentItem(xDocument.Root.Element("WindowStateButtonsForeColor")),
            TitleLabelForeColor = ParseColorFromXDocumentItem(xDocument.Root.Element("TitleLabelForeColor")),
            BorderColor = ParseColorFromXDocumentItem(xDocument.Root.Element("BorderColor")),
            HeaderBackColor = ParseColorFromXDocumentItem(xDocument.Root.Element("HeaderBackColor")),
            ColorHeaderUnderline = ParseColorFromXDocumentItem(xDocument.Root.Element("ColorHeaderUnderline")),

            HeaderHeight = Convert.ToInt32(xDocument.Root.Element("HeaderHeight").Value),
            IconHeight = Convert.ToInt32(xDocument.Root.Element("IconHeight").Value),
            IconPadding = Convert.ToInt32(xDocument.Root.Element("IconPadding").Value),
            WidthHeaderUnderline = Convert.ToInt32(xDocument.Root.Element("WidthHeaderUnderline").Value)
        };
        public void PrintInfo()
        {
            Console.WriteLine("{0,-20} = {1,5}", "RootName",  RootName);
            Console.WriteLine("{0,-20} = {1,5}", "ThemeName", ThemeName);

            Console.WriteLine("{0,-20} = {1,5}", "Color1", Color1);
            Console.WriteLine("{0,-20} = {1,5}", "Color2", Color2);
            Console.WriteLine("{0,-20} = {1,5}", "Color3", Color3);

            Console.WriteLine("{0,-20} = {1,5}", "BorderColor", BorderColor);
            Console.WriteLine("{0,-20} = {1,5}", "HeaderBackColor", HeaderBackColor);
            Console.WriteLine("{0,-20} = {1,5}", "ColorHeaderUnderline", ColorHeaderUnderline);

            Console.WriteLine("{0,-20} = {1,5}", "HeaderHeight", HeaderHeight);
            Console.WriteLine("{0,-20} = {1,5}", "IconHeight", IconHeight);
            Console.WriteLine("{0,-20} = {1,5}", "IconPadding", IconPadding);
            Console.WriteLine("{0,-20} = {1,5}", "WidthHeaderUnderline", WidthHeaderUnderline);
        }

        private static Color ParseColorFromXDocumentItem(XElement xElement)
        {
            return ColorParser.ParseColor(xElement.Attribute("Type").Value, xElement.Value);
        }
    }
}
