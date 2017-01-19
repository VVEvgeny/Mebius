namespace PerformanceTest
{
    public sealed class BmDebugFile : ITest
    {
        private void Warming(int iterations)
        {
            for (var i = 0; i < iterations; i++)//прогрев
            {
                BMTools.BmDebug.Debug.Info("test");
            }
        }
        public void Test()
        {
            var iterations = 100;
            BMTools.BmDebug.Debug.ClassUsing = "test1";
            BMTools.BmDebug.Debug.DebugLevel = BMTools.BmDebug.DebugLevels.None;
            BMTools.BmDebug.Debug.Output = BMTools.BmDebug.OutputModes.File;

            Warming(iterations);

            using (new OperationTimer("debug Disabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.Info("test");
                }
            }
            using (new OperationTimer("debug Disabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.Info(i);
                }
            }


            BMTools.BmDebug.Debug.DebugLevel = BMTools.BmDebug.DebugLevels.All;
            using (new OperationTimer("debug Enabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.Info("test");
                }
            }
            using (new OperationTimer("debug Enabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.Info(i);
                }
            }
            
            using (new OperationTimer("debug Async Enabled call reference file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.InfoAsync("test");
                }
            }
            using (new OperationTimer("debug Async Enabled call value file"))
            {
                for (var i = 0; i < iterations; i++)
                {
                    BMTools.BmDebug.Debug.InfoAsync(i);
                }
            }
        }
    }
}
