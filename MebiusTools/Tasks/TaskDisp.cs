using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using static System.Reflection.MethodBase;
using static BMTools.BmDebug;

namespace Tasks
{
    public class TaskDisp
    {
        private enum DispStates
        {
            None,
            Loaded,
            Running,
            Aborted
        }
        private DispStates _state = DispStates.None;

        private class TaskDispItem
        {
            public enum StateStates
            {
                None,
                Working,
                Ended
            }
            public int IdInList { get; set; }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public int Repeat { get; set; }
            public string StateText { get; set; }
            public StateStates State = StateStates.None;
            public string PrevStateText { get; set; }
            public string StopResult { get; set; }

            public static TaskDispItem Map(Task task)
            {
                return new TaskDispItem
                {
                    Name = task.Name,
                    Date = task.Date,
                    Repeat = task.Repeat,
                    StopResult = task.StopResult
                };
            }

            public override string ToString()
            {
                return IdInList + " " + Name + " " + Date + " " + (Task.RepeatModes)Repeat + " " + StateText + " " + PrevStateText + " " + StopResult;
            }

            public bool NeedExec()
            {
                switch ((Task.RepeatModes)Repeat)
                {
                    case Task.RepeatModes.Once:
                        return DateTime.Now.Year == Date.Year && DateTime.Now.Month == Date.Month &&
                               DateTime.Now.Day == Date.Day && DateTime.Now.Hour == Date.Hour &&
                               DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryHalfMinute:
                        return DateTime.Now.Second == Date.Second || DateTime.Now.Second - 30 == Date.Second;
                    case Task.RepeatModes.EveryMinute:
                        return DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryHalfHour:
                        return (DateTime.Now.Minute == Date.Minute || DateTime.Now.Minute - 30 == Date.Minute) && DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryHour:
                        return DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryHalfDay:
                        return (DateTime.Now.Hour == Date.Hour || DateTime.Now.Hour - 12 == Date.Hour) &&
                               DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryDay:
                        return DateTime.Now.Hour == Date.Hour && DateTime.Now.Minute == Date.Minute && DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryThirdDay:
                        return (DateTime.Now.Hour == Date.Hour || DateTime.Now.Hour - 8 == Date.Hour ||
                                DateTime.Now.Hour + 8 == Date.Hour) && DateTime.Now.Minute == Date.Minute &&
                               DateTime.Now.Second == Date.Second;
                    case Task.RepeatModes.EveryFourthDay:
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
                switch ((Task.RepeatModes)Repeat)
                {
                    case Task.RepeatModes.Once:
                        break;
                    case Task.RepeatModes.EveryHalfMinute:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddSeconds(30);
                        break;
                    case Task.RepeatModes.EveryMinute:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(1);
                        break;
                    case Task.RepeatModes.EveryHalfHour:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(30);
                        break;
                    case Task.RepeatModes.EveryHour:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(1);
                        break;
                    case Task.RepeatModes.EveryHalfDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(12);
                        break;
                    case Task.RepeatModes.EveryDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddDays(1);
                        break;
                    case Task.RepeatModes.EveryThirdDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(8);
                        break;
                    case Task.RepeatModes.EveryFourthDay:
                        nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Date.Hour, Date.Minute, Date.Second);
                        if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(6);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                return nearDate;
            }
        }

        private class MyList : List<TaskDispItem>
        {
            public delegate void ItemAddedEventHandler(TaskDispItem item);

            public event ItemAddedEventHandler Added;

            public new void Add(TaskDispItem item)
            {
                base.Add(item);
                Added?.Invoke(item);
            }
        }
        private readonly MyList _taskDispItem = new MyList();

        private readonly ListView _listview;
        public TaskDisp(ListView listview)
        {
            _listview = listview;
            _taskDispItem.Added += item =>
            {
                if (_state == DispStates.Running) ThreadPool.QueueUserWorkItem(Calculate, item);
            };
        }

        public void LoadTasks()
        {
            Debug.InfoAsync(GetCurrentMethod());

            if (_state != DispStates.None && _state != DispStates.Aborted) throw new Exception("Invalid state=" + _state);

            using (var db = new DatabaseContext())
            {
                foreach (var task in db.Tasks)
                {
                    Add(task);
                }
            }

            _state = DispStates.Loaded;
        }

        private void UpdateListViewState(int id, string state)
        {
            _listview.BeginInvoke((MethodInvoker)(() => _listview.Items[id].SubItems[2].Text = state));
        }
        private void UpdateListViewPrevState(int id, string state)
        {
            _listview.BeginInvoke((MethodInvoker)(() => _listview.Items[id].SubItems[3].Text = state));
        }

        private void Exec(TaskDispItem item)
        {
            Debug.InfoAsync(GetCurrentMethod(), item);

            item.PrevStateText = Files.GetStatusPm("04300020");

            Debug.InfoAsync(GetCurrentMethod(), "result=", item.PrevStateText);

            UpdateListViewPrevState(item.IdInList, item.PrevStateText);

            if (item.PrevStateText == item.StopResult)
            {
                item.StateText = "Ended by result";
                item.State = TaskDispItem.StateStates.Ended;
            }
        }
        private void ExecWait(TaskDispItem item)
        {
            //Debug.InfoAsync(GetCurrentMethod(), item);

            if ((Task.RepeatModes)item.Repeat == Task.RepeatModes.Once && DateTime.Now > item.Date)
            {
                item.StateText = "Ended by time";
                item.State = TaskDispItem.StateStates.Ended;
            }
            else
            {
                item.State = TaskDispItem.StateStates.Working;
                var ts = item.GetNearDate() - DateTime.Now;
                item.StateText = "To exec=" + ts.ToTime();
            }
        }

        private void Calculate(object state)
        {
            var item = state as TaskDispItem;
            if (item == null)
                throw new NullReferenceException("Calculate(object state) item(TaskDispItem) == null");

            Debug.InfoAsync(GetCurrentMethod(), item, Thread.CurrentThread.ManagedThreadId);

            for (; item.State != TaskDispItem.StateStates.Ended;)
            {
                if (_state == DispStates.Aborted)
                {
                    item.StateText = "Ended by Abort";
                    item.State = TaskDispItem.StateStates.Ended;
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

                if (item.State != TaskDispItem.StateStates.Ended)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void Run()
        {
            Debug.InfoAsync(GetCurrentMethod(), _state);
            if (_state != DispStates.Loaded && _state != DispStates.Aborted) throw new Exception("Invalid state=" + _state);
            _state = DispStates.Running;

            foreach (var item in _taskDispItem)
            {
                item.State = TaskDispItem.StateStates.None;
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

        public void Add(Task task)
        {
            Debug.InfoAsync(GetCurrentMethod(), task);

            var item = TaskDispItem.Map(task);
            _listview.Items.Add(new ListViewItem(new[] { item.Name, ((Task.RepeatModes)item.Repeat).ToString(), item.StateText, item.PrevStateText }));
            item.IdInList = _listview.Items.Count - 1;

            _taskDispItem.Add(item);
        }
    }

}
