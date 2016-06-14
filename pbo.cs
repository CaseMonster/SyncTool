using System;
using System.Collections;

namespace SyncTool
{
    class PBO
    {
        public string name = "NULL";
        public string sdir = "NULL";
        public string hash = "NULL";

        public PBO(string name, string sdir, string hash)
        {
            this.name = name;
            this.sdir = sdir;
            this.hash = hash;
        }
    }
}
