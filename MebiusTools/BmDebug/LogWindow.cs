using System;
using System.Windows.Forms;

namespace BmDebug
{
    public partial class LogWindow : Form
    {
        private RichTextBox richTextBoxText;
    
        public LogWindow()
        {
            InitializeComponent();
        }
        public void Write_Line(string text)
        {
            if (!Visible) Visible = true;
            richTextBoxText.Text += text + Environment.NewLine;
        }

        private void InitializeComponent()
        {
            this.richTextBoxText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxText
            // 
            this.richTextBoxText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxText.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxText.Name = "richTextBoxText";
            this.richTextBoxText.Size = new System.Drawing.Size(808, 210);
            this.richTextBoxText.TabIndex = 0;
            this.richTextBoxText.Text = "";
            this.richTextBoxText.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.richTextBoxText_MouseDoubleClick);
            // 
            // LogWindow
            // 
            this.ClientSize = new System.Drawing.Size(808, 210);
            this.Controls.Add(this.richTextBoxText);
            this.Name = "LogWindow";
            this.ResumeLayout(false);

        }

        private void richTextBoxText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            richTextBoxText.Text = string.Empty;
        }
    }
}
