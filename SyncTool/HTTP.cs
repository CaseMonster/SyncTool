using System.Net;

namespace SyncTool
{
    class HTTP
    {
        public static void Download(string uri, string file)
        {
            try
            {
                FileHandler.CheckFolder(file);
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile(uri, file);
            }
            catch
            {
            };
        }

        public static void DownloadList(PBOList dlList)
        {
            foreach (PBO dlObject in dlList)
            {
                Log.Info(dlObject.fileName);
                Download(Program.localSettings.server + dlObject.filePath.Replace(@"\","/") + "/" + dlObject.fileName, Program.localSettings.modfolder + "\\" + dlObject.filePath + dlObject.fileName);
            };
        }
    }
}
