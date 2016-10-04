using System;
using System.Diagnostics;

namespace PerformanceTest
{
    sealed class OperationTimer: IDisposable
    {
        private readonly Stopwatch _startTime;
        private readonly string _text;
        private readonly int _collectionCount;

        public OperationTimer(string text)
        {
            Prepare();

            _text = text;
            _collectionCount = GC.CollectionCount(0);
            _startTime = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Console.WriteLine($"time={_startTime.Elapsed} GC={_collectionCount} text={_text}");
        }

        private static void Prepare()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}