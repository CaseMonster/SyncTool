using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyncTool
{
    class Program
    {
        public static string LOCAL_REPO = "repo.xml";
        public static string LOCAL_SETTINGS = "settings.xml";

        static void Main(string[] args)
        {
            if (false)
            {
                Application.Run(new OptionsMenu());
                return;
            }
            //load settings
            Log.Startup();
            LocalSettings localSettings = XML.ReadLocalSettingsXML(LOCAL_SETTINGS);
            RemoteSettings remoteSettings = XML.ReadRemoteSettingsXML(localSettings.server + "settings.xml");

            //load repo info
            PBOList localRepo = XML.ReadXML(LOCAL_REPO);
            PBOList remoteRepo = XML.ReadXML(localSettings.server + "repo.xml");

            Console.WriteLine("DONE");
            Console.ReadKey();

            //generate object chain of loaded dirs/pbos

            //create list of pbos that have changed, hashes that have changed
            PBOList downloadList = localRepo.DownloadList(remoteRepo);
            PBOList deleteList = localRepo.DeleteList(remoteRepo);

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
