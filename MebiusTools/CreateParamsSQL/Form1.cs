using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CreateParamsSQL
{
    public partial class Form1 : Form
    {
        private bool Fulloldcopy => checkBoxOldNewLines.Checked;

        public Form1()
        {
            InitializeComponent();

            richTextBoxDescribe_TextChanged(new object(), new EventArgs());
            richTextBoxDescribeShort_TextChanged(new object(), new EventArgs());

        }

        private void richTextBoxDescribe_TextChanged(object sender, EventArgs e)
        {
            labelDescribeSymbols.Text = richTextBoxDescribe.TextLength.ToString();
            labelDescribeSymbols.ForeColor = richTextBoxDescribe.TextLength == richTextBoxDescribe.MaxLength
                ? Color.Red
                : new Color();
        }

        private void richTextBoxDescribeShort_TextChanged(object sender, EventArgs e)
        {
            labelDescribeShortSymbols.Text = richTextBoxDescribeShort.TextLength.ToString();
            labelDescribeShortSymbols.ForeColor = richTextBoxDescribeShort.TextLength ==
                                                  richTextBoxDescribeShort.MaxLength
                ? Color.Red
                : new Color();
        }


        private string DeleteParEnum(string section, string name)
        {
            return
                "DELETE FROM xpress.par_enum WHERE id_par=(SELECT id_par " + (Fulloldcopy ? NewLine() : "") +
                "FROM xpress.tune_par par, xpress.tune_sec sec WHERE " + (Fulloldcopy ? NewLine() : "")+
                "sec.xtype=3" + (Fulloldcopy ? NewLine() : "")+
                " AND sec.name='" + section + "'" + (Fulloldcopy ? NewLine() : "") +
                " AND par.name='" + name + "'" + (Fulloldcopy ? NewLine() : "") +
                " AND par.id_sec=sec.id_sec);";
        }
        private string DeleteTunePar(string section, string name)
        {
            return
                "DELETE FROM xpress.tune_par WHERE id_sec=" +
                "(SELECT id_sec FROM xpress.tune_sec WHERE xtype=" + (Fulloldcopy ? NewLine() : "") +
                "3 AND name='" + section + "')" + (Fulloldcopy ? NewLine() : "") +
                " AND name='" + name +
                "';";
        }
        private string DeleteTuneSys(string section, string name)
        {
            return
                "DELETE FROM xpress.tune_sys WHERE name_sec='" + section + "'" + (Fulloldcopy ? NewLine() : "") +
                " AND name_par='" + name + "';";
        }
        private string WheneverSQLErrorExit()
        {
            return "WHENEVER SQLERROR EXIT;";
        }
        private string WheneverSQLErrorContinue()
        {
            return "WHENEVER SQLERROR continue;";
        }
        private string CreateTmpTpTable()
        {
            return "CREATE TABLE xpress.tp_tmp (" + (Fulloldcopy ? NewLine() : "") +
                   "id_sec INTEGER," + (Fulloldcopy ? NewLine() : "") +
                   "complex SMALLINT," + (Fulloldcopy ? NewLine() : "") +
                   "kind SMALLINT," + (Fulloldcopy ? NewLine() : "") +
                   "xtype SMALLINT," + (Fulloldcopy ? NewLine() : "") +
                   "always SMALLINT," + (Fulloldcopy ? NewLine() : "") +
                   "name VARCHAR(26)," + (Fulloldcopy ? NewLine() : "") +
                   "describe VARCHAR(56)," + (Fulloldcopy ? NewLine() : "") +
                   "describe1 VARCHAR(61)," + (Fulloldcopy ? NewLine() : "") +
                   "describe2 VARCHAR(61)," + (Fulloldcopy ? NewLine() : "") +
                   "describe3 VARCHAR(61)," + (Fulloldcopy ? NewLine() : "") +
                   "describe4 VARCHAR(61)," + (Fulloldcopy ? NewLine() : "") +
                   "id_par INTEGER" +
                   ");";
        }

        private string CreateTmpTlTable()
        {
            return "CREATE TABLE xpress.tl_tmp (" + (Fulloldcopy ? NewLine() : "") +
                   "name_sec VARCHAR(26)," + (Fulloldcopy ? NewLine() : "") +
                   "name_par VARCHAR(26)," + (Fulloldcopy ? NewLine() : "") +
                   "result VARCHAR(251));";
        }

        private string InsertToTmpTlTable(string section, string name, string value)
        {
            return "INSERT INTO xpress.tl_tmp (name_sec,name_par,result)" + (Fulloldcopy ? NewLine() : "") +
                   "SELECT DISTINCT " + (Fulloldcopy ? NewLine() : "") +
                   "'" + section + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + name + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + value + "'" + (Fulloldcopy ? NewLine() : "") +
                   " FROM xpress.TUNE_BNK;";
        }

        private string InsertToTmpTpTable(string section, string name, string describeShort, string describeFull)
        {
            var desc =new List<string>();
            for (var i = 0; i < 5; i++)
            {
                string str;
                if (describeFull.Length > 0)
                {
                    if (describeFull.Length > 60)
                    {
                        str = describeFull.Substring(0, 60);
                        describeFull = describeFull.Remove(0, 60);
                    }
                    else
                    {
                        str = describeFull;
                        describeFull = string.Empty;
                    }
                }
                else
                {
                    str = string.Empty;
                }
                desc.Add(str);
            }

            return "INSERT INTO xpress.tp_tmp (id_sec,complex,kind,xtype,always,name,describe," + (Fulloldcopy ? NewLine() : "") +
                   "describe1,describe2,describe3,describe4,id_par) " + (Fulloldcopy ? NewLine() : "") +
                   "SELECT AVG(sec.id_sec)," + (Fulloldcopy ? NewLine() : "") +
                   "31," + (Fulloldcopy ? NewLine() : "") +
                   "0," + (Fulloldcopy ? NewLine() : "") +
                   "3," + (Fulloldcopy ? NewLine() : "") +
                   "1," + (Fulloldcopy ? NewLine() : "") +
                   "'" + name + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + describeShort + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + desc[0] + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + desc[1] + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + desc[2] + "'," + (Fulloldcopy ? NewLine() : "") +
                   "'" + desc[3] + "'," + (Fulloldcopy ? NewLine() : "") +
                   "MAX(id_par) + 1 " + (Fulloldcopy ? NewLine() : "") +
                   "FROM xpress.tune_par par,xpress.tune_sec sec WHERE sec.name='" + section + "' AND sec.xtype=3;";
        }

        private string InserToTuneParFromTmpTp()
        {
            return "INSERT INTO xpress.tune_par SELECT * FROM xpress.tp_tmp;";
        }

        private string InsertToTuneSysFromTmpTl()
        {
            return "INSERT INTO xpress.tune_sys SELECT * FROM xpress.tl_tmp;";
        }

        private string DropTmpTpTable()
        {
            return "DROP table xpress.tp_tmp;";
        }
        private string DropTmpTlTable()
        {
            return "DROP TABLE xpress.tl_tmp;";
        }

        private string NewLine()
        {
            return Environment.NewLine;
        }
        private string Commit()
        {
            return "COMMIT;";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sf = new SaveFileDialog
            {
                Filter = @"SQL (*.sql)|*.sql",
                FileName = (radioButtonComplexOITU.Checked ? "OITU" : "") + "_" +
                           (radioButtonTypeSystem.Checked ? "System" : "") + "_" + textBoxSection.Text + "_" +
                           textBoxName.Text
            };

            if (sf.ShowDialog() == DialogResult.OK)
            {
                BMTools.BmDebug.Debug.Info("DialogResult.OK", sf.FileName);
                //if (sf.CheckFileExists)


                var fs = sf.OpenFile();
                var sw = new StreamWriter(fs, Encoding.GetEncoding("cp866")) {AutoFlush = true};










                sw.WriteLine(DeleteParEnum(textBoxSection.Text, textBoxName.Text));
                //sw.WriteLine(NewLine());
                sw.WriteLine(DeleteTunePar(textBoxSection.Text, textBoxName.Text));
                //sw.WriteLine(NewLine());
                sw.WriteLine(DeleteTuneSys(textBoxSection.Text, textBoxName.Text));

                sw.WriteLine(string.Empty);
                sw.WriteLine(WheneverSQLErrorExit());
                sw.WriteLine(string.Empty);

                sw.WriteLine(CreateTmpTpTable());
                //sw.WriteLine(NewLine());

                sw.WriteLine(InsertToTmpTpTable(textBoxSection.Text, textBoxName.Text, richTextBoxDescribeShort.Text,
                    richTextBoxDescribe.Text));

                sw.WriteLine(string.Empty);
                sw.WriteLine(Commit());
                sw.WriteLine(string.Empty);
                sw.WriteLine(WheneverSQLErrorContinue());
                sw.WriteLine(string.Empty);

                sw.WriteLine(InserToTuneParFromTmpTp());
                sw.WriteLine(DropTmpTpTable());
                sw.WriteLine(CreateTmpTlTable());
                sw.WriteLine(InsertToTmpTlTable(textBoxSection.Text, textBoxName.Text, textBoxValue.Text));
                sw.WriteLine(InsertToTuneSysFromTmpTl());
                sw.WriteLine(string.Empty);
                sw.WriteLine(Commit());
                sw.WriteLine(string.Empty);
                sw.WriteLine(WheneverSQLErrorContinue());
                sw.WriteLine(string.Empty);
                sw.WriteLine(DropTmpTlTable());
                sw.WriteLine(string.Empty);
                sw.WriteLine(Commit());



                sw.Close();
                fs.Close();

            }
        }
    }
}
