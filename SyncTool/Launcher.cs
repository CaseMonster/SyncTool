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
            this.Play_Button.Click += Play;
            if (!File.Exists(LOCAL_SETTINGS))
            {
                XML.GenerateLocalSettingsXML(LOCAL_SETTINGS);
                var optionsForm = new OptionsMenu();
                optionsForm.FormClosing += optionsFormClosing;
                optionsForm.Show();
                optionsForm.Activate();
                optionsForm.TopMost = true;
                this.Options_Button.Enabled = false;
                this.Sync_Button.Enabled = false;
              
            }

        }

        private void Open_Options_Form(Object sender, EventArgs e)
        {
            var optionsForm = new OptionsMenu();
            optionsForm.FormClosing += optionsFormClosing;
            optionsForm.Show();
            this.Options_Button.Enabled = false;
            this.Sync_Button.Enabled = false;
        }

        private void Start_Sync(Object sender, EventArgs e)
        {
            this.Sync_Button.Enabled = false;
            this.Play_Button.Enabled = false;
            Thread SyncThread = new Thread(new ThreadStart(() =>
            {
                //Add Run Function here
                //Pull local repo, remote repo, generate quick repo
                var localSettings = XML.ReadLocalSettingsXML(SyncTool.Program.LOCAL_SETTINGS);
                RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");
                PBOList remoteRepo = new PBOList();
                remoteRepo = remoteRepo.ReadFromDisk(localSettings.server + "//" + "repo.xml");
                PBOList localRepo = new PBOList();
                localRepo = localRepo.ReadFromDisk(LOCAL_REPO);
                PBOList quickRepo = new PBOList();
                quickRepo.GeneratePBOListFromDirs(remoteSettings.modsArray, localSettings);
                Sync_updateProgress(100);
                //Comb through directories and hash folders, if nessesary
                if (localRepo.HaveFileNamesChanged(quickRepo))
                {
                    quickRepo.AddHashesToList();

                    //DeleteFromDisk PBOs that are no longer in Repo
                    PBOList deleteList = quickRepo.GetDeleteList(remoteRepo);
                    if (deleteList.Count > 0)
                        deleteList.DeleteFilesOnDisk();

                    //cycle list of pbo downloads, store in temp location
                    PBOList downloadList = quickRepo.GetDownloadList(remoteRepo);
                    if (downloadList.Count > 0)
                        HTTP.DownloadList(downloadList, localSettings);

                    //add the repo from the server after adding back our modfolder
                    localRepo.Clear();
                    localRepo.DeleteXML(LOCAL_REPO);
                    remoteRepo.AddModPathToList(localSettings);
                    localRepo.AddRange(remoteRepo);
                    localRepo.WriteXMLToDisk(LOCAL_REPO);
                };
                Download_updateProgress(100);
                Play_update();
                Sync_update();
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
        public void Download_updateProgress(int n)
        {
            Download_Progress.BeginInvoke(
            new Action(() =>
            {
                if (n >= 100) n = 100;
                Download_Progress.Value = n;
            }));

        }
        public void Play_update()
        {
            Play_Button.BeginInvoke(
            new Action(() =>
            {
                Play_Button.Enabled = true;
            }));

        }

        public void Sync_update()
        {
            Sync_Button.BeginInvoke(
            new Action(() =>
            {
                Sync_Button.Enabled = true;
            }));

        }

        private void optionsFormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Options_Button.Enabled = true;
            this.Sync_Button.Enabled = true;
        }

        private void Play(Object sender, EventArgs e)
        {
            var localSettings = XML.ReadLocalSettingsXML(SyncTool.Program.LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");
            Run.Execute(localSettings, remoteSettings);
        }
    }
}
