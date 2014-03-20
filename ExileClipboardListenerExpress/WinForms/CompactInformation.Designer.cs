namespace ExileClipboardListener.WinForms
{
    partial class CompactInformation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompactInformation));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Stuff = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AddStash = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Dismiss = new System.Windows.Forms.Button();
            this.newProgressBar5 = new ExileClipboardListener.Classes.NewProgressBar();
            this.newProgressBar4 = new ExileClipboardListener.Classes.NewProgressBar();
            this.newProgressBar3 = new ExileClipboardListener.Classes.NewProgressBar();
            this.newProgressBar1 = new ExileClipboardListener.Classes.NewProgressBar();
            this.newProgressBar2 = new ExileClipboardListener.Classes.NewProgressBar();
            this.newProgressBar6 = new ExileClipboardListener.Classes.NewProgressBar();
            this.SuspendLayout();
            // 
            // Stuff
            // 
            this.Stuff.Location = new System.Drawing.Point(0, 0);
            this.Stuff.Name = "Stuff";
            this.Stuff.Size = new System.Drawing.Size(304, 65);
            this.Stuff.TabIndex = 6;
            this.Stuff.Text = "Stuff will go here...";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            // 
            // AddStash
            // 
            this.AddStash.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.AddStash.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddStash.Location = new System.Drawing.Point(259, 431);
            this.AddStash.Name = "AddStash";
            this.AddStash.Size = new System.Drawing.Size(45, 22);
            this.AddStash.TabIndex = 13;
            this.AddStash.Text = "Stash";
            this.AddStash.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(303, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(303, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(1, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(303, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(1, 309);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(303, 23);
            this.label5.TabIndex = 17;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(1, 368);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(303, 23);
            this.label6.TabIndex = 18;
            this.label6.Text = "label6";
            // 
            // Dismiss
            // 
            this.Dismiss.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Dismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Dismiss.Location = new System.Drawing.Point(203, 431);
            this.Dismiss.Name = "Dismiss";
            this.Dismiss.Size = new System.Drawing.Size(50, 22);
            this.Dismiss.TabIndex = 19;
            this.Dismiss.Text = "Dismiss";
            this.Dismiss.UseVisualStyleBackColor = true;
            this.Dismiss.Click += new System.EventHandler(this.Dismiss_Click);
            // 
            // newProgressBar5
            // 
            this.newProgressBar5.Location = new System.Drawing.Point(3, 335);
            this.newProgressBar5.Maximum = 300;
            this.newProgressBar5.Name = "newProgressBar5";
            this.newProgressBar5.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar5.TabIndex = 23;
            // 
            // newProgressBar4
            // 
            this.newProgressBar4.Location = new System.Drawing.Point(3, 276);
            this.newProgressBar4.Maximum = 300;
            this.newProgressBar4.Name = "newProgressBar4";
            this.newProgressBar4.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar4.TabIndex = 22;
            // 
            // newProgressBar3
            // 
            this.newProgressBar3.Location = new System.Drawing.Point(3, 217);
            this.newProgressBar3.Maximum = 300;
            this.newProgressBar3.Name = "newProgressBar3";
            this.newProgressBar3.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar3.TabIndex = 21;
            // 
            // newProgressBar1
            // 
            this.newProgressBar1.Location = new System.Drawing.Point(3, 99);
            this.newProgressBar1.Maximum = 300;
            this.newProgressBar1.Name = "newProgressBar1";
            this.newProgressBar1.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar1.TabIndex = 0;
            // 
            // newProgressBar2
            // 
            this.newProgressBar2.Location = new System.Drawing.Point(3, 158);
            this.newProgressBar2.Maximum = 300;
            this.newProgressBar2.Name = "newProgressBar2";
            this.newProgressBar2.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar2.TabIndex = 20;
            // 
            // newProgressBar6
            // 
            this.newProgressBar6.Location = new System.Drawing.Point(3, 394);
            this.newProgressBar6.Maximum = 300;
            this.newProgressBar6.Name = "newProgressBar6";
            this.newProgressBar6.Size = new System.Drawing.Size(301, 31);
            this.newProgressBar6.TabIndex = 24;
            // 
            // CompactInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(306, 456);
            this.Controls.Add(this.newProgressBar5);
            this.Controls.Add(this.newProgressBar4);
            this.Controls.Add(this.newProgressBar3);
            this.Controls.Add(this.Dismiss);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AddStash);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Stuff);
            this.Controls.Add(this.newProgressBar1);
            this.Controls.Add(this.newProgressBar2);
            this.Controls.Add(this.newProgressBar6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "CompactInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CompactInformation";
            this.Load += new System.EventHandler(this.CompactInformationLoad);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CompactInformation_KeyPress);
            this.ResumeLayout(false);

        }

        #endregion

        private Classes.NewProgressBar newProgressBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label Stuff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AddStash;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Dismiss;
        private Classes.NewProgressBar newProgressBar2;
        private Classes.NewProgressBar newProgressBar3;
        private Classes.NewProgressBar newProgressBar4;
        private Classes.NewProgressBar newProgressBar5;
        private Classes.NewProgressBar newProgressBar6;
    }
}