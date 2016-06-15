
namespace SyncTool
{
    class RemoteSettings
    {
        public string mods = "NULL";
        public string[] modsArray;

        public string forceHash = "NULL";
        public string version = "NULL";
        public string currentVersion = "NULL";

        public RemoteSettings(string mods, string version, string forceHash)
        {
            this.mods = mods;
            ParseRemoteSettings(mods);
            this.forceHash = forceHash;
            this.version = version;
            this.currentVersion = currentVersion = "NULL";
        }

        public void ParseRemoteSettings(string s)
        {
            this.modsArray = s.Split(';');
        }
    }
}
