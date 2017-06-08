using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Tasks.Database;
using Tasks.Database.Models;
using Tasks.Lib.Base;
using static System.Reflection.MethodBase;
using static BMTools.BmDebug;

namespace Tasks
{
    public class JobDisp
    {
        private readonly IUnitOfWork _unitOfWork;
        public IEnumerable<IMebiusTaskBase> MebiusTaskBases;
        private enum DispStates
        {
            None,
            Loaded,
            Running,
            Aborted
        }
        private DispStates _state = DispStates.None;

        public class JobDispItem
        {
            public enum StateStates
            {
                None,
                Working,
                Ended
            }
            public int IdInList { get; set; }
            public string Name { get; set; }
            public string Task { get; set; }
            public DateTime Date { get; set; }
            public int Repeat { get; set; }
            public string StateText { get; set; }
            public StateStates State = StateStates.None;
            public string PrevStateText { get; set; }
            public string StopResult { get; set; }
            public string ErrorResult { get; set; }
            public string Param { get; set; }

            public static JobDispItem Map(Job job)
            {
                return new JobDispItem
                {
                    Name = job.Name,
                    Task = job.Task,
                    Date = job.Date,
                    Repeat = job.Repeat,
                    StopResult = job.StopResult,
                    ErrorResult = job.ErrorResult,
                    Param = job.Param
                };
            }

            public override string ToString()
            {
                return "IdInList=" + IdInList + ";"
                       + "Name=" + Name + ";"
                       + "Task=" + Task + ";"
                       + "Date=" + Date + ";"
                       + "Repeat=" + (Job.RepeatModes) Repeat + ";"
                       + "StopResult=" + StopResult + ";"
                       + "ErrorResult=" + ErrorResult + ";"
                       + "Param=" + Param + ";"
                       + "StateText=" + StateText + ";"
                       + "PrevStateText=" + PrevStateText + ";"
                    ;
            }

            public bool NeedExec()
            {
                switch ((Job.RepeatModes)Repeat)
                {
                    case Job.RepeatModes.Once:
                        return DateTime.Now.Year == Date.Year && DateTime.Now.Month == Date.Month &&
                               DateTime.Now.Day == Date.Day && DateTime.Now.Hour == Date.Hour &&
                               DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryHalfMinute:
                        return DateTime.Now.Second == Date.Second || DateTime.Now.Second - 30 == Date.Second;
                    case Job.RepeatModes.EveryMinute:
                        return DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryHalfHour:
                        return (DateTime.Now.Minute == Date.Minute || DateTime.Now.Minute - 30 == Date.Minute) && DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryHour:
                        return DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryHalfDay:
                        return (DateTime.Now.Hour == Date.Hour || DateTime.Now.Hour - 12 == Date.Hour) &&
                               DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryDay:
                        return DateTime.Now.Hour == Date.Hour && DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryThirdDay:
                        return (DateTime.Now.Hour == Date.Hour || DateTime.Now.Hour - 8 == Date.Hour ||
                                DateTime.Now.Hour + 8 == Date.Hour) && DateTime.Now.Minute == Date.Minute &&
                               DateTime.Now.Second == Date.Second;
                    case Job.RepeatModes.EveryFourthDay:
                        return (DateTime.Now.Hour == Date.Hour || DateTime.Now.Hour - 6 == Date.Hour ||
                                DateTime.Now.Hour + 6 == Date.Hour || DateTime.Now.Hour - 12 == Date.Hour ||
                                DateTime.Now.Hour + 12 == Date.Hour) &&
                               DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    default:
                        return false;
                }
            }

            public DateTime GetNearDate()
            {
                var nearDate = Date;
                switch ((Job.RepeatModes) Repeat)
                {
                    case Job.RepeatModes.EveryHalfMinute:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, DateTime.Now.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddSeconds(30);
                        break;
                    case Job.RepeatModes.EveryMinute:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, DateTime.Now.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(1);
                        break;
                    case Job.RepeatModes.EveryHalfHour:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(30);
                        break;
                    case Job.RepeatModes.EveryHour:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(1);
                        break;
                    case Job.RepeatModes.EveryHalfDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour,
                            Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(12);
                        break;
                    case Job.RepeatModes.EveryDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour,
                            Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddDays(1);
                        break;
                    case Job.RepeatModes.EveryThirdDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour,
                            Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(8);
                        break;
                    case Job.RepeatModes.EveryFourthDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour,
                            Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(6);
                        break;
                }
                return nearDate;
            }
        }

        private class MyList : List<JobDispItem>
        {
            public delegate void ItemAddedEventHandler(JobDispItem item);

            public event ItemAddedEventHandler Added;

            public new void Add(JobDispItem item)
            {
                base.Add(item);
                Added?.Invoke(item);
            }
        }
        private readonly MyList _taskDispItem = new MyList();

        public ListView ListView { private get; set; }

        public JobDisp(IUnitOfWork unitOfWork)
        {
            Debug.InfoAsync(GetCurrentMethod());


            _unitOfWork = unitOfWork;
            _taskDispItem.Added += item =>
            {
                if (_state == DispStates.Running) ThreadPool.QueueUserWorkItem(Calculate, item);
            };
        }

        public async void LoadJobs()
        {
            Debug.InfoAsync(GetCurrentMethod());

            if (_state != DispStates.None && _state != DispStates.Aborted)
                throw new Exception("Invalid state=" + _state);

            var allJobs = await _unitOfWork.JobRepository.GetAllAsync();

            foreach (var task in allJobs)
            {
                Add(JobDispItem.Map(task));
            }

            _state = DispStates.Loaded;
        }

        private void UpdateListViewState(int id, string state)
        {
            ListView.BeginInvoke((MethodInvoker)(() => ListView.Items[id].SubItems[4].Text = state));
        }
        private void UpdateListViewPrevState(int id, string state)
        {
            ListView.BeginInvoke((MethodInvoker)(() => ListView.Items[id].SubItems[5].Text = state));
        }

        private void Exec(JobDispItem item)
        {
            Debug.InfoAsync(GetCurrentMethod(), item);

            item.PrevStateText = MebiusTaskBases.Get(item.Task.RemoveSplitUppers()).Exec(item.Param);

            Debug.InfoAsync(GetCurrentMethod(), "result=", item.PrevStateText);

            UpdateListViewPrevState(item.IdInList, item.PrevStateText);

            if (item.PrevStateText == item.StopResult)
            {
                item.StateText = "Ended by Stop result";
                item.State = JobDispItem.StateStates.Ended;
            }
            else if (item.PrevStateText == item.ErrorResult)
            {
                item.StateText = "Ended by Error result";
                item.State = JobDispItem.StateStates.Ended;
            }
        }
        private void ExecWait(JobDispItem item)
        {
            //Debug.InfoAsync(GetCurrentMethod(), item);

            if ((Job.RepeatModes)item.Repeat == Job.RepeatModes.Once && DateTime.Now > item.Date)
            {
                item.StateText = "Ended by time";
                item.State = JobDispItem.StateStates.Ended;
            }
            else
            {
                item.State = JobDispItem.StateStates.Working;
                var ts = item.GetNearDate() - DateTime.Now;
                item.StateText = "To exec=" + ts.ToTime();
            }
        }

        private void Calculate(object state)
        {
            var item = state as JobDispItem;
            if (item == null)
                throw new NullReferenceException("Calculate(object state) item(JobDispItem) == null");

            Debug.InfoAsync(GetCurrentMethod(), item, Thread.CurrentThread.ManagedThreadId);

            for (; item.State != JobDispItem.StateStates.Ended;)
            {
                if (_state == DispStates.Aborted)
                {
                    item.StateText = "Ended by Abort";
                    item.State = JobDispItem.StateStates.Ended;
                }
                else
                {
                    if (item.NeedExec())
                    {
                        Exec(item);
                    }
                    else
                    {
                        ExecWait(item);
                    }
                }
                UpdateListViewState(item.IdInList, item.StateText);

                if (item.State != JobDispItem.StateStates.Ended)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void Run()
        {
            Debug.InfoAsync(GetCurrentMethod(), _state);
            if (_state == DispStates.None) LoadJobs();
            if (_state != DispStates.Loaded && _state != DispStates.Aborted) throw new Exception("Invalid state=" + _state);
            _state = DispStates.Running;

            foreach (var item in _taskDispItem)
            {
                item.State = JobDispItem.StateStates.None;
                item.PrevStateText = item.StateText;
                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                ThreadPool.QueueUserWorkItem(Calculate, item);
            }
        }

        public void Abort()
        {
            Debug.InfoAsync(GetCurrentMethod(), _state);
            _state = DispStates.Aborted;
        }


        public void Add(JobDispItem item)
        {
            Debug.InfoAsync(GetCurrentMethod(), item);

            ListView.BeginInvoke((MethodInvoker)(() =>
            {
                ListView.Items.Add(
                    new ListViewItem(new[]
                    {item.Name, item.Task.SplitUppers(), item.Param, ((Job.RepeatModes) item.Repeat).ToString().SplitUppers(), item.StateText.SplitUppers(), item.PrevStateText.SplitUppers()}));
                item.IdInList = ListView.Items.Count - 1;
                _taskDispItem.Add(item);
            }));
        }
    }

}
