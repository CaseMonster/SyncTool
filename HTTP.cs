using System.Net;

namespace SyncTool
{
    class HTTP
    {
        public static void Download(string remoteName,string pboName)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(remoteName, "temp\\" + pboName);
        }
    }
}
