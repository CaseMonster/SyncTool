namespace SyncTool
{
    partial class OptionsMenu
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
            this.Arma_Folder_Textbox = new System.Windows.Forms.TextBox();
            this.Repo_Add_Textbox = new System.Windows.Forms.TextBox();
            this.Launch_Text = new System.Windows.Forms.TextBox();
            this.Save_Button = new System.Windows.Forms.Button();
            this.Mods_Text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Arma_Folder_Textbox
            // 
            this.Arma_Folder_Textbox.Location = new System.Drawing.Point(12, 12);
            this.Arma_Folder_Textbox.Name = "Arma_Folder_Textbox";
            this.Arma_Folder_Textbox.Size = new System.Drawing.Size(501, 20);
            this.Arma_Folder_Textbox.TabIndex = 3;
            // 
            // Repo_Add_Textbox
            // 
            this.Repo_Add_Textbox.Location = new System.Drawing.Point(12, 41);
            this.Repo_Add_Textbox.Name = "Repo_Add_Textbox";
            this.Repo_Add_Textbox.Size = new System.Drawing.Size(501, 20);
            this.Repo_Add_Textbox.TabIndex = 4;
            // 
            // Launch_Text
            // 
            this.Launch_Text.Location = new System.Drawing.Point(12, 69);
            this.Launch_Text.Name = "Launch_Text";
            this.Launch_Text.Size = new System.Drawing.Size(501, 20);
            this.Launch_Text.TabIndex = 5;
            // 
            // Save_Button
            // 
            this.Save_Button.Location = new System.Drawing.Point(523, 92);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(75, 23);
            this.Save_Button.TabIndex = 6;
            this.Save_Button.Text = "Save";
            this.Save_Button.UseVisualStyleBackColor = true;
            // 
            // Mods_Text
            // 
            this.Mods_Text.Location = new System.Drawing.Point(12, 95);
            this.Mods_Text.Name = "Mods_Text";
            this.Mods_Text.Size = new System.Drawing.Size(501, 20);
            this.Mods_Text.TabIndex = 7;
            // 
            // OptionsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 136);
            this.Controls.Add(this.Mods_Text);
            this.Controls.Add(this.Save_Button);
            this.Controls.Add(this.Launch_Text);
            this.Controls.Add(this.Repo_Add_Textbox);
            this.Controls.Add(this.Arma_Folder_Textbox);
            this.Name = "OptionsMenu";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Arma_Folder_Textbox;
        private System.Windows.Forms.TextBox Repo_Add_Textbox;
        private System.Windows.Forms.TextBox Launch_Text;
        private System.Windows.Forms.Button Save_Button;
        private System.Windows.Forms.TextBox Mods_Text;
    }
}