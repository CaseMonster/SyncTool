using System;
using System.Windows.Forms;

namespace SyncTool
{
    public partial class TempUI : Form
    {
        public TempUI()
        {
            InitializeComponent();
            this.Show();
        }

        //Resync
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Program.Sync(true);
            this.Show();
        }

        //Settings
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if((string)folderBrowserDialog.SelectedPath != "")
                XML.GenerateLocalSettingsXML(Program.LOCAL_SETTINGS, (string)folderBrowserDialog.SelectedPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Program.Run();
            Application.Exit();
        }

        private void TempUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
