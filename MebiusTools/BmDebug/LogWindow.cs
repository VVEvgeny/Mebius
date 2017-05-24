using System;
using System.Drawing;
using System.Windows.Forms;

namespace BmDebug
{
    public class LogWindow : Form
    {
        private readonly RichTextBox _richTextBoxText = new RichTextBox();

        public LogWindow()
        {
            SuspendLayout();
            // 
            // richTextBoxText
            // 
            _richTextBoxText.Dock = DockStyle.Fill;
            _richTextBoxText.Location = new Point(0, 0);
            _richTextBoxText.Name = nameof(_richTextBoxText);
            _richTextBoxText.Size = new Size(808, 210);
            _richTextBoxText.TabIndex = 0;
            _richTextBoxText.Text = string.Empty;
            _richTextBoxText.MouseDoubleClick +=
                (s, e) => _richTextBoxText.BeginInvoke((MethodInvoker) (() => _richTextBoxText.Text = string.Empty));
            

            ClientSize = new Size(808, 210);
            Controls.Add(_richTextBoxText);
            Name = GetType().Name;
            ResumeLayout(false);
        }

        public void WriteLine(string text)
        {
            if (!Visible) Visible = true;

            _richTextBoxText.BeginInvoke((MethodInvoker) (() => _richTextBoxText.Text += text + Environment.NewLine));
        }
    }
}