using System;
using System.Windows.Forms;
using static BMTools.BmDebug;

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

            Debug.ClassUsing = "UnitTests";
            Debug.Output = OutputModes.LogWindow;
            Debug.DebugLevel = DebugLevels.All;
            Debug.Info("start");

            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                Debug.Crit("Exception=", e.Message);
                throw;
            }
        }
    }
}
