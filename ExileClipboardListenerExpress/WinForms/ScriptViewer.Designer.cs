namespace ExileClipboardListener.WinForms
{
    partial class ScriptViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptViewer));
            this.ItemScript = new System.Windows.Forms.RichTextBox();
            this.Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ItemScript
            // 
            this.ItemScript.Location = new System.Drawing.Point(12, 12);
            this.ItemScript.Name = "ItemScript";
            this.ItemScript.ReadOnly = true;
            this.ItemScript.Size = new System.Drawing.Size(301, 348);
            this.ItemScript.TabIndex = 16;
            this.ItemScript.Text = "";
            // 
            // Exit
            // 
            this.Exit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Exit.Location = new System.Drawing.Point(238, 366);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(75, 23);
            this.Exit.TabIndex = 17;
            this.Exit.Text = "Close";
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // ScriptViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 400);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.ItemScript);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScriptViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Script Viewer";
            this.Load += new System.EventHandler(this.ScriptViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox ItemScript;
        private System.Windows.Forms.Button Exit;
    }
}