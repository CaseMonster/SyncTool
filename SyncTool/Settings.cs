using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncTool
{
    class Settings
    {
        public string server    = "NULL";
        public string mods      = "NULL";
        public string arma3dir  = "NULL";
        public string arma3args = "NULL";

        public Settings(string server, string arma3dir, string arma3args)
        {
            this.server     = server;
            this.arma3dir   = arma3dir;
            this.arma3args  = arma3args;
        }
    }
}
