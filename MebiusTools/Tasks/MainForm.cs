using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tasks.Database;
using Tasks.Database.Models;
using Tasks.Lib.Base;
using Tasks.Lib.MebiusPMStatus;
using static BMTools.BmDebug;
using static System.Reflection.MethodBase;

namespace Tasks
{
    public partial class MainForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JobDisp _jobDisp;
        private readonly IEnumerable<IMebiusTaskBase> _tasks = new List<IMebiusTaskBase>
        {
            new MebiusPmStatus(),
            new MebiusPmStatus2()
        };
        public MainForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            InitializeComponent();

            _jobDisp = CompositionRoot.Resolve<JobDisp>();
            _jobDisp.ListView = listView;
            _jobDisp.MebiusTaskBases = _tasks;
            //_jobDisp = new JobDisp(listView);

            _jobDisp.LoadJobs();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _jobDisp.Abort();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == @"Start")
            {
                _jobDisp.Run();
                buttonStart.Text = @"Stop";
            }
            else
            {
                _jobDisp.Abort();
                buttonStart.Text = @"Start";
            }
        }

        public class MebiusPmStatus2: IMebiusTaskBase
        {
            public string Name => "TEST";
            public string ErrorResult => Results.End.ToString();
            public string Exec(string param)
            {
                return Results.End.ToString();
            }

            private enum Results
            {
                Error,
                End,
                Result1,
                ResultN
            }
            public IEnumerable<string> GetResults => Enum.GetNames(typeof(Results));
        }

        private async void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AddForm(_tasks, Job.GetRepeatseEnumerable.SplitUppers());

            if (form.ShowDialog() == DialogResult.OK)
            {
                var t = new Job
                {
                    Name = form.GetName(),
                    Task = form.GetTask().RemoveSplitUppers(),
                    Repeat = (int) Enum.Parse(typeof(Job.RepeatModes), form.GetRepeat().RemoveSplitUppers()),
                    StopResult = form.GetStopResult().RemoveSplitUppers(),
                    ErrorResult = _tasks.Get(form.GetTask().RemoveSplitUppers()).ErrorResult,
                    Date = form.GetDate(),
                    Param = form.GetParam()
                };

                _jobDisp.Add(JobDisp.JobDispItem.Map(t));

                Task.WaitAll(_unitOfWork.JobRepository.AddAsync(t)); //commit only after add...
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
