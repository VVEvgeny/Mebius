using System;
using System.Windows.Forms;

namespace Tasks
{
    public partial class MainForm : Form
    {
        private readonly TaskDisp _taskDisp;
        public MainForm()
        {
            InitializeComponent();

            _taskDisp = new TaskDisp(listView);
            _taskDisp.LoadTasks();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _taskDisp.Abort();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == @"Start")
            {
                _taskDisp.Run();
                buttonStart.Text = @"Stop";
            }
            else
            {
                _taskDisp.Abort();
                buttonStart.Text = @"Start";
            }
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Task
            {
                Name = "test",
                Repeat = (int)Task.RepeatModes.Once,
                StopResult = Files.Results.Commited.ToString(),
                Date = DateTime.Now.AddSeconds(10)
            };

            using (var db = new DatabaseContext())
            {
                db.Tasks.Add(t);
                db.SaveChanges();
            }
            _taskDisp.Add(t);
        }
    }
}
