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
        private Settings _settings;
        private List<SettingsPathes> _pathes;
        private readonly List<WorkFile> _workFiles;
        private readonly List<int> _selectedList;

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

            _settings = new Settings();
            _pathes = _settings.SettingsPathes ?? new List<SettingsPathes>();
            _workFiles = new List<WorkFile>();
            _selectedList = new List<int>();
        }

        private void toolStripMenuItemConfiguration_Click(object sender, EventArgs e)
        {
            new Setting().ShowDialog();
            _settings = new Settings();
            _pathes = _settings.SettingsPathes ?? new List<SettingsPathes>();
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true)) e.Effect = DragDropEffects.Copy;
        }

        private void LoadList(bool saveSelection = false)
        {
            if (saveSelection)
            {
                _selectedList.Clear();
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    _selectedList.Add((int) sel);
                }
            }

            listViewFiles.Items.Clear();
            var num = 1;
            foreach (var file in _workFiles)
            {
                listViewFiles.Items.Add(
                    new ListViewItem(new[]
                    {num.ToString(), file.FileName, file.PacketNumber, file.VersionName, file.HomeOrBaseText}));
                num++;
            }

            if (saveSelection)
            {
                if (_selectedList.Count == 0) return;
                for (var i = 0; i < listViewFiles.Items.Count; i++)
                {
                    if (_selectedList.Contains(i))
                    {
                        listViewFiles.Items[i].Selected = true;
                        listViewFiles.Items[i].Focused = true;
                    }
                }
            }
        }

        private void AddWorkFile(WorkFile workFile)
        {
            if (
                !_workFiles.Exists(
                    w =>
                        w.Path == workFile.Path && w.FileName == workFile.FileName &&
                        w.HomeOrBaseText == workFile.HomeOrBaseText && w.VersionName == workFile.VersionName))
            {
                _workFiles.Add(workFile);
            }
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var files = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                if (files == null) return;
                if (files.Length == 1)
                {
                    var fileName = Path.GetFileName(files[0]);
                    if (fileName != null && fileName.Contains("master"))
                    {
                        var masterNumber = Path.GetExtension(files[0]);
                        if (!string.IsNullOrEmpty(masterNumber) && masterNumber.Contains("."))
                            masterNumber = masterNumber.Replace(".", "");
                        //BMTools.BmDebug.Info("master file!!");

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

                            foreach (var path in _pathes)
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
                        LoadList();
                        return;
                    }
                }
                foreach (var file in files)
                {
                    foreach (var path in _pathes)
                    {
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
                    pathForRemove.Add(_workFiles[(int) sel]);
                }

                foreach (var path in pathForRemove)
                {
                    _workFiles.Remove(path);
                }
                LoadList();
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
            //BMTools.BmDebug.Warn("Unknown key=", e.KeyCode);
        }

        private void NextEtalon(WorkFile workFile)
        {
            if (_pathes.Count > 1)
            {
                for (var i = 0; i < _pathes.Count; i++)
                {
                    if (workFile.VersionName == _pathes[i].Name)
                    {
                        var index = i == _pathes.Count - 1 ? 0 : i + 1;
                        workFile.VersionName = _pathes[index].Name;
                        workFile.Path = workFile.Path.Replace(_pathes[i].PathLocal, _pathes[index].PathLocal);
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
                    NextEtalon(_workFiles[(int) sel]);
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
                    _workFiles[(int)sel].HomeOrBaseText = IsHomeSelector(_workFiles[(int)sel].HomeOrBaseText) ? BaseSelector : HomeSelector;
                }
                LoadList(true);
            }
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    var set = GetSettingsPathesByName(_workFiles[(int)sel].VersionName);
                    RunCompare(_workFiles[(int) sel].Path,
                        _workFiles[(int) sel].Path.Replace(set.PathLocal,
                            _workFiles[(int) sel].HomeOrBaseText == "H" ? set.PathRemoteHome : set.PathRemoteBase));
                }
            }
        }

        private void RunCompare(string leftFile, string rightFile)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT) return;
            if (string.IsNullOrEmpty(_settings.MergeAppPath))
            {
                MessageBox.Show(@"No have configured Merge Application");
                return;
            }
            var startinfo = new ProcessStartInfo
            {
                FileName = _settings.MergeAppPath,
                Arguments = leftFile + " " + rightFile,
                UseShellExecute = true,
                CreateNoWindow = true
            };
            Process.Start(startinfo);
        }
        private void compareEtalonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    var set = GetSettingsPathesByName(_workFiles[(int) sel].VersionName);
                    RunCompare(_workFiles[(int)sel].Path, _workFiles[(int)sel].Path.Replace(set.PathLocal, set.PathEtalon));
                }
            }
        }

        private SettingsPathes GetSettingsPathesByName(string versionName)
        {
            return _pathes.First(p => p.Name == versionName);
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
                            GetUnixPath(_workFiles[(int) sel].Path, _workFiles[(int) sel].VersionName, _workFiles[(int) sel].HomeOrBaseText))
                            );
            }
        }

        private void takeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var sel in listViewFiles.SelectedIndices)
            {
                CopyFile(GetRemotePath(_workFiles[(int)sel]), _workFiles[(int)sel].Path);
            }
        }

        private void CopyFile(string from, string to)
        {
            new FileInfo(from).CopyTo(to, true);
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
                CopyFile(_workFiles[(int)sel].Path, GetRemotePath(_workFiles[(int)sel]));
            }
        }
    }
}
