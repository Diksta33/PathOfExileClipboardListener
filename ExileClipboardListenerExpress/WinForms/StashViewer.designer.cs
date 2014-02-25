namespace ExileClipboardListener.WinForms
{
    partial class StashViewer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StashViewer));
            this.StashGrid = new System.Windows.Forms.DataGridView();
            this.ItemType = new System.Windows.Forms.ComboBox();
            this.ItemSubType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Mod = new System.Windows.Forms.ComboBox();
            this.Export = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.League = new System.Windows.Forms.ComboBox();
            this.FilterList = new System.Windows.Forms.ComboBox();
            this.ItemCount = new System.Windows.Forms.Label();
            this.MinItemLevel = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ViewItem = new System.Windows.Forms.Button();
            this.Filter = new System.Windows.Forms.Button();
            this.MaxItemLevel = new System.Windows.Forms.NumericUpDown();
            this.HideZeroScores = new System.Windows.Forms.CheckBox();
            this.DeleteItem = new System.Windows.Forms.Button();
            this.CompactView = new System.Windows.Forms.CheckBox();
            this.MaxReqLevel = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.MinReqLevel = new System.Windows.Forms.NumericUpDown();
            this.ViewScript = new System.Windows.Forms.Button();
            this.ReparseStash = new System.Windows.Forms.Button();
            this.ReparseLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ItemCategory = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.StashGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinItemLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxItemLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxReqLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinReqLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // StashGrid
            // 
            this.StashGrid.AllowUserToAddRows = false;
            this.StashGrid.AllowUserToDeleteRows = false;
            this.StashGrid.AllowUserToOrderColumns = true;
            this.StashGrid.AllowUserToResizeColumns = false;
            this.StashGrid.AllowUserToResizeRows = false;
            this.StashGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.StashGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.StashGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.StashGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.StashGrid.Location = new System.Drawing.Point(12, 52);
            this.StashGrid.MultiSelect = false;
            this.StashGrid.Name = "StashGrid";
            this.StashGrid.ReadOnly = true;
            this.StashGrid.RowHeadersVisible = false;
            this.StashGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.StashGrid.Size = new System.Drawing.Size(1570, 543);
            this.StashGrid.TabIndex = 0;
            // 
            // ItemType
            // 
            this.ItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemType.FormattingEnabled = true;
            this.ItemType.Items.AddRange(new object[] {
            "(All)",
            "Armour",
            "Jewellery",
            "Weapons"});
            this.ItemType.Location = new System.Drawing.Point(216, 25);
            this.ItemType.Name = "ItemType";
            this.ItemType.Size = new System.Drawing.Size(125, 21);
            this.ItemType.Sorted = true;
            this.ItemType.TabIndex = 1;
            this.ItemType.SelectedIndexChanged += new System.EventHandler(this.ItemType_SelectedIndexChanged);
            // 
            // ItemSubType
            // 
            this.ItemSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemSubType.FormattingEnabled = true;
            this.ItemSubType.Location = new System.Drawing.Point(518, 25);
            this.ItemSubType.Name = "ItemSubType";
            this.ItemSubType.Size = new System.Drawing.Size(131, 21);
            this.ItemSubType.Sorted = true;
            this.ItemSubType.TabIndex = 2;
            this.ItemSubType.SelectedIndexChanged += new System.EventHandler(this.ItemSubType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Item Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(515, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Item Sub Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(652, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Mod";
            // 
            // Mod
            // 
            this.Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Mod.FormattingEnabled = true;
            this.Mod.Location = new System.Drawing.Point(655, 25);
            this.Mod.Name = "Mod";
            this.Mod.Size = new System.Drawing.Size(287, 21);
            this.Mod.Sorted = true;
            this.Mod.TabIndex = 3;
            this.Mod.SelectedIndexChanged += new System.EventHandler(this.Mod_SelectedIndexChanged);
            // 
            // Export
            // 
            this.Export.Location = new System.Drawing.Point(12, 601);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(96, 23);
            this.Export.TabIndex = 4;
            this.Export.Text = "Export Data";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "stash.csv";
            this.saveFileDialog1.Filter = "CSV File|*.csv";
            this.saveFileDialog1.Title = "Select a file to save yout stash to";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "League";
            // 
            // League
            // 
            this.League.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.League.FormattingEnabled = true;
            this.League.Items.AddRange(new object[] {
            "(All)",
            "Armour",
            "Jewellery",
            "Weapons"});
            this.League.Location = new System.Drawing.Point(12, 25);
            this.League.Name = "League";
            this.League.Size = new System.Drawing.Size(198, 21);
            this.League.Sorted = true;
            this.League.TabIndex = 0;
            this.League.SelectedIndexChanged += new System.EventHandler(this.League_SelectedIndexChanged);
            // 
            // FilterList
            // 
            this.FilterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterList.FormattingEnabled = true;
            this.FilterList.Location = new System.Drawing.Point(948, 25);
            this.FilterList.Name = "FilterList";
            this.FilterList.Size = new System.Drawing.Size(287, 21);
            this.FilterList.Sorted = true;
            this.FilterList.TabIndex = 10;
            this.FilterList.SelectedIndexChanged += new System.EventHandler(this.FilterList_SelectedIndexChanged);
            // 
            // ItemCount
            // 
            this.ItemCount.AutoSize = true;
            this.ItemCount.Location = new System.Drawing.Point(1524, 606);
            this.ItemCount.Name = "ItemCount";
            this.ItemCount.Size = new System.Drawing.Size(31, 13);
            this.ItemCount.TabIndex = 12;
            this.ItemCount.Text = "items";
            // 
            // MinItemLevel
            // 
            this.MinItemLevel.Location = new System.Drawing.Point(1295, 614);
            this.MinItemLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinItemLevel.Name = "MinItemLevel";
            this.MinItemLevel.Size = new System.Drawing.Size(47, 20);
            this.MinItemLevel.TabIndex = 13;
            this.MinItemLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinItemLevel.ValueChanged += new System.EventHandler(this.MinItemLevel_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(945, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Filter";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1292, 598);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Min  Item Level  Max";
            // 
            // ViewItem
            // 
            this.ViewItem.Location = new System.Drawing.Point(114, 601);
            this.ViewItem.Name = "ViewItem";
            this.ViewItem.Size = new System.Drawing.Size(96, 23);
            this.ViewItem.TabIndex = 15;
            this.ViewItem.Text = "View Item";
            this.ViewItem.UseVisualStyleBackColor = true;
            this.ViewItem.Click += new System.EventHandler(this.ViewItem_Click);
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(1507, 24);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(75, 23);
            this.Filter.TabIndex = 16;
            this.Filter.Text = "Filter";
            this.Filter.UseVisualStyleBackColor = true;
            this.Filter.Click += new System.EventHandler(this.Filter_Click);
            // 
            // MaxItemLevel
            // 
            this.MaxItemLevel.Location = new System.Drawing.Point(1348, 614);
            this.MaxItemLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxItemLevel.Name = "MaxItemLevel";
            this.MaxItemLevel.Size = new System.Drawing.Size(47, 20);
            this.MaxItemLevel.TabIndex = 17;
            this.MaxItemLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxItemLevel.ValueChanged += new System.EventHandler(this.MaxItemLevel_ValueChanged);
            // 
            // HideZeroScores
            // 
            this.HideZeroScores.AutoSize = true;
            this.HideZeroScores.Location = new System.Drawing.Point(1241, 28);
            this.HideZeroScores.Name = "HideZeroScores";
            this.HideZeroScores.Size = new System.Drawing.Size(109, 17);
            this.HideZeroScores.TabIndex = 18;
            this.HideZeroScores.Text = "Hide Zero Scores";
            this.HideZeroScores.UseVisualStyleBackColor = true;
            // 
            // DeleteItem
            // 
            this.DeleteItem.Location = new System.Drawing.Point(564, 601);
            this.DeleteItem.Name = "DeleteItem";
            this.DeleteItem.Size = new System.Drawing.Size(96, 23);
            this.DeleteItem.TabIndex = 19;
            this.DeleteItem.Text = "Delete Item";
            this.DeleteItem.UseVisualStyleBackColor = true;
            this.DeleteItem.Click += new System.EventHandler(this.DeleteItem_Click);
            // 
            // CompactView
            // 
            this.CompactView.AutoSize = true;
            this.CompactView.Checked = true;
            this.CompactView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CompactView.Location = new System.Drawing.Point(1407, 28);
            this.CompactView.Name = "CompactView";
            this.CompactView.Size = new System.Drawing.Size(94, 17);
            this.CompactView.TabIndex = 20;
            this.CompactView.Text = "Compact View";
            this.CompactView.UseVisualStyleBackColor = true;
            // 
            // MaxReqLevel
            // 
            this.MaxReqLevel.Location = new System.Drawing.Point(1195, 614);
            this.MaxReqLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxReqLevel.Name = "MaxReqLevel";
            this.MaxReqLevel.Size = new System.Drawing.Size(47, 20);
            this.MaxReqLevel.TabIndex = 23;
            this.MaxReqLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1139, 598);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Min  Req Level  Max";
            // 
            // MinReqLevel
            // 
            this.MinReqLevel.Location = new System.Drawing.Point(1142, 614);
            this.MinReqLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinReqLevel.Name = "MinReqLevel";
            this.MinReqLevel.Size = new System.Drawing.Size(47, 20);
            this.MinReqLevel.TabIndex = 21;
            this.MinReqLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ViewScript
            // 
            this.ViewScript.Location = new System.Drawing.Point(216, 601);
            this.ViewScript.Name = "ViewScript";
            this.ViewScript.Size = new System.Drawing.Size(96, 23);
            this.ViewScript.TabIndex = 24;
            this.ViewScript.Text = "View Script";
            this.ViewScript.UseVisualStyleBackColor = true;
            this.ViewScript.Click += new System.EventHandler(this.ViewScriptClick);
            // 
            // ReparseStash
            // 
            this.ReparseStash.Location = new System.Drawing.Point(666, 600);
            this.ReparseStash.Name = "ReparseStash";
            this.ReparseStash.Size = new System.Drawing.Size(96, 23);
            this.ReparseStash.TabIndex = 25;
            this.ReparseStash.Text = "Reparse Stash";
            this.ReparseStash.UseVisualStyleBackColor = true;
            this.ReparseStash.Click += new System.EventHandler(this.ReparseStash_Click);
            // 
            // ReparseLabel
            // 
            this.ReparseLabel.AutoSize = true;
            this.ReparseLabel.Location = new System.Drawing.Point(768, 605);
            this.ReparseLabel.Name = "ReparseLabel";
            this.ReparseLabel.Size = new System.Drawing.Size(38, 13);
            this.ReparseLabel.TabIndex = 26;
            this.ReparseLabel.Text = "Ready";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(344, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Item Category";
            // 
            // ItemCategory
            // 
            this.ItemCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemCategory.Enabled = false;
            this.ItemCategory.FormattingEnabled = true;
            this.ItemCategory.Items.AddRange(new object[] {
            "(All)",
            "Armour",
            "Jewellery",
            "Weapons"});
            this.ItemCategory.Location = new System.Drawing.Point(347, 23);
            this.ItemCategory.Name = "ItemCategory";
            this.ItemCategory.Size = new System.Drawing.Size(165, 21);
            this.ItemCategory.Sorted = true;
            this.ItemCategory.TabIndex = 27;
            // 
            // StashViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1594, 636);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ItemCategory);
            this.Controls.Add(this.ReparseLabel);
            this.Controls.Add(this.ReparseStash);
            this.Controls.Add(this.ViewScript);
            this.Controls.Add(this.MaxReqLevel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MinReqLevel);
            this.Controls.Add(this.CompactView);
            this.Controls.Add(this.DeleteItem);
            this.Controls.Add(this.HideZeroScores);
            this.Controls.Add(this.MaxItemLevel);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.ViewItem);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.MinItemLevel);
            this.Controls.Add(this.ItemCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FilterList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.League);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Mod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ItemSubType);
            this.Controls.Add(this.ItemType);
            this.Controls.Add(this.StashGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StashViewer";
            this.Text = "Stash Viewer";
            this.Load += new System.EventHandler(this.Stash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.StashGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinItemLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxItemLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxReqLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinReqLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView StashGrid;
        private System.Windows.Forms.ComboBox ItemType;
        private System.Windows.Forms.ComboBox ItemSubType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Mod;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox League;
        private System.Windows.Forms.ComboBox FilterList;
        private System.Windows.Forms.Label ItemCount;
        private System.Windows.Forms.NumericUpDown MinItemLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ViewItem;
        private System.Windows.Forms.Button Filter;
        private System.Windows.Forms.NumericUpDown MaxItemLevel;
        private System.Windows.Forms.CheckBox HideZeroScores;
        private System.Windows.Forms.Button DeleteItem;
        private System.Windows.Forms.CheckBox CompactView;
        private System.Windows.Forms.NumericUpDown MaxReqLevel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown MinReqLevel;
        private System.Windows.Forms.Button ViewScript;
        private System.Windows.Forms.Button ReparseStash;
        private System.Windows.Forms.Label ReparseLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ItemCategory;
    }
}