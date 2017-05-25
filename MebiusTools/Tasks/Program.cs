using System;
using System.Windows.Forms;
using static System.Reflection.Assembly;
using static BMTools.BmDebug;

namespace Tasks
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

            Debug.ClassUsing = GetExecutingAssembly().GetName().Name;
            Debug.DebugLevel = DebugLevels.All;
            Debug.Output = OutputModes.LogWindow;
            Debug.Info("Started");
            //Debug.InfoAsync("Started");

            Application.Run(new MainForm());
        }
    }
}
