using System;
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

            Color1 = ParseColorFromXDocumentItem(xDocument.Root.Element("Color1"))
        };

        public void PrintInfo()
        {
            Console.WriteLine("{0,-20} = {1,5}", "RootName",  RootName);
            Console.WriteLine("{0,-20} = {1,5}", "ThemeName", ThemeName);
        }

        private static Color ParseColorFromXDocumentItem(XElement xElement)
        {
            return ColorParser.ParseColor(xElement.Attribute("Type").Value, xElement.Value);
        }
    }
}
