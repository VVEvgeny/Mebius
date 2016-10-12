using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExtensionMethods;
using Timer = System.Threading.Timer;

namespace CppClean
{
    public partial class MainForm : Form
    {
        private readonly Encoding _encoding;
        private List<AllFilesClass> _allFiles;
        private DateTime _checkTime;

        private ProgramStates _programState;

        private bool _timerWork;

        private Thread _workThread;
        private List<Thread> _threadsList;

        private ProgramStates ProgramState
        {
            set
            {
                _programState = value;
                switch (_programState)
                {
                    case ProgramStates.Started:
                    {
                        buttonLoadFiles.Enabled = true;
                        LockControlButtons(true);
                        buttonChangeDirectory.Enabled = true;
                        _allFiles = null;
                        labelLoadedCodeFilesCount.Text = @"0";
                    }
                        break;
                    case ProgramStates.WaitAction:
                    {
                        buttonLoadFiles.Enabled = true;
                        LockControlButtons(false);
                    }
                        break;
                    case ProgramStates.Worked:
                    {
                        buttonLoadFiles.Enabled = false;
                        LockControlButtons(true);
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private const int Timertimeout = 10;

        private static Timer _progressTimer;

        public MainForm()
        {
            InitializeComponent();

            ProgressEvent.Event += Work;
            ProgramState = ProgramStates.Started;
            _progressTimer = new Timer(ProgrressTimerTick, null, Timertimeout, Timeout.Infinite);

            _encoding = Encoding.GetEncoding("cp866");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            _allFiles = new List<AllFilesClass>();
            LoadAllFiles(textBoxDirectory.Text);
            ProgramState = ProgramStates.WaitAction;

            labelLoadedCodeFilesCount.Text = _allFiles.Count(f => f.Type == AllFilesClass.ExtType.CppFile).ToString();
            labelLoadedHeadersFilesCount.Text = _allFiles.Count(f => f.Type == AllFilesClass.ExtType.HppFile).ToString();
            labelLoadedMakesFilesCount.Text = _allFiles.Count(f => f.Type == AllFilesClass.ExtType.MakeFile).ToString();
            labelLoadedForFilesCount.Text = _allFiles.Count(f => f.Type == AllFilesClass.ExtType.ForFile).ToString();
            labelLoadedUnknownFilesCount.Text =
                _allFiles.Count(f => f.Type == AllFilesClass.ExtType.UnknownFile).ToString();
            labelLoadedTotalFilesCount.Text = _allFiles.Count.ToString();
        }

        private AllFilesClass.ExtType GetType(string extension)
        {
            switch (extension)
            {
                case ".cpp":
                case ".c":
                {
                    return AllFilesClass.ExtType.CppFile;
                }
                case ".mak":
                case ".inc":
                {
                    return AllFilesClass.ExtType.MakeFile;
                }
                case ".rec":
                case ".h":
                case ".hpp":
                case ".rc":
                {
                    return AllFilesClass.ExtType.HppFile;
                }
                case ".for":
                {
                    return AllFilesClass.ExtType.ForFile;
                }
            }
            return AllFilesClass.ExtType.UnknownFile;
        }

        private void LoadAllFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            foreach (var f in dir.GetFiles())
                _allFiles.Add(new AllFilesClass
                {
                    Address = f.FullName,
                    Name = f.Name.ToLower(),
                    Type = GetType(f.Extension)
                });
            foreach (var d in dir.GetDirectories())
                LoadAllFiles(d.FullName);
        }

        private void LockControlButtons(bool needLock)
        {
            buttonFindCppInMak.Enabled = !needLock;
            buttonFindHppInCpp.Enabled = !needLock;
            buttonFindForInAll.Enabled = !needLock;
            textBoxDirectory.Enabled = !needLock;
            buttonExcludeTypes.Enabled = !needLock;
            textBoxExcludeTypes.Enabled = !needLock;
            buttonExcludeAddr.Enabled = !needLock;
            textBoxExcludeAddr.Enabled = !needLock;
            comboBoxThreadsCount.Enabled = !needLock;
            buttonChangeDirectory.Enabled = !needLock;
            buttonFindCppInMak.Enabled = !needLock;
            buttonFindHppInCpp.Enabled = !needLock;
            buttonFindForInAll.Enabled = !needLock;
            richTextBoxFileNames.Enabled = !needLock;
            richTextBoxFileAddresses.Enabled = !needLock;
        }

        private void ProgrressTimerTick(object state)
        {
            if (!_timerWork) return;

            var watch = new Stopwatch();
            watch.Start();

            var ts = DateTime.Now - _checkTime;
            labelProgressTime.Text = ts.ToTime();

            _progressTimer.Change(Math.Max(0, Timertimeout - watch.ElapsedMilliseconds), Timeout.Infinite);
        }

        private void Work(string text, int percent, ProgressEventType eventType)
        {
            labelProgressAction.Text = text;
            progressBar1.Value = GetPercent(percent, 100);

            switch (eventType)
            {
                case ProgressEventType.Start:
                {
                    richTextBoxFileNames.Text = string.Empty;
                    richTextBoxFileAddresses.Text = string.Empty;
                    labelFilesInList.Text = @"0";
                    labelProgressAction.ForeColor = Color.Red;
                    labelProgressTime.Text = @"Time: 0";

                    _checkTime = DateTime.Now;
                    _timerWork = true;
                    _progressTimer.Change(Math.Max(0, 10), Timeout.Infinite);
                }
                    break;
                case ProgressEventType.Update:
                {
                }
                    break;
                case ProgressEventType.End:
                {
                    labelProgressAction.ForeColor = Color.Green;

                    _timerWork = false;
                    _progressTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                    break;
            }
        }

        private static string ParseFor(string s)
        {
            //if (s.IndexOf("#include ") != -1) s = s.Remove(s.IndexOf("#include "), 9);
            //if (s.IndexOf('#') != -1 && s.IndexOf("include ") != -1) s = s.Remove(0, s.IndexOf("include ") + 8);
            //if (s.IndexOf('<') != -1) s = s.Remove(s.IndexOf('<'), 1);
            //if (s.IndexOf('>') != -1) s = s.Remove(s.IndexOf('>'), 1);
            while (s.IndexOf('"') != -1) s = s.Remove(s.IndexOf('"'), 1);
            //if (s.IndexOf("/*") != -1 && s.IndexOf("*/") != -1) s = s.Remove(s.IndexOf("/*"), (s.IndexOf("*/") - s.IndexOf("/*")) + 2);
            //while (s.IndexOf('/') != -1) s = s.Remove(0, s.IndexOf('/') + 1);
            //while (s.IndexOf(' ') != -1) s = s.Remove(s.IndexOf(' '), 1);
            return s;
        }

        private static string ParseHeader(string s)
        {
            s = s.ToLower();
            if (s.IndexOf("#include ", StringComparison.OrdinalIgnoreCase) != -1)
                s = s.Remove(s.IndexOf("#include ", StringComparison.OrdinalIgnoreCase), 9);
            if (s.IndexOf('#') != -1 && s.IndexOf("include ", StringComparison.OrdinalIgnoreCase) != -1)
                s = s.Remove(0, s.IndexOf("include ", StringComparison.OrdinalIgnoreCase) + 8);
            if (s.IndexOf('<') != -1) s = s.Remove(s.IndexOf('<'), 1);
            if (s.IndexOf('>') != -1) s = s.Remove(s.IndexOf('>'), 1);
            while (s.IndexOf('"') != -1) s = s.Remove(s.IndexOf('"'), 1);
            if (s.IndexOf("/*", StringComparison.OrdinalIgnoreCase) != -1 &&
                s.IndexOf("*/", StringComparison.OrdinalIgnoreCase) != -1)
                s = s.Remove(s.IndexOf("/*", StringComparison.OrdinalIgnoreCase),
                    s.IndexOf("*/", StringComparison.OrdinalIgnoreCase) -
                    s.IndexOf("/*", StringComparison.OrdinalIgnoreCase) + 2);
            while (s.IndexOf('/') != -1) s = s.Remove(0, s.IndexOf('/') + 1);
            while (s.IndexOf(' ') != -1) s = s.Remove(s.IndexOf(' '), 1);
            return s;
        }

        private static int GetPercent(int current, int max, int minPercent = 0, int maxPercent = 100)
        {
            var onePercent = (double) max/100;
            var currPercent = current/onePercent;

            if (currPercent < minPercent) currPercent = minPercent;
            if (currPercent > maxPercent) currPercent = maxPercent;
            return (int) Math.Round(currPercent);
        }

        private static string RestoreFileRegister(string s, string ss)
        {
            return s.Replace(Path.GetFileName(s), Path.GetFileName(ss));
        }

        private void ListToPanel(IReadOnlyCollection<AllFilesClass> list)
        {
            foreach (var s in list) richTextBoxFileNames.Text += s.Name + Environment.NewLine;
            foreach (var s in list)
            {
                richTextBoxFileAddresses.Text +=
                    RestoreFileRegister(
                        s.Address.ToLower().Replace(textBoxDirectory.Text.ToLower(), "./").Replace('\\', '/'), s.Address) +
                    Environment.NewLine;
            }
            labelFilesInList.Text = list.Count.ToString();
        }

        private void FindNotUsedCpp()
        {
            ProgressEvent.Exec("Begin", 0, ProgressEventType.Start);
            ProgramState = ProgramStates.Worked;

            ListToPanel(GetDelete(_allFiles.Count(f => f.Type == AllFilesClass.ExtType.MakeFile),
                Convert.ToInt32(comboBoxThreadsCount.Text),
                new List<AllFilesClass>(_allFiles.Where(f => f.Type == AllFilesClass.ExtType.CppFile)),
                @"\b(?<word>\w+)\.cp?p?\b", new List<AllFilesClass.ExtType> {AllFilesClass.ExtType.MakeFile}, s => s));

            ProgramState = ProgramStates.WaitAction;
            ProgressEvent.Exec("Complete Find Not Used Cpp", 100, ProgressEventType.End);
        }

        private void FindNotUsedHpp()
        {
            ProgressEvent.Exec("Begin", 0, ProgressEventType.Start);
            ProgramState = ProgramStates.Worked;

            ListToPanel(
                GetDelete(
                    _allFiles.Count(
                        f => f.Type == AllFilesClass.ExtType.CppFile || f.Type == AllFilesClass.ExtType.HppFile),
                    Convert.ToInt32(comboBoxThreadsCount.Text),
                    new List<AllFilesClass>(_allFiles.Where(f => f.Type == AllFilesClass.ExtType.HppFile)),
                    @".*#\s*include\s*.*(<|\"").*(>|"")",
                    new List<AllFilesClass.ExtType> {AllFilesClass.ExtType.CppFile, AllFilesClass.ExtType.HppFile},
                    ParseHeader));

            ProgramState = ProgramStates.WaitAction;
            ProgressEvent.Exec("Complete Find Not Used Hpp", 100, ProgressEventType.End);
        }

        private void FindNotUsedFor()
        {
            ProgressEvent.Exec("Begin", 0, ProgressEventType.Start);
            ProgramState = ProgramStates.Worked;

            ListToPanel(
                GetDelete(
                    _allFiles.Count(
                        f => f.Type == AllFilesClass.ExtType.CppFile || f.Type == AllFilesClass.ExtType.HppFile),
                    Convert.ToInt32(comboBoxThreadsCount.Text),
                    new List<AllFilesClass>(_allFiles.Where(f => f.Type == AllFilesClass.ExtType.ForFile)),
                    @"[""](?<word>\w+)[.][f][o][r][""]",
                    new List<AllFilesClass.ExtType> {AllFilesClass.ExtType.CppFile, AllFilesClass.ExtType.HppFile},
                    ParseFor));

            ProgramState = ProgramStates.WaitAction;
            ProgressEvent.Exec("Complete Find Not Used For", 100, ProgressEventType.End);
        }

        private void GetDelete(object sendToThread)
        {
            var obj = sendToThread as SendToThreadClass;
            if (obj == null) return;

            var rgx = new Regex(obj.Pattern, RegexOptions.IgnoreCase);

            foreach (var s in _allFiles.Where(af => obj.TypesFind.Contains(af.Type)).Skip(obj.Skip).Take(obj.Take))
            {
                int current;
                lock (obj.Counter)
                {
                    current = obj.Counter.Count++;
                }

                var f = new FileStream(s.Address, FileMode.Open);
                var r = new StreamReader(f, _encoding);
                var matches = rgx.Matches(r.ReadToEnd());
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Value.ToLower().Contains(@"//")) continue;
                        lock (obj.List)
                        {
                            obj.List.RemoveAll(
                                af =>
                                    string.Equals(af.Name, obj.ProcessMatch(match.Value),
                                        StringComparison.CurrentCultureIgnoreCase));
                        }
                    }
                }
                r.Close();
                f.Close();

                ProgressEvent.Exec($"Process {current}/{obj.Count} ({GetPercent(current, obj.Count)}%)",
                    GetPercent(current, obj.Count), ProgressEventType.Update);
            }
        }

        private List<AllFilesClass> GetDelete(int count, int threadsCount, List<AllFilesClass> list, string pattern,
            ICollection<AllFilesClass.ExtType> typesFind, Func<string, string> processMatch)
        {
            var cnt = new IntToRefType {Count = 0};
            _threadsList = new List<Thread>();
            var part = count/threadsCount;
            for (var i = 0; i < threadsCount; i++)
            {
                _threadsList.Add(new Thread(GetDelete));
                _threadsList[i].Start(new SendToThreadClass
                {
                    Count = count,
                    List = list,
                    Pattern = pattern,
                    TypesFind = typesFind,
                    ProcessMatch = processMatch,
                    Counter = cnt,
                    Skip = part*i,
                    Take = i != threadsCount - 1 ? part : count - part*i
                });
            }

            for (var i = 0; i < threadsCount; i++) if (_threadsList[i].IsAlive) _threadsList[i].Join();

            return list;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (textBoxDirectory.TextLength != 0) folderBrowserDialog.SelectedPath = textBoxDirectory.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxDirectory.Text = folderBrowserDialog.SelectedPath;
                if (textBoxDirectory.Text[textBoxDirectory.TextLength - 1] != '\\') textBoxDirectory.Text += @"\";
                ProgramState = ProgramStates.Started;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (_workThread = new Thread(FindNotUsedCpp)).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (_workThread = new Thread(FindNotUsedHpp)).Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            (_workThread = new Thread(FindNotUsedFor)).Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_workThread != null)
            {
                if (_workThread.IsAlive)
                    _workThread.Abort();
            }
            _timerWork = false;
            _progressTimer?.Change(Timeout.Infinite, Timeout.Infinite);

            if (_threadsList != null)
            {
                foreach (var t in _threadsList)
                    if (t.IsAlive) t.Abort();
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var listExcluded = new List<string>();
            listExcluded.AddRange(textBoxExcludeTypes.Text.Split(','));

            richTextBoxFileNames.Text =
                richTextBoxFileNames.Lines.Where(str => !listExcluded.Contains(Path.GetExtension(str)))
                    .Aggregate(string.Empty, (current, str) => current + str + Environment.NewLine);

            richTextBoxFileAddresses.Text =
                richTextBoxFileAddresses.Lines.Where(str => !listExcluded.Contains(Path.GetExtension(str)))
                    .Aggregate(string.Empty, (current, str) => current + str + Environment.NewLine);

            labelFilesInList.Text = richTextBoxFileNames.Lines.Length.ToString();
        }

        private void buttonExcludeAddr_Click(object sender, EventArgs e)
        {
            var listExcluded = new List<string>();
            listExcluded.AddRange(textBoxExcludeAddr.Text.Split(','));

            richTextBoxFileAddresses.Text = richTextBoxFileAddresses.Lines.Where(str => !str.Contains(listExcluded))
                .Aggregate(string.Empty, (current, str) => current + str + Environment.NewLine);

            richTextBoxFileNames.Text = richTextBoxFileAddresses.Lines.Aggregate(string.Empty,
                (current, str) => current + Path.GetFileName(str) + Environment.NewLine);

            labelFilesInList.Text = richTextBoxFileNames.Lines.Length.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxThreadsCount.Text = comboBoxThreadsCount.Items[comboBoxThreadsCount.Items.Count - 1].ToString();
        }

        private class IntToRefType
        {
            public int Count;
        }

        private class SendToThreadClass
        {
            public int Count;
            public IntToRefType Counter;
            public List<AllFilesClass> List;
            public string Pattern;
            public Func<string, string> ProcessMatch;
            public int Skip;
            public int Take;
            public ICollection<AllFilesClass.ExtType> TypesFind;
        }

        private enum ProgramStates
        {
            Started,
            WaitAction,
            Worked
        }

        private class AllFilesClass
        {
            public string Address;
            public string Name;
            public ExtType Type;

            public enum ExtType
            {
                CppFile,
                HppFile,
                MakeFile,
                ForFile,
                UnknownFile
            }
        }
    }
}