using System;
using System.Collections.Generic;
using System.IO;
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
            public string FileName => System.IO.Path.GetFileName(Path);
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
                    new ListViewItem(new[] {num.ToString(), file.FileName, file.PacketNumber, file.VersionName}));
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
                        bm_debug._Info("master file");

                        var f = new FileStream(fileName, FileMode.Open);
                        var re = new StreamReader(f);
                        var allMaster = re.ReadToEnd();
                        re.Close();
                        f.Close();
                        var allMaster2 = allMaster.Split('\n');
                        foreach (var line in allMaster2)
                        {
                            if (line.ToLower().Contains("#")) continue;

                            foreach (var path in _pathes)
                            {
                                if (line.Contains(path.PathRemoteHome.ToUnixPath()))
                                {
                                    
                                }
                                else if (line.Contains(path.PathRemoteBase.ToUnixPath()))
                                {
                                    
                                }
                            }
                        }
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
                                VersionName = path.Name
                            });
                        }
                    }
                    bm_debug._Info(file);
                }
                LoadList();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
