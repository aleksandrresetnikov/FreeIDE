using FreeIDE.Common;
using FreeIDE.Components;

namespace FreeIDE
{
    static class Program
    {
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

        static void InitAllComponents()
        {
            SettingsTable.InitSettingsTable();
            ThemeMaster.LoadUseTheme();
        }
    }
}
