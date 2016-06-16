using Microsoft.Win32;

namespace SyncTool
{
    class Reg
    {
        public static string GetArmaRegValue()
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432NODE\\BOHEMIA INTERACTIVE\\ARMA 3");
            return (string)registryKey.GetValue("main");
        }
    }
}
