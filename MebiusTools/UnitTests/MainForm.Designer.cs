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
            this.button1 = new System.Windows.Forms.Button();
            this.labelUserText = new System.Windows.Forms.Label();
            this.labelPathEtalonText = new System.Windows.Forms.Label();
            this.textBoxPathEtalon = new System.Windows.Forms.TextBox();
            this.comboBoxPacket = new System.Windows.Forms.ComboBox();
            this.labelPacketText = new System.Windows.Forms.Label();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(216, 170);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 277);
            this.Controls.Add(this.labelPacketText);
            this.Controls.Add(this.comboBoxPacket);
            this.Controls.Add(this.textBoxPathEtalon);
            this.Controls.Add(this.labelPathEtalonText);
            this.Controls.Add(this.labelUserText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxVersion);
            this.Controls.Add(this.textBoxUser);
            this.Controls.Add(this.labelVersionText);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVersionText;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.ComboBox comboBoxVersion;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelUserText;
        private System.Windows.Forms.Label labelPathEtalonText;
        private System.Windows.Forms.TextBox textBoxPathEtalon;
        private System.Windows.Forms.ComboBox comboBoxPacket;
        private System.Windows.Forms.Label labelPacketText;
    }
}

