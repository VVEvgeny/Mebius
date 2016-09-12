namespace WorkCopy
{
    partial class Setting
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
            this.listViewSettings = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPathLocal = new System.Windows.Forms.TextBox();
            this.textBoxPathRemoteHome = new System.Windows.Forms.TextBox();
            this.textBoxPathRemoteBase = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxPathEtalon = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMergeAppPath = new System.Windows.Forms.TextBox();
            this.buttonUpdateMergeAppPath = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewSettings
            // 
            this.listViewSettings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewSettings.FullRowSelect = true;
            this.listViewSettings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewSettings.Location = new System.Drawing.Point(6, 19);
            this.listViewSettings.MultiSelect = false;
            this.listViewSettings.Name = "listViewSettings";
            this.listViewSettings.Size = new System.Drawing.Size(146, 187);
            this.listViewSettings.TabIndex = 0;
            this.listViewSettings.UseCompatibleStateImageBehavior = false;
            this.listViewSettings.View = System.Windows.Forms.View.Details;
            this.listViewSettings.SelectedIndexChanged += new System.EventHandler(this.listViewSettings_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 141;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(158, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name........................";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(266, 29);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(223, 20);
            this.textBoxName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Number.....................";
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Location = new System.Drawing.Point(266, 54);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(223, 20);
            this.textBoxNumber.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(158, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Path Local................";
            // 
            // textBoxPathLocal
            // 
            this.textBoxPathLocal.Location = new System.Drawing.Point(266, 79);
            this.textBoxPathLocal.Name = "textBoxPathLocal";
            this.textBoxPathLocal.Size = new System.Drawing.Size(223, 20);
            this.textBoxPathLocal.TabIndex = 7;
            // 
            // textBoxPathRemoteHome
            // 
            this.textBoxPathRemoteHome.Location = new System.Drawing.Point(266, 104);
            this.textBoxPathRemoteHome.Name = "textBoxPathRemoteHome";
            this.textBoxPathRemoteHome.Size = new System.Drawing.Size(223, 20);
            this.textBoxPathRemoteHome.TabIndex = 8;
            // 
            // textBoxPathRemoteBase
            // 
            this.textBoxPathRemoteBase.Location = new System.Drawing.Point(266, 129);
            this.textBoxPathRemoteBase.Name = "textBoxPathRemoteBase";
            this.textBoxPathRemoteBase.Size = new System.Drawing.Size(223, 20);
            this.textBoxPathRemoteBase.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Path Remote Home..";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(158, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Path Remote Base....";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Path Etalon...............";
            // 
            // textBoxPathEtalon
            // 
            this.textBoxPathEtalon.Location = new System.Drawing.Point(266, 154);
            this.textBoxPathEtalon.Name = "textBoxPathEtalon";
            this.textBoxPathEtalon.Size = new System.Drawing.Size(223, 20);
            this.textBoxPathEtalon.TabIndex = 13;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(161, 183);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(99, 23);
            this.buttonAdd.TabIndex = 14;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(296, 183);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 15;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(414, 183);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 16;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewSettings);
            this.groupBox1.Controls.Add(this.buttonRemove);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonUpdate);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxPathEtalon);
            this.groupBox1.Controls.Add(this.textBoxNumber);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxPathLocal);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxPathRemoteHome);
            this.groupBox1.Controls.Add(this.textBoxPathRemoteBase);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(499, 214);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Etalons";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBoxMergeAppPath);
            this.groupBox2.Controls.Add(this.buttonUpdateMergeAppPath);
            this.groupBox2.Location = new System.Drawing.Point(0, 216);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(499, 48);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Others";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(171, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Merge App Path.............................";
            // 
            // textBoxMergeAppPath
            // 
            this.textBoxMergeAppPath.Location = new System.Drawing.Point(185, 16);
            this.textBoxMergeAppPath.Name = "textBoxMergeAppPath";
            this.textBoxMergeAppPath.Size = new System.Drawing.Size(223, 20);
            this.textBoxMergeAppPath.TabIndex = 17;
            this.textBoxMergeAppPath.TextChanged += new System.EventHandler(this.textBoxMergeAppPath_TextChanged);
            // 
            // buttonUpdateMergeAppPath
            // 
            this.buttonUpdateMergeAppPath.Location = new System.Drawing.Point(414, 14);
            this.buttonUpdateMergeAppPath.Name = "buttonUpdateMergeAppPath";
            this.buttonUpdateMergeAppPath.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdateMergeAppPath.TabIndex = 17;
            this.buttonUpdateMergeAppPath.Text = "Update";
            this.buttonUpdateMergeAppPath.UseVisualStyleBackColor = true;
            this.buttonUpdateMergeAppPath.Click += new System.EventHandler(this.buttonUpdateMergeAppPath_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 266);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.ShowIcon = false;
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.Setting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewSettings;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPathLocal;
        private System.Windows.Forms.TextBox textBoxPathRemoteHome;
        private System.Windows.Forms.TextBox textBoxPathRemoteBase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxPathEtalon;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxMergeAppPath;
        private System.Windows.Forms.Button buttonUpdateMergeAppPath;
    }
}