using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SyncTool
{

    public partial class OptionsMenu : Form
    {
        public OptionsMenu()
        {
            InitializeComponent();
            this.Add_Arma_Button.Enabled = false;
            this.Add_Repo_Button.Enabled = false;
            this.Launch_Options_Button.Enabled = false;
            this.Save_Button.Enabled = false;
            LocalSettings localSettings = XML.ReadLocalSettingsXML(SyncTool.Program.LOCAL_SETTINGS);
            this.Repo_Add_Textbox.Text = localSettings.server;
            this.Launch_Text.Text = localSettings.arma3args;
            this.Arma_Folder_Textbox.Text = localSettings.arma3file;
            this.Add_Arma_Button.Enabled = true;
            this.Add_Repo_Button.Enabled = true;
            this.Launch_Options_Button.Enabled = true;
            this.Save_Button.Enabled = true;
        }
    }
}
