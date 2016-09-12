using System;
using System.Windows.Forms;

namespace BmDebug
{
    public partial class LogWindow : Form
    {
        private RichTextBox _richTextBox1;
    
        public LogWindow()
        {
            InitializeComponent();
        }
        public void Write_Line(string text)
        {
            if (!Visible) Visible = true;
            _richTextBox1.Text += text + Environment.NewLine;
        }

        private void InitializeComponent()
        {
            _richTextBox1 = new RichTextBox();
            SuspendLayout();

            _richTextBox1.Dock = DockStyle.Fill;
            _richTextBox1.Location = new System.Drawing.Point(0, 0);
            _richTextBox1.Name = "_richTextBox1";
            _richTextBox1.Size = new System.Drawing.Size(808, 210);
            _richTextBox1.TabIndex = 0;
            _richTextBox1.Text = "";

            ClientSize = new System.Drawing.Size(808, 210);
            Controls.Add(_richTextBox1);
            Name = "LogWindow";
            ResumeLayout(false);
        }
    }
}
