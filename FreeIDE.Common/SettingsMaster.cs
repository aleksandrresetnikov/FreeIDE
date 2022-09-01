using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeIDE.Common
{
    public class SettingsMaster
    {
        // Values for default settings:
        public static readonly Dictionary<string, object> SettingsDefaultValuePreset = new Dictionary<string, object>() 
        {
            { "Theme", (object)"Light" }
        };

        public static string GetThemeName()
        {
            CheckSetting("Theme");
            return SettingsTable.GetValue("Theme").ToString();
        }

        public static void SetThemeName(string Value)
        {
            CheckSetSetting("Theme", Value.ToString());
        }

        // Check if the setting exists, if not, then set the default value.
        private static void CheckSetting(string SettingName)
        {
            if (!SettingsTable.ContainSetting(SettingName)) 
                SettingsTable.SetValue(SettingName, SettingsDefaultValuePreset[SettingName]);
        }

        // Checks whether the value of the setting is null, if the value is null, then set the default value.
        private static void CheckSetSetting(string SettingName, object SettingValue)
        {
            SettingsTable.SetValue(SettingName, SettingValue != null ? SettingValue : SettingsDefaultValuePreset[SettingName]);
        }
    }
}
