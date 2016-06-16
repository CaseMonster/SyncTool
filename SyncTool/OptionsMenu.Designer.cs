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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Arma_Folder_Textbox
            // 
            this.Arma_Folder_Textbox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Arma_Folder_Textbox.Location = new System.Drawing.Point(129, 5);
            this.Arma_Folder_Textbox.Name = "Arma_Folder_Textbox";
            this.Arma_Folder_Textbox.Size = new System.Drawing.Size(501, 23);
            this.Arma_Folder_Textbox.TabIndex = 3;
            // 
            // Repo_Add_Textbox
            // 
            this.Repo_Add_Textbox.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Repo_Add_Textbox.Location = new System.Drawing.Point(129, 31);
            this.Repo_Add_Textbox.Name = "Repo_Add_Textbox";
            this.Repo_Add_Textbox.Size = new System.Drawing.Size(501, 23);
            this.Repo_Add_Textbox.TabIndex = 4;
            this.Repo_Add_Textbox.TextChanged += new System.EventHandler(this.Repo_Add_Textbox_TextChanged);
            // 
            // Launch_Text
            // 
            this.Launch_Text.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Launch_Text.Location = new System.Drawing.Point(129, 58);
            this.Launch_Text.Name = "Launch_Text";
            this.Launch_Text.Size = new System.Drawing.Size(501, 23);
            this.Launch_Text.TabIndex = 5;
            this.Launch_Text.TextChanged += new System.EventHandler(this.Launch_Text_TextChanged);
            // 
            // Save_Button
            // 
            this.Save_Button.Location = new System.Drawing.Point(636, 83);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(75, 23);
            this.Save_Button.TabIndex = 6;
            this.Save_Button.Text = "Save";
            this.Save_Button.UseVisualStyleBackColor = true;
            // 
            // Mods_Text
            // 
            this.Mods_Text.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mods_Text.Location = new System.Drawing.Point(129, 85);
            this.Mods_Text.Name = "Mods_Text";
            this.Mods_Text.Size = new System.Drawing.Size(501, 23);
            this.Mods_Text.TabIndex = 7;
            this.Mods_Text.TextChanged += new System.EventHandler(this.Mods_Text_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Arma Location";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Repo Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Launch Options";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Mods Folder";
            // 
            // OptionsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 133);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}