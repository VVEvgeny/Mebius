namespace WorkCopy
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
            this.components = new System.ComponentModel.Container();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.compareLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareEtalonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.takeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeEtalonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.listForMasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.changeVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeHBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differentToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectDifferentEtalonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.differentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCountSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.filtrToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMain.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewFiles
            // 
            this.listViewFiles.AllowDrop = true;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewFiles.ContextMenuStrip = this.contextMenuStripMain;
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.Location = new System.Drawing.Point(0, 27);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(335, 423);
            this.listViewFiles.TabIndex = 0;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.SelectedIndexChanged += new System.EventHandler(this.listViewFiles_SelectedIndexChanged);
            this.listViewFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragDrop);
            this.listViewFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragEnter);
            this.listViewFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listViewFiles_KeyUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 22;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "File";
            this.columnHeader2.Width = 172;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "# Pack";
            this.columnHeader3.Width = 47;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Version";
            this.columnHeader4.Width = 47;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "H/B";
            this.columnHeader5.Width = 34;
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.compareLocalToolStripMenuItem,
            this.compareEtalonToolStripMenuItem,
            this.toolStripSeparator2,
            this.takeToolStripMenuItem,
            this.takeEtalonToolStripMenuItem,
            this.toolStripSeparator5,
            this.listForMasterToolStripMenuItem,
            this.toolStripSeparator3,
            this.removeToolStripMenuItem,
            this.toolStripSeparator4,
            this.changeVersionToolStripMenuItem,
            this.changeHBToolStripMenuItem,
            this.toolStripSeparator6,
            this.selectToolStripMenuItem,
            this.selectDifferentEtalonToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(174, 282);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // compareLocalToolStripMenuItem
            // 
            this.compareLocalToolStripMenuItem.Name = "compareLocalToolStripMenuItem";
            this.compareLocalToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.compareLocalToolStripMenuItem.Text = "Compare";
            this.compareLocalToolStripMenuItem.Click += new System.EventHandler(this.compareLocalToolStripMenuItem_Click_1);
            // 
            // compareEtalonToolStripMenuItem
            // 
            this.compareEtalonToolStripMenuItem.Name = "compareEtalonToolStripMenuItem";
            this.compareEtalonToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.compareEtalonToolStripMenuItem.Text = "Compare Etalon";
            this.compareEtalonToolStripMenuItem.Click += new System.EventHandler(this.compareEtalonToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // takeToolStripMenuItem
            // 
            this.takeToolStripMenuItem.Name = "takeToolStripMenuItem";
            this.takeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.takeToolStripMenuItem.Text = "Take";
            this.takeToolStripMenuItem.Click += new System.EventHandler(this.takeToolStripMenuItem_Click);
            // 
            // takeEtalonToolStripMenuItem
            // 
            this.takeEtalonToolStripMenuItem.Name = "takeEtalonToolStripMenuItem";
            this.takeEtalonToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.takeEtalonToolStripMenuItem.Text = "Take Etalon";
            this.takeEtalonToolStripMenuItem.Click += new System.EventHandler(this.takeEtalonToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(170, 6);
            // 
            // listForMasterToolStripMenuItem
            // 
            this.listForMasterToolStripMenuItem.Name = "listForMasterToolStripMenuItem";
            this.listForMasterToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.listForMasterToolStripMenuItem.Text = "List For Master";
            this.listForMasterToolStripMenuItem.Click += new System.EventHandler(this.listForMasterToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(170, 6);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(170, 6);
            // 
            // changeVersionToolStripMenuItem
            // 
            this.changeVersionToolStripMenuItem.Name = "changeVersionToolStripMenuItem";
            this.changeVersionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.changeVersionToolStripMenuItem.Text = "Change Version (-)";
            this.changeVersionToolStripMenuItem.Click += new System.EventHandler(this.changeVersionToolStripMenuItem_Click);
            // 
            // changeHBToolStripMenuItem
            // 
            this.changeHBToolStripMenuItem.Name = "changeHBToolStripMenuItem";
            this.changeHBToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.changeHBToolStripMenuItem.Text = "Change H/B (*)";
            this.changeHBToolStripMenuItem.Click += new System.EventHandler(this.changeHBToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(170, 6);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.differentToolStripMenuItem1,
            this.sameToolStripMenuItem1});
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.selectToolStripMenuItem.Text = "Select";
            // 
            // differentToolStripMenuItem1
            // 
            this.differentToolStripMenuItem1.Name = "differentToolStripMenuItem1";
            this.differentToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.differentToolStripMenuItem1.Text = "Different";
            this.differentToolStripMenuItem1.Click += new System.EventHandler(this.differentToolStripMenuItem1_Click);
            // 
            // sameToolStripMenuItem1
            // 
            this.sameToolStripMenuItem1.Name = "sameToolStripMenuItem1";
            this.sameToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.sameToolStripMenuItem1.Text = "Same";
            this.sameToolStripMenuItem1.Click += new System.EventHandler(this.sameToolStripMenuItem1_Click);
            // 
            // selectDifferentEtalonToolStripMenuItem
            // 
            this.selectDifferentEtalonToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.differentToolStripMenuItem,
            this.sameToolStripMenuItem});
            this.selectDifferentEtalonToolStripMenuItem.Name = "selectDifferentEtalonToolStripMenuItem";
            this.selectDifferentEtalonToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.selectDifferentEtalonToolStripMenuItem.Text = "Select Etalon";
            // 
            // differentToolStripMenuItem
            // 
            this.differentToolStripMenuItem.Name = "differentToolStripMenuItem";
            this.differentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.differentToolStripMenuItem.Text = "Different";
            this.differentToolStripMenuItem.Click += new System.EventHandler(this.differentToolStripMenuItem_Click);
            // 
            // sameToolStripMenuItem
            // 
            this.sameToolStripMenuItem.Name = "sameToolStripMenuItem";
            this.sameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sameToolStripMenuItem.Text = "Same";
            this.sameToolStripMenuItem.Click += new System.EventHandler(this.sameToolStripMenuItem_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemConfiguration,
            this.allFilesToolStripMenuItem,
            this.separatorToolStripMenuItem,
            this.toolStripMenuItemCountSelected,
            this.filtrToolStripTextBox,
            this.toolStripMenuItem1});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(335, 27);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // toolStripMenuItemConfiguration
            // 
            this.toolStripMenuItemConfiguration.Name = "toolStripMenuItemConfiguration";
            this.toolStripMenuItemConfiguration.Size = new System.Drawing.Size(93, 23);
            this.toolStripMenuItemConfiguration.Text = "Configuration";
            this.toolStripMenuItemConfiguration.Click += new System.EventHandler(this.toolStripMenuItemConfiguration_Click);
            // 
            // toolStripMenuItemCountSelected
            // 
            this.toolStripMenuItemCountSelected.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItemCountSelected.Name = "toolStripMenuItemCountSelected";
            this.toolStripMenuItemCountSelected.Size = new System.Drawing.Size(12, 23);
            // 
            // filtrToolStripTextBox
            // 
            this.filtrToolStripTextBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.filtrToolStripTextBox.Name = "filtrToolStripTextBox";
            this.filtrToolStripTextBox.ReadOnly = true;
            this.filtrToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.filtrToolStripTextBox.Click += new System.EventHandler(this.filtrToolStripTextBox_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(24, 23);
            this.toolStripMenuItem1.Text = "*";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // separatorToolStripMenuItem
            // 
            this.separatorToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.separatorToolStripMenuItem.Name = "separatorToolStripMenuItem";
            this.separatorToolStripMenuItem.Size = new System.Drawing.Size(24, 23);
            this.separatorToolStripMenuItem.Text = "/";
            // 
            // allFilesToolStripMenuItem
            // 
            this.allFilesToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.allFilesToolStripMenuItem.Name = "allFilesToolStripMenuItem";
            this.allFilesToolStripMenuItem.Size = new System.Drawing.Size(12, 23);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 450);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Work Copy";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragEnter);
            this.contextMenuStripMain.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemConfiguration;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem compareEtalonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem listForMasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem changeVersionToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ToolStripMenuItem changeHBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem compareLocalToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox filtrToolStripTextBox;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCountSelected;
        private System.Windows.Forms.ToolStripMenuItem takeEtalonToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem selectDifferentEtalonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem differentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem differentToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem allFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem separatorToolStripMenuItem;
    }
}

