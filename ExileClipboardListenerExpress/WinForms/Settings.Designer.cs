namespace ExileClipboardListener.WinForms
{
    partial class Settings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CollectionMode = new System.Windows.Forms.RadioButton();
            this.StashMode = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StashPopUpSeconds = new System.Windows.Forms.NumericUpDown();
            this.StashPopUpPerm = new System.Windows.Forms.RadioButton();
            this.StashPopUpTimed = new System.Windows.Forms.RadioButton();
            this.StashNoPopUp = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DefaultTab = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CompareLevel = new System.Windows.Forms.RadioButton();
            this.CompareBest = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.DuplicatesNo = new System.Windows.Forms.RadioButton();
            this.DuplicatesYes = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ToleranceGoodTo = new System.Windows.Forms.NumericUpDown();
            this.ToleranceAverageTo = new System.Windows.Forms.NumericUpDown();
            this.TolerancePoorTo = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ToleranceGoodFrom = new System.Windows.Forms.NumericUpDown();
            this.ToleranceAverageFrom = new System.Windows.Forms.NumericUpDown();
            this.TolerancePoorFrom = new System.Windows.Forms.NumericUpDown();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.CollectionPopUpSeconds = new System.Windows.Forms.NumericUpDown();
            this.CollectionPopUpPerm = new System.Windows.Forms.RadioButton();
            this.CollectionPopUpTimed = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StashPopUpSeconds)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceGoodTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceAverageTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TolerancePoorTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceGoodFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceAverageFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TolerancePoorFrom)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionPopUpSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CollectionMode);
            this.groupBox1.Controls.Add(this.StashMode);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode on Startup";
            // 
            // CollectionMode
            // 
            this.CollectionMode.AutoSize = true;
            this.CollectionMode.Location = new System.Drawing.Point(6, 42);
            this.CollectionMode.Name = "CollectionMode";
            this.CollectionMode.Size = new System.Drawing.Size(101, 17);
            this.CollectionMode.TabIndex = 1;
            this.CollectionMode.TabStop = true;
            this.CollectionMode.Text = "Collection Mode";
            this.CollectionMode.UseVisualStyleBackColor = true;
            // 
            // StashMode
            // 
            this.StashMode.AutoSize = true;
            this.StashMode.Location = new System.Drawing.Point(6, 19);
            this.StashMode.Name = "StashMode";
            this.StashMode.Size = new System.Drawing.Size(82, 17);
            this.StashMode.TabIndex = 0;
            this.StashMode.TabStop = true;
            this.StashMode.Text = "Stash Mode";
            this.StashMode.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.StashPopUpSeconds);
            this.groupBox2.Controls.Add(this.StashPopUpPerm);
            this.groupBox2.Controls.Add(this.StashPopUpTimed);
            this.groupBox2.Controls.Add(this.StashNoPopUp);
            this.groupBox2.Location = new System.Drawing.Point(12, 90);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(209, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pop Up Behaviour - Stash Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "seconds";
            // 
            // StashPopUpSeconds
            // 
            this.StashPopUpSeconds.Location = new System.Drawing.Point(96, 40);
            this.StashPopUpSeconds.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.StashPopUpSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StashPopUpSeconds.Name = "StashPopUpSeconds";
            this.StashPopUpSeconds.Size = new System.Drawing.Size(34, 20);
            this.StashPopUpSeconds.TabIndex = 4;
            this.StashPopUpSeconds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // StashPopUpPerm
            // 
            this.StashPopUpPerm.AutoSize = true;
            this.StashPopUpPerm.Location = new System.Drawing.Point(6, 65);
            this.StashPopUpPerm.Name = "StashPopUpPerm";
            this.StashPopUpPerm.Size = new System.Drawing.Size(140, 17);
            this.StashPopUpPerm.TabIndex = 2;
            this.StashPopUpPerm.TabStop = true;
            this.StashPopUpPerm.Text = "Pops Up Until Dismissed";
            this.StashPopUpPerm.UseVisualStyleBackColor = true;
            // 
            // StashPopUpTimed
            // 
            this.StashPopUpTimed.AutoSize = true;
            this.StashPopUpTimed.Location = new System.Drawing.Point(6, 42);
            this.StashPopUpTimed.Name = "StashPopUpTimed";
            this.StashPopUpTimed.Size = new System.Drawing.Size(84, 17);
            this.StashPopUpTimed.TabIndex = 1;
            this.StashPopUpTimed.TabStop = true;
            this.StashPopUpTimed.Text = "Pops Up For";
            this.StashPopUpTimed.UseVisualStyleBackColor = true;
            // 
            // StashNoPopUp
            // 
            this.StashNoPopUp.AutoSize = true;
            this.StashNoPopUp.Location = new System.Drawing.Point(6, 19);
            this.StashNoPopUp.Name = "StashNoPopUp";
            this.StashNoPopUp.Size = new System.Drawing.Size(78, 17);
            this.StashNoPopUp.TabIndex = 0;
            this.StashNoPopUp.TabStop = true;
            this.StashNoPopUp.Text = "No Pop Up";
            this.StashNoPopUp.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DefaultTab);
            this.groupBox3.Location = new System.Drawing.Point(227, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(209, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Default Tab";
            // 
            // DefaultTab
            // 
            this.DefaultTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefaultTab.FormattingEnabled = true;
            this.DefaultTab.Location = new System.Drawing.Point(12, 19);
            this.DefaultTab.Name = "DefaultTab";
            this.DefaultTab.Size = new System.Drawing.Size(121, 21);
            this.DefaultTab.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CompareLevel);
            this.groupBox4.Controls.Add(this.CompareBest);
            this.groupBox4.Location = new System.Drawing.Point(12, 267);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(209, 72);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Collection Mode Rated Against";
            // 
            // CompareLevel
            // 
            this.CompareLevel.AutoSize = true;
            this.CompareLevel.Location = new System.Drawing.Point(6, 42);
            this.CompareLevel.Name = "CompareLevel";
            this.CompareLevel.Size = new System.Drawing.Size(180, 17);
            this.CompareLevel.TabIndex = 1;
            this.CompareLevel.TabStop = true;
            this.CompareLevel.Text = "Best Possible Statistics For Level";
            this.CompareLevel.UseVisualStyleBackColor = true;
            // 
            // CompareBest
            // 
            this.CompareBest.AutoSize = true;
            this.CompareBest.Location = new System.Drawing.Point(6, 19);
            this.CompareBest.Name = "CompareBest";
            this.CompareBest.Size = new System.Drawing.Size(133, 17);
            this.CompareBest.TabIndex = 0;
            this.CompareBest.TabStop = true;
            this.CompareBest.Text = "Best Possible Statistics";
            this.CompareBest.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.DuplicatesNo);
            this.groupBox5.Controls.Add(this.DuplicatesYes);
            this.groupBox5.Location = new System.Drawing.Point(227, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(209, 81);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Stash Allows Duplicates";
            // 
            // DuplicatesNo
            // 
            this.DuplicatesNo.AutoSize = true;
            this.DuplicatesNo.Location = new System.Drawing.Point(6, 42);
            this.DuplicatesNo.Name = "DuplicatesNo";
            this.DuplicatesNo.Size = new System.Drawing.Size(39, 17);
            this.DuplicatesNo.TabIndex = 2;
            this.DuplicatesNo.TabStop = true;
            this.DuplicatesNo.Text = "No";
            this.DuplicatesNo.UseVisualStyleBackColor = true;
            // 
            // DuplicatesYes
            // 
            this.DuplicatesYes.AutoSize = true;
            this.DuplicatesYes.Location = new System.Drawing.Point(6, 19);
            this.DuplicatesYes.Name = "DuplicatesYes";
            this.DuplicatesYes.Size = new System.Drawing.Size(43, 17);
            this.DuplicatesYes.TabIndex = 1;
            this.DuplicatesYes.TabStop = true;
            this.DuplicatesYes.Text = "Yes";
            this.DuplicatesYes.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(358, 405);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Save_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(277, 405);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.ToleranceGoodTo);
            this.groupBox6.Controls.Add(this.ToleranceAverageTo);
            this.groupBox6.Controls.Add(this.TolerancePoorTo);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.ToleranceGoodFrom);
            this.groupBox6.Controls.Add(this.ToleranceAverageFrom);
            this.groupBox6.Controls.Add(this.TolerancePoorFrom);
            this.groupBox6.Location = new System.Drawing.Point(227, 99);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(209, 100);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Tolerances";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(190, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(190, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(190, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "%";
            // 
            // ToleranceGoodTo
            // 
            this.ToleranceGoodTo.Enabled = false;
            this.ToleranceGoodTo.Location = new System.Drawing.Point(138, 71);
            this.ToleranceGoodTo.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ToleranceGoodTo.Name = "ToleranceGoodTo";
            this.ToleranceGoodTo.Size = new System.Drawing.Size(46, 20);
            this.ToleranceGoodTo.TabIndex = 11;
            this.ToleranceGoodTo.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // ToleranceAverageTo
            // 
            this.ToleranceAverageTo.Location = new System.Drawing.Point(138, 45);
            this.ToleranceAverageTo.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.ToleranceAverageTo.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ToleranceAverageTo.Name = "ToleranceAverageTo";
            this.ToleranceAverageTo.Size = new System.Drawing.Size(46, 20);
            this.ToleranceAverageTo.TabIndex = 10;
            this.ToleranceAverageTo.Value = new decimal(new int[] {
            74,
            0,
            0,
            0});
            this.ToleranceAverageTo.ValueChanged += new System.EventHandler(this.ToleranceAverageTo_ValueChanged);
            // 
            // TolerancePoorTo
            // 
            this.TolerancePoorTo.Location = new System.Drawing.Point(138, 19);
            this.TolerancePoorTo.Maximum = new decimal(new int[] {
            98,
            0,
            0,
            0});
            this.TolerancePoorTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TolerancePoorTo.Name = "TolerancePoorTo";
            this.TolerancePoorTo.Size = new System.Drawing.Size(46, 20);
            this.TolerancePoorTo.TabIndex = 9;
            this.TolerancePoorTo.Value = new decimal(new int[] {
            39,
            0,
            0,
            0});
            this.TolerancePoorTo.ValueChanged += new System.EventHandler(this.TolerancePoorTo_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Good";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Average";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "to";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(117, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "to";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Poor";
            // 
            // ToleranceGoodFrom
            // 
            this.ToleranceGoodFrom.Location = new System.Drawing.Point(65, 71);
            this.ToleranceGoodFrom.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.ToleranceGoodFrom.Name = "ToleranceGoodFrom";
            this.ToleranceGoodFrom.Size = new System.Drawing.Size(46, 20);
            this.ToleranceGoodFrom.TabIndex = 2;
            this.ToleranceGoodFrom.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.ToleranceGoodFrom.ValueChanged += new System.EventHandler(this.ToleranceGoodFrom_ValueChanged);
            // 
            // ToleranceAverageFrom
            // 
            this.ToleranceAverageFrom.Location = new System.Drawing.Point(65, 45);
            this.ToleranceAverageFrom.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.ToleranceAverageFrom.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.ToleranceAverageFrom.Name = "ToleranceAverageFrom";
            this.ToleranceAverageFrom.Size = new System.Drawing.Size(46, 20);
            this.ToleranceAverageFrom.TabIndex = 1;
            this.ToleranceAverageFrom.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.ToleranceAverageFrom.ValueChanged += new System.EventHandler(this.ToleranceAverageFrom_ValueChanged);
            // 
            // TolerancePoorFrom
            // 
            this.TolerancePoorFrom.Enabled = false;
            this.TolerancePoorFrom.Location = new System.Drawing.Point(65, 19);
            this.TolerancePoorFrom.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.TolerancePoorFrom.Name = "TolerancePoorFrom";
            this.TolerancePoorFrom.Size = new System.Drawing.Size(46, 20);
            this.TolerancePoorFrom.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.CollectionPopUpSeconds);
            this.groupBox7.Controls.Add(this.CollectionPopUpPerm);
            this.groupBox7.Controls.Add(this.CollectionPopUpTimed);
            this.groupBox7.Location = new System.Drawing.Point(12, 196);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(209, 65);
            this.groupBox7.TabIndex = 8;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Pop Up Behaviour - Collection Mode";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(136, 21);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "seconds";
            // 
            // CollectionPopUpSeconds
            // 
            this.CollectionPopUpSeconds.Location = new System.Drawing.Point(96, 17);
            this.CollectionPopUpSeconds.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.CollectionPopUpSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CollectionPopUpSeconds.Name = "CollectionPopUpSeconds";
            this.CollectionPopUpSeconds.Size = new System.Drawing.Size(34, 20);
            this.CollectionPopUpSeconds.TabIndex = 4;
            this.CollectionPopUpSeconds.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CollectionPopUpPerm
            // 
            this.CollectionPopUpPerm.AutoSize = true;
            this.CollectionPopUpPerm.Location = new System.Drawing.Point(6, 42);
            this.CollectionPopUpPerm.Name = "CollectionPopUpPerm";
            this.CollectionPopUpPerm.Size = new System.Drawing.Size(140, 17);
            this.CollectionPopUpPerm.TabIndex = 2;
            this.CollectionPopUpPerm.TabStop = true;
            this.CollectionPopUpPerm.Text = "Pops Up Until Dismissed";
            this.CollectionPopUpPerm.UseVisualStyleBackColor = true;
            // 
            // CollectionPopUpTimed
            // 
            this.CollectionPopUpTimed.AutoSize = true;
            this.CollectionPopUpTimed.Location = new System.Drawing.Point(6, 19);
            this.CollectionPopUpTimed.Name = "CollectionPopUpTimed";
            this.CollectionPopUpTimed.Size = new System.Drawing.Size(84, 17);
            this.CollectionPopUpTimed.TabIndex = 1;
            this.CollectionPopUpTimed.TabStop = true;
            this.CollectionPopUpTimed.Text = "Pops Up For";
            this.CollectionPopUpTimed.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 440);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StashPopUpSeconds)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceGoodTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceAverageTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TolerancePoorTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceGoodFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ToleranceAverageFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TolerancePoorFrom)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionPopUpSeconds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CollectionMode;
        private System.Windows.Forms.RadioButton StashMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown StashPopUpSeconds;
        private System.Windows.Forms.RadioButton StashPopUpPerm;
        private System.Windows.Forms.RadioButton StashPopUpTimed;
        private System.Windows.Forms.RadioButton StashNoPopUp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton CompareLevel;
        private System.Windows.Forms.RadioButton CompareBest;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton DuplicatesNo;
        private System.Windows.Forms.RadioButton DuplicatesYes;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ToleranceGoodTo;
        private System.Windows.Forms.NumericUpDown ToleranceAverageTo;
        private System.Windows.Forms.NumericUpDown TolerancePoorTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ToleranceGoodFrom;
        private System.Windows.Forms.NumericUpDown ToleranceAverageFrom;
        private System.Windows.Forms.NumericUpDown TolerancePoorFrom;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown CollectionPopUpSeconds;
        private System.Windows.Forms.RadioButton CollectionPopUpPerm;
        private System.Windows.Forms.RadioButton CollectionPopUpTimed;
        private System.Windows.Forms.ComboBox DefaultTab;
    }
}