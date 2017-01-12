using System;
using System.Windows.Forms;

namespace CreateParamsSQL
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
            BMTools.BmDebug.ClassUsing = "CreateParamsSQL";
            BMTools.BmDebug.Output = BMTools.BmDebug.OutputModes.File;
            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
            BMTools.BmDebug.Info("start");

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception e)
            {
                BMTools.BmDebug.Crit("Exception=", e.Message);
                throw;
            }
        }
    }
}
