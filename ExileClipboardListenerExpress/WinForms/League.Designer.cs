namespace ExileClipboardListener.WinForms
{
    partial class League
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
            this.LeagueGrid = new System.Windows.Forms.DataGridView();
            this.LeagueDefault = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LeagueName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.EditLeague = new System.Windows.Forms.Button();
            this.CancelLeague = new System.Windows.Forms.Button();
            this.DeleteLeague = new System.Windows.Forms.Button();
            this.SaveLeague = new System.Windows.Forms.Button();
            this.NewLeague = new System.Windows.Forms.Button();
            this.ParentLeague = new System.Windows.Forms.ComboBox();
            this.LeagueStart = new System.Windows.Forms.DateTimePicker();
            this.LeagueEnd = new System.Windows.Forms.DateTimePicker();
            this.ClearStash = new System.Windows.Forms.Button();
            this.MergeStash = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LeagueGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // LeagueGrid
            // 
            this.LeagueGrid.AllowUserToAddRows = false;
            this.LeagueGrid.AllowUserToDeleteRows = false;
            this.LeagueGrid.AllowUserToResizeRows = false;
            this.LeagueGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.LeagueGrid.Location = new System.Drawing.Point(12, 12);
            this.LeagueGrid.Name = "LeagueGrid";
            this.LeagueGrid.ReadOnly = true;
            this.LeagueGrid.RowHeadersVisible = false;
            this.LeagueGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LeagueGrid.Size = new System.Drawing.Size(657, 186);
            this.LeagueGrid.TabIndex = 1;
            this.LeagueGrid.SelectionChanged += new System.EventHandler(this.LeagueGrid_SelectionChanged);
            // 
            // LeagueDefault
            // 
            this.LeagueDefault.Location = new System.Drawing.Point(12, 204);
            this.LeagueDefault.Name = "LeagueDefault";
            this.LeagueDefault.Size = new System.Drawing.Size(90, 23);
            this.LeagueDefault.TabIndex = 2;
            this.LeagueDefault.Text = "Mark Default";
            this.LeagueDefault.UseVisualStyleBackColor = true;
            this.LeagueDefault.Click += new System.EventHandler(this.LeagueDefault_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // LeagueName
            // 
            this.LeagueName.Location = new System.Drawing.Point(15, 255);
            this.LeagueName.Name = "LeagueName";
            this.LeagueName.Size = new System.Drawing.Size(175, 20);
            this.LeagueName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parent";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(374, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Start";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(507, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "End";
            this.label4.Visible = false;
            // 
            // EditLeague
            // 
            this.EditLeague.Location = new System.Drawing.Point(189, 204);
            this.EditLeague.Name = "EditLeague";
            this.EditLeague.Size = new System.Drawing.Size(75, 23);
            this.EditLeague.TabIndex = 11;
            this.EditLeague.Text = "Edit";
            this.EditLeague.UseVisualStyleBackColor = true;
            this.EditLeague.Click += new System.EventHandler(this.EditLeague_Click);
            // 
            // CancelLeague
            // 
            this.CancelLeague.Location = new System.Drawing.Point(270, 204);
            this.CancelLeague.Name = "CancelLeague";
            this.CancelLeague.Size = new System.Drawing.Size(75, 23);
            this.CancelLeague.TabIndex = 12;
            this.CancelLeague.Text = "Cancel";
            this.CancelLeague.UseVisualStyleBackColor = true;
            this.CancelLeague.Click += new System.EventHandler(this.CancelLeague_Click);
            // 
            // DeleteLeague
            // 
            this.DeleteLeague.Location = new System.Drawing.Point(351, 204);
            this.DeleteLeague.Name = "DeleteLeague";
            this.DeleteLeague.Size = new System.Drawing.Size(75, 23);
            this.DeleteLeague.TabIndex = 13;
            this.DeleteLeague.Text = "Delete";
            this.DeleteLeague.UseVisualStyleBackColor = true;
            this.DeleteLeague.Click += new System.EventHandler(this.DeleteLeague_Click);
            // 
            // SaveLeague
            // 
            this.SaveLeague.Location = new System.Drawing.Point(594, 204);
            this.SaveLeague.Name = "SaveLeague";
            this.SaveLeague.Size = new System.Drawing.Size(75, 23);
            this.SaveLeague.TabIndex = 14;
            this.SaveLeague.Text = "Save";
            this.SaveLeague.UseVisualStyleBackColor = true;
            this.SaveLeague.Click += new System.EventHandler(this.SaveLeague_Click);
            // 
            // NewLeague
            // 
            this.NewLeague.Location = new System.Drawing.Point(108, 204);
            this.NewLeague.Name = "NewLeague";
            this.NewLeague.Size = new System.Drawing.Size(75, 23);
            this.NewLeague.TabIndex = 15;
            this.NewLeague.Text = "New";
            this.NewLeague.UseVisualStyleBackColor = true;
            this.NewLeague.Click += new System.EventHandler(this.NewLeague_Click);
            // 
            // ParentLeague
            // 
            this.ParentLeague.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ParentLeague.FormattingEnabled = true;
            this.ParentLeague.Location = new System.Drawing.Point(196, 255);
            this.ParentLeague.Name = "ParentLeague";
            this.ParentLeague.Size = new System.Drawing.Size(175, 21);
            this.ParentLeague.TabIndex = 16;
            // 
            // LeagueStart
            // 
            this.LeagueStart.Location = new System.Drawing.Point(377, 256);
            this.LeagueStart.Name = "LeagueStart";
            this.LeagueStart.Size = new System.Drawing.Size(127, 20);
            this.LeagueStart.TabIndex = 17;
            this.LeagueStart.Visible = false;
            // 
            // LeagueEnd
            // 
            this.LeagueEnd.Location = new System.Drawing.Point(510, 256);
            this.LeagueEnd.Name = "LeagueEnd";
            this.LeagueEnd.Size = new System.Drawing.Size(127, 20);
            this.LeagueEnd.TabIndex = 18;
            this.LeagueEnd.Visible = false;
            // 
            // ClearStash
            // 
            this.ClearStash.Location = new System.Drawing.Point(513, 204);
            this.ClearStash.Name = "ClearStash";
            this.ClearStash.Size = new System.Drawing.Size(75, 23);
            this.ClearStash.TabIndex = 19;
            this.ClearStash.Text = "Clear Stash";
            this.ClearStash.UseVisualStyleBackColor = true;
            this.ClearStash.Click += new System.EventHandler(this.ClearStash_Click);
            // 
            // MergeStash
            // 
            this.MergeStash.Location = new System.Drawing.Point(432, 204);
            this.MergeStash.Name = "MergeStash";
            this.MergeStash.Size = new System.Drawing.Size(75, 23);
            this.MergeStash.TabIndex = 20;
            this.MergeStash.Text = "Merge Stash";
            this.MergeStash.UseVisualStyleBackColor = true;
            this.MergeStash.Click += new System.EventHandler(this.MergeStash_Click);
            // 
            // League
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 286);
            this.Controls.Add(this.MergeStash);
            this.Controls.Add(this.ClearStash);
            this.Controls.Add(this.LeagueEnd);
            this.Controls.Add(this.LeagueStart);
            this.Controls.Add(this.ParentLeague);
            this.Controls.Add(this.NewLeague);
            this.Controls.Add(this.SaveLeague);
            this.Controls.Add(this.DeleteLeague);
            this.Controls.Add(this.CancelLeague);
            this.Controls.Add(this.EditLeague);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LeagueName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LeagueDefault);
            this.Controls.Add(this.LeagueGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "League";
            this.Text = "League Manager";
            this.Load += new System.EventHandler(this.League_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LeagueGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView LeagueGrid;
        private System.Windows.Forms.Button LeagueDefault;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LeagueName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button EditLeague;
        private System.Windows.Forms.Button CancelLeague;
        private System.Windows.Forms.Button DeleteLeague;
        private System.Windows.Forms.Button SaveLeague;
        private System.Windows.Forms.Button NewLeague;
        private System.Windows.Forms.ComboBox ParentLeague;
        private System.Windows.Forms.DateTimePicker LeagueStart;
        private System.Windows.Forms.DateTimePicker LeagueEnd;
        private System.Windows.Forms.Button ClearStash;
        private System.Windows.Forms.Button MergeStash;
    }
}