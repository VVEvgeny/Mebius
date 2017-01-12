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
            BMTools.BmDebug.Output = BMTools.BmDebug.OutputModes.LogWindow;
            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
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
