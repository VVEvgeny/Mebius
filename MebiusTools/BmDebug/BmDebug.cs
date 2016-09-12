using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using BmDebug;

namespace BMTools
{
    /// <summary>
    /// Класс для работы с дебагами
    /// Для работы обязательно включить его !!
    /// BMDebug.Enabled = true;
    /// </summary>
    public static class BmDebug
    {
        private static readonly Encoding Enc = Encoding.GetEncoding("cp866");

        private static bool _mUseFile;
        private static bool _mUseLogWindow = true;
        public enum DebugOutputModes { File, LogWindow };//Останется только 1 :)
        public static DebugOutputModes DebugOutput
        {
            set
            {
                _mUseFile = false;
                _mUseLogWindow = false;
                if (value == DebugOutputModes.File) _mUseFile = true;
                if (value == DebugOutputModes.LogWindow) _mUseLogWindow = true;
            }
        }

        private static bool _inited;
        private static LogWindow _logWind;

        private static Mutex _mutex;

        private enum Mode { Info, Warning, Critical }
        private static readonly string[] ModeTxt = { " (I) ", " (W) ", " (C) " };
        private static readonly string MEnabledModes = ModeTxt[(int)Mode.Info] + " " + ModeTxt[(int)Mode.Warning] + " " + ModeTxt[(int)Mode.Critical];//По умолчанию выводм все

        /// <summary>
        /// Можно только задать
        /// </summary>
        public static string ClassUsing { get; set; } = "self";

        /// <summary>
        /// Включить/выключить отладку
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Вывод с флагом (I)
        /// </summary>
        /// <param name="text">Текст отладочной информации</param>
        public static void Info(params object[] text)
        {
            Execute(Mode.Info, text);
        }
        /// <summary>
        /// Вывод с флагом (W)
        /// </summary>
        /// <param name="text">Текст отладочной информации</param>
        public static void Warn(params object[] text)
        {
            Execute(Mode.Warning, text);
        }
        /// <summary>
        /// Вывод с флагом (C)
        /// </summary>
        /// <param name="text"></param>
        public static void Crit(params object[] text)
        {
            Execute(Mode.Critical, text);
        }
        private static void Execute(Mode m, params object[] text)
        {
            if (!Enabled) return;
            if (text == null) return;//ссылки может не быть воообще
            if (text.Length == 0) return;
            var outp = string.Empty;
            foreach (var s in text)//все параметры пишем 1 строкой
            {
                if (s == null) continue;
                if (s.GetType() == typeof(List<string>))//массивы надо разложить в единую строку
                {
                    foreach (object oo in (List<string>)s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(List<int>))//массивы надо разложить в единую строку
                {
                    foreach (var oo in (List<int>)s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(List<double>))//массивы надо разложить в единую строку
                {
                    foreach (var oo in (List<double>)s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(List<double>))//массивы надо разложить в единую строку
                {
                    foreach (var oo in (List<double>)s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(object[]))//массивы надо разложить в единую строку
                {
                    foreach (var oo in (object[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(Object[]))//массивы надо разложить в единую строку
                {
                    foreach (var oo in (Object[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(string[]))
                {
                    foreach (object oo in (string[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(int[]))
                {
                    foreach (var oo in (int[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(double[]))
                {
                    foreach (var oo in (double[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                if (s.GetType() == typeof(long[]))
                {
                    foreach (var oo in (long[])s)
                    {
                        outp += oo + " ";
                    }
                    outp += ",";
                    continue;
                }
                outp += s + " ";
            }
            write_debug(outp, m);
        }


        /// <summary>
        /// Пока только для окна логирования + имя мьютекса
        /// </summary>
        private static void Init()
        {
            _mutex = new Mutex(false, "debug_class" + "_" + ClassUsing);
            _inited = true;
            if (_mUseLogWindow)
            {
                _logWind = new LogWindow();
                _logWind.Show();
            }
        }



        //Тут основная логика дебага
        private static void write_debug(string text, Mode m)
        {
            if (!_inited) Init();
            _mutex.WaitOne();
            if (!MEnabledModes.Contains(ModeTxt[(int)m])) return;
            if (_mUseFile) write_debug_file(text, m);
            if (_mUseLogWindow)
            {
                try
                {
                    _logWind.Write_Line(nice_date_time_string() + ModeTxt[(int)m] + ":" + text);
                }
                catch (Exception e)
                {
                    try
                    {
                        _logWind = new LogWindow();
                        _logWind.Show();
                    }
                    catch
                    {
                        _mUseLogWindow = false;
                        if (_mUseFile) write_debug_file(e.Message, Mode.Critical);
                    }
                }
            }
            _mutex.ReleaseMutex();
        }
        private static void write_debug_file(string text, Mode m)
        {
            var f = new FileStream(ClassUsing + ".txt", FileMode.OpenOrCreate);
            var wr = new StreamWriter(f, Enc) { AutoFlush = true };
            f.Position = f.Length;
            wr.WriteLine(nice_date_time_string() + ModeTxt[(int)m] + ":" + text);
            wr.Close();
            f.Close();
        }
        private static string nice_date_time_string()
        {
            var forRet = string.Empty;
            if (DateTime.Now.Day < 10) forRet += "0";
            forRet += DateTime.Now.Day + ".";
            if (DateTime.Now.Month < 10) forRet += "0";
            forRet += DateTime.Now.Month + ".";
            forRet += (DateTime.Now.Year % 1000) + " ";
            if (DateTime.Now.Hour < 10) forRet += "0";
            forRet += DateTime.Now.Hour + ":";
            if (DateTime.Now.Minute < 10) forRet += "0";
            forRet += DateTime.Now.Minute + ":";
            if (DateTime.Now.Second < 10) forRet += "0";
            forRet += DateTime.Now.Second + ":";
            if (DateTime.Now.Millisecond < 100) forRet += "0";
            if (DateTime.Now.Millisecond < 10) forRet += "0";
            forRet += DateTime.Now.Millisecond;
            return forRet;
        }
    }
}
