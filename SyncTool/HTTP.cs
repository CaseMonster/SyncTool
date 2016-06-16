using System.Net;

namespace SyncTool
{
    class HTTP
    {
        public static void Download(string uri, string file)
        {
            FileHandler.CheckFolder(file);
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(uri, file);
        }

        public static void DownloadList(PBOList dlList, LocalSettings localSettings)
        {
            foreach (PBO dlObject in dlList)
            {
                Log.Info(dlObject.fileName);
                Download(localSettings.server + dlObject.filePath.Replace(@"\","/") + "/" + dlObject.fileName, localSettings.modfolder + "\\" + dlObject.filePath + dlObject.fileName);
            };
            Log.Info("file(s) downloaded");
        }
    }
}
