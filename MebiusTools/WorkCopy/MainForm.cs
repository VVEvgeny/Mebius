using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ExtensionMethods;

namespace WorkCopy
{
    public partial class MainForm : Form
    {
        private Settings _settings;
        private List<SettingsPathes> _pathes;
        private readonly List<WorkFile> _workFiles;

        private class WorkFile
        {
            public string Path;
            public string VersionName;
            public string PacketNumber;
            public string HomeOrBaseText;
            //public string FileName => System.IO.Path.GetFileName(Path);
            public string FileName => Path;
        }

        public MainForm()
        {
            InitializeComponent();

            bm_debug.Class_using = "WorkCopy";
            bm_debug.Debug_Output = bm_debug.Debug_Output_Modes.Log_Window;
            bm_debug.Enabled = true;

            bm_debug._Info("start");

            _settings = new Settings();
            _pathes = _settings.SettingsPathes ?? new List<SettingsPathes>();
            _workFiles = new List<WorkFile>();
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

        private void LoadList()
        {
            listViewFiles.Items.Clear();
            var num = 1;
            foreach (var file in _workFiles)
            {
                listViewFiles.Items.Add(
                    new ListViewItem(new[]
                    {num.ToString(), file.FileName, file.PacketNumber, file.VersionName, file.HomeOrBaseText}));
                num++;
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
                        if (!string.IsNullOrEmpty(masterNumber) && masterNumber.Contains(".")) masterNumber = masterNumber.Replace(".", "");
                        bm_debug._Info("master file!!");

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
                                    _workFiles.Add(new WorkFile
                                    {
                                        PacketNumber = masterNumber,
                                        Path = line.Replace(path.PathRemoteHome.ToUnixPath(),path.PathLocal).Replace("/","\\"),
                                        VersionName = path.Name,
                                        HomeOrBaseText = "H"
                                    });
                                    break;
                                }
                                if (line.Contains(path.PathRemoteBase.ToUnixPath()))
                                {
                                    _workFiles.Add(new WorkFile
                                    {
                                        PacketNumber = masterNumber,
                                        Path = line.Replace(path.PathRemoteBase.ToUnixPath(), path.PathLocal).Replace("/", "\\"),
                                        VersionName = path.Name,
                                        HomeOrBaseText = "B"
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
                            _workFiles.Add( new WorkFile
                            {
                                PacketNumber = "",
                                Path = file,
                                VersionName = path.Name,
                                HomeOrBaseText = "H"
                            });
                        }
                    }
                }
                LoadList();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedIndices.Count > 0)
            {
                var pathForRemove = new List<WorkFile>();

                foreach (var sel in listViewFiles.SelectedIndices)
                {
                    pathForRemove.Add(_workFiles[(int)sel]);
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
            if (e.KeyCode == Keys.Delete)
            {
                removeToolStripMenuItem_Click(sender, new EventArgs());
            }
        }
    }
}
