using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace SyncTool
{
    class HashGenerator
    {
        public static string GetHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    // Get raw MD5
                    string md5String = Encoding.Default.GetString(md5.ComputeHash(stream));
                    
                    // Convert to plain text byte array
                    byte[] md5PlainText = Encoding.UTF8.GetBytes(md5String);
                    
                    // Generate a base64 string
                    return Convert.ToBase64String(md5PlainText);
                }
            }
        }
    }
}
