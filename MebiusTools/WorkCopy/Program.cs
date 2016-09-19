using System;
using System.Windows.Forms;

namespace WorkCopy
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

            BMTools.BmDebug.ClassUsing = "WorkCopy";
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
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
