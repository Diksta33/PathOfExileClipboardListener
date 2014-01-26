namespace ExileClipboardListener.WinForms
{
    partial class AffixDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.AffixGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.AffixCategory = new System.Windows.Forms.TextBox();
            this.Weapons = new System.Windows.Forms.CheckBox();
            this.Armour = new System.Windows.Forms.CheckBox();
            this.Jewellery = new System.Windows.Forms.CheckBox();
            this.AffixIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AffixLevelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.AffixGrid)).BeginInit();
            this.SuspendLayout();
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
            this.Column13,
            this.Column6,
            this.Column7,
            this.Column14,
            this.Column8});
            this.AffixGrid.Location = new System.Drawing.Point(12, 38);
            this.AffixGrid.Name = "AffixGrid";
            this.AffixGrid.ReadOnly = true;
            this.AffixGrid.RowHeadersVisible = false;
            this.AffixGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AffixGrid.Size = new System.Drawing.Size(952, 405);
            this.AffixGrid.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Affix Category";
            // 
            // AffixCategory
            // 
            this.AffixCategory.Enabled = false;
            this.AffixCategory.Location = new System.Drawing.Point(94, 12);
            this.AffixCategory.Name = "AffixCategory";
            this.AffixCategory.Size = new System.Drawing.Size(210, 20);
            this.AffixCategory.TabIndex = 23;
            // 
            // Weapons
            // 
            this.Weapons.AutoSize = true;
            this.Weapons.Enabled = false;
            this.Weapons.Location = new System.Drawing.Point(389, 12);
            this.Weapons.Name = "Weapons";
            this.Weapons.Size = new System.Drawing.Size(72, 17);
            this.Weapons.TabIndex = 24;
            this.Weapons.Text = "Weapons";
            this.Weapons.UseVisualStyleBackColor = true;
            // 
            // Armour
            // 
            this.Armour.AutoSize = true;
            this.Armour.Enabled = false;
            this.Armour.Location = new System.Drawing.Point(324, 11);
            this.Armour.Name = "Armour";
            this.Armour.Size = new System.Drawing.Size(59, 17);
            this.Armour.TabIndex = 25;
            this.Armour.Text = "Armour";
            this.Armour.UseVisualStyleBackColor = true;
            // 
            // Jewellery
            // 
            this.Jewellery.AutoSize = true;
            this.Jewellery.Enabled = false;
            this.Jewellery.Location = new System.Drawing.Point(467, 12);
            this.Jewellery.Name = "Jewellery";
            this.Jewellery.Size = new System.Drawing.Size(69, 17);
            this.Jewellery.TabIndex = 26;
            this.Jewellery.Text = "Jewellery";
            this.Jewellery.UseVisualStyleBackColor = true;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N0";
            this.AffixLevelColumn.DefaultCellStyle = dataGridViewCellStyle1;
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
            this.Column5.HeaderText = "Primary Mod Name";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 102;
            // 
            // Column13
            // 
            this.Column13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column13.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column13.HeaderText = "Min";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column13.Width = 30;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            this.Column6.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column6.HeaderText = "Max";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 33;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.HeaderText = "Secondary Mod Name";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 119;
            // 
            // Column14
            // 
            this.Column14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column14.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column14.HeaderText = "Min";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column14.Width = 30;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N0";
            this.Column8.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column8.HeaderText = "Max";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 33;
            // 
            // AffixDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 455);
            this.Controls.Add(this.Jewellery);
            this.Controls.Add(this.Armour);
            this.Controls.Add(this.Weapons);
            this.Controls.Add(this.AffixCategory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AffixGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AffixDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Affix Details";
            this.Load += new System.EventHandler(this.AffixDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AffixGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView AffixGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AffixCategory;
        private System.Windows.Forms.CheckBox Weapons;
        private System.Windows.Forms.CheckBox Armour;
        private System.Windows.Forms.CheckBox Jewellery;
        private System.Windows.Forms.DataGridViewTextBoxColumn AffixIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AffixLevelColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}