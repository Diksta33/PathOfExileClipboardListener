namespace ExileClipboardListener.WinForms
{
    partial class GemViewer
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
            this.label4 = new System.Windows.Forms.Label();
            this.League = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GemType = new System.Windows.Forms.ComboBox();
            this.GemGrid = new System.Windows.Forms.DataGridView();
            this.Filter = new System.Windows.Forms.Button();
            this.ViewScript = new System.Windows.Forms.Button();
            this.GemCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GemGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "League";
            // 
            // League
            // 
            this.League.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.League.FormattingEnabled = true;
            this.League.Items.AddRange(new object[] {
            "(All)",
            "Armour",
            "Weapons",
            "Jewellery"});
            this.League.Location = new System.Drawing.Point(12, 25);
            this.League.Name = "League";
            this.League.Size = new System.Drawing.Size(198, 21);
            this.League.TabIndex = 10;
            this.League.SelectedIndexChanged += new System.EventHandler(this.League_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(213, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Gem Type";
            // 
            // GemType
            // 
            this.GemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GemType.FormattingEnabled = true;
            this.GemType.Items.AddRange(new object[] {
            "(All)",
            "Armour",
            "Jewellery",
            "Weapons"});
            this.GemType.Location = new System.Drawing.Point(216, 25);
            this.GemType.Name = "GemType";
            this.GemType.Size = new System.Drawing.Size(159, 21);
            this.GemType.Sorted = true;
            this.GemType.TabIndex = 12;
            // 
            // GemGrid
            // 
            this.GemGrid.AllowUserToAddRows = false;
            this.GemGrid.AllowUserToDeleteRows = false;
            this.GemGrid.AllowUserToResizeColumns = false;
            this.GemGrid.AllowUserToResizeRows = false;
            this.GemGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GemGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GemGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.GemGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.GemGrid.Location = new System.Drawing.Point(12, 52);
            this.GemGrid.MultiSelect = false;
            this.GemGrid.Name = "GemGrid";
            this.GemGrid.ReadOnly = true;
            this.GemGrid.RowHeadersVisible = false;
            this.GemGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GemGrid.Size = new System.Drawing.Size(1148, 397);
            this.GemGrid.TabIndex = 11;
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(1085, 25);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(75, 23);
            this.Filter.TabIndex = 19;
            this.Filter.Text = "Filter";
            this.Filter.UseVisualStyleBackColor = true;
            this.Filter.Click += new System.EventHandler(this.Filter_Click);
            // 
            // ViewScript
            // 
            this.ViewScript.Location = new System.Drawing.Point(12, 455);
            this.ViewScript.Name = "ViewScript";
            this.ViewScript.Size = new System.Drawing.Size(96, 23);
            this.ViewScript.TabIndex = 25;
            this.ViewScript.Text = "View Script";
            this.ViewScript.UseVisualStyleBackColor = true;
            this.ViewScript.Click += new System.EventHandler(this.ViewScript_Click);
            // 
            // GemCount
            // 
            this.GemCount.AutoSize = true;
            this.GemCount.Location = new System.Drawing.Point(1105, 460);
            this.GemCount.Name = "GemCount";
            this.GemCount.Size = new System.Drawing.Size(32, 13);
            this.GemCount.TabIndex = 26;
            this.GemCount.Text = "gems";
            // 
            // GemViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1172, 490);
            this.Controls.Add(this.GemCount);
            this.Controls.Add(this.ViewScript);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.League);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GemType);
            this.Controls.Add(this.GemGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GemViewer";
            this.Text = "Gem Viewer";
            this.Load += new System.EventHandler(this.GemViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GemGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox League;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox GemType;
        private System.Windows.Forms.DataGridView GemGrid;
        private System.Windows.Forms.Button Filter;
        private System.Windows.Forms.Button ViewScript;
        private System.Windows.Forms.Label GemCount;
    }
}