namespace CreateParamsSQL
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSection = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonTypeSystem = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonComplexOITU = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBoxDescribe = new System.Windows.Forms.RichTextBox();
            this.labelDescribeSymbols = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelDescribeShortSymbols = new System.Windows.Forms.Label();
            this.richTextBoxDescribeShort = new System.Windows.Forms.RichTextBox();
            this.checkBoxOldNewLines = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxOldNewLines);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.textBoxValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxSection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 341);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(76, 120);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(92, 20);
            this.textBoxValue.TabIndex = 10;
            this.textBoxValue.Text = "YES";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Значение";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(76, 94);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(92, 20);
            this.textBoxName.TabIndex = 8;
            this.textBoxName.Text = "DBU817";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Параметр";
            // 
            // textBoxSection
            // 
            this.textBoxSection.Location = new System.Drawing.Point(76, 68);
            this.textBoxSection.Name = "textBoxSection";
            this.textBoxSection.Size = new System.Drawing.Size(92, 20);
            this.textBoxSection.TabIndex = 6;
            this.textBoxSection.Text = "set";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Секция";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 306);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(385, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonTypeSystem);
            this.groupBox3.Location = new System.Drawing.Point(102, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(92, 46);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Тип";
            // 
            // radioButtonTypeSystem
            // 
            this.radioButtonTypeSystem.AutoSize = true;
            this.radioButtonTypeSystem.Checked = true;
            this.radioButtonTypeSystem.Location = new System.Drawing.Point(6, 19);
            this.radioButtonTypeSystem.Name = "radioButtonTypeSystem";
            this.radioButtonTypeSystem.Size = new System.Drawing.Size(83, 17);
            this.radioButtonTypeSystem.TabIndex = 1;
            this.radioButtonTypeSystem.TabStop = true;
            this.radioButtonTypeSystem.Text = "Системный";
            this.radioButtonTypeSystem.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonComplexOITU);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(81, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Комплекс";
            // 
            // radioButtonComplexOITU
            // 
            this.radioButtonComplexOITU.AutoSize = true;
            this.radioButtonComplexOITU.Checked = true;
            this.radioButtonComplexOITU.Location = new System.Drawing.Point(6, 19);
            this.radioButtonComplexOITU.Name = "radioButtonComplexOITU";
            this.radioButtonComplexOITU.Size = new System.Drawing.Size(56, 17);
            this.radioButtonComplexOITU.TabIndex = 1;
            this.radioButtonComplexOITU.TabStop = true;
            this.radioButtonComplexOITU.Text = "ОИТУ";
            this.radioButtonComplexOITU.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Краткое";
            // 
            // richTextBoxDescribe
            // 
            this.richTextBoxDescribe.Location = new System.Drawing.Point(73, 67);
            this.richTextBoxDescribe.MaxLength = 240;
            this.richTextBoxDescribe.Name = "richTextBoxDescribe";
            this.richTextBoxDescribe.Size = new System.Drawing.Size(306, 79);
            this.richTextBoxDescribe.TabIndex = 12;
            this.richTextBoxDescribe.Text = resources.GetString("richTextBoxDescribe.Text");
            this.richTextBoxDescribe.TextChanged += new System.EventHandler(this.richTextBoxDescribe_TextChanged);
            // 
            // labelDescribeSymbols
            // 
            this.labelDescribeSymbols.AutoSize = true;
            this.labelDescribeSymbols.Location = new System.Drawing.Point(24, 133);
            this.labelDescribeSymbols.Name = "labelDescribeSymbols";
            this.labelDescribeSymbols.Size = new System.Drawing.Size(13, 13);
            this.labelDescribeSymbols.TabIndex = 13;
            this.labelDescribeSymbols.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Полное";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.richTextBoxDescribeShort);
            this.groupBox4.Controls.Add(this.labelDescribeShortSymbols);
            this.groupBox4.Controls.Add(this.richTextBoxDescribe);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.labelDescribeSymbols);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(6, 146);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 154);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Описание";
            // 
            // labelDescribeShortSymbols
            // 
            this.labelDescribeShortSymbols.AutoSize = true;
            this.labelDescribeShortSymbols.Location = new System.Drawing.Point(24, 48);
            this.labelDescribeShortSymbols.Name = "labelDescribeShortSymbols";
            this.labelDescribeShortSymbols.Size = new System.Drawing.Size(13, 13);
            this.labelDescribeShortSymbols.TabIndex = 15;
            this.labelDescribeShortSymbols.Text = "0";
            // 
            // richTextBoxDescribeShort
            // 
            this.richTextBoxDescribeShort.Location = new System.Drawing.Point(73, 19);
            this.richTextBoxDescribeShort.MaxLength = 55;
            this.richTextBoxDescribeShort.Name = "richTextBoxDescribeShort";
            this.richTextBoxDescribeShort.Size = new System.Drawing.Size(306, 42);
            this.richTextBoxDescribeShort.TabIndex = 16;
            this.richTextBoxDescribeShort.Text = "ПП, провед. в корреспонденции с БС 30719/30720 в ДБУиО";
            this.richTextBoxDescribeShort.TextChanged += new System.EventHandler(this.richTextBoxDescribeShort_TextChanged);
            // 
            // checkBoxOldNewLines
            // 
            this.checkBoxOldNewLines.AutoSize = true;
            this.checkBoxOldNewLines.Location = new System.Drawing.Point(272, 19);
            this.checkBoxOldNewLines.Name = "checkBoxOldNewLines";
            this.checkBoxOldNewLines.Size = new System.Drawing.Size(119, 17);
            this.checkBoxOldNewLines.TabIndex = 11;
            this.checkBoxOldNewLines.Text = "Старое поведение";
            this.checkBoxOldNewLines.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 358);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonComplexOITU;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonTypeSystem;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelDescribeSymbols;
        private System.Windows.Forms.RichTextBox richTextBoxDescribe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox richTextBoxDescribeShort;
        private System.Windows.Forms.Label labelDescribeShortSymbols;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxOldNewLines;
    }
}

