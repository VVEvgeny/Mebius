namespace CppClean
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonFindCppInMak = new System.Windows.Forms.Button();
            this.richTextBoxFileNames = new System.Windows.Forms.RichTextBox();
            this.labelFilesInList = new System.Windows.Forms.Label();
            this.buttonFindHppInCpp = new System.Windows.Forms.Button();
            this.richTextBoxFileAddresses = new System.Windows.Forms.RichTextBox();
            this.buttonFindForInAll = new System.Windows.Forms.Button();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelLoadedTotalFilesCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelLoadedUnknownFilesCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelLoadedForFilesCount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelLoadedMakesFilesCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelLoadedHeadersFilesCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelLoadedCodeFilesCount = new System.Windows.Forms.Label();
            this.buttonLoadFiles = new System.Windows.Forms.Button();
            this.buttonChangeDirectory = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonExcludeAddr = new System.Windows.Forms.Button();
            this.textBoxExcludeAddr = new System.Windows.Forms.TextBox();
            this.textBoxExcludeTypes = new System.Windows.Forms.TextBox();
            this.buttonExcludeTypes = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxThreadsCount = new System.Windows.Forms.ComboBox();
            this.labelProgressTime = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelProgressAction = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonFindCppInMak
            // 
            this.buttonFindCppInMak.Location = new System.Drawing.Point(6, 22);
            this.buttonFindCppInMak.Name = "buttonFindCppInMak";
            this.buttonFindCppInMak.Size = new System.Drawing.Size(178, 23);
            this.buttonFindCppInMak.TabIndex = 1;
            this.buttonFindCppInMak.Text = "(.c .cpp) without (.mak .inc)";
            this.buttonFindCppInMak.UseVisualStyleBackColor = true;
            this.buttonFindCppInMak.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBoxFileNames
            // 
            this.richTextBoxFileNames.Location = new System.Drawing.Point(12, 133);
            this.richTextBoxFileNames.Name = "richTextBoxFileNames";
            this.richTextBoxFileNames.Size = new System.Drawing.Size(294, 341);
            this.richTextBoxFileNames.TabIndex = 3;
            this.richTextBoxFileNames.Text = "";
            // 
            // labelFilesInList
            // 
            this.labelFilesInList.AutoSize = true;
            this.labelFilesInList.Location = new System.Drawing.Point(198, 111);
            this.labelFilesInList.Name = "labelFilesInList";
            this.labelFilesInList.Size = new System.Drawing.Size(13, 13);
            this.labelFilesInList.TabIndex = 4;
            this.labelFilesInList.Text = "0";
            // 
            // buttonFindHppInCpp
            // 
            this.buttonFindHppInCpp.Location = new System.Drawing.Point(6, 51);
            this.buttonFindHppInCpp.Name = "buttonFindHppInCpp";
            this.buttonFindHppInCpp.Size = new System.Drawing.Size(178, 23);
            this.buttonFindHppInCpp.TabIndex = 5;
            this.buttonFindHppInCpp.Text = "(.h .hpp) without (.c .cpp .h .hpp)";
            this.buttonFindHppInCpp.UseVisualStyleBackColor = true;
            this.buttonFindHppInCpp.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBoxFileAddresses
            // 
            this.richTextBoxFileAddresses.Location = new System.Drawing.Point(312, 133);
            this.richTextBoxFileAddresses.Name = "richTextBoxFileAddresses";
            this.richTextBoxFileAddresses.Size = new System.Drawing.Size(294, 341);
            this.richTextBoxFileAddresses.TabIndex = 8;
            this.richTextBoxFileAddresses.Text = "";
            // 
            // buttonFindForInAll
            // 
            this.buttonFindForInAll.Location = new System.Drawing.Point(6, 80);
            this.buttonFindForInAll.Name = "buttonFindForInAll";
            this.buttonFindForInAll.Size = new System.Drawing.Size(178, 23);
            this.buttonFindForInAll.TabIndex = 16;
            this.buttonFindForInAll.Text = "(.for) without (.c .cpp .rc .h .hpp)";
            this.buttonFindForInAll.UseVisualStyleBackColor = true;
            this.buttonFindForInAll.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Enabled = false;
            this.textBoxDirectory.Location = new System.Drawing.Point(62, 17);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(266, 20);
            this.textBoxDirectory.TabIndex = 18;
            this.textBoxDirectory.Text = "d:\\Work_vve\\etalon_nix\\";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.buttonLoadFiles);
            this.groupBox1.Controls.Add(this.buttonChangeDirectory);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxDirectory);
            this.groupBox1.Location = new System.Drawing.Point(11, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 97);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelLoadedTotalFilesCount);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.labelLoadedUnknownFilesCount);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.labelLoadedForFilesCount);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.labelLoadedMakesFilesCount);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.labelLoadedHeadersFilesCount);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.labelLoadedCodeFilesCount);
            this.groupBox4.Location = new System.Drawing.Point(74, 43);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(325, 48);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Stats";
            // 
            // labelLoadedTotalFilesCount
            // 
            this.labelLoadedTotalFilesCount.AutoSize = true;
            this.labelLoadedTotalFilesCount.Location = new System.Drawing.Point(266, 30);
            this.labelLoadedTotalFilesCount.Name = "labelLoadedTotalFilesCount";
            this.labelLoadedTotalFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedTotalFilesCount.TabIndex = 31;
            this.labelLoadedTotalFilesCount.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(263, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Total:";
            // 
            // labelLoadedUnknownFilesCount
            // 
            this.labelLoadedUnknownFilesCount.AutoSize = true;
            this.labelLoadedUnknownFilesCount.Location = new System.Drawing.Point(209, 31);
            this.labelLoadedUnknownFilesCount.Name = "labelLoadedUnknownFilesCount";
            this.labelLoadedUnknownFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedUnknownFilesCount.TabIndex = 29;
            this.labelLoadedUnknownFilesCount.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(196, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Unknown:";
            // 
            // labelLoadedForFilesCount
            // 
            this.labelLoadedForFilesCount.AutoSize = true;
            this.labelLoadedForFilesCount.Location = new System.Drawing.Point(169, 32);
            this.labelLoadedForFilesCount.Name = "labelLoadedForFilesCount";
            this.labelLoadedForFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedForFilesCount.TabIndex = 27;
            this.labelLoadedForFilesCount.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "For:";
            // 
            // labelLoadedMakesFilesCount
            // 
            this.labelLoadedMakesFilesCount.AutoSize = true;
            this.labelLoadedMakesFilesCount.Location = new System.Drawing.Point(125, 32);
            this.labelLoadedMakesFilesCount.Name = "labelLoadedMakesFilesCount";
            this.labelLoadedMakesFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedMakesFilesCount.TabIndex = 25;
            this.labelLoadedMakesFilesCount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Makes:";
            // 
            // labelLoadedHeadersFilesCount
            // 
            this.labelLoadedHeadersFilesCount.AutoSize = true;
            this.labelLoadedHeadersFilesCount.Location = new System.Drawing.Point(67, 32);
            this.labelLoadedHeadersFilesCount.Name = "labelLoadedHeadersFilesCount";
            this.labelLoadedHeadersFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedHeadersFilesCount.TabIndex = 23;
            this.labelLoadedHeadersFilesCount.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(59, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Headers:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Code:";
            // 
            // labelLoadedCodeFilesCount
            // 
            this.labelLoadedCodeFilesCount.AutoSize = true;
            this.labelLoadedCodeFilesCount.Location = new System.Drawing.Point(22, 32);
            this.labelLoadedCodeFilesCount.Name = "labelLoadedCodeFilesCount";
            this.labelLoadedCodeFilesCount.Size = new System.Drawing.Size(13, 13);
            this.labelLoadedCodeFilesCount.TabIndex = 20;
            this.labelLoadedCodeFilesCount.Text = "0";
            // 
            // buttonLoadFiles
            // 
            this.buttonLoadFiles.Location = new System.Drawing.Point(9, 43);
            this.buttonLoadFiles.Name = "buttonLoadFiles";
            this.buttonLoadFiles.Size = new System.Drawing.Size(59, 23);
            this.buttonLoadFiles.TabIndex = 19;
            this.buttonLoadFiles.Text = "Load";
            this.buttonLoadFiles.UseVisualStyleBackColor = true;
            this.buttonLoadFiles.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // buttonChangeDirectory
            // 
            this.buttonChangeDirectory.Location = new System.Drawing.Point(334, 16);
            this.buttonChangeDirectory.Name = "buttonChangeDirectory";
            this.buttonChangeDirectory.Size = new System.Drawing.Size(59, 23);
            this.buttonChangeDirectory.TabIndex = 2;
            this.buttonChangeDirectory.Text = "Change";
            this.buttonChangeDirectory.UseVisualStyleBackColor = true;
            this.buttonChangeDirectory.Click += new System.EventHandler(this.button8_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Directory:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonExcludeAddr);
            this.groupBox2.Controls.Add(this.textBoxExcludeAddr);
            this.groupBox2.Controls.Add(this.textBoxExcludeTypes);
            this.groupBox2.Controls.Add(this.buttonExcludeTypes);
            this.groupBox2.Controls.Add(this.buttonFindCppInMak);
            this.groupBox2.Controls.Add(this.buttonFindHppInCpp);
            this.groupBox2.Controls.Add(this.buttonFindForInAll);
            this.groupBox2.Location = new System.Drawing.Point(612, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(196, 335);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Work";
            // 
            // buttonExcludeAddr
            // 
            this.buttonExcludeAddr.Location = new System.Drawing.Point(6, 233);
            this.buttonExcludeAddr.Name = "buttonExcludeAddr";
            this.buttonExcludeAddr.Size = new System.Drawing.Size(178, 23);
            this.buttonExcludeAddr.TabIndex = 24;
            this.buttonExcludeAddr.Text = "Remove Addr from lists";
            this.buttonExcludeAddr.UseVisualStyleBackColor = true;
            this.buttonExcludeAddr.Click += new System.EventHandler(this.buttonExcludeAddr_Click);
            // 
            // textBoxExcludeAddr
            // 
            this.textBoxExcludeAddr.Location = new System.Drawing.Point(6, 262);
            this.textBoxExcludeAddr.Name = "textBoxExcludeAddr";
            this.textBoxExcludeAddr.Size = new System.Drawing.Size(178, 20);
            this.textBoxExcludeAddr.TabIndex = 23;
            this.textBoxExcludeAddr.Text = "sys00";
            // 
            // textBoxExcludeTypes
            // 
            this.textBoxExcludeTypes.Location = new System.Drawing.Point(6, 315);
            this.textBoxExcludeTypes.Name = "textBoxExcludeTypes";
            this.textBoxExcludeTypes.Size = new System.Drawing.Size(178, 20);
            this.textBoxExcludeTypes.TabIndex = 22;
            this.textBoxExcludeTypes.Text = ".rc,.rec";
            // 
            // buttonExcludeTypes
            // 
            this.buttonExcludeTypes.Location = new System.Drawing.Point(6, 288);
            this.buttonExcludeTypes.Name = "buttonExcludeTypes";
            this.buttonExcludeTypes.Size = new System.Drawing.Size(178, 23);
            this.buttonExcludeTypes.TabIndex = 17;
            this.buttonExcludeTypes.Text = "Remove Type from lists";
            this.buttonExcludeTypes.UseVisualStyleBackColor = true;
            this.buttonExcludeTypes.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(126, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Files:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.comboBoxThreadsCount);
            this.groupBox3.Controls.Add(this.labelProgressTime);
            this.groupBox3.Controls.Add(this.progressBar1);
            this.groupBox3.Controls.Add(this.labelProgressAction);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(417, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 92);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Progress";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(330, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Threads:";
            // 
            // comboBoxThreadsCount
            // 
            this.comboBoxThreadsCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThreadsCount.FormattingEnabled = true;
            this.comboBoxThreadsCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBoxThreadsCount.Location = new System.Drawing.Point(323, 30);
            this.comboBoxThreadsCount.Name = "comboBoxThreadsCount";
            this.comboBoxThreadsCount.Size = new System.Drawing.Size(62, 21);
            this.comboBoxThreadsCount.TabIndex = 22;
            // 
            // labelProgressTime
            // 
            this.labelProgressTime.AutoSize = true;
            this.labelProgressTime.Location = new System.Drawing.Point(142, 38);
            this.labelProgressTime.Name = "labelProgressTime";
            this.labelProgressTime.Size = new System.Drawing.Size(0, 13);
            this.labelProgressTime.TabIndex = 21;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 59);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(378, 23);
            this.progressBar1.TabIndex = 21;
            // 
            // labelProgressAction
            // 
            this.labelProgressAction.AutoSize = true;
            this.labelProgressAction.Location = new System.Drawing.Point(52, 21);
            this.labelProgressAction.Name = "labelProgressAction";
            this.labelProgressAction.Size = new System.Drawing.Size(0, 13);
            this.labelProgressAction.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Action:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 480);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBoxFileAddresses);
            this.Controls.Add(this.labelFilesInList);
            this.Controls.Add(this.richTextBoxFileNames);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Cpp Cleaner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFindCppInMak;
        private System.Windows.Forms.RichTextBox richTextBoxFileNames;
        private System.Windows.Forms.Label labelFilesInList;
        private System.Windows.Forms.Button buttonFindHppInCpp;
        private System.Windows.Forms.RichTextBox richTextBoxFileAddresses;
        private System.Windows.Forms.Button buttonFindForInAll;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonChangeDirectory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelProgressTime;
        private System.Windows.Forms.Label labelProgressAction;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonLoadFiles;
        private System.Windows.Forms.Label labelLoadedCodeFilesCount;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelLoadedHeadersFilesCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLoadedMakesFilesCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelLoadedUnknownFilesCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelLoadedForFilesCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBoxExcludeTypes;
        private System.Windows.Forms.Button buttonExcludeTypes;
        private System.Windows.Forms.Button buttonExcludeAddr;
        private System.Windows.Forms.TextBox textBoxExcludeAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxThreadsCount;
        private System.Windows.Forms.Label labelLoadedTotalFilesCount;
        private System.Windows.Forms.Label label10;
    }
}

