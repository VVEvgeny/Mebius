using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTests
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBoxPathEtalon.Text))
            {
                var dirs = new DirectoryInfo(textBoxPathEtalon.Text).GetDirectories();
                foreach (var dir in dirs)
                {
                    if (dir.Name.Contains("etalon."))
                    {
                        comboBoxVersion.Items.Add(dir.Name.Remove(0, "etalon.".Length));
                    }
                }
                if (comboBoxVersion.Items.Count > 0)
                {
                    comboBoxVersion.Text = comboBoxVersion.Items[comboBoxVersion.Items.Count - 1].ToString();
                }
            }
        }

        private void comboBoxVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxPacket.Items.Clear();
            if (!string.IsNullOrEmpty(textBoxUser.Text))
            {
                //\\linux3.minsk.mebius.net\exe\kernel.mt\data\
                //vve


                //\\linux3.minsk.mebius.net\exe\kernel.mt\data\home\vve\
                var dirs = new DirectoryInfo(textBoxPathEtalon.Text + "home\\"+ textBoxUser.Text).GetDirectories();
                foreach (var dir in dirs)
                {
                    if (dir.Name.Contains(comboBoxVersion.Text+"00"))
                    {
                        comboBoxPacket.Items.Add(dir.Name);
                    }
                }
                if (comboBoxPacket.Items.Count > 0)
                {
                    comboBoxPacket.Text = comboBoxPacket.Items[comboBoxPacket.Items.Count - 1].ToString();
                }
            }
        }
    }
}
