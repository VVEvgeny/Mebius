using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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


//First call only Sync!!
namespace BMTools
{
    /// <summary>
    ///     For Enable set DebugLevel;
    /// </summary>
    public class BmDebug
    {
        private static readonly Lazy<BmDebug> Instance = new Lazy<BmDebug>(() => new BmDebug());
        public static BmDebug Debug => Instance.Value;

        private enum Mode
        {
            Info,
            Warning,
            Critical
        }

        //private LogWindow _logWind = new LogWindow();
        private LogWindow _logWind;
        private LogWindow LogWindow
        {
            get
            {
                lock (_monitorLogWindow)
                {
                    if (_logWind == null || _logWind.IsDisposed) _logWind = new LogWindow();
                }
                return _logWind;
            }
        }
        public OutputModes Output
        {
            get => _output;
            set
            {
                _output = value;
                lock (_monitorLogWindow)
                {
                    
                    if (value == OutputModes.File) LogWindow.Hide();
                    else LogWindow.Show();
                }
            }
        }

        public bool DisableTimeStamp = false;

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
        public DebugLevels DebugLevel { get; set; } = DebugLevels.None;

        /// <summary>
        ///     Only set
        /// </summary>
        public string ClassUsing { private get; set; } = "Unknown";
        
        public enum OutputModes
        {
            File,
            LogWindow
        }
        public Encoding Encoding = Encoding.GetEncoding("cp866");

        private readonly object _monitor = new object();
        private readonly object _monitorLogWindow = new object();
        private readonly string[] _modeTxt = {" (I) ", " (W) ", " (C) "};
        private OutputModes _output;


        private class CallerInfo
        {
            public static bool HaveCaller = false;

            public static string MemberName = string.Empty;
            public static string FilePath = string.Empty;
            public static int LineNumber;

        }

        public BmDebug SaveCall(bool fullFilePath = false,[CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            CallerInfo.HaveCaller = true;
            CallerInfo.MemberName = memberName;
            CallerInfo.FilePath = fullFilePath ? filePath : Path.GetFileName(filePath);
            CallerInfo.LineNumber = lineNumber;

            return Instance.Value;
        }

        /// <summary>
        ///     Info
        /// </summary>
        /// <param name="text">text/object</param>
        public void Info(params object[] text)
        {
            Execute(Mode.Info, false, text);
        }
        public void InfoAsync(params object[] text)
        {
            Execute(Mode.Info, true, text);
        }

        public void Info(int val)
        {
            Execute(Mode.Info, false, val);
        }
        public void InfoAsync(int val)
        {
            Execute(Mode.Info, true, val);
        }

        /// <summary>
        ///     Warning
        /// </summary>
        /// <param name="text">text/object</param>
        public void Warning(params object[] text)
        {
            Execute(Mode.Warning, false, text);
        }
        public void WarningAsync(params object[] text)
        {
            Execute(Mode.Warning, true, text);
        }

        /// <summary>
        ///     Critical
        /// </summary>
        /// <param name="text">text/object</param>
        public void Crit(params object[] text)
        {
            Execute(Mode.Critical, false, text);
        }
        public void CritAsyc(params object[] text)
        {
            Execute(Mode.Critical, true, text);
        }

        private void Execute(Mode m, bool isAsync, params object[] text)
        {
            if (!isAsync)
                Execute(m, text);
            else
                ThreadPool.QueueUserWorkItem(Execute, new ExecuteState {Mode = m, ParamsText = text});
        }
        private void Execute(Mode m, bool isAsync, int val)
        {
            if (!isAsync)
                Execute(m, val);
            else
                ThreadPool.QueueUserWorkItem(ExecuteVal, new ExecuteStateValue { Mode = m, ParamsText = val });
        }

        private class ExecuteState
        {
            public Mode Mode;
            public object[] ParamsText;
        }
        private class ExecuteStateValue
        {
            public Mode Mode;
            public int ParamsText;
        }

        private void Execute(object state)
        {
            var executeState = state as ExecuteState;
            if (executeState == null) throw new NullReferenceException("Execute(object state) state == null");

            Execute(executeState.Mode, executeState.ParamsText);
        }

        private void ExecuteVal(object state)
        {
            var executeState = state as ExecuteStateValue;
            if (executeState == null) throw new NullReferenceException("ExecuteVal(object state) state == null");

            Execute(executeState.Mode, executeState.ParamsText);
        }

        private void Execute(Mode m, int val)
        {
            if (DebugLevel != DebugLevels.All &&
                (
                    DebugLevel == DebugLevels.None ||
                    DebugLevel == DebugLevels.Info && m != Mode.Info ||
                    DebugLevel == DebugLevels.Warning && m != Mode.Warning ||
                    DebugLevel == DebugLevels.Critical && m != Mode.Critical
                    )
                ) return;

            lock (_monitor)
            {
                if (Output == OutputModes.File)
                {
                    using (var f = new FileStream(ClassUsing + ".txt", FileMode.OpenOrCreate))
                    {
                        f.Position = f.Length;
                        using (var wr = new StreamWriter(f, Encoding) { AutoFlush = true })
                        {
                            wr.WriteLine(DateTime.Now.ToString("dd.MM.yy hh:mm:ss.fff") + _modeTxt[(int)m] + ":" + val);
                            wr.Close();
                        }
                        f.Close();
                    }
                }
                else
                {
                    lock (_monitorLogWindow)
                    {
                        LogWindow.WriteLine(DateTime.Now.ToString("dd.MM.yy hh:mm:ss.fff") + _modeTxt[(int) m] + ":" +
                                            val);
                    }
                }
            }
        }

        private void Execute(Mode m, params object[] text)
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

                if (s.GetType() == typeof(MethodBase))
                {
                    var mb = (MethodBase)s;
                    outp += $"Method {mb.Name} (" + mb.GetParameters().Aggregate(outp, (current, param) => current + param.Name + param.ToString()) + ")";
                }
                else if (s.GetType() == typeof(List<string>))
                {
                    outp += ((List<string>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                }
                else if (s.GetType() == typeof(List<int>))
                {
                    outp += ((List<int>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                }
                else if (s.GetType() == typeof(List<double>))
                {
                    outp += ((List<double>) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                }
                else if (s.GetType() == typeof(object[]))
                {
                    outp += ((object[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                }
                else if (s.GetType() == typeof(string[]))
                {
                    outp += ((string[]) s).Aggregate(outp, (current, oo) => current + oo + " ") + ",";
                }
                else if (s.GetType() == typeof(int[]))
                {
                    outp += ((int[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                }
                else if (s.GetType() == typeof(double[]))
                {
                    outp += ((double[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                }
                else if (s.GetType() == typeof(long[]))
                {
                    outp += ((long[]) s).Aggregate(outp, (current, oo) => current + (oo + " ")) + ",";
                }
                else
                {
                    outp += s + " ";
                }
            }

            lock (_monitor)
            {
                var strAppend = string.Empty;
                if (CallerInfo.HaveCaller)
                {
                    CallerInfo.HaveCaller = false;
                    strAppend = "(" + CallerInfo.FilePath + "("+CallerInfo.MemberName+")" + ":" + CallerInfo.LineNumber + "):";
                }

                var str = (DisableTimeStamp ? string.Empty : DateTime.Now.ToString("dd.MM.yy hh:mm:ss.fff"))
                    + _modeTxt[(int)m] + ":"+ strAppend + outp;

                if (Output == OutputModes.File)
                {
                    using (var f = new FileStream(ClassUsing + ".txt", FileMode.OpenOrCreate))
                    {
                        f.Position = f.Length;
                        using (var wr = new StreamWriter(f, Encoding) {AutoFlush = true})
                        {
                            
                            wr.WriteLine(str);
                            wr.Close();
                        }
                        f.Close();
                    }
                }
                else
                {
                    lock (_monitorLogWindow)
                    {
                        LogWindow.WriteLine(str);
                    }
                }
            }
        }
    }
}