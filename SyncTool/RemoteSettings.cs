
namespace SyncTool
{
    class RemoteSettings
    {
        public string mods = "NULL";
        public bool forceHash = false;
        public string args = "NULL";

        public string[] modsArray;

        public RemoteSettings(string mods, bool forceHash, string args)
        {
            this.mods = mods;
            this.forceHash = forceHash;
            this.args = args;

            ParseModsArray(mods);
        }

        public void ParseModsArray(string s)
        {
            this.modsArray = s.Split(';');
        }
    }
}
