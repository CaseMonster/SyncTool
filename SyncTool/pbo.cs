using System;
using System.Collections;

namespace SyncTool
{
    class PBO
    {
        public string fileName = "NULL";
        public string filePath = "NULL";
        public string fileHash = "NULL";

        public PBO(string name, string sdir, string hash)
        {
            this.fileName = name;
            this.filePath = sdir;
            this.fileHash = hash;
        }
    }
}
