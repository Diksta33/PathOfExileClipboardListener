namespace ExileClipboardListener.WinForms
{
    partial class JSONReader
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
            this.Logon = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.StashTab = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.League = new System.Windows.Forms.ComboBox();
            this.ItemIcon = new System.Windows.Forms.PictureBox();
            this.ItemScript = new System.Windows.Forms.RichTextBox();
            this.GrabCharacters = new System.Windows.Forms.Button();
            this.ItemList = new System.Windows.Forms.ListBox();
            this.GrabStash = new System.Windows.Forms.Button();
            this.StashAll = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.CharacterGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddStash = new System.Windows.Forms.Button();
            this.GrabStashTabs = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ItemIcon)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CharacterGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Logon
            // 
            this.Logon.Location = new System.Drawing.Point(12, 12);
            this.Logon.Name = "Logon";
            this.Logon.Size = new System.Drawing.Size(75, 23);
            this.Logon.TabIndex = 1;
            this.Logon.Text = "Logon";
            this.Logon.UseVisualStyleBackColor = true;
            this.Logon.Click += new System.EventHandler(this.Logon_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 238);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Stash Tab";
            // 
            // StashTab
            // 
            this.StashTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StashTab.Enabled = false;
            this.StashTab.FormattingEnabled = true;
            this.StashTab.Location = new System.Drawing.Point(75, 233);
            this.StashTab.Name = "StashTab";
            this.StashTab.Size = new System.Drawing.Size(128, 21);
            this.StashTab.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 209);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "League";
            // 
            // League
            // 
            this.League.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.League.Enabled = false;
            this.League.FormattingEnabled = true;
            this.League.Location = new System.Drawing.Point(75, 206);
            this.League.Name = "League";
            this.League.Size = new System.Drawing.Size(212, 21);
            this.League.Sorted = true;
            this.League.TabIndex = 7;
            this.League.SelectedIndexChanged += new System.EventHandler(this.League_SelectedIndexChanged);
            // 
            // ItemIcon
            // 
            this.ItemIcon.Location = new System.Drawing.Point(691, 297);
            this.ItemIcon.Name = "ItemIcon";
            this.ItemIcon.Size = new System.Drawing.Size(32, 32);
            this.ItemIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ItemIcon.TabIndex = 16;
            this.ItemIcon.TabStop = false;
            // 
            // ItemScript
            // 
            this.ItemScript.Location = new System.Drawing.Point(384, 297);
            this.ItemScript.Name = "ItemScript";
            this.ItemScript.Size = new System.Drawing.Size(301, 277);
            this.ItemScript.TabIndex = 15;
            this.ItemScript.Text = "";
            // 
            // GrabCharacters
            // 
            this.GrabCharacters.Enabled = false;
            this.GrabCharacters.Location = new System.Drawing.Point(93, 12);
            this.GrabCharacters.Name = "GrabCharacters";
            this.GrabCharacters.Size = new System.Drawing.Size(78, 23);
            this.GrabCharacters.TabIndex = 14;
            this.GrabCharacters.Text = "Grab Chars";
            this.GrabCharacters.UseVisualStyleBackColor = true;
            this.GrabCharacters.Click += new System.EventHandler(this.GrabCharacters_Click);
            // 
            // ItemList
            // 
            this.ItemList.FormattingEnabled = true;
            this.ItemList.Location = new System.Drawing.Point(12, 297);
            this.ItemList.Name = "ItemList";
            this.ItemList.Size = new System.Drawing.Size(366, 277);
            this.ItemList.TabIndex = 13;
            this.ItemList.SelectedIndexChanged += new System.EventHandler(this.ItemList_SelectedIndexChanged);
            // 
            // GrabStash
            // 
            this.GrabStash.Enabled = false;
            this.GrabStash.Location = new System.Drawing.Point(209, 233);
            this.GrabStash.Name = "GrabStash";
            this.GrabStash.Size = new System.Drawing.Size(78, 23);
            this.GrabStash.TabIndex = 12;
            this.GrabStash.Text = "Grab Stash";
            this.GrabStash.UseVisualStyleBackColor = true;
            this.GrabStash.Click += new System.EventHandler(this.GrabStash_Click);
            // 
            // StashAll
            // 
            this.StashAll.Enabled = false;
            this.StashAll.Location = new System.Drawing.Point(12, 580);
            this.StashAll.Name = "StashAll";
            this.StashAll.Size = new System.Drawing.Size(366, 23);
            this.StashAll.TabIndex = 17;
            this.StashAll.Text = "Add all these items to the Global Stash";
            this.StashAll.UseVisualStyleBackColor = true;
            this.StashAll.Click += new System.EventHandler(this.StashAll_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 606);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(777, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // CharacterGrid
            // 
            this.CharacterGrid.AllowUserToAddRows = false;
            this.CharacterGrid.AllowUserToDeleteRows = false;
            this.CharacterGrid.AllowUserToResizeRows = false;
            this.CharacterGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.CharacterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.CharacterGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column4,
            this.Column2,
            this.Column3});
            this.CharacterGrid.Location = new System.Drawing.Point(12, 41);
            this.CharacterGrid.Name = "CharacterGrid";
            this.CharacterGrid.ReadOnly = true;
            this.CharacterGrid.RowHeadersVisible = false;
            this.CharacterGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CharacterGrid.Size = new System.Drawing.Size(673, 159);
            this.CharacterGrid.TabIndex = 19;
            // 
            // Column1
            // 
            dataGridViewCellStyle4.Format = "N0";
            this.Column1.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column1.HeaderText = "Level";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 58;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Class";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 57;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 60;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "League";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 68;
            // 
            // AddStash
            // 
            this.AddStash.Enabled = false;
            this.AddStash.Location = new System.Drawing.Point(691, 551);
            this.AddStash.Name = "AddStash";
            this.AddStash.Size = new System.Drawing.Size(78, 23);
            this.AddStash.TabIndex = 20;
            this.AddStash.Text = "Add to Stash";
            this.AddStash.UseVisualStyleBackColor = true;
            this.AddStash.Click += new System.EventHandler(this.AddStash_Click);
            // 
            // GrabStashTabs
            // 
            this.GrabStashTabs.Enabled = false;
            this.GrabStashTabs.Location = new System.Drawing.Point(293, 206);
            this.GrabStashTabs.Name = "GrabStashTabs";
            this.GrabStashTabs.Size = new System.Drawing.Size(99, 23);
            this.GrabStashTabs.TabIndex = 21;
            this.GrabStashTabs.Text = "Grab Stash Tabs";
            this.GrabStashTabs.UseVisualStyleBackColor = true;
            this.GrabStashTabs.Click += new System.EventHandler(this.GrabStashTabs_Click);
            // 
            // JSONReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 628);
            this.Controls.Add(this.GrabStashTabs);
            this.Controls.Add(this.AddStash);
            this.Controls.Add(this.CharacterGrid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.StashAll);
            this.Controls.Add(this.ItemIcon);
            this.Controls.Add(this.ItemScript);
            this.Controls.Add(this.GrabCharacters);
            this.Controls.Add(this.ItemList);
            this.Controls.Add(this.GrabStash);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StashTab);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.League);
            this.Controls.Add(this.Logon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JSONReader";
            this.Text = "JSONReader";
            this.Load += new System.EventHandler(this.JSONReader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ItemIcon)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CharacterGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Logon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox StashTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox League;
        private System.Windows.Forms.PictureBox ItemIcon;
        private System.Windows.Forms.RichTextBox ItemScript;
        private System.Windows.Forms.Button GrabCharacters;
        private System.Windows.Forms.ListBox ItemList;
        private System.Windows.Forms.Button GrabStash;
        private System.Windows.Forms.Button StashAll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.DataGridView CharacterGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Button AddStash;
        private System.Windows.Forms.Button GrabStashTabs;
    }
}