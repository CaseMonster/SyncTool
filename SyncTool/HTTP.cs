using System.IO;
using System.Net;

namespace SyncTool
{
    class HTTP
    {
        public static void Download(string uri, string file)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(uri, file);
        }

        public static void DownloadList(PBOList dlList, LocalSettings localSettings)
        {
            Log.InfoStamp("downloading file(s)");
            foreach (PBO dlObject in dlList)
            {
                Log.Info(localSettings.server + (new DirectoryInfo(dlObject.sdir).Name) + "/" + dlObject.name);
                Download(localSettings.server + (new DirectoryInfo(dlObject.sdir).Name) + "/" + dlObject.name, dlObject.sdir + dlObject.name);
            };
            Log.Info("file(s) downloaded");
        }
    }
}
