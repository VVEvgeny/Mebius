using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTest
{
    public sealed class BmDebugLogWindow : ITest
    {
        private void Warming(int iterations)
        {
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
        }
        public void Test()
        {
            var iterations = 100;
            BMTools.BmDebug.ClassUsing = "BmDebugLogWindow";
            BMTools.BmDebug.Enabled = false;
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;

            Warming(iterations);

            using (new OperationTimer("debug Disabled call reference log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Disabled call value log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }

            BMTools.BmDebug.Enabled = true;
            using (new OperationTimer("debug Enabled call reference log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Enabled call value log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }
        }
    }
    public sealed class BmDebugFile : ITest
    {
        private void Warming(int iterations)
        {
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
        }
        public void Test()
        {
            var iterations = 100;
            BMTools.BmDebug.ClassUsing = "test1";
            BMTools.BmDebug.Enabled = false;
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;

            Warming(iterations);

            using (new OperationTimer("debug Disabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Disabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }

            BMTools.BmDebug.Enabled = true;

            Warming(iterations);

            using (new OperationTimer("debug Enabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info("test");
                }
            }
            using (new OperationTimer("debug Enabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Info(105);
                }
            }
        }
    }
}
