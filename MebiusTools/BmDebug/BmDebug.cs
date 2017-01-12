using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BmDebug;

// Performance results
// file test

//Mutex WaitOne/Release;
//00:00:00.0000062
//00:00:00.0000065
//00:00:00.2482967
//00:00:00.2454702
//00:00:00.0075797
//00:00:00.0000177

//Monitor.lock
//00:00:00.0000059
//00:00:00.0000069
//00:00:00.1734397
//00:00:00.1267051
//00:00:00.0018302
//00:00:00.0000171


//Not Async
//00:00:05.3720133
//00:00:00.2117382

//Async Task.Factory.StartNew(() => Execute(m, text));
//00:00:00.0604626
//00:00:00.0000299

//ThreadPool.QueueUserWorkItem(Execute, new ExecuteState {Mode = m, ParamsText = text});
//00:00:00.0312737
//00:00:00.0000171

namespace BMTools
{
    /// <summary>
    ///     For Enable set DebugLevel;
    /// </summary>
    public static class BmDebug
    {
        private enum Mode
        {
            Info,
            Warning,
            Critical
        }
        private static string CurrentDateTime => (DateTime.Now.Day < 10 ? "0" : "") + DateTime.Now.Day + "."
                                                 + (DateTime.Now.Month < 10 ? "0" : "") + DateTime.Now.Month + "."
                                                 + DateTime.Now.Year%1000
                                                 + " "
                                                 + (DateTime.Now.Hour < 10 ? "0" : "") + DateTime.Now.Hour + ":"
                                                 + (DateTime.Now.Minute < 10 ? "0" : "") + DateTime.Now.Minute + ":"
                                                 + (DateTime.Now.Second < 10 ? "0" : "") + DateTime.Now.Second + ":"
                                                 + (DateTime.Now.Millisecond < 100 ? "0" : "")
                                                 + (DateTime.Now.Millisecond < 10 ? "0" : "") + DateTime.Now.Millisecond
            ;

        public static OutputModes Output { get; set; } = OutputModes.File;

        public enum DebugLevels
        {
            None,
            All,
            Info,
            Warning,
            Critical
        }

        //Debug disabled for default
        /// <summary>
        /// Change for enable
        /// </summary>
        public static DebugLevels DebugLevel { get; set; } = DebugLevels.None;

        /// <summary>
        ///     Only set
        /// </summary>
        public static string ClassUsing { private get; set; } = "Unknown";
        
        public enum OutputModes
        {
            File,
            LogWindow
        }
        public static Encoding Encoding = Encoding.GetEncoding("cp866");


        private static LogWindow _logWind;
        private static readonly object Monitor = new object();
        private static readonly string[] ModeTxt = {" (I) ", " (W) ", " (C) "};

        /// <summary>
        ///     Info
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Info(params object[] text)
        {
            Execute(Mode.Info, false, text);
        }
        public static void InfoAsync(params object[] text)
        {
            Execute(Mode.Info, true, text);
        }

        /// <summary>
        ///     Warning
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Warning(params object[] text)
        {
            Execute(Mode.Warning, false, text);
        }
        public static void WarningAsync(params object[] text)
        {
            Execute(Mode.Warning, true, text);
        }

        /// <summary>
        ///     Critical
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Crit(params object[] text)
        {
            Execute(Mode.Critical, false, text);
        }
        public static void CritAsyc(params object[] text)
        {
            Execute(Mode.Critical, true, text);
        }

        private static void Execute(Mode m, bool isAsync, params object[] text)
        {
            if (!isAsync)
                Execute(m, text);
            else
                ThreadPool.QueueUserWorkItem(Execute, new ExecuteState {Mode = m, ParamsText = text});
        }

        private class ExecuteState
        {
            public Mode Mode;
            public object[] ParamsText;
        }
        private static void Execute(object state)
        {
            var executeState = state as ExecuteState;
            if (executeState == null) throw new NullReferenceException("Execute(object state) state == null");

            Execute(executeState.Mode, executeState.ParamsText);
        }
        private static void Execute(Mode m, params object[] text)
        {
            if (DebugLevel != DebugLevels.All &&
                (
                    DebugLevel == DebugLevels.None ||
                    DebugLevel == DebugLevels.Info && m != Mode.Info ||
                    DebugLevel == DebugLevels.Warning && m != Mode.Warning ||
                    DebugLevel == DebugLevels.Critical && m != Mode.Critical
                    )
                ) return;
            if (text == null) return;
            if (text.Length == 0) return;
            var outp = string.Empty;
            foreach (var s in text)
            {
                if (s == null) continue;

                if (s.GetType() == typeof(List<string>))
                {
                    outp += ((List<string>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                    continue;
                }
                if (s.GetType() == typeof(List<int>))
                {
                    outp += ((List<int>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                    continue;
                }
                if (s.GetType() == typeof(List<double>))
                {
                    outp += ((List<double>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                    continue;
                }
                if (s.GetType() == typeof(object[]))
                {
                    outp += ((object[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                    continue;
                }
                if (s.GetType() == typeof(string[]))
                {
                    outp += ((string[]) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                    continue;
                }
                if (s.GetType() == typeof(int[]))
                {
                    outp += ((int[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                    continue;
                }
                if (s.GetType() == typeof(double[]))
                {
                    outp += ((double[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                    continue;
                }
                if (s.GetType() == typeof(long[]))
                {
                    outp += ((long[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                    continue;
                }
                outp += s + " ";
            }
            WriteDebug(outp, m);
        }

        private static void WriteDebug(string text, Mode m)
        {
            lock (Monitor)
            {
                if (Output == OutputModes.File) WriteDebugFile(text, m);
                else
                {
                    if (_logWind == null) _logWind = new LogWindow();
                    _logWind.WriteLine(CurrentDateTime + ModeTxt[(int) m] + ":" + text);
                }
            }
        }

        private static void WriteDebugFile(string text, Mode m)
        {
            using (var f = new FileStream(ClassUsing + ".txt", FileMode.OpenOrCreate))
            {
                f.Position = f.Length;
                using (var wr = new StreamWriter(f, Encoding) {AutoFlush = true})
                {
                    wr.WriteLine(CurrentDateTime + ModeTxt[(int) m] + ":" + text);
                    wr.Close();
                }
                f.Close();
            }
        }
    }
}