using System;
using System.Windows.Forms;

namespace TestChecks
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BMTools.BmDebug.Debug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
            BMTools.BmDebug.Debug.Output = BMTools.BmDebug.OutputModes.File;
            BMTools.BmDebug.Debug.Info("start");

            Application.Run(new Form1());
        }
    }
}
