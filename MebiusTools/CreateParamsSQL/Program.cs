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
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
            BMTools.BmDebug.Enabled = true;
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
