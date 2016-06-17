
namespace SyncTool
{
    class RemoteSettings
    {
        public string mods = "NULL";
        public string forceHash = "NULL";
        public string version = "NULL";
        public string downloadLocation = "NULL";

        public string[] modsArray;

        public RemoteSettings(string mods, string forceHash, string version, string downloadsLocation)
        {
            this.mods = mods;
            this.forceHash = forceHash;
            this.version = version;
            this.downloadLocation = downloadsLocation;

            ParseModsArray(mods);
        }

        public void ParseModsArray(string s)
        {
            this.modsArray = s.Split(';');
        }
    }
}
