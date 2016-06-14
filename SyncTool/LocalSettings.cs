
namespace SyncTool
{
    class LocalSettings
    {
        public string server    = "NULL";
        public string arma3file = "NULL";
        public string arma3args = "NULL";

        public LocalSettings(string server, string arma3file, string arma3args)
        {
            this.server     = server;
            this.arma3file  = arma3file;
            this.arma3args  = arma3args;
        }
    }
}
