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
        private LocalSettings localSettings = null;
        public OptionsMenu()
        {
            InitializeComponent();
            this.Save_Button.Enabled = false;
            localSettings = XML.ReadLocalSettingsXML(SyncTool.Program.LOCAL_SETTINGS);
            this.Repo_Add_Textbox.Text = localSettings.server;
            this.Launch_Text.Text = localSettings.arma3args;
            this.Arma_Folder_Textbox.Text = localSettings.arma3file;
            this.Mods_Text.Text = localSettings.modfolder;
            this.Save_Button.Enabled = true;
            this.Save_Button.Click += save_Click;
        }
        void save_Click(object sender, System.EventArgs e)
        {
            if (localSettings == null)
            {
                Log.Error("Failed to Get localsetting xml");
            }
            else
            {
                localSettings.arma3args = this.Launch_Text.Text;
                localSettings.arma3file = this.Arma_Folder_Textbox.Text;
                localSettings.server = this.Repo_Add_Textbox.Text;
                localSettings.modfolder = this.Mods_Text.Text;
                XML.OverWriteLocalSettingsXML(localSettings, SyncTool.Program.LOCAL_SETTINGS);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
