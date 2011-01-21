using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Wamz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (AppSettings.Default.ShowSettingsAtStartup)
            {
                AppSettings.Default.ShowSettingsAtStartup = false;
                AppSettings.Default.Save();
                Application.Run(new Settings());
            }
            else
            {
                new Settings();
                Application.Run();
            }
        }
    }
}
