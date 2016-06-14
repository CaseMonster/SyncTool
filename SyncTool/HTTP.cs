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

        public static void DownloadList(PBOList dlList)
        {
            Log.InfoStamp("downloading file(s)");
            foreach (PBO dlObject in dlList)
                Download(dlObject.sdir + "\\" + dlObject.name, dlObject.sdir + "\\" + dlObject.name);
            Log.Info("file(s) downloaded");
        }
    }
}
