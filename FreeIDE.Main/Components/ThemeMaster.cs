using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using FreeIDE.Common;

namespace FreeIDE.Components
{
    internal class ThemeMaster
    {
        public static readonly string ThemesFolder = @"\Themes";
        public static XDocument ThemeXDocument;

        public static void LoadUseTheme()
        {
            try
            {
                ThemeXDocument = XDocument.Load(GetSelectThemePath());
            }
            catch (Exception ex)
            {
                Logger.AddNewLog("ThemeMaster.LoadUseTheme", $"Exception: {ex}");
            }
        }

        private static string GetSelectThemePath()
        {
            return $@"{ThemesFolder}\{SettingsMaster.GetThemeName()}.xml";
        }
    }
}
