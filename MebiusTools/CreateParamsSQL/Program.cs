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
            BMTools.BmDebug.Debug.ClassUsing = "CreateParamsSQL";
            BMTools.BmDebug.Debug.Output = BMTools.BmDebug.OutputModes.File;
            BMTools.BmDebug.Debug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
            BMTools.BmDebug.Debug.Info("start");

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception e)
            {
                BMTools.BmDebug.Debug.Crit("Exception=", e.Message);
                throw;
            }
        }
    }
}
