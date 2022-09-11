using FreeIDE.Common;
using FreeIDE.Common.IDE.Profiles;
using FreeIDE.Components;

namespace FreeIDE
{
    static class Program
    {
        public static ProfilesCollector Profiles;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            InitAllComponents();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Forms.EditorForm());
        }

        internal static void InitAllComponents()
        {
            Profiles = ProfilesLoader.GetProfiles();
            Profiles.PrintInfo();

            SettingsTable.InitSettingsTable();
            ThemeMaster.LoadUseTheme();
            DirectoriesHelper.CheckDirectories();
        }
    }
}
