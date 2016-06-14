using System.Windows.Forms;

namespace SyncTool
{
    class Program
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";

        static void Main(string[] args)
        {
            Log.Startup();

            if (false)
            {
                Application.Run(new Launcher());
                return;
            }

            //load settings
            LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");

            //Generate new localRepo
            //XML.BackupXML("repo.xml");

            //if(!File.Exists(LOCAL_REPO))
            //{
            //    XML.GenerateBlankXML(LOCAL_REPO);
            //}

            //We need an additional check for filenames?  then if we see a change in the dir, hash that one dir

            //Comb through directories and hash folders, if nessesary
            //FileHandler.HashFolders(remoteSettings, localSettings);

            //Pull remote repo
            PBOList remoteRepo = XML.ReadRepoXML(localSettings.server + "repo.xml");
            PBOList localRepo = XML.ReadRepoXML(LOCAL_REPO);

            //create list of pbos that have changed, hashes that have changed
            PBOList downloadList = localRepo.DownloadList(remoteRepo);
            PBOList deleteList = localRepo.DeleteList(remoteRepo);

            //Delete PBOs that are no longer in Repo
            FileHandler.DeleteList(deleteList);

            //cycle list of pbo downloads, store in temp location
            HTTP.DownloadList(downloadList);

            //hash downloaded pbos, compare to remote list of pbos

            //replace local pbos from temp location

            //update local xml as replace

            //compare two xml checksums for pbos, again

            //Run A3
            //Run.Execute(localSettings);
        }
    }
}
