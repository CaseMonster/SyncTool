using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncTool
{
    public partial class Launcher : Form
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";
        public Launcher()
        {
            InitializeComponent();
            this.Play_Button.Enabled = false;
            this.Options_Button.Click += Open_Options_Form;
            this.Sync_Button.Click += Start_Sync;

        }
        private void Open_Options_Form(Object sender, EventArgs e)
        {
            var optionsForm = new OptionsMenu();
            optionsForm.Show();
        }
        private void Start_Sync(Object sender, EventArgs e)
        {
            this.Sync_Button.Enabled = false;
            Thread SyncThread = new Thread(new ThreadStart(() =>{
                //Add Run Function here
                // The following is bad and should not be done
                int total = 0;
                LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
                RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");

                //Generate new localRepo
                XML.BackupXML("repo.xml");

                if (!File.Exists(LOCAL_REPO))
                {
                    XML.GenerateBlankXML(LOCAL_REPO);
                }
                total = remoteSettings.modsArray.Count();
                total = (100 / total)-1;
                foreach (string mod in remoteSettings.modsArray)
                {
                    Log.Info("generating repo for " + mod);
                    FileHandler.GenerateLocalRepo(string.Format("{0}\\{1}", localSettings.modfolder, mod));
                    Sync_updateProgress(total += total);
                }

                //Pull remote repo
                PBOList remoteRepo = XML.ReadRepoXML(localSettings.server + "repo.xml");
                PBOList localRepo = XML.ReadRepoXML(LOCAL_REPO);

                //generate object chain of loaded dirs/pbos

                //create list of pbos that have changed, hashes that have changed
                PBOList downloadList = localRepo.DownloadList(remoteRepo);
                PBOList deleteList = localRepo.DeleteList(remoteRepo);
            }));
            SyncThread.Start();
        }
        public void Sync_updateProgress(int n)
        {
            Sync_Progress.BeginInvoke(
            new Action(() =>
            {
                if (n >= 100) n = 100;
                Sync_Progress.Value = n;
            }));

        }
    }
}
