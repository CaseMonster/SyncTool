using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncTool
{
    class Program
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string REMOTE_REPO = "http://rollingkeg.com/repo/repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";
        public static string REMOTE_SETTINGS = "http://rollingkeg.com/repo/settings.xml";

        static void Main(string[] args)
        {
            //load settings (settings.xml)
            Log.Startup();
            LocalSettings localSettings   = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(REMOTE_SETTINGS);

            //load local repo info (repo.xml), generate if doesn't exist
            PBOList localRepo = XML.ReadXML(LOCAL_REPO);

            //generate object chain of loaded dirs/pbos

            //download remote repo (http://rollingkeg.com/repo/repo.xml) -> (remote.xml)
            PBOList remoteRepo = XML.ReadXML(REMOTE_REPO);

            //generate object chain of loaded dirs/pbos

            //create list of pbos that have changed, hashes that have changed
            PBOList downloadList = localRepo.DownloadList(remoteRepo);
            PBOList deleteList   = localRepo.DeleteList(remoteRepo);

            //cycle list of pbo downloads, store in temp location

            //hash downloaded pbos, compare to remote list of pbos

            //replace local pbos from temp location

            //update local xml as replace

            //compare two xml checksums for pbos, again

            //Run A3
            Run.Execute(localSettings);
        }
    }
}
