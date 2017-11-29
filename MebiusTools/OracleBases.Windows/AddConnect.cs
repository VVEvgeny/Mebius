using System;
using System.Windows.Forms;

namespace OracleBases.Windows
{
    public partial class AddConnect : Form
    {
        public AddConnect()
        {
            InitializeComponent();
        }

        private void textBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxPort.Text)) return;

            try
            {
                int.Parse(textBoxPort.Text);
            }
            catch
            {
                textBoxPort.Text = string.Empty;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {

        }

        public string GetName => textBoxName.Text;
        public string GetHost => textBoxHost.Text;
        public new string GetService => textBoxService.Text;

        public int GetPort
        {
            get
            {
                var port = 0;
                try
                {
                    port = int.Parse(textBoxPort.Text);
                }
                catch
                {
                    port = 0;
                }
                return port;
            }
        }
        public string GetUserId => textBoxUserId.Text;
        public string GetPassword => textBoxPassword.Text;

    }
}
