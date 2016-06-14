
namespace SyncTool
{
    class RemoteSettings
    {
        public string mods = "NULL";
        public string[] modsArray;

        public RemoteSettings(string s)
        {
            this.mods = s;
            ParseRemoteSettings(s);
        }

        public void ParseRemoteSettings(string s)
        {
            this.modsArray = s.Split(';');
        }
    }
}
