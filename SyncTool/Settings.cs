
namespace SyncTool
{
    class Settings
    {
        public string server    = "NULL";
        public string arma3file = "NULL";
        public string arma3args = "NULL";

        public Settings(string server, string arma3dir, string arma3args)
        {
            this.server     = server;
            this.arma3file  = arma3dir;
            this.arma3args  = arma3args;
        }
    }
}
