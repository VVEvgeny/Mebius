using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using static System.Reflection.MethodBase;
using static BMTools.BmDebug;

namespace Tasks
{
    public static class Extension
    {
        public static string ToTime(this TimeSpan ts)
        {
            return ts.ToString(@"dd\.hh\:mm\:ss");
        }
    }
    public partial class MainForm : Form
    {
        private readonly TaskDisp _taskDisp;
        public MainForm()
        {
            InitializeComponent();
            
            Debug.ClassUsing = GetType().FullName;
            Debug.DebugLevel = DebugLevels.All;
            Debug.Output = OutputModes.LogWindow;
            Debug.Info("Started");

            _taskDisp = new TaskDisp(listView);
            _taskDisp.LoadTasks();
        }

        private class TaskDisp
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
                public StateStates PrevState = StateStates.None;

                public static TaskDispItem Map(Task task)
                {
                    return new TaskDispItem
                    {
                        Name = task.Name,
                        Date = task.Date,
                        Repeat = task.Repeat
                    };
                }

                public override string ToString()
                {
                    return IdInList + " " + Name + " " + Date + " " + (Task.RepeatModes)Repeat + " " + StateText + " " + PrevStateText;
                }
            }

            private class MyList: List<TaskDispItem>
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

                item.PrevStateText = "Ok";
            }
            private void Calculate(object state)
            {
                var item = state as TaskDispItem;
                if (item == null) throw new NullReferenceException("Calculate(object state) item(TaskDispItem) == null");

                Debug.InfoAsync(GetCurrentMethod(), item, Thread.CurrentThread.ManagedThreadId);

                for (; item.State != TaskDispItem.StateStates.Ended;)
                {
                    if (_state == DispStates.Aborted)
                    {
                        item.StateText = "Ended by Abort";
                        item.State = TaskDispItem.StateStates.Ended;
                    }
                    else switch ((Task.RepeatModes)item.Repeat)
                    {
                        case Task.RepeatModes.Once:
                            if (item.Date > DateTime.Now)
                            {
                                item.State = TaskDispItem.StateStates.Working;
                                var ts = item.Date - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            else
                            {
                                if (item.State == TaskDispItem.StateStates.Working)
                                {
                                    Exec(item);
                                }
                                item.StateText = "Ended by time";
                                item.State = TaskDispItem.StateStates.Ended;
                            }
                            break;
                        case Task.RepeatModes.EveryMinute:
                            if (DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(1);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryHalfMinute:
                            if (DateTime.Now.Second == item.Date.Second || DateTime.Now.Second - 30 == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddSeconds(30);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryHour:
                            if (DateTime.Now.Minute == item.Date.Minute && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(1);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryHalfHour:
                            if ((DateTime.Now.Minute == item.Date.Minute || DateTime.Now.Minute - 30 == item.Date.Minute) && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddMinutes(30);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryDay:
                            if (DateTime.Now.Hour == item.Date.Hour && DateTime.Now.Minute == item.Date.Minute && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Date.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddDays(1);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryHalfDay:
                            if ((DateTime.Now.Hour == item.Date.Hour || DateTime.Now.Hour - 12 == item.Date.Hour) && DateTime.Now.Minute == item.Date.Minute && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Date.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(12);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryThirdDay:
                            if ((DateTime.Now.Hour == item.Date.Hour || DateTime.Now.Hour - 8 == item.Date.Hour || DateTime.Now.Hour + 8 == item.Date.Hour) && DateTime.Now.Minute == item.Date.Minute && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Date.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(8);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        case Task.RepeatModes.EveryFourthDay:
                            if ((DateTime.Now.Hour == item.Date.Hour
                                 || DateTime.Now.Hour - 6 == item.Date.Hour || DateTime.Now.Hour + 6 == item.Date.Hour
                                 || DateTime.Now.Hour - 12 == item.Date.Hour || DateTime.Now.Hour + 12 == item.Date.Hour
                                ) 
                                && DateTime.Now.Minute == item.Date.Minute && DateTime.Now.Second == item.Date.Second)
                            {
                                Exec(item);
                                UpdateListViewPrevState(item.IdInList, item.PrevStateText);
                            }
                            else
                            {
                                var nearDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.Date.Hour, item.Date.Minute, item.Date.Second);
                                if (nearDate < DateTime.Now) nearDate = nearDate.AddHours(6);

                                item.State = TaskDispItem.StateStates.Working;
                                var ts = nearDate - DateTime.Now;
                                item.StateText = "To exec=" + ts.ToTime();
                            }
                            break;
                        default:
                            item.StateText = "Ended by unknown repeat type";
                            item.State = TaskDispItem.StateStates.Ended;
                            break;
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
                    UpdateListViewPrevState(item.IdInList,item.PrevStateText);
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
                _listview.Items.Add(new ListViewItem(new[]{item.Name, ((Task.RepeatModes) item.Repeat).ToString(), item.StateText, item.PrevStateText}));
                item.IdInList = _listview.Items.Count - 1;

                _taskDispItem.Add(item);
            }
        }
        

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Task
            {
                Name = "test",
                Repeat = (int) Task.RepeatModes.EveryFourthDay,
                Date = DateTime.Now.AddSeconds(10)
            };

            using (var db = new DatabaseContext())
            {
                db.Tasks.Add(t);
                db.SaveChanges();
            }
            _taskDisp.Add(t);
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
    }
}
