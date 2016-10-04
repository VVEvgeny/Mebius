namespace PerformanceTest
{
    class Program
    {
        static void Main()
        {
            var iterations = 100;

            BMTools.BmDebug.ClassUsing = "test1";
            
            BMTools.BmDebug.Enabled = false;
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
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

            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
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
            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.File;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
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

            BMTools.BmDebug.DebugOutput = BMTools.BmDebug.DebugOutputModes.LogWindow;
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Info("test");
            }
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
}
