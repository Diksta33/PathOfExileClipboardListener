namespace ExileClipboardListener.WinForms
{
    partial class Character
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
            this.CharacterClass = new System.Windows.Forms.ComboBox();
            this.NewLeague = new System.Windows.Forms.Button();
            this.SaveLeague = new System.Windows.Forms.Button();
            this.DeleteLeague = new System.Windows.Forms.Button();
            this.CancelLeague = new System.Windows.Forms.Button();
            this.EditLeague = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CharacterName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LeagueDefault = new System.Windows.Forms.Button();
            this.LeagueGrid = new System.Windows.Forms.DataGridView();
            this.League = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LeagueGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // CharacterClass
            // 
            this.CharacterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CharacterClass.FormattingEnabled = true;
            this.CharacterClass.Location = new System.Drawing.Point(196, 255);
            this.CharacterClass.Name = "CharacterClass";
            this.CharacterClass.Size = new System.Drawing.Size(129, 21);
            this.CharacterClass.TabIndex = 31;
            // 
            // NewLeague
            // 
            this.NewLeague.Location = new System.Drawing.Point(238, 204);
            this.NewLeague.Name = "NewLeague";
            this.NewLeague.Size = new System.Drawing.Size(75, 23);
            this.NewLeague.TabIndex = 30;
            this.NewLeague.Text = "New";
            this.NewLeague.UseVisualStyleBackColor = true;
            // 
            // SaveLeague
            // 
            this.SaveLeague.Location = new System.Drawing.Point(562, 204);
            this.SaveLeague.Name = "SaveLeague";
            this.SaveLeague.Size = new System.Drawing.Size(75, 23);
            this.SaveLeague.TabIndex = 29;
            this.SaveLeague.Text = "Save";
            this.SaveLeague.UseVisualStyleBackColor = true;
            // 
            // DeleteLeague
            // 
            this.DeleteLeague.Location = new System.Drawing.Point(481, 204);
            this.DeleteLeague.Name = "DeleteLeague";
            this.DeleteLeague.Size = new System.Drawing.Size(75, 23);
            this.DeleteLeague.TabIndex = 28;
            this.DeleteLeague.Text = "Delete";
            this.DeleteLeague.UseVisualStyleBackColor = true;
            // 
            // CancelLeague
            // 
            this.CancelLeague.Location = new System.Drawing.Point(400, 204);
            this.CancelLeague.Name = "CancelLeague";
            this.CancelLeague.Size = new System.Drawing.Size(75, 23);
            this.CancelLeague.TabIndex = 27;
            this.CancelLeague.Text = "Cancel";
            this.CancelLeague.UseVisualStyleBackColor = true;
            // 
            // EditLeague
            // 
            this.EditLeague.Location = new System.Drawing.Point(319, 204);
            this.EditLeague.Name = "EditLeague";
            this.EditLeague.Size = new System.Drawing.Size(75, 23);
            this.EditLeague.TabIndex = 26;
            this.EditLeague.Text = "Edit";
            this.EditLeague.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(509, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Level";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "League";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 239);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Class";
            // 
            // CharacterName
            // 
            this.CharacterName.Location = new System.Drawing.Point(15, 255);
            this.CharacterName.Name = "CharacterName";
            this.CharacterName.Size = new System.Drawing.Size(175, 20);
            this.CharacterName.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Name";
            // 
            // LeagueDefault
            // 
            this.LeagueDefault.Location = new System.Drawing.Point(12, 204);
            this.LeagueDefault.Name = "LeagueDefault";
            this.LeagueDefault.Size = new System.Drawing.Size(100, 23);
            this.LeagueDefault.TabIndex = 20;
            this.LeagueDefault.Text = "Mark as Default";
            this.LeagueDefault.UseVisualStyleBackColor = true;
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
            this.LeagueGrid.Size = new System.Drawing.Size(625, 186);
            this.LeagueGrid.TabIndex = 19;
            // 
            // League
            // 
            this.League.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.League.FormattingEnabled = true;
            this.League.Location = new System.Drawing.Point(331, 255);
            this.League.Name = "League";
            this.League.Size = new System.Drawing.Size(175, 21);
            this.League.TabIndex = 32;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(512, 256);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(38, 20);
            this.textBox1.TabIndex = 33;
            // 
            // Character
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 297);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.League);
            this.Controls.Add(this.CharacterClass);
            this.Controls.Add(this.NewLeague);
            this.Controls.Add(this.SaveLeague);
            this.Controls.Add(this.DeleteLeague);
            this.Controls.Add(this.CancelLeague);
            this.Controls.Add(this.EditLeague);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CharacterName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LeagueDefault);
            this.Controls.Add(this.LeagueGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Character";
            this.ShowInTaskbar = false;
            this.Text = "Character Manager";
            this.Load += new System.EventHandler(this.Character_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LeagueGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CharacterClass;
        private System.Windows.Forms.Button NewLeague;
        private System.Windows.Forms.Button SaveLeague;
        private System.Windows.Forms.Button DeleteLeague;
        private System.Windows.Forms.Button CancelLeague;
        private System.Windows.Forms.Button EditLeague;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CharacterName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LeagueDefault;
        private System.Windows.Forms.DataGridView LeagueGrid;
        private System.Windows.Forms.ComboBox League;
        private System.Windows.Forms.TextBox textBox1;
    }
}