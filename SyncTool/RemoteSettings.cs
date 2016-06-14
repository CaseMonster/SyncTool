
namespace SyncTool
{
    class RemoteSettings
    {
        public string mods = "NULL";
        public string[] modsArray;

        public string forceHash = "NULL";
        public string version = "NULL";

        public RemoteSettings(string mods, string version, string forceHash)
        {
            this.mods = mods;
            ParseRemoteSettings(mods);
            this.version = version;
            this.forceHash = forceHash;
        }

        public void ParseRemoteSettings(string s)
        {
            this.modsArray = s.Split(';');
        }
    }
}
