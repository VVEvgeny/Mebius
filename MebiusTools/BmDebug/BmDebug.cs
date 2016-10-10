using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BmDebug;

namespace BMTools
{
    /// <summary>
    ///     Enable For use
    ///     BMDebug.Enabled = true;
    /// </summary>
    public static class BmDebug
    {
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

        public static DebugOutputModes DebugOutput
        {
            set
            {
                _mUseFile = value == DebugOutputModes.File;
                _mUseLogWindow = !_mUseFile;
            }
        }

        /// <summary>
        ///     Only set
        /// </summary>
        public static string ClassUsing { private get; set; } = "NOT SET";

        /// <summary>
        ///     Enable
        /// </summary>
        public static bool Enabled { get; set; }

        public enum DebugOutputModes
        {
            File,
            LogWindow
        }

        private static readonly Encoding Enc = Encoding.GetEncoding("cp866");

        private static bool _mUseFile;
        private static bool _mUseLogWindow;

        private static bool _inited;
        private static LogWindow _logWind;

        private static Mutex _mutex;
        private static readonly string[] ModeTxt = {" (I) ", " (W) ", " (C) "};

        private static readonly string MEnabledModes = ModeTxt[(int) Mode.Info] + " " + ModeTxt[(int) Mode.Warning] +
                                                       " " + ModeTxt[(int) Mode.Critical];

        /// <summary>
        ///     Info
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Info(params object[] text)
        {
            Execute(Mode.Info, text);
        }

        /// <summary>
        ///     Warning
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Warning(params object[] text)
        {
            Execute(Mode.Warning, text);
        }

        /// <summary>
        ///     Critical
        /// </summary>
        /// <param name="text">text/object</param>
        public static void Crit(params object[] text)
        {
            Execute(Mode.Critical, text);
        }

        private static void Execute(Mode m, params object[] text)
        {
            if (!Enabled) return;
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

        private static void Init()
        {
            _mutex = new Mutex(false, "debug_class" + "_" + ClassUsing);
            _inited = true;
        }

        private static void WriteDebug(string text, Mode m)
        {
            if (!_inited) Init();
            if (!MEnabledModes.Contains(ModeTxt[(int) m])) return;

            _mutex.WaitOne();
            if (_mUseFile) WriteDebugFile(text, m);
            else if (_mUseLogWindow)
            {
                if (_logWind == null) _logWind = new LogWindow();
                _logWind.WriteLine(CurrentDateTime + ModeTxt[(int) m] + ":" + text);
            }
            _mutex.ReleaseMutex();
        }

        private static void WriteDebugFile(string text, Mode m)
        {
            using (var f = new FileStream(ClassUsing + ".txt", FileMode.OpenOrCreate))
            {
                f.Position = f.Length;
                using (var wr = new StreamWriter(f, Enc) {AutoFlush = true})
                {
                    wr.WriteLine(CurrentDateTime + ModeTxt[(int) m] + ":" + text);
                    wr.Close();
                }
                f.Close();
            }
        }

        private enum Mode
        {
            Info,
            Warning,
            Critical
        }
    }
}