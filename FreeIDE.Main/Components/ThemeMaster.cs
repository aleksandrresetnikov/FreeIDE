using System;
using System.Drawing;
using System.Windows.Forms;
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

        public Color BorderColor = Color.Transparent;
        public Color HeaderBackColor = Color.Transparent;
        public Color ColorHeaderUnderline = Color.Black;

        public int HeaderHeight = 20;
        public int IconHeight = 20;
        public int IconPadding = 1;
        public int WidthHeaderUnderline = 2;

        public static ThemeData ParseThemeData(XDocument xDocument) => new ThemeData
        {
            RootName = xDocument.Root.Name.LocalName,
            ThemeName = xDocument.Root.Element("Name").Value
        };

        public void PrintInfo()
        {
            Console.WriteLine("{0,-20} = {1,5}", "RootName",  RootName);
            Console.WriteLine("{0,-20} = {1,5}", "ThemeName", ThemeName);
        }
    }
}
