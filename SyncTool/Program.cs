using System.Windows.Forms;

namespace SyncTool
{
    class Program
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";
        public static string QUICK_REPO = "temp.xml";

        static void Main(string[] args)
        {
            Log.Startup();

            //load settings
            LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");

            if (args.Length > 0)
            {
                if (args[0] == "-server")
                {
                    localSettings.modfolder = "\\";
                    FileHandler.HashFolders(remoteSettings, localSettings);
                    return;
                }

                if (args[0] == "-reset")
                {
                    XML.BackupXML(LOCAL_REPO);
                    XML.BackupXML(LOCAL_SETTINGS);
                    return;
                }
            };

            if (false)
            {
                Application.Run(new Launcher());
                return;
            }

            //Generate new localRepo
            //XML.BackupXML("repo.xml");

            //if(!File.Exists(LOCAL_REPO))
            //{
            //    XML.GenerateBlankXML(LOCAL_REPO);
            //}

            //We need an additional check for filenames?  then if we see a change in the dir, hash that one dir

            //Pull local repo, remote repo, generate quick repo
            PBOList remoteRepo = XML.ReadRepoXML(localSettings.server + "repo.xml");
            PBOList localRepo = XML.ReadRepoXML(LOCAL_REPO);
            FileHandler.ListFolders(remoteSettings, localSettings);
            PBOList quickRepo = XML.ReadRepoXML(QUICK_REPO);

            //Comb through directories and hash folders, if nessesary
            if (localRepo.HaveFileNamesChanged(quickRepo))
                FileHandler.HashFolders(remoteSettings, localSettings);

            //create list of pbos that have changed, hashes that have changed
            PBOList downloadList = localRepo.GenerateDownloadList(remoteRepo);
            PBOList deleteList = localRepo.GenerateDeleteList(remoteRepo);

            //Delete PBOs that are no longer in Repo
            if(deleteList.Count > 0)
                FileHandler.DeleteList(deleteList);

            //cycle list of pbo downloads, store in temp location
            if (downloadList.Count > 0)
                HTTP.DownloadList(downloadList, localSettings);

            //hash downloaded pbos, compare to remote list of pbos

            //replace local pbos from temp location

            //update local xml as replace

            //compare two xml checksums for pbos, again

            //Run A3
            //Run.Execute(localSettings);

            Log.InfoStamp("all done");
            System.Console.ReadKey();
        }
    }
}
