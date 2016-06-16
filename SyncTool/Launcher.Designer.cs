namespace SyncTool
{
    partial class Launcher
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
            this.label1 = new System.Windows.Forms.Label();
            this.Download_Progress = new System.Windows.Forms.ProgressBar();
            this.Sync_Progress = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Sync_Button = new System.Windows.Forms.Button();
            this.Options_Button = new System.Windows.Forms.Button();
            this.Play_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "V 1.0 (Salty Slug)";
            // 
            // Download_Progress
            // 
            this.Download_Progress.Location = new System.Drawing.Point(13, 85);
            this.Download_Progress.Name = "Download_Progress";
            this.Download_Progress.Size = new System.Drawing.Size(367, 23);
            this.Download_Progress.TabIndex = 1;
            // 
            // Sync_Progress
            // 
            this.Sync_Progress.BackColor = System.Drawing.SystemColors.Control;
            this.Sync_Progress.Location = new System.Drawing.Point(13, 26);
            this.Sync_Progress.Name = "Sync_Progress";
            this.Sync_Progress.Size = new System.Drawing.Size(367, 23);
            this.Sync_Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Sync_Progress.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sync";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Download";
            // 
            // Sync_Button
            // 
            this.Sync_Button.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sync_Button.Location = new System.Drawing.Point(398, 26);
            this.Sync_Button.Name = "Sync_Button";
            this.Sync_Button.Size = new System.Drawing.Size(75, 23);
            this.Sync_Button.TabIndex = 5;
            this.Sync_Button.Text = "Sync";
            this.Sync_Button.UseVisualStyleBackColor = true;
            // 
            // Options_Button
            // 
            this.Options_Button.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Options_Button.Location = new System.Drawing.Point(398, 85);
            this.Options_Button.Name = "Options_Button";
            this.Options_Button.Size = new System.Drawing.Size(75, 23);
            this.Options_Button.TabIndex = 6;
            this.Options_Button.Text = "Options";
            this.Options_Button.UseVisualStyleBackColor = true;
            // 
            // Play_Button
            // 
            this.Play_Button.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play_Button.Location = new System.Drawing.Point(215, 114);
            this.Play_Button.Name = "Play_Button";
            this.Play_Button.Size = new System.Drawing.Size(258, 57);
            this.Play_Button.TabIndex = 7;
            this.Play_Button.Text = "Play";
            this.Play_Button.UseVisualStyleBackColor = true;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 195);
            this.Controls.Add(this.Play_Button);
            this.Controls.Add(this.Options_Button);
            this.Controls.Add(this.Sync_Button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Sync_Progress);
            this.Controls.Add(this.Download_Progress);
            this.Controls.Add(this.label1);
            this.Name = "Launcher";
            this.Text = "Launcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Sync_Button;
        private System.Windows.Forms.Button Options_Button;
        private System.Windows.Forms.Button Play_Button;
        public System.Windows.Forms.ProgressBar Download_Progress;
        public System.Windows.Forms.ProgressBar Sync_Progress;
    }
}