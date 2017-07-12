using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionMethods;
using static WorkCopy.Settings;

namespace WorkCopy
{
    public partial class MainForm : Form
    {
        private Settings Settings { get; set; }
        private List<SettingsPathes> Pathes { get; set; }
        private WorkFileList WorkFiles { get; } = new WorkFileList();

        private class WorkFile
        {
            public string Path;
            public string VersionName;
            public string PacketNumber;
            public string HomeOrBaseText;
            public string FileName => System.IO.Path.GetFileName(Path);
            //public string FileName => Path;
        }

        public MainForm()
        {
            InitializeComponent();

            Settings = new Settings();
            Pathes = Settings.SettingsPathes ?? new List<SettingsPathes>();
            WorkFiles.UpdateListViewCounts += (s, e) => { allFilesToolStripMenuItem.Text = e.Count.ToString(); };

            LocalsToolStripItems();
        }
        private void toolStripMenuItemConfiguration_Click(object sender, EventArgs e)
        {
            new Setting().ShowDialog();
            Settings = new Settings();
            Pathes = Settings.SettingsPathes ?? new List<SettingsPathes>();
            LocalsToolStripItems();
        }

        private void LocalsToolStripItems()
        {
            compareLocalToolStripMenuItem.DropDownItems.Clear();
            compareEtalonToolStripMenuItem.DropDownItems.Clear();

            for (var i = 0; i < Pathes.Count - 1; i++)
            {
                if (Pathes[i].Name == @"0") continue;
                for (var j = i + 1; j < Pathes.Count; j++)
                {
                    if (Pathes[j].Name == @"0") continue;

                    var toolStripMenuItem = new ToolStripMenuItem
                    {
                        Name = "local" + "_" + Pathes[i].Name + "_" + Pathes[j].Name,
                        Size = new System.Drawing.Size(152, 22),
                        Text = Pathes[i].Name + @" -> " + Pathes[j].Name
                    };
                    toolStripMenuItem.Click += compareLocalToolStripMenuItem_Click;
                    compareLocalToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);

                    var toolStripMenuItemEt = new ToolStripMenuItem
                    {
                        Name = "etalon" + "_" + Pathes[i].Name + "_" + Pathes[j].Name,
                        Size = new System.Drawing.Size(152, 22),
                        Text = Pathes[i].Name + @" -> " + Pathes[j].Name
                    };
                    toolStripMenuItemEt.Click += compareLocalToolStripMenuItem_Click;
                    compareEtalonToolStripMenuItem.DropDownItems.Add(toolStripMenuItemEt);
                }
            }
        }

        private void compareLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    //BMTools.BmDebug.Info(((ToolStripMenuItem)sender).Name);
                    //BMTools.BmDebug.Info(_workFiles[(int)sel].Path);

                    var local = ((ToolStripMenuItem) sender).Name.Split('_')[0] == @"local";

                    var set = GetSettingsPathesByName(WorkFiles[(int) sel].VersionName);
                    var setFrom = GetSettingsPathesByName(((ToolStripMenuItem) sender).Name.Split('_')[1]);
                    var setTo = GetSettingsPathesByName(((ToolStripMenuItem) sender).Name.Split('_')[2]);

                    //BMTools.BmDebug.Info(_workFiles[(int)sel].Path.Replace(set.PathLocal, setFrom.PathLocal));
                    //BMTools.BmDebug.Info(_workFiles[(int)sel].Path.Replace(set.PathLocal, setTo.PathLocal));

                    RunCompare(WorkFiles[(int) sel].Path.Replace(set.PathLocal, local ? setFrom.PathLocal : setFrom.PathEtalon),
                        WorkFiles[(int) sel].Path.Replace(set.PathLocal, local ? setTo.PathLocal : setTo.PathEtalon));
                }
            }
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true)) e.Effect = DragDropEffects.Copy;
        }

        private void LoadList(bool saveSelection = false)
        {
            var selectedList = new List<int>();
            if (saveSelection)
            {
                selectedList.Clear();
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    selectedList.Add((int) sel);
                }
            }

            listViewFiles.Items.Clear();
            var num = 1;
            foreach (var file in WorkFiles)
            {
                listViewFiles.Items.Add(
                    new ListViewItem(new[]
                    {num.ToString(), file.FileName, file.PacketNumber, file.VersionName, file.HomeOrBaseText}));
                num++;
            }

            if (saveSelection)
            {
                if (selectedList.Count == 0) return;
                for (var i = 0; i < listViewFiles.Items.Count; i++)
                {
                    if (selectedList.Contains(i))
                    {
                        listViewFiles.Items[i].Selected = true;
                        listViewFiles.Items[i].Focused = true;
                    }
                }
            }
            listViewFiles_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void AddWorkFile(WorkFile workFile)
        {
            BMTools.BmDebug.Debug.InfoAsync("add file="+ workFile.FileName);
            if (
                !WorkFiles.Exists(
                    w =>
                        w.Path == workFile.Path && w.FileName == workFile.FileName &&
                        w.HomeOrBaseText == workFile.HomeOrBaseText && w.VersionName == workFile.VersionName))
            {
                //one etalon diff h/b selector
                if (WorkFiles.Exists(
                    w =>
                        w.Path == workFile.Path && w.FileName == workFile.FileName &&
                        w.VersionName == workFile.VersionName))
                {
                    if (MessageBox.Show(@"File Exist in Work List =" + workFile.FileName, @"Ignore file?", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        WorkFiles.Add(workFile);
                    }
                }
                else
                {
                    WorkFiles.Add(workFile);
                }
            }
            else
            {
                MessageBox.Show(@"File Exist in Work List =" + workFile.FileName);
            }
        }

        private void GetFilesFromDirectory(string path, ICollection<string> files)
        {
            var rootDir = new DirectoryInfo(path);
            foreach (var dir in rootDir.GetDirectories())
            {
                GetFilesFromDirectory(dir.FullName, files);
            }
            foreach (var file in rootDir.GetFiles())
            {
                files.Add(file.FullName);
            }
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var files = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (files == null) return;

                //BMTools.BmDebug.Debug.InfoAsync("file=", files);

                var realFiles = new List<string>();

                foreach (var file in files)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                    {
                        GetFilesFromDirectory(file, realFiles);
                    }
                    else
                    {
                        realFiles.Add(file);
                    }
                }
                //BMTools.BmDebug.Debug.InfoAsync("realFiles=", realFiles);

                foreach (var file in realFiles)
                {
                    var fileName = Path.GetFileName(files[0]);
                    if (fileName != null && fileName.Contains("master"))
                    {
                        var masterNumber = Path.GetExtension(files[0]);
                        if (!string.IsNullOrEmpty(masterNumber) && masterNumber.Contains("."))
                            masterNumber = masterNumber.Replace(".", "");
                        BMTools.BmDebug.Debug.Info("master file!!");

                        var f = new FileStream(files[0], FileMode.Open);
                        var re = new StreamReader(f);
                        var allMaster = re.ReadToEnd();

                        re.Close();
                        f.Close();
                        var allMaster2 = allMaster.Split('\n');
                        foreach (var line in allMaster2)
                        {
                            if (line.ToLower().Contains("#")) continue;
                            if (line.IndexOf("./", StringComparison.Ordinal) != 0) continue;

                            foreach (var path in Pathes)
                            {
                                if (line.Contains(path.PathRemoteHome.ToUnixPath()))
                                {
                                    AddWorkFile(new WorkFile
                                    {
                                        PacketNumber = masterNumber,
                                        Path =
                                            line.Replace(path.PathRemoteHome.ToUnixPath(), path.PathLocal)
                                                .Replace("/", "\\"),
                                        VersionName = path.Name,
                                        HomeOrBaseText = HomeSelector
                                    });
                                    break;
                                }
                                if (line.Contains(path.PathRemoteBase.ToUnixPath()))
                                {
                                    AddWorkFile(new WorkFile
                                    {
                                        PacketNumber = masterNumber,
                                        Path =
                                            line.Replace(path.PathRemoteBase.ToUnixPath(), path.PathLocal)
                                                .Replace("/", "\\"),
                                        VersionName = path.Name,
                                        HomeOrBaseText = BaseSelector
                                    });
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //BMTools.BmDebug.Info("NOT master file=",file);
                        foreach (var path in Pathes)
                        {
                            //BMTools.BmDebug.Info("path.PathLocal=", path.PathLocal);
                            if (file.ToLower().Contains(path.PathLocal.ToLower()))
                            {
                                AddWorkFile(new WorkFile
                                {
                                    PacketNumber = "",
                                    Path = file,
                                    VersionName = path.Name,
                                    HomeOrBaseText = HomeSelector
                                });
                            }
                        }
                    }
                }
                LoadList();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                var pathForRemove = new List<WorkFile>();

                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    pathForRemove.Add(WorkFiles[(int) sel]);
                }

                foreach (var path in pathForRemove)
                {
                    WorkFiles.Remove(path);
                }
                LoadList();
            }
        }

        private readonly List<Keys> _ignoreKeyses = new List<Keys>
        {
            Keys.Down,
            Keys.Up,
            Keys.Shift,
            Keys.ShiftKey,
            Keys.LShiftKey,
            Keys.RShiftKey,
            Keys.Control,
            Keys.ControlKey,
            Keys.LControlKey,
            Keys.RControlKey,
            Keys.Left,
            Keys.Right
        };
        private void ProcessFiltr(KeyEventArgs e)
        {
            if (_ignoreKeyses.Contains(e.KeyCode)) return;
            if (e.KeyCode == Keys.Escape)
            {
                filtrToolStripTextBox.Text = string.Empty;
                for (var i = 0; i < WorkFiles.Count; i++)
                {
                    listViewFiles.Items[i].Selected = false;
                    listViewFiles.Items[i].Focused = false;
                }
                return;
            }
            if (e.KeyCode == Keys.Back)
            {
                filtrToolStripTextBox.Text = string.Empty;
                return;
            }
            if (e.KeyCode == Keys.D0 || e.KeyCode == Keys.D1 || e.KeyCode == Keys.D2 || e.KeyCode == Keys.D3 || e.KeyCode == Keys.D4 || e.KeyCode == Keys.D5
                 || e.KeyCode == Keys.D6 || e.KeyCode == Keys.D7 || e.KeyCode == Keys.D8 || e.KeyCode == Keys.D9
                || e.KeyCode == Keys.NumPad0 || e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.NumPad4
                 || e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.NumPad7 || e.KeyCode == Keys.NumPad8 || e.KeyCode == Keys.NumPad9
                 || e.KeyCode == Keys.OemPeriod
                 )
            {
                switch (e.KeyCode)
                {
                    case Keys.OemPeriod:
                        filtrToolStripTextBox.Text += @".";
                        break;
                    case Keys.D0:
                    case Keys.NumPad0:
                        filtrToolStripTextBox.Text += @"0";
                        break;
                    case Keys.D1:
                    case Keys.NumPad1:
                        filtrToolStripTextBox.Text += @"1";
                        break;
                    case Keys.D2:
                    case Keys.NumPad2:
                        filtrToolStripTextBox.Text += @"2";
                        break;
                    case Keys.D3:
                    case Keys.NumPad3:
                        filtrToolStripTextBox.Text += @"3";
                        break;
                    case Keys.D4:
                    case Keys.NumPad4:
                        filtrToolStripTextBox.Text += @"4";
                        break;
                    case Keys.D5:
                    case Keys.NumPad5:
                        filtrToolStripTextBox.Text += @"5";
                        break;
                    case Keys.D6:
                    case Keys.NumPad6:
                        filtrToolStripTextBox.Text += @"6";
                        break;
                    case Keys.D7:
                    case Keys.NumPad7:
                        filtrToolStripTextBox.Text += @"7";
                        break;
                    case Keys.D8:
                    case Keys.NumPad8:
                        filtrToolStripTextBox.Text += @"8";
                        break;
                    case Keys.D9:
                    case Keys.NumPad9:
                        filtrToolStripTextBox.Text += @"9";
                        break;
                }
            }
            else
            {
                filtrToolStripTextBox.Text += e.KeyData.ToString().ToLower();
            }
            
            for (var i = 0; i < WorkFiles.Count; i++)
            {
                listViewFiles.Items[i].Selected = WorkFiles[i].FileName.ToLower().Contains(filtrToolStripTextBox.Text);
                listViewFiles.Items[i].Focused = WorkFiles[i].FileName.ToLower().Contains(filtrToolStripTextBox.Text);
            }
        }
        private void listViewFiles_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    removeToolStripMenuItem_Click(sender, new EventArgs());
                    return;
                case Keys.Subtract:
                    changeVersionToolStripMenuItem_Click(sender, new EventArgs());
                    return;
                case Keys.Multiply:
                    changeHBToolStripMenuItem_Click(sender, new EventArgs());
                    return;
            }
            ProcessFiltr(e);
        }

        private void NextEtalon(WorkFile workFile)
        {
            if (Pathes.Count > 1)
            {
                for (var i = 0; i < Pathes.Count; i++)
                {
                    if (workFile.VersionName == Pathes[i].Name)
                    {
                        var index = i == Pathes.Count - 1 ? 0 : i + 1;
                        workFile.VersionName = Pathes[index].Name;
                        workFile.Path = workFile.Path.Replace(Pathes[i].PathLocal, Pathes[index].PathLocal);
                        return;
                    }
                }
            }
        }


        private void changeVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    NextEtalon(WorkFiles[(int) sel]);
                }
                LoadList(true);
            }
        }

        private void changeHBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    WorkFiles[(int)sel].HomeOrBaseText = IsHomeSelector(WorkFiles[(int)sel].HomeOrBaseText) ? BaseSelector : HomeSelector;
                }
                LoadList(true);
            }
        }

        private bool IsFilesDifferent(string leftFile, string rightFile)
        {
            BMTools.BmDebug.Debug.InfoAsync("compare type=", Settings.CompareType);

            if (Settings.CompareType != CompareTypes.None)
            {
                try
                {
                    BMTools.BmDebug.Debug.InfoAsync("lf=", leftFile, "rf=", rightFile);
                    var lf = new FileInfo(leftFile);
                    var rf = new FileInfo(rightFile);
                    if (Settings.CompareType == CompareTypes.Size)
                    {
                        BMTools.BmDebug.Debug.InfoAsync("lf.Length=", lf.Length, "rf.Length=", rf.Length);
                        if (lf.Length != rf.Length) return true;
                    }
                    else if (Settings.CompareType == CompareTypes.Date)
                    {
                        BMTools.BmDebug.Debug.InfoAsync("lf.LastWriteTime=", lf.LastWriteTime, "rf.LastWriteTime=", rf.LastWriteTime);
                        //BMTools.BmDebug.Info("lf=", lf.CreationTime, "rf=", rf.CreationTime);
                        //BMTools.BmDebug.Info("lf=", lf.LastAccessTime, "rf=", rf.LastAccessTime);
                        if (lf.LastWriteTime.Date.Year != rf.LastWriteTime.Date.Year ||
                            lf.LastWriteTime.Date.Day != rf.LastWriteTime.Date.Day ||
                            lf.LastWriteTime.Date.Hour != rf.LastWriteTime.Date.Hour ||
                            lf.LastWriteTime.Date.Minute != rf.LastWriteTime.Date.Minute ||
                            lf.LastWriteTime.Date.Second != rf.LastWriteTime.Date.Second
                        ) return true;
                    }
                    else if (Settings.CompareType == CompareTypes.Crc)
                    {
                        var crcL = lf.CalculateCrc();
                        var crcR = rf.CalculateCrc();

                        BMTools.BmDebug.Debug.InfoAsync("lf.crc=", crcL, "rf.crc=", crcR);

                        if (crcL != crcR) return true;
                    }
                }
                catch (Exception e)
                {
                    BMTools.BmDebug.Debug.CritAsyc("RunCompare ", e.Message);
                    MessageBox.Show(@"File open error=" + e.Message);
                }
            }
            return false;
        }
        private void RunCompare(string leftFile, string rightFile)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) return;

            if (IsFilesDifferent(leftFile, rightFile))
            {
                if (string.IsNullOrEmpty(Settings.MergeAppPath))
                {
                    MessageBox.Show(@"No have configured Merge Application");
                    return;
                }
                var startinfo = new ProcessStartInfo
                {
                    FileName = Settings.MergeAppPath,
                    Arguments = leftFile + " " + rightFile,
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                Process.Start(startinfo);
            }
        }

        private void RunCompareFiles(bool etalon)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (int sel in listViewFiles.SelectedIndices)
                {
                    var set = GetSettingsPathesByName(WorkFiles[sel].VersionName);
                    RunCompare(WorkFiles[sel].Path,
                        WorkFiles[sel].Path.Replace(set.PathLocal,
                            etalon
                                ? set.PathEtalon
                                : (WorkFiles[sel].HomeOrBaseText == "H" ? set.PathRemoteHome : set.PathRemoteBase)));
                }
            }
        }
        private void compareEtalonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RunCompareFiles(true);
        }

        private SettingsPathes GetSettingsPathesByName(string versionName)
        {
            return Pathes.First(p => p.Name == versionName);
        }

        private string GetUnixPath(string path, string versionName, string homeOrBaseText)
        {
            var pathes = GetSettingsPathesByName(versionName);
            return
                path.Replace(pathes.PathLocal,
                    IsHomeSelector(homeOrBaseText) ? pathes.PathRemoteHome.ToUnixPath() : pathes.PathRemoteBase.ToUnixPath()
                    ).Replace("\\", "/") + Environment.NewLine;
        }
        private void listForMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                Clipboard.SetText(listViewFiles.SelectedIndices.Cast<object>()
                    .Aggregate(string.Empty,
                        (current, sel) =>
                            current +
                            GetUnixPath(WorkFiles[(int) sel].Path, WorkFiles[(int) sel].VersionName, WorkFiles[(int) sel].HomeOrBaseText))
                            );
            }
        }

        private void takeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var sel in listViewFiles.SelectedIndices)
            {
                CopyFile(GetRemotePath(WorkFiles[(int)sel]), WorkFiles[(int)sel].Path);
            }
        }

        private static void CopyFile(string from, string to)
        {
            BMTools.BmDebug.Debug.Info("CopyFile from", from, "to", to);
            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var dir = new DirectoryInfo(Path.GetDirectoryName(to));
                if (!dir.Exists) dir.Create();
                new FileInfo(from).CopyTo(to, true);
            }
            catch (Exception e)
            {
                BMTools.BmDebug.Debug.Crit("CopyFile ", e.Message);
                MessageBox.Show(e.Message);
            }
        }

        private string GetRemotePath(WorkFile workFile)
        {
            return GetRemotePath(workFile.Path, workFile.VersionName, workFile.HomeOrBaseText);
        }
        private string GetRemotePath(string localPath, string versionName, string homeOrBaseText)
        {
            var set = GetSettingsPathesByName(versionName);
            return localPath.Replace(set.PathLocal, IsHomeSelector(homeOrBaseText) ? set.PathRemoteHome : set.PathRemoteBase);
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var sel in listViewFiles.SelectedIndices)
            {
                CopyFile(WorkFiles[(int)sel].Path, GetRemotePath(WorkFiles[(int)sel]));
            }
        }

        private void filtrToolStripTextBox_Click(object sender, EventArgs e)
        {
            filtrToolStripTextBox.Text = string.Empty;
        }

        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripMenuItemCountSelected.Text = listViewFiles.SelectedItems.Count.ToString();
            if (listViewFiles.SelectedItems.Count > 0) listViewFiles.EnsureVisible(listViewFiles.SelectedIndices[0]);
        }

        private void takeEtalonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var sel in listViewFiles.SelectedIndices)
            {
                var set = GetSettingsPathesByName(WorkFiles[(int)sel].VersionName);
                CopyFile(WorkFiles[(int)sel].Path.Replace(set.PathLocal, set.PathEtalon), WorkFiles[(int)sel].Path);
            }
        }

        private void compareLocalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RunCompareFiles(false);
        }


        private void SelectDifferents(bool etalon, bool different)
        {
            var indexes = new List<int>();
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (int sel in listViewFiles.SelectedIndices)
                {
                    var set = GetSettingsPathesByName(WorkFiles[sel].VersionName);
                    if (IsFilesDifferent(WorkFiles[sel].Path,
                        WorkFiles[sel].Path.Replace(set.PathLocal,
                            etalon
                                ? set.PathEtalon
                                : (WorkFiles[sel].HomeOrBaseText == "H" ? set.PathRemoteHome : set.PathRemoteBase))) == different)
                    {
                        indexes.Add(sel);
                    }
                }
            }
            for (var i = 0; i < WorkFiles.Count; i++)
            {
                listViewFiles.Items[i].Focused = listViewFiles.Items[i].Selected = indexes.Contains(i);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < WorkFiles.Count; i++)
            {
                listViewFiles.Items[i].Focused = listViewFiles.Items[i].Selected = true;
            }
        }

        private void differentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectDifferents(true, true);
        }

        private void sameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectDifferents(true, false);
        }

        private void differentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectDifferents(false, true);
        }

        private void sameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SelectDifferents(false, false);
        }
    }
}
