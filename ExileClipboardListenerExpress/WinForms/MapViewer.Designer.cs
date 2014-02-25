namespace ExileClipboardListener.WinForms
{
    partial class MapViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapViewer));
            this.MapCount = new System.Windows.Forms.Label();
            this.ViewScript = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.League = new System.Windows.Forms.ComboBox();
            this.MapGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.MapLevel = new System.Windows.Forms.ComboBox();
            this.Filter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MapGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // MapCount
            // 
            this.MapCount.AutoSize = true;
            this.MapCount.Location = new System.Drawing.Point(592, 460);
            this.MapCount.Name = "MapCount";
            this.MapCount.Size = new System.Drawing.Size(32, 13);
            this.MapCount.TabIndex = 31;
            this.MapCount.Text = "maps";
            // 
            // ViewScript
            // 
            this.ViewScript.Location = new System.Drawing.Point(12, 455);
            this.ViewScript.Name = "ViewScript";
            this.ViewScript.Size = new System.Drawing.Size(96, 23);
            this.ViewScript.TabIndex = 30;
            this.ViewScript.Text = "View Script";
            this.ViewScript.UseVisualStyleBackColor = true;
            this.ViewScript.Click += new System.EventHandler(this.ViewScript_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "League";
            // 
            // League
            // 
            this.League.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.League.FormattingEnabled = true;
            this.League.Location = new System.Drawing.Point(12, 25);
            this.League.Name = "League";
            this.League.Size = new System.Drawing.Size(198, 21);
            this.League.TabIndex = 27;
            this.League.SelectedIndexChanged += new System.EventHandler(this.League_SelectedIndexChanged);
            // 
            // MapGrid
            // 
            this.MapGrid.AllowUserToAddRows = false;
            this.MapGrid.AllowUserToDeleteRows = false;
            this.MapGrid.AllowUserToResizeColumns = false;
            this.MapGrid.AllowUserToResizeRows = false;
            this.MapGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MapGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MapGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MapGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.MapGrid.Location = new System.Drawing.Point(12, 52);
            this.MapGrid.MultiSelect = false;
            this.MapGrid.Name = "MapGrid";
            this.MapGrid.ReadOnly = true;
            this.MapGrid.RowHeadersVisible = false;
            this.MapGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MapGrid.Size = new System.Drawing.Size(634, 397);
            this.MapGrid.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Map Level";
            // 
            // MapLevel
            // 
            this.MapLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MapLevel.FormattingEnabled = true;
            this.MapLevel.Location = new System.Drawing.Point(216, 25);
            this.MapLevel.Name = "MapLevel";
            this.MapLevel.Size = new System.Drawing.Size(71, 21);
            this.MapLevel.Sorted = true;
            this.MapLevel.TabIndex = 32;
            this.MapLevel.SelectedIndexChanged += new System.EventHandler(this.MapLevel_SelectedIndexChanged);
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(570, 23);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(75, 23);
            this.Filter.TabIndex = 34;
            this.Filter.Text = "Filter";
            this.Filter.UseVisualStyleBackColor = true;
            this.Filter.Click += new System.EventHandler(this.Filter_Click);
            // 
            // MapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 490);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MapLevel);
            this.Controls.Add(this.MapCount);
            this.Controls.Add(this.ViewScript);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.League);
            this.Controls.Add(this.MapGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapViewer";
            this.Text = "Map Viewer";
            this.Load += new System.EventHandler(this.MapViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MapGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MapCount;
        private System.Windows.Forms.Button ViewScript;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox League;
        private System.Windows.Forms.DataGridView MapGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MapLevel;
        private System.Windows.Forms.Button Filter;
    }
}