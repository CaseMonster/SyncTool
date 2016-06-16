﻿
namespace SyncTool
{
    class LocalSettings
    {
        public string server = "NULL";
        public string modfolder = "NULL";
        public string arma3file = "NULL";
        public string arma3args = "NULL";
        public string launched_once = "NULL";

        public LocalSettings(string server, string modfolder, string arma3file, string arma3args, string launch)
        {
            this.server = server;
            this.modfolder = modfolder;
            this.arma3file = arma3file;
            this.arma3args = arma3args;
            this.launched_once = launch;
        }
    }
}
