/*
�����
-����� ����������� ��� ������ ������� ���� �����(int long....)
-����� ��������� �� �����������������
-����� ����� ��������� ��� � ���� �����
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;//test

/// <summary>
/// ����� ��� ������ � ��������
/// ��� ������ ����������� �������� ��� !!
/// bm_debug.Enabled = true;
/// </summary>
static class bm_debug
{
    private static Encoding enc = Encoding.GetEncoding("cp866");
    private static string m_class_using = "self";
    private static bool m_enabled = false;

    private static bool m_use_file = false;
    private static bool m_use_forms = false;//������ ��� ������ ����
    private static bool m_use_log_window = true;
    public enum Debug_Output_Modes { File, MessageBox, Log_Window };//��������� ������ 1 :)
    public static Debug_Output_Modes Debug_Output
    {
        set
        {
            m_use_file = false;
            m_use_forms = false;
            m_use_log_window = false;
            if (value == Debug_Output_Modes.File) m_use_file = true;
            if (value == Debug_Output_Modes.MessageBox) m_use_forms = true;
            if (value == Debug_Output_Modes.Log_Window) m_use_log_window = true;
        }
    }

    private static bool inited = false;
    private static log_window log_wind;

    private static Mutex mutex;// = new Mutex(false,"debug_class");//����� ����������, ����� 2 ���������� ����� ����� ���������� ������, ���� ������� ������� ��������
    public enum Mode { Info, Warning, Critical };
    private static string[] mode_txt = new string[] { " (I) ", " (W) "," (C) " };
    private static string[] mode_txt_nice = new string[] { " Information ", " Warning ", " Critical " };
    private static string m_enabled_modes = mode_txt[(Int32)Mode.Info] + " " + mode_txt[(Int32)Mode.Warning] + " " + mode_txt[(Int32)Mode.Critical];//�� ��������� ������ ���

    /// <summary>
    /// ��������� ����� ��� ������
    /// </summary>
    /// <param name="_m">�� ������� bm_debug.Modes</param>
    public static void Mode_Add(Mode _m)
    {
        if (!m_enabled_modes.Contains(mode_txt[(Int32)_m]))
        {
            m_enabled_modes += " " + mode_txt[(Int32)_m];
        }
    }
    /// <summary>
    /// ������� ����� ��� ������
    /// </summary>
    /// <param name="_m">�� ������� bm_debug.Modes</param>
    public static void Mode_Del(Mode _m)
    {
        if (m_enabled_modes.Contains(mode_txt[(Int32)_m]))
        {
            m_enabled_modes = m_enabled_modes.Replace(mode_txt[(Int32)_m], "");
        }
    }
    /// <summary>
    /// ����� ������ ������
    /// </summary>
    public static string Class_using
    {
        set
        {
            m_class_using = value;
        }
    }
    /// <summary>
    /// ��������/��������� �������
    /// </summary>
    public static bool Enabled
    {
        get
        {
            return m_enabled;
        }
        set
        {
            m_enabled = value;
        }
    }
    /// <summary>
    /// ����� � ������ (I)
    /// </summary>
    /// <param name="_text">����� ���������� ����������</param>
    public static void _Info(params object[] _text)
    {
        Execute(Mode.Info, _text);
    }
    /// <summary>
    /// ����� � ������ (W)
    /// </summary>
    /// <param name="_text">����� ���������� ����������</param>
    public static void _Warn(params object[] _text)
    {
        Execute(Mode.Warning, _text);
    }
    /// <summary>
    /// ����� � ������ (C)
    /// </summary>
    /// <param name="_text"></param>
    public static void _Crit(params object[] _text)
    {
        Execute(Mode.Critical, _text);
    }
    private static void Execute(Mode _m, params object[] _text)//�������� ��� ����������!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (!m_enabled) return;
        if (_text == null) return;//������ ����� �� ���� �������
        if (_text.Length == 0) return;
        string outp = "";
        foreach (object s in _text)//��� ��������� ����� 1 �������
        {
            if(s==null)continue;
            else if (s.GetType() == typeof(List<string>))//������� ���� ��������� � ������ ������
            {
                foreach (object oo in (List<string>)s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(List<int>))//������� ���� ��������� � ������ ������
            {
                foreach (object oo in (List<int>)s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(List<double>))//������� ���� ��������� � ������ ������
            {
                foreach (object oo in (List<double>)s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(List<double>))//������� ���� ��������� � ������ ������
            {
                foreach (object oo in (List<double>)s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(object[]))//������� ���� ��������� � ������ ������
            {
                foreach (object oo in (object[])s)
                {
                    outp += oo+" ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(Object[]))//������� ���� ��������� � ������ ������
            {
                foreach (Object oo in (Object[])s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(string[]))
            {
                foreach (object oo in (string[])s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(int[]))
            {
                foreach (object oo in (int[])s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(double[]))
            {
                foreach (object oo in (double[])s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            else if (s.GetType() == typeof(long[]))
            {
                foreach (object oo in (long[])s)
                {
                    outp += oo + " ";
                }
                outp += ",";
                continue;
            }
            outp += s+" ";
        }
        write_debug(outp, _m);
    }


    /// <summary>
    /// ���� ������ ��� ���� ����������� + ��� ��������
    /// </summary>
    private static void Init()
    {
        mutex = new Mutex(false, "debug_class" + "_" + m_class_using);
        inited = true;
        if (m_use_log_window)
        {
            log_wind = new log_window();
            log_wind.Show();
        }
    }
    
    
    
    //��� �������� ������ ������
    private static void write_debug(string _text,Mode _m)
    {
        //TEST


        //StackTrace stackTrace =  new StackTrace();
        //string tmp = "test " + stackTrace.GetFrame(0).GetFileName + ":" + _text;


        //_text = tmp;


        if (!inited) Init();
        mutex.WaitOne();
        if (!m_enabled_modes.Contains(mode_txt[(Int32)_m])) return;
        if (m_use_file) write_debug_file(_text,_m);
        if (m_use_forms) MessageBox.Show(_text, mode_txt_nice[(Int32)_m], MessageBoxButtons.OK, (_m == Mode.Info ? MessageBoxIcon.Information : _m == Mode.Warning ? MessageBoxIcon.Warning : _m == Mode.Critical ? MessageBoxIcon.Error : MessageBoxIcon.None));
        if (m_use_log_window)
        {
            try
            {
                log_wind.Write_Line(nice_date_time_string() + mode_txt[(Int32)_m] + ":" + _text);
            }
            catch (Exception e)
            {
                try
                {
                    log_wind = new log_window();
                    log_wind.Show();
                }
                catch
                {
                    m_use_log_window = false;
                    if (m_use_file) write_debug_file(e.Message, Mode.Critical);
                    if (m_use_forms) MessageBox.Show(e.Message, mode_txt_nice[(Int32)Mode.Critical], MessageBoxButtons.OK,  MessageBoxIcon.Error);
                }
            }
        }
        mutex.ReleaseMutex();
    }
    private static void write_debug_file(string _text,Mode _m)
    {
        FileStream f = new FileStream(m_class_using.ToString()+".txt", FileMode.OpenOrCreate);
        StreamWriter wr = new StreamWriter(f,enc);
        wr.AutoFlush = true;
        f.Position = f.Length;
        wr.WriteLine(nice_date_time_string()+mode_txt[(Int32)_m]+":"+_text);
        wr.Close();
        f.Close();
    }
    private static string nice_date_time_string()
    {
        string for_ret = "";
        if (DateTime.Now.Day < 10) for_ret += "0";
        for_ret += DateTime.Now.Day+".";
        if (DateTime.Now.Month < 10) for_ret += "0";
        for_ret += DateTime.Now.Month + ".";
        for_ret += (DateTime.Now.Year % 1000).ToString() + " ";
        if (DateTime.Now.Hour < 10) for_ret += "0";
        for_ret += DateTime.Now.Hour + ":";
        if (DateTime.Now.Minute < 10) for_ret += "0";
        for_ret += DateTime.Now.Minute + ":";
        if (DateTime.Now.Second < 10) for_ret += "0";
        for_ret += DateTime.Now.Second + ":";
        if (DateTime.Now.Millisecond < 100) for_ret += "0";
        if (DateTime.Now.Millisecond < 10) for_ret += "0";
        for_ret += DateTime.Now.Millisecond;
        return for_ret;
    }
}
