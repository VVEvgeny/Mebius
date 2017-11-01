using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TestChecks
{
    public partial class Form1 : Form
    {
        private readonly List<string> _packets = new List<string>();
        private const string MergeAppAddr = @"C:\Program Files (x86)\Araxis2\compare.exe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVersions();
        }

        private void LoadVersions()
        {
            var dir = new DirectoryInfo(@"\\linux3.minsk.mebius.net\exe\kernel.mt\data\home\" +
                                        textBoxUser.Text + @"\");
            comboBoxVersions.Items.Clear();
            comboBoxPackets.Items.Clear();
            _packets.Clear();

            foreach (var d in dir.GetDirectories())
                if (d.Name.Length == 8 && d.Name.Contains("00"))
                {
                    if (!comboBoxVersions.Items.Contains(d.Name.Substring(0, 3)))
                        comboBoxVersions.Items.Add(d.Name.Substring(0, 3));
                    _packets.Add(d.Name);
                }
            if (comboBoxVersions.Items.Count > 0)
                comboBoxVersions.Text = comboBoxVersions.Items[comboBoxVersions.Items.Count - 1].ToString();
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            LoadVersions();
        }

        private void comboBoxVersions_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxPackets.Items.Clear();
            foreach (var p in _packets.OrderBy(t=>t))
                if (p.Substring(0, 3) == comboBoxVersions.Text)
                    comboBoxPackets.Items.Add(p);
            if (comboBoxPackets.Items.Count > 0)
                comboBoxPackets.Text = comboBoxPackets.Items[comboBoxPackets.Items.Count - 1].ToString();
        }

        private void comboBoxPackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxRunPM.Text = @"/exe/kernel.mt/tools/_testpm " + comboBoxPackets.Text;
        }

        private void buttonCopyToBuff_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBoxRunPM.Text);
        }

        private void GetFiles(string curPath, List<string> checkFiles)
        {
            var dir = new DirectoryInfo(curPath);
            foreach (var f in dir.GetFiles())
                if (f.Name.Contains("check") && f.Extension.ToLower() == ".txt")
                {
                    checkFiles.Add(f.FullName);
                    BMTools.BmDebug.Debug.InfoAsync("added to check=" + f.FullName);
                }
            foreach (var d in dir.GetDirectories())
                GetFiles(d.FullName, checkFiles); //recursive
        }

        private string GetTestDir()
        {
            return @"\\linux3.minsk.mebius.net\exe\kernel.mt\data\home\" + textBoxUser.Text + @"\" +
                   comboBoxPackets.Text + @"\";
        }

        private string GetUserTestDir()
        {
            return @"\\linux3.minsk.mebius.net\exe\kernel.mt\data\home\" + textBoxUser.Text + @"\etalon." +
                   comboBoxVersions.Text + @".local\";
        }

        private static void ReplaceFiles(IEnumerable<string> from, string fromDir, string toDir)
        {
            foreach (var s in from)
                //bm_debug._Info("Копирую файл из=", s, " в=", s.Replace(from_dir, to_dir));
                new FileInfo(s).CopyTo(s.Replace(fromDir, toDir), true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkFiles = new List<string>();
            GetFiles(GetTestDir(), checkFiles);
            ReplaceFiles(checkFiles, GetTestDir(), GetUserTestDir());
            MessageBox.Show(@"Copied " + checkFiles.Count + @" files");
        }

        private void CompareFiles(IEnumerable<string> from, string fromDir, string toDir)
        {
            var lastTask = "-";
            var skip = false;

            TasksCountDifference.Clear();

            var from2 = new List<string>();
            foreach (var s in from)
            {
                var task = s.Substring(s.IndexOf("\\task", StringComparison.Ordinal) + 5,
                    s.IndexOf("test\\", StringComparison.Ordinal) -
                    (s.IndexOf("\\task", StringComparison.Ordinal) + 5));
                try
                {
                    var f1 = new FileInfo(s);
                    var f2 = new FileInfo(s.Replace(fromDir, toDir));

                    if (f1.Length != f2.Length)
                    {
                        TasksCountDifference.Add(task);
                        from2.Add(s);
                    }
                    else
                    {
                        if (File.ReadAllText(f1.FullName) != File.ReadAllText(f2.FullName))
                        {
                            TasksCountDifference.Add(task);
                            from2.Add(s);
                        }
                    }
                }
                catch (Exception)
                {
                    //bm_debug._Crit("Нет файла или ещё что=", e.Message);
                }
            }

            foreach (var s in from2) //Тут покажет только файлы и задачи которые отличаются
            {
                var task = s.Substring(s.IndexOf("\\task", StringComparison.Ordinal) + 5,
                    s.IndexOf("test\\", StringComparison.Ordinal) -
                    (s.IndexOf("\\task", StringComparison.Ordinal) + 5));
                //bm_debug._Info("задача=",task);
                if (lastTask != task)
                {
                    lastTask = task;
                    skip = false;
                    var dr = MessageBox.Show(@"Task " + task + Environment.NewLine + "\t" + @"have different files " +
                                             TasksCountDifference.GetCol(task)
                                             + Environment.NewLine + "\t" + @"Compare ?"
                        , @"Compare " + task + @" ?"
                        , MessageBoxButtons.YesNoCancel);

                    switch (dr)
                    {
                        case DialogResult.Cancel:
                            return;
                        case DialogResult.No:
                            skip = true;
                            break;
                    }
                }
                if (skip) continue;

                RunCompare(s.Replace(fromDir, toDir), s);
            }
        }

        private void RunCompare(string leftFile, string rightFile)
        {
            try
            {
                if (Environment.OSVersion.Platform != PlatformID.Win32NT) return;
                var startinfo = new ProcessStartInfo
                {
                    FileName = MergeAppAddr,
                    Arguments = leftFile + " " + rightFile,
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                Process.Start(startinfo);
            }
            catch
            {
                // ignored
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var checkFiles = new List<string>();
            GetFiles(GetTestDir(), checkFiles);
            CompareFiles(checkFiles, GetTestDir(), GetUserTestDir());
        }

        private string get_etalon()
        {
            return @"\\linux3.minsk.mebius.net\exe\kernel.mt\data\etalon." + comboBoxVersions.Text + @"\";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var checkFiles = new List<string>();
            GetFiles(GetTestDir(), checkFiles);
            CompareFiles(checkFiles, GetTestDir(), get_etalon());
        }

        private static class TasksCountDifference
        {
            private static readonly List<string> Task = new List<string>();
            private static readonly List<int> Col = new List<int>();

            public static void Add(string task)
            {
                if (task == null) throw new ArgumentNullException(nameof(task));
                var idx = Task.IndexOf(task);
                if (idx == -1)
                {
                    Task.Add(task);
                    Col.Add(1);
                }
                else
                {
                    Col[idx]++;
                }
            }

            public static int GetCol(string task)
            {
                if (task == null) throw new ArgumentNullException(nameof(task));
                var idx = Task.IndexOf(task);
                if (idx == -1) return -1;
                return Col[idx];
            }

            public static void Clear()
            {
                Task.Clear();
                Col.Clear();
            }
        }
    }
}