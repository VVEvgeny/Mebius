using System;
using System.Windows.Forms;
using Ninject;
using Ninject.Modules;
using OracleBases.Database;
using static System.Reflection.Assembly;
using static BMTools.BmDebug;

namespace OracleBases.Windows
{
    public static class CompositionRoot
    {
        private static IKernel _ninjectKernel;

        public static void Wire(INinjectModule module)
        {
            _ninjectKernel = new StandardKernel(module);
        }

        public static T Resolve<T>()
        {
            return _ninjectKernel.Get<T>();
        }
    }

    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IUnitOfWork)).To(typeof(EfUnitOfWork));
        }
    }
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CompositionRoot.Wire(new ApplicationModule());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Debug.ClassUsing = GetExecutingAssembly().GetName().Name;
            Debug.DebugLevel = DebugLevels.All;
            Debug.Output = OutputModes.LogWindow;
            Debug.Info("start");

            //Application.Run(new MainForm());
            Application.Run(CompositionRoot.Resolve<MainForm>());
        }
    }
}
