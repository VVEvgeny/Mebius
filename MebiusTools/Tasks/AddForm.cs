using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Tasks.Lib.Base;

namespace Tasks
{
    public partial class AddForm : Form
    {
        private readonly IEnumerable<IMebiusTaskBase> _iMebiusTaskBases;
        public AddForm(IEnumerable<IMebiusTaskBase> iMebiusTaskBases, IEnumerable<string> listRepeats)
        {
            InitializeComponent();

            _iMebiusTaskBases = iMebiusTaskBases;

            foreach (var v in _iMebiusTaskBases)
            {
                comboBoxTask.Items.Add(v.Name.SplitUppers());
            }
            
            if (comboBoxTask.Items.Count > 0) comboBoxTask.Text = comboBoxTask.Items[0].ToString();

            foreach (var l in listRepeats) comboBoxRepeat.Items.Add(l);
            if (comboBoxRepeat.Items.Count > 0) comboBoxRepeat.Text = comboBoxRepeat.Items[0].ToString();

            buttonAdd.DialogResult = DialogResult.OK;

            dateTimePickerDate.Value = DateTime.Now.AddSeconds(10);
        }

        public string GetName()
        {
            return textBoxName.Text;
        }

        public string GetTask()
        {
            return comboBoxTask.Text;
        }

        public DateTime GetDate()
        {
            return dateTimePickerDate.Value;
        }

        public string GetParam()
        {
            return textBoxParam.Text;
        }

        public string GetStopResult()
        {
            return comboBoxStopResult.Text;
        }

        public string GetRepeat()
        {
            return comboBoxRepeat.Text;
        }

        private void comboBoxTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxStopResult.Items.Clear();
            comboBoxStopResult.Items.Add(string.Empty);

            foreach (var v in _iMebiusTaskBases.Get(comboBoxTask.Text.RemoveSplitUppers()).GetResults)
            {
                comboBoxStopResult.Items.Add(v.SplitUppers());
            }

            comboBoxStopResult.Text = string.Empty;
        }
    }
}
