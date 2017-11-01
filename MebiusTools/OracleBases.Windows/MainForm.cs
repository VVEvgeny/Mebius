using System;
using System.Windows.Forms;

namespace OracleBases.Windows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var add = new AddConnect();
            if(add.ShowDialog() == DialogResult.OK)
            {

            }
        }
    }
}
