using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Q.Common
{
    public static class SystemCfg
    {
        public static string TransferFileToServer(string localFilePath, string fileName, string remoteHostAddress, bool replaceFile = true)
        {
            ServerAuthCredentialStore credentials = new ServerAuthCredentialStore();

            string hostName = new UriBuilder(remoteHostAddress).Host;
            string remoteHostUNCPath = $@"\\{hostName}\C$";
            string remoteFilePath = Path.Combine(remoteHostUNCPath, fileName);
            
            using (NetworkConnection connection = new NetworkConnection(remoteHostUNCPath, new NetworkCredential(credentials.UserDomain, credentials.UserName, credentials.Password)))
            {
                try
                {
                    // Copy file to remote server
                    if (replaceFile && File.Exists(remoteFilePath))
                    {
                        File.Delete(remoteFilePath);
                    }

                    File.Copy(localFilePath, remoteFilePath);

                    return remoteFilePath;
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Error transferring file to remote server", e);
                }
                finally
                {
                    // Disconnect from the remote server
                    connection.Dispose();
                }
            }
        }
    }

    public class ServerAuthCredentialStore
    {
        public ServerAuthCredentialStore() 
        {
            ServerName = Dns.GetHostEntry(Get.Parameter("server")).HostName;
            UserDomain = Get.Parameter("domain");
            UserName = Get.Parameter("username");
            Password = Get.Parameter("password");
        }

        public string ServerName { get; private set; }
        public string UserDomain { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
}
