using Microsoft.Win32;

namespace FreeIDE.Common
{
    public class SettingsTable
    {
        public static RegistryKey CurrentUserKey;
        public static RegistryKey GeneralKey;

        public static void InitSettingsTable()
        {
            CurrentUserKey = Registry.CurrentUser;
            InitGeneralKey();
        }
        private static void InitGeneralKey()
        {
            GeneralKey = CurrentUserKey.OpenSubKey("FreeIDE", true);
            if (GeneralKey == null) // If the program root directory is missing
                CreateDefaultGeneralKey(); // Create a root directory and its subdirectories
        }
        private static void CreateDefaultGeneralKey()
        {
            GeneralKey = CurrentUserKey.CreateSubKey("FreeIDE", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        public static void SetValue(string settingsName, object settingsValue)
        {
            GeneralKey.SetValue(settingsName, settingsValue);
        }

        public static object GetValue(string settingsName)
        {
            return GeneralKey.GetValue(settingsName);
        }

        public static bool ContainSetting(string settingsName)
        {
            return GeneralKey.OpenSubKey(settingsName) != null;
        }
    }
}
