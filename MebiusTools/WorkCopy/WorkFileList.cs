using System;
using System.Collections.Generic;

namespace WorkCopy
{
    public partial class MainForm
    {
        private class UpdateListViewEventArgs : EventArgs
        {
            public int Count { get; set; }
        }
        private class WorkFileList : List<WorkFile>
        {
            public event EventHandler<UpdateListViewEventArgs> UpdateListViewCounts;

            public new void Add(WorkFile workFile)
            {
                base.Add(workFile);
                BMTools.BmDebug.Debug.InfoAsync("+++");
                UpdateListViewCounts?.Invoke(this, new UpdateListViewEventArgs { Count = Count });
            }

            public void Remove(WorkFile workFile)
            {
                base.Remove(workFile);
                UpdateListViewCounts?.Invoke(this, new UpdateListViewEventArgs { Count = Count });
            }
        }
    }
}