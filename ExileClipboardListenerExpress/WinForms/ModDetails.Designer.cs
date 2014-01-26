namespace ExileClipboardListener.WinForms
{
    partial class ModDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Jewellery = new System.Windows.Forms.CheckBox();
            this.Armour = new System.Windows.Forms.CheckBox();
            this.Weapons = new System.Windows.Forms.CheckBox();
            this.ModClass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AffixGrid = new System.Windows.Forms.DataGridView();
            this.ModName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AffixIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AffixLevelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.AffixGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Jewellery
            // 
            this.Jewellery.AutoSize = true;
            this.Jewellery.Enabled = false;
            this.Jewellery.Location = new System.Drawing.Point(741, 15);
            this.Jewellery.Name = "Jewellery";
            this.Jewellery.Size = new System.Drawing.Size(69, 17);
            this.Jewellery.TabIndex = 32;
            this.Jewellery.Text = "Jewellery";
            this.Jewellery.UseVisualStyleBackColor = true;
            // 
            // Armour
            // 
            this.Armour.AutoSize = true;
            this.Armour.Enabled = false;
            this.Armour.Location = new System.Drawing.Point(598, 14);
            this.Armour.Name = "Armour";
            this.Armour.Size = new System.Drawing.Size(59, 17);
            this.Armour.TabIndex = 31;
            this.Armour.Text = "Armour";
            this.Armour.UseVisualStyleBackColor = true;
            // 
            // Weapons
            // 
            this.Weapons.AutoSize = true;
            this.Weapons.Enabled = false;
            this.Weapons.Location = new System.Drawing.Point(663, 15);
            this.Weapons.Name = "Weapons";
            this.Weapons.Size = new System.Drawing.Size(72, 17);
            this.Weapons.TabIndex = 30;
            this.Weapons.Text = "Weapons";
            this.Weapons.UseVisualStyleBackColor = true;
            // 
            // ModClass
            // 
            this.ModClass.Enabled = false;
            this.ModClass.Location = new System.Drawing.Point(86, 12);
            this.ModClass.Name = "ModClass";
            this.ModClass.Size = new System.Drawing.Size(210, 20);
            this.ModClass.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Mod Class";
            // 
            // AffixGrid
            // 
            this.AffixGrid.AllowUserToAddRows = false;
            this.AffixGrid.AllowUserToDeleteRows = false;
            this.AffixGrid.AllowUserToResizeRows = false;
            this.AffixGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AffixGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AffixIdColumn,
            this.AffixLevelColumn,
            this.Column4,
            this.Column5,
            this.Column1,
            this.Column13,
            this.Column6});
            this.AffixGrid.Location = new System.Drawing.Point(4, 58);
            this.AffixGrid.Name = "AffixGrid";
            this.AffixGrid.ReadOnly = true;
            this.AffixGrid.RowHeadersVisible = false;
            this.AffixGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AffixGrid.Size = new System.Drawing.Size(952, 380);
            this.AffixGrid.TabIndex = 27;
            // 
            // ModName
            // 
            this.ModName.Enabled = false;
            this.ModName.Location = new System.Drawing.Point(382, 12);
            this.ModName.Name = "ModName";
            this.ModName.Size = new System.Drawing.Size(210, 20);
            this.ModName.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Mod Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Appears in the following Affixes";
            // 
            // AffixIdColumn
            // 
            this.AffixIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AffixIdColumn.HeaderText = "AffixId";
            this.AffixIdColumn.Name = "AffixIdColumn";
            this.AffixIdColumn.ReadOnly = true;
            this.AffixIdColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AffixIdColumn.Visible = false;
            this.AffixIdColumn.Width = 42;
            // 
            // AffixLevelColumn
            // 
            this.AffixLevelColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.AffixLevelColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.AffixLevelColumn.HeaderText = "Level";
            this.AffixLevelColumn.Name = "AffixLevelColumn";
            this.AffixLevelColumn.ReadOnly = true;
            this.AffixLevelColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AffixLevelColumn.Width = 39;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.HeaderText = "Name";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 41;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.HeaderText = "Affix Type";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Mod Position";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column13
            // 
            this.Column13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column13.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column13.HeaderText = "Min";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column13.Width = 30;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N0";
            this.Column6.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column6.HeaderText = "Max";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 33;
            // 
            // ModDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 445);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ModName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Jewellery);
            this.Controls.Add(this.Armour);
            this.Controls.Add(this.Weapons);
            this.Controls.Add(this.ModClass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AffixGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mod Details";
            this.Load += new System.EventHandler(this.ModDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AffixGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox Jewellery;
        private System.Windows.Forms.CheckBox Armour;
        private System.Windows.Forms.CheckBox Weapons;
        private System.Windows.Forms.TextBox ModClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView AffixGrid;
        private System.Windows.Forms.TextBox ModName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn AffixIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AffixLevelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}