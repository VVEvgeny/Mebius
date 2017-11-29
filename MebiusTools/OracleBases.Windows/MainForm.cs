using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OracleBases.Database;
using OracleBases.Database.Models;
using Timer = System.Windows.Forms.Timer;

namespace OracleBases.Windows
{
    public partial class MainForm : Form
    {
        private readonly IUnitOfWork _unitOfWork;
        public MainForm(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InitializeComponent();

            _progressTimer = new Timer {Interval = 1, Enabled = true};
            _progressTimer.Tick += (sender, args) =>
            {
                if (toolStripProgressBar.Value == toolStripProgressBar.Maximum) toolStripProgressBar.Value = toolStripProgressBar.Minimum;
                else
                {
                    toolStripProgressBar.Value++;
                }
            };

            Task.Run(() => LoadList());
        }

        private readonly Timer _progressTimer;
        private void StartProgressBar()
        {
            _progressTimer.Start();
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() =>
                        toolStripProgressBar.Value = toolStripProgressBar.Minimum
                ));
            }
            else
            {
                toolStripProgressBar.Value = toolStripProgressBar.Minimum;
            }

            if (listViewServers.InvokeRequired)
            {
                listViewServers.BeginInvoke((MethodInvoker) (() =>
                    listViewServers.Enabled = false
                ));
            }
            else
            {
                listViewServers.Enabled = false;
            }
            if (listViewCommands.InvokeRequired)
            {
                listViewCommands.BeginInvoke((MethodInvoker)(() =>
                    listViewCommands.Enabled = false
                ));
            }
            else
            {
                listViewCommands.Enabled = false;
            }
        }
        private void StopProgressBar()
        {
            _progressTimer.Stop();
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)(() =>
                    toolStripProgressBar.Value = toolStripProgressBar.Maximum
                ));
            }
            else
            {
                toolStripProgressBar.Value = toolStripProgressBar.Maximum;
            }

            if (listViewServers.InvokeRequired)
            {
                listViewServers.BeginInvoke((MethodInvoker)(() =>
                    listViewServers.Enabled = true
                ));
            }
            else
            {
                listViewServers.Enabled = true;
            }
            if (listViewCommands.InvokeRequired)
            {
                listViewCommands.BeginInvoke((MethodInvoker)(() =>
                    listViewCommands.Enabled = true
                ));
            }
            else
            {
                listViewCommands.Enabled = true;
            }
        }

        private async void LoadList(Connect inConnect = null)
        {
            BMTools.BmDebug.Debug.SaveCall().Info("start");

            if (inConnect == null)
            {
                StartProgressBar();

                listViewServers.BeginInvoke((MethodInvoker) (() =>
                    listViewServers.Items.Clear()
                ));

                var connects = await _unitOfWork.ConnectRepository.GetAllAsync();

                foreach (var connect in connects.OrderByDescending(c => c.LastUsed))
                {
                    listViewServers.BeginInvoke((MethodInvoker) (() =>
                        listViewServers.Items.Add(
                            new ListViewItem(new[]
                            {
                                connect.Name,
                                connect.Host + ":" + connect.Port,
                                connect.OperDay,
                                connect.PrevDay,
                                connect.Regions
                            }))
                    ));
                }

                StopProgressBar();
            }
            else
            {
                listViewServers.BeginInvoke((MethodInvoker)(() =>
                    {
                        foreach (ListViewItem item in listViewServers.Items)
                        {
                            if (item.SubItems[0].Text == inConnect.Name)
                            {
                                item.SubItems[1].Text = inConnect.Host + @":" + inConnect.Port;
                                item.SubItems[2].Text = inConnect.OperDay;
                                item.SubItems[3].Text = inConnect.PrevDay;
                                item.SubItems[4].Text = inConnect.Regions;
                            }
                        }
                    }
                ));
            }
        }


        private bool CheckExist(string connectName)
        {
            var ret = _unitOfWork.ConnectRepository.FirstOrDefaultAsync(c => c.Name == connectName).Result;

            BMTools.BmDebug.Debug.SaveCall().InfoAsync(connectName, ret);

            return ret != null;
        }
        private async void AddConnectToDb(AddConnect add)
        {
            BMTools.BmDebug.Debug.SaveCall().InfoAsync(add.GetName);

            if (CheckExist(add.GetName))
            {
                MessageBox.Show(@"Name exist");
                return;
            }

            await _unitOfWork.ConnectRepository.AddAsync(new Connect
            {
                Host = add.GetHost,
                Name = add.GetName,
                Password = add.GetPassword,
                Port = add.GetPort,
                ServiceName = add.GetService,
                UserId = add.GetUserId,
                DateAdd = DateTime.Now,
                LastUsed = DateTime.Now
            });

            await _unitOfWork.CommitAsync();
            LoadList();
        }
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var add = new AddConnect();
            if(add.ShowDialog() == DialogResult.OK)
            {
                AddConnectToDb(add);
            }
        }

        private async void RemoveConnect()
        {
            BMTools.BmDebug.Debug.SaveCall().Info(" ");

            if (listViewServers.SelectedItems.Count > 0)
            {
                foreach (ListViewItem i in listViewServers.SelectedItems)
                {
                    var connectName = i.SubItems[0].Text;
                    var connect = await _unitOfWork.ConnectRepository.FirstAsync(c => c.Name == connectName);
                    await _unitOfWork.ConnectRepository.DeleteAsync(connect);
                }
                await _unitOfWork.CommitAsync();
                LoadList();
            }
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveConnect();
        }

        private async void Work(Connect connect, string command)
        {
            BMTools.BmDebug.Debug.SaveCall().Info(command);
            BMTools.BmDebug.Debug.SaveCall().Info("BEGIN" + " connect=" + connect.Name + " command=" + command);
            
            var oracle = new Ora();
            var answer = oracle.GetFromDb(connect, command);

            foreach (var a in answer)
            {
                BMTools.BmDebug.Debug.SaveCall().Info("connect=" + connect.Name + " command=" + command + " DATA=", a);
            }

            if (command == Ora.OraStrings.SqlOperDay
                || command == Ora.OraStrings.SqlLastDay
                || command == Ora.OraStrings.SqlRegions)
            {
                connect.LastUsed = DateTime.Now;

                try
                {
                    if (answer.Count == 1 && answer[0].Contains("ORA-"))
                    {
                        throw new Exception();
                    }

                    if (command == Ora.OraStrings.SqlOperDay)
                        connect.OperDay = Convert.ToDateTime(answer[0]).ToShortDateString();
                    if (command == Ora.OraStrings.SqlLastDay)
                        connect.PrevDay = Convert.ToDateTime(answer[0]).ToShortDateString();
                    if (command == Ora.OraStrings.SqlRegions) connect.Regions = answer[0];
                }
                catch
                {
                    if (command == Ora.OraStrings.SqlOperDay)
                        connect.OperDay = answer.Aggregate(string.Empty, (current, a) => current + (a + ";"));
                    if (command == Ora.OraStrings.SqlLastDay)
                        connect.PrevDay = answer.Aggregate(string.Empty, (current, a) => current + (a + ";"));
                    if (command == Ora.OraStrings.SqlRegions)
                        connect.Regions = answer.Aggregate(string.Empty, (current, a) => current + (a + ";"));
                }

                LoadList(connect);
            }
            else
            {
                throw new Exception("Unknown command");
            }

            BMTools.BmDebug.Debug.SaveCall().Info("END"+ " connect=" + connect.Name + " command=" + command);
        }
        private async Task<List<string>> WorkWithResult(Connect connect, string command)
        {
            return await Task.Run(() =>
            {
                BMTools.BmDebug.Debug.SaveCall().Info(command);
                BMTools.BmDebug.Debug.SaveCall().Info("BEGIN" + " connect=" + connect.Name + " command=" + command);

                var oracle = new Ora();
                var answer = oracle.GetFromDb(connect, command);

                foreach (var a in answer)
                {
                    BMTools.BmDebug.Debug.SaveCall().Info("connect=" + connect.Name + " command=" + command + " DATA=", a);
                }

                BMTools.BmDebug.Debug.SaveCall().Info("END" + " connect=" + connect.Name + " command=" + command);

                return answer;
            });
        }

        private async Task<Connect> GetConnectByName(string connectName)
        {
            return await _unitOfWork.ConnectRepository.FirstOrDefaultAsync(c => c.Name == connectName);
        }
        private async void UpdateConnect()
        {
            BMTools.BmDebug.Debug.SaveCall().Info(" ");

            if (listViewServers.SelectedItems.Count > 0)
            {
                StartProgressBar();


                await Task.Run(async () =>
                {
                    var tasks = new Task[listViewServers.SelectedItems.Count * 3];
                    var index = 0;
                    foreach (ListViewItem i in listViewServers.SelectedItems)
                    {
                        var connect = await GetConnectByName(i.SubItems[0].Text);

                        tasks[index++] = Task.Run(() => Work(connect, Ora.OraStrings.SqlOperDay));
                        tasks[index++] = Task.Run(() => Work(connect, Ora.OraStrings.SqlLastDay));
                        tasks[index++] =  Task.Run(() => Work(connect, Ora.OraStrings.SqlRegions));
                    }
                    Task.WaitAll(tasks);

                    BMTools.BmDebug.Debug.SaveCall().Info("CommitAsync");
                    await _unitOfWork.CommitAsync();

                }).ContinueWith((a) =>
                {
                    StopProgressBar();
                });
            }
        }
        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateConnect();
        }

        private async void CustomCommand(string command)
        {
            if (listViewServers.SelectedItems.Count > 0)
            {
                tabControl.SelectTab(tabPage1);

                StartProgressBar();

                listViewCommands.BeginInvoke((MethodInvoker) (() =>
                    {
                        listViewCommands.Items.Clear();
                    }
                ));

                await Task.Run(async () =>
                {
                    var tasks = new Task[listViewServers.SelectedItems.Count];
                    var index = 0;
                    foreach (ListViewItem i in listViewServers.SelectedItems)
                    {
                        var connect = await GetConnectByName(i.SubItems[0].Text);

                        tasks[index++] = WorkWithResult(connect, command).ContinueWith((ans) =>
                        {
                            listViewCommands.BeginInvoke((MethodInvoker) (() =>
                                {
                                    listViewCommands.Items.Add(
                                        new ListViewItem(new[]
                                        {
                                            connect.Name,
                                            command,
                                            ans.Result.Aggregate(string.Empty, (current, a) => current + (a + ";"))
                                        }));
                                }
                            ));
                        });
                    }
                    Task.WaitAll(tasks);
                }).ContinueWith((a) =>
                {
                    StopProgressBar();
                });
            }
        }

        private void toolStripTextBoxCustomCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                CustomCommand(toolStripTextBoxCustomCommand.Text);
            }
        }
    }
}
