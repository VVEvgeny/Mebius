using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WorkCopy
{
    public partial class Setting : Form
    {
        private readonly List<SettingsPathes> _pathes;
        private readonly Settings _settings;
        public Setting()
        {
            InitializeComponent();

            _settings = new Settings();
            _pathes = _settings.SettingsPathes ?? new List<SettingsPathes>();
        }

        private void LoadList()
        {
            listViewSettings.Items.Clear();
            foreach (var path in _pathes)
            {
                listViewSettings.Items.Add(path.Name);
            }
        }
        private void Setting_Load(object sender, EventArgs e)
        {
            LoadList();
            listViewSettings_SelectedIndexChanged(sender, e);

            textBoxMergeAppPath.Text = _settings.MergeAppPath;

            SetCompareTypeSelected();

            checkBoxOnlyExistingFiles.Checked = _settings.OnlyExistingFilesCompare;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text) || string.IsNullOrEmpty(textBoxNumber.Text) || string.IsNullOrEmpty(textBoxPathEtalon.Text) || string.IsNullOrEmpty(textBoxPathLocal.Text) 
                || string.IsNullOrEmpty(textBoxPathRemoteBase.Text) || string.IsNullOrEmpty(textBoxPathRemoteHome.Text))
            {
                MessageBox.Show(@"Not all field filled");
                return;
            }
            if (_pathes.Any(path => path.Name == textBoxName.Text))
            {
                MessageBox.Show(@"Name already exist");
                return;
            }

            try
            {
                _pathes.Add(new SettingsPathes
                {
                    Name = textBoxName.Text,
                    Number = Convert.ToInt32(textBoxNumber.Text),
                    PathEtalon = textBoxPathEtalon.Text,
                    PathLocal = textBoxPathLocal.Text,
                    PathRemoteBase = textBoxPathRemoteBase.Text,
                    PathRemoteHome = textBoxPathRemoteHome.Text
                });
                _settings.SettingsPathes = _pathes;
                _settings.Save();

                LoadList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearTextBoxes()
        {
            textBoxName.Text = string.Empty;
            textBoxNumber.Text = string.Empty;
            textBoxPathEtalon.Text = string.Empty;
            textBoxPathLocal.Text = string.Empty;
            textBoxPathRemoteBase.Text = string.Empty;
            textBoxPathRemoteHome.Text = string.Empty;
        }
        private void listViewSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSettings.SelectedItems.Count == 0)
            {
                buttonRemove.Enabled = false;
                buttonUpdate.Enabled = false;

                ClearTextBoxes();
            }
            else
            {
                buttonRemove.Enabled = true;
                buttonUpdate.Enabled = true;

                var path = _pathes.Find(p => p.Name == listViewSettings.SelectedItems[0].Text);
                textBoxName.Text = path.Name;
                textBoxNumber.Text = path.Number.ToString();
                textBoxPathEtalon.Text = path.PathEtalon;
                textBoxPathLocal.Text = path.PathLocal;
                textBoxPathRemoteBase.Text = path.PathRemoteBase;
                textBoxPathRemoteHome.Text = path.PathRemoteHome;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            _pathes.RemoveRange(listViewSettings.SelectedIndices[0], 1);
            _settings.SettingsPathes = _pathes;
            _settings.Save();

            ClearTextBoxes();

            LoadList();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var path = _pathes.Find(p => p.Name == listViewSettings.SelectedItems[0].Text);
                path.Name = textBoxName.Text;
                path.Number = Convert.ToInt32(textBoxNumber.Text);
                path.PathEtalon = textBoxPathEtalon.Text;
                path.PathLocal = textBoxPathLocal.Text;
                path.PathRemoteBase = textBoxPathRemoteBase.Text;
                path.PathRemoteHome = textBoxPathRemoteHome.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            _settings.SettingsPathes = _pathes;
            _settings.Save();

            var selected = listViewSettings.SelectedIndices[0];
            LoadList();
            listViewSettings.SelectedIndices.Add(selected);
        }

        private Settings.CompareTypes GetCompareTypeSelected()
        {
            if (radioButtonAlways.Checked) return Settings.CompareTypes.None;
            if (radioButtonSize.Checked) return Settings.CompareTypes.Size;
            if (radioButtonDate.Checked) return Settings.CompareTypes.Date;
            if (radioButtonCrc.Checked) return Settings.CompareTypes.Crc;

            return Settings.CompareTypes.None;
        }

        private void SetCompareTypeSelected()
        {
            switch (_settings.CompareType)
            {
                case Settings.CompareTypes.None:
                    radioButtonAlways.Checked = true;
                    break;
                case Settings.CompareTypes.Size:
                    radioButtonSize.Checked = true;
                    break;
                case Settings.CompareTypes.Date:
                    radioButtonDate.Checked = true;
                    break;
                case Settings.CompareTypes.Crc:
                    radioButtonCrc.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void buttonUpdateMergeAppPath_Click(object sender, EventArgs e)
        {
            _settings.MergeAppPath = textBoxMergeAppPath.Text;
            _settings.CompareType = GetCompareTypeSelected();
            _settings.OnlyExistingFilesCompare = checkBoxOnlyExistingFiles.Checked;
            _settings.Save();
        }

        private void CompareSettingsChanged(object sender, EventArgs e)
        {
            buttonUpdateMergeAppPath.Enabled = _settings.MergeAppPath != textBoxMergeAppPath.Text 
                || _settings.CompareType != GetCompareTypeSelected() 
                || _settings.OnlyExistingFilesCompare != checkBoxOnlyExistingFiles.Checked;
        }
    }
}
