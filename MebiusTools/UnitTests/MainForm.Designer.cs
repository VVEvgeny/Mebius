namespace UnitTests
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
            this.labelVersionText = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.comboBoxVersion = new System.Windows.Forms.ComboBox();
            this.buttonCopyRunTest = new System.Windows.Forms.Button();
            this.labelUserText = new System.Windows.Forms.Label();
            this.labelPathEtalonText = new System.Windows.Forms.Label();
            this.textBoxPathEtalon = new System.Windows.Forms.TextBox();
            this.comboBoxPacket = new System.Windows.Forms.ComboBox();
            this.labelPacketText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRunTest = new System.Windows.Forms.TextBox();
            this.buttonCompareLocal = new System.Windows.Forms.Button();
            this.buttonCompareEtalon = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelVersionText
            // 
            this.labelVersionText.AutoSize = true;
            this.labelVersionText.Location = new System.Drawing.Point(12, 12);
            this.labelVersionText.Name = "labelVersionText";
            this.labelVersionText.Size = new System.Drawing.Size(42, 13);
            this.labelVersionText.TabIndex = 0;
            this.labelVersionText.Text = "Version";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(181, 14);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(100, 20);
            this.textBoxUser.TabIndex = 1;
            this.textBoxUser.Text = "vve";
            // 
            // comboBoxVersion
            // 
            this.comboBoxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVersion.FormattingEnabled = true;
            this.comboBoxVersion.Location = new System.Drawing.Point(60, 9);
            this.comboBoxVersion.Name = "comboBoxVersion";
            this.comboBoxVersion.Size = new System.Drawing.Size(58, 21);
            this.comboBoxVersion.TabIndex = 2;
            this.comboBoxVersion.SelectedIndexChanged += new System.EventHandler(this.comboBoxVersion_SelectedIndexChanged);
            // 
            // buttonCopyRunTest
            // 
            this.buttonCopyRunTest.Location = new System.Drawing.Point(344, 98);
            this.buttonCopyRunTest.Name = "buttonCopyRunTest";
            this.buttonCopyRunTest.Size = new System.Drawing.Size(44, 23);
            this.buttonCopyRunTest.TabIndex = 3;
            this.buttonCopyRunTest.Text = "Copy";
            this.buttonCopyRunTest.UseVisualStyleBackColor = true;
            this.buttonCopyRunTest.Click += new System.EventHandler(this.buttonCopyRunTest_Click);
            // 
            // labelUserText
            // 
            this.labelUserText.AutoSize = true;
            this.labelUserText.Location = new System.Drawing.Point(133, 17);
            this.labelUserText.Name = "labelUserText";
            this.labelUserText.Size = new System.Drawing.Size(29, 13);
            this.labelUserText.TabIndex = 4;
            this.labelUserText.Text = "User";
            // 
            // labelPathEtalonText
            // 
            this.labelPathEtalonText.AutoSize = true;
            this.labelPathEtalonText.Location = new System.Drawing.Point(12, 54);
            this.labelPathEtalonText.Name = "labelPathEtalonText";
            this.labelPathEtalonText.Size = new System.Drawing.Size(62, 13);
            this.labelPathEtalonText.TabIndex = 5;
            this.labelPathEtalonText.Text = "Path Etalon";
            // 
            // textBoxPathEtalon
            // 
            this.textBoxPathEtalon.Location = new System.Drawing.Point(80, 47);
            this.textBoxPathEtalon.Name = "textBoxPathEtalon";
            this.textBoxPathEtalon.Size = new System.Drawing.Size(269, 20);
            this.textBoxPathEtalon.TabIndex = 6;
            this.textBoxPathEtalon.Text = "\\\\linux3.minsk.mebius.net\\exe\\kernel.mt\\data\\";
            // 
            // comboBoxPacket
            // 
            this.comboBoxPacket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPacket.FormattingEnabled = true;
            this.comboBoxPacket.Location = new System.Drawing.Point(121, 73);
            this.comboBoxPacket.Name = "comboBoxPacket";
            this.comboBoxPacket.Size = new System.Drawing.Size(153, 21);
            this.comboBoxPacket.TabIndex = 7;
            this.comboBoxPacket.SelectedIndexChanged += new System.EventHandler(this.comboBoxPacket_SelectedIndexChanged);
            // 
            // labelPacketText
            // 
            this.labelPacketText.AutoSize = true;
            this.labelPacketText.Location = new System.Drawing.Point(21, 81);
            this.labelPacketText.Name = "labelPacketText";
            this.labelPacketText.Size = new System.Drawing.Size(41, 13);
            this.labelPacketText.TabIndex = 8;
            this.labelPacketText.Text = "Packet";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Run Test";
            // 
            // textBoxRunTest
            // 
            this.textBoxRunTest.Location = new System.Drawing.Point(69, 100);
            this.textBoxRunTest.Name = "textBoxRunTest";
            this.textBoxRunTest.Size = new System.Drawing.Size(269, 20);
            this.textBoxRunTest.TabIndex = 10;
            // 
            // buttonCompareLocal
            // 
            this.buttonCompareLocal.Location = new System.Drawing.Point(15, 135);
            this.buttonCompareLocal.Name = "buttonCompareLocal";
            this.buttonCompareLocal.Size = new System.Drawing.Size(373, 23);
            this.buttonCompareLocal.TabIndex = 11;
            this.buttonCompareLocal.Text = "Compare Local";
            this.buttonCompareLocal.UseVisualStyleBackColor = true;
            this.buttonCompareLocal.Click += new System.EventHandler(this.buttonCompareLocal_Click);
            // 
            // buttonCompareEtalon
            // 
            this.buttonCompareEtalon.Location = new System.Drawing.Point(15, 164);
            this.buttonCompareEtalon.Name = "buttonCompareEtalon";
            this.buttonCompareEtalon.Size = new System.Drawing.Size(373, 23);
            this.buttonCompareEtalon.TabIndex = 12;
            this.buttonCompareEtalon.Text = "Compare Etalon";
            this.buttonCompareEtalon.UseVisualStyleBackColor = true;
            this.buttonCompareEtalon.Click += new System.EventHandler(this.buttonCompareEtalon_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(373, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Copy Packet data to Local";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 255);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCompareEtalon);
            this.Controls.Add(this.buttonCompareLocal);
            this.Controls.Add(this.textBoxRunTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelPacketText);
            this.Controls.Add(this.comboBoxPacket);
            this.Controls.Add(this.textBoxPathEtalon);
            this.Controls.Add(this.labelPathEtalonText);
            this.Controls.Add(this.labelUserText);
            this.Controls.Add(this.buttonCopyRunTest);
            this.Controls.Add(this.comboBoxVersion);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.labelVersionText);
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Unit Tests";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVersionText;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.ComboBox comboBoxVersion;
        private System.Windows.Forms.Button buttonCopyRunTest;
        private System.Windows.Forms.Label labelUserText;
        private System.Windows.Forms.Label labelPathEtalonText;
        private System.Windows.Forms.TextBox textBoxPathEtalon;
        private System.Windows.Forms.ComboBox comboBoxPacket;
        private System.Windows.Forms.Label labelPacketText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRunTest;
        private System.Windows.Forms.Button buttonCompareLocal;
        private System.Windows.Forms.Button buttonCompareEtalon;
        private System.Windows.Forms.Button button1;
    }
}

