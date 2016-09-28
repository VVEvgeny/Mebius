using System;
using System.Windows.Forms;

namespace UnitTests
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

            BMTools.BmDebug.ClassUsing = "UnitTests";
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;
            BMTools.BmDebug.Enabled = true;
            BMTools.BmDebug.Info("start");

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                BMTools.BmDebug.Crit("Exception=", e.Message);
                throw;
            }
        }
    }
}
