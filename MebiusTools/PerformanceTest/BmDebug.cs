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
            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.None;
            BMTools.BmDebug.Output = BMTools.BmDebug.OutputModes.LogWindow;

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
                    BMTools.BmDebug.Info(i);
                }
            }

            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
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
                    BMTools.BmDebug.Info(i);
                }
            }
            using (new OperationTimer("debug Async Enabled call reference log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.InfoAsync("test");
                }
            }
            using (new OperationTimer("debug Async Enabled call value log window"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.InfoAsync(i);
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
            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.None;
            BMTools.BmDebug.Output = BMTools.BmDebug.OutputModes.File;

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
                    BMTools.BmDebug.Info(i);
                }
            }

            BMTools.BmDebug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
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
                    BMTools.BmDebug.Info(i);
                }
            }
            
            using (new OperationTimer("debug Async Enabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.InfoAsync("test");
                }
            }
            using (new OperationTimer("debug Async Enabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.InfoAsync(i);
                }
            }
        }
    }
}
