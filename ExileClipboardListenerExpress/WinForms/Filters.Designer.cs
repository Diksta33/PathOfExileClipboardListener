namespace ExileClipboardListener.WinForms
{
    partial class Filters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Filters));
            this.FilterGrid = new System.Windows.Forms.DataGridView();
            this.Prefix1ModClass = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FilterName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Prefix1Mod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Prefix2Mod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Prefix2ModClass = new System.Windows.Forms.ComboBox();
            this.Prefix3Mod = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Prefix3ModClass = new System.Windows.Forms.ComboBox();
            this.Suffix1Mod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Suffix1ModClass = new System.Windows.Forms.ComboBox();
            this.Suffix2Mod = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Suffix2ModClass = new System.Windows.Forms.ComboBox();
            this.Suffix3Mod = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Suffix3ModClass = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ItemType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ItemSubType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EditFilter = new System.Windows.Forms.Button();
            this.NewFilter = new System.Windows.Forms.Button();
            this.CancelFilter = new System.Windows.Forms.Button();
            this.DeleteFilter = new System.Windows.Forms.Button();
            this.SaveFilter = new System.Windows.Forms.Button();
            this.Exit = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.ItemCategory = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.FilterGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FilterGrid
            // 
            this.FilterGrid.AllowUserToAddRows = false;
            this.FilterGrid.AllowUserToDeleteRows = false;
            this.FilterGrid.AllowUserToResizeRows = false;
            this.FilterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FilterGrid.Location = new System.Drawing.Point(12, 12);
            this.FilterGrid.Name = "FilterGrid";
            this.FilterGrid.ReadOnly = true;
            this.FilterGrid.RowHeadersVisible = false;
            this.FilterGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FilterGrid.Size = new System.Drawing.Size(548, 329);
            this.FilterGrid.TabIndex = 2;
            this.FilterGrid.SelectionChanged += new System.EventHandler(this.FilterGrid_SelectionChanged);
            // 
            // Prefix1ModClass
            // 
            this.Prefix1ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix1ModClass.FormattingEnabled = true;
            this.Prefix1ModClass.Location = new System.Drawing.Point(90, 32);
            this.Prefix1ModClass.Name = "Prefix1ModClass";
            this.Prefix1ModClass.Size = new System.Drawing.Size(111, 21);
            this.Prefix1ModClass.TabIndex = 3;
            this.Prefix1ModClass.SelectedIndexChanged += new System.EventHandler(this.Prefix1ModClass_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(566, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Filter Name";
            // 
            // FilterName
            // 
            this.FilterName.Location = new System.Drawing.Point(646, 24);
            this.FilterName.MaxLength = 100;
            this.FilterName.Name = "FilterName";
            this.FilterName.Size = new System.Drawing.Size(400, 20);
            this.FilterName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Prefix 1";
            // 
            // Prefix1Mod
            // 
            this.Prefix1Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix1Mod.FormattingEnabled = true;
            this.Prefix1Mod.Location = new System.Drawing.Point(207, 32);
            this.Prefix1Mod.Name = "Prefix1Mod";
            this.Prefix1Mod.Size = new System.Drawing.Size(264, 21);
            this.Prefix1Mod.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Mod Class";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(204, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mod";
            // 
            // Prefix2Mod
            // 
            this.Prefix2Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix2Mod.FormattingEnabled = true;
            this.Prefix2Mod.Location = new System.Drawing.Point(207, 59);
            this.Prefix2Mod.Name = "Prefix2Mod";
            this.Prefix2Mod.Size = new System.Drawing.Size(264, 21);
            this.Prefix2Mod.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Prefix 2";
            // 
            // Prefix2ModClass
            // 
            this.Prefix2ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix2ModClass.FormattingEnabled = true;
            this.Prefix2ModClass.Location = new System.Drawing.Point(90, 59);
            this.Prefix2ModClass.Name = "Prefix2ModClass";
            this.Prefix2ModClass.Size = new System.Drawing.Size(111, 21);
            this.Prefix2ModClass.TabIndex = 10;
            this.Prefix2ModClass.SelectedIndexChanged += new System.EventHandler(this.Prefix2ModClass_SelectedIndexChanged);
            // 
            // Prefix3Mod
            // 
            this.Prefix3Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix3Mod.FormattingEnabled = true;
            this.Prefix3Mod.Location = new System.Drawing.Point(207, 86);
            this.Prefix3Mod.Name = "Prefix3Mod";
            this.Prefix3Mod.Size = new System.Drawing.Size(264, 21);
            this.Prefix3Mod.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Prefix 3";
            // 
            // Prefix3ModClass
            // 
            this.Prefix3ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Prefix3ModClass.FormattingEnabled = true;
            this.Prefix3ModClass.Location = new System.Drawing.Point(90, 86);
            this.Prefix3ModClass.Name = "Prefix3ModClass";
            this.Prefix3ModClass.Size = new System.Drawing.Size(111, 21);
            this.Prefix3ModClass.TabIndex = 13;
            this.Prefix3ModClass.SelectedIndexChanged += new System.EventHandler(this.Prefix3ModClass_SelectedIndexChanged);
            // 
            // Suffix1Mod
            // 
            this.Suffix1Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix1Mod.FormattingEnabled = true;
            this.Suffix1Mod.Location = new System.Drawing.Point(207, 113);
            this.Suffix1Mod.Name = "Suffix1Mod";
            this.Suffix1Mod.Size = new System.Drawing.Size(264, 21);
            this.Suffix1Mod.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Suffix 1";
            // 
            // Suffix1ModClass
            // 
            this.Suffix1ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix1ModClass.FormattingEnabled = true;
            this.Suffix1ModClass.Location = new System.Drawing.Point(90, 113);
            this.Suffix1ModClass.Name = "Suffix1ModClass";
            this.Suffix1ModClass.Size = new System.Drawing.Size(111, 21);
            this.Suffix1ModClass.TabIndex = 16;
            this.Suffix1ModClass.SelectedIndexChanged += new System.EventHandler(this.Suffix1ModClass_SelectedIndexChanged);
            // 
            // Suffix2Mod
            // 
            this.Suffix2Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix2Mod.FormattingEnabled = true;
            this.Suffix2Mod.Location = new System.Drawing.Point(207, 140);
            this.Suffix2Mod.Name = "Suffix2Mod";
            this.Suffix2Mod.Size = new System.Drawing.Size(264, 21);
            this.Suffix2Mod.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Suffix 2";
            // 
            // Suffix2ModClass
            // 
            this.Suffix2ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix2ModClass.FormattingEnabled = true;
            this.Suffix2ModClass.Location = new System.Drawing.Point(90, 140);
            this.Suffix2ModClass.Name = "Suffix2ModClass";
            this.Suffix2ModClass.Size = new System.Drawing.Size(111, 21);
            this.Suffix2ModClass.TabIndex = 19;
            this.Suffix2ModClass.SelectedIndexChanged += new System.EventHandler(this.Suffix2ModClass_SelectedIndexChanged);
            // 
            // Suffix3Mod
            // 
            this.Suffix3Mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix3Mod.FormattingEnabled = true;
            this.Suffix3Mod.Location = new System.Drawing.Point(207, 167);
            this.Suffix3Mod.Name = "Suffix3Mod";
            this.Suffix3Mod.Size = new System.Drawing.Size(264, 21);
            this.Suffix3Mod.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Suffix 3";
            // 
            // Suffix3ModClass
            // 
            this.Suffix3ModClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Suffix3ModClass.FormattingEnabled = true;
            this.Suffix3ModClass.Location = new System.Drawing.Point(90, 167);
            this.Suffix3ModClass.Name = "Suffix3ModClass";
            this.Suffix3ModClass.Size = new System.Drawing.Size(111, 21);
            this.Suffix3ModClass.TabIndex = 22;
            this.Suffix3ModClass.SelectedIndexChanged += new System.EventHandler(this.Suffix3ModClass_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 53);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 26;
            this.label10.Text = "Item Class";
            // 
            // ItemType
            // 
            this.ItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemType.FormattingEnabled = true;
            this.ItemType.Location = new System.Drawing.Point(646, 50);
            this.ItemType.Name = "ItemType";
            this.ItemType.Size = new System.Drawing.Size(111, 21);
            this.ItemType.TabIndex = 25;
            this.ItemType.SelectedIndexChanged += new System.EventHandler(this.ItemType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(566, 80);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Item Type";
            // 
            // ItemSubType
            // 
            this.ItemSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemSubType.FormattingEnabled = true;
            this.ItemSubType.Location = new System.Drawing.Point(646, 77);
            this.ItemSubType.Name = "ItemSubType";
            this.ItemSubType.Size = new System.Drawing.Size(111, 21);
            this.ItemSubType.TabIndex = 27;
            this.ItemSubType.SelectedIndexChanged += new System.EventHandler(this.ItemSubType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Prefix1ModClass);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Prefix1Mod);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Suffix3Mod);
            this.groupBox1.Controls.Add(this.Prefix2ModClass);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Suffix3ModClass);
            this.groupBox1.Controls.Add(this.Prefix2Mod);
            this.groupBox1.Controls.Add(this.Suffix2Mod);
            this.groupBox1.Controls.Add(this.Prefix3ModClass);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Suffix2ModClass);
            this.groupBox1.Controls.Add(this.Prefix3Mod);
            this.groupBox1.Controls.Add(this.Suffix1Mod);
            this.groupBox1.Controls.Add(this.Suffix1ModClass);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(569, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 208);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mods";
            // 
            // EditFilter
            // 
            this.EditFilter.Location = new System.Drawing.Point(566, 318);
            this.EditFilter.Name = "EditFilter";
            this.EditFilter.Size = new System.Drawing.Size(75, 23);
            this.EditFilter.TabIndex = 30;
            this.EditFilter.Text = "Edit";
            this.EditFilter.UseVisualStyleBackColor = true;
            this.EditFilter.Click += new System.EventHandler(this.EditFilter_Click);
            // 
            // NewFilter
            // 
            this.NewFilter.Location = new System.Drawing.Point(647, 318);
            this.NewFilter.Name = "NewFilter";
            this.NewFilter.Size = new System.Drawing.Size(75, 23);
            this.NewFilter.TabIndex = 31;
            this.NewFilter.Text = "New";
            this.NewFilter.UseVisualStyleBackColor = true;
            this.NewFilter.Click += new System.EventHandler(this.NewFilter_Click);
            // 
            // CancelFilter
            // 
            this.CancelFilter.Location = new System.Drawing.Point(728, 318);
            this.CancelFilter.Name = "CancelFilter";
            this.CancelFilter.Size = new System.Drawing.Size(75, 23);
            this.CancelFilter.TabIndex = 32;
            this.CancelFilter.Text = "Cancel";
            this.CancelFilter.UseVisualStyleBackColor = true;
            this.CancelFilter.Click += new System.EventHandler(this.CancelFilter_Click);
            // 
            // DeleteFilter
            // 
            this.DeleteFilter.Location = new System.Drawing.Point(809, 318);
            this.DeleteFilter.Name = "DeleteFilter";
            this.DeleteFilter.Size = new System.Drawing.Size(75, 23);
            this.DeleteFilter.TabIndex = 33;
            this.DeleteFilter.Text = "Delete";
            this.DeleteFilter.UseVisualStyleBackColor = true;
            this.DeleteFilter.Click += new System.EventHandler(this.DeleteFilter_Click);
            // 
            // SaveFilter
            // 
            this.SaveFilter.Location = new System.Drawing.Point(890, 318);
            this.SaveFilter.Name = "SaveFilter";
            this.SaveFilter.Size = new System.Drawing.Size(75, 23);
            this.SaveFilter.TabIndex = 34;
            this.SaveFilter.Text = "Save";
            this.SaveFilter.UseVisualStyleBackColor = true;
            this.SaveFilter.Click += new System.EventHandler(this.SaveFilter_Click);
            // 
            // Exit
            // 
            this.Exit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Exit.Location = new System.Drawing.Point(971, 318);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 35;
            this.Exit.Text = "Exit";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(810, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "Item Category";
            // 
            // ItemCategory
            // 
            this.ItemCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemCategory.FormattingEnabled = true;
            this.ItemCategory.Location = new System.Drawing.Point(890, 50);
            this.ItemCategory.Name = "ItemCategory";
            this.ItemCategory.Size = new System.Drawing.Size(156, 21);
            this.ItemCategory.TabIndex = 36;
            this.ItemCategory.SelectedIndexChanged += new System.EventHandler(this.ItemCategory_SelectedIndexChanged);
            // 
            // Filters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 350);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ItemCategory);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.SaveFilter);
            this.Controls.Add(this.DeleteFilter);
            this.Controls.Add(this.CancelFilter);
            this.Controls.Add(this.NewFilter);
            this.Controls.Add(this.EditFilter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ItemSubType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ItemType);
            this.Controls.Add(this.FilterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilterGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Filters";
            this.Text = "Filter Management";
            this.Load += new System.EventHandler(this.Filters_Load);
            ((System.ComponentModel.ISupportInitialize)(this.FilterGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView FilterGrid;
        private System.Windows.Forms.ComboBox Prefix1ModClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FilterName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Prefix1Mod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Prefix2Mod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Prefix2ModClass;
        private System.Windows.Forms.ComboBox Prefix3Mod;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox Prefix3ModClass;
        private System.Windows.Forms.ComboBox Suffix1Mod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox Suffix1ModClass;
        private System.Windows.Forms.ComboBox Suffix2Mod;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox Suffix2ModClass;
        private System.Windows.Forms.ComboBox Suffix3Mod;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox Suffix3ModClass;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ItemType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ItemSubType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button EditFilter;
        private System.Windows.Forms.Button NewFilter;
        private System.Windows.Forms.Button CancelFilter;
        private System.Windows.Forms.Button DeleteFilter;
        private System.Windows.Forms.Button SaveFilter;
        private System.Windows.Forms.Button Exit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox ItemCategory;
    }
}