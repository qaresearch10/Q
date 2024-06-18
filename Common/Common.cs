using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Q.Common
{
    public static class Create
    {
        public static DirectoryInfo CreateFolder()
        {
            // Create a new folder in system root
            string path = Path.GetPathRoot(Environment.ExpandEnvironmentVariables("%systemroot%"));
            string folderName = Path.GetRandomFileName();
            string folderPath = Path.Combine(path, folderName);
            DirectoryInfo directoryInfo = Directory.CreateDirectory(folderPath);

            return directoryInfo;
        }
    }

    public static class Get
    {
        public static TestContext? TestContext { get; set; }

        /// <summary>
        /// Retrieves a parameter value from the TestContext's parameters collection
        /// </summary>
        /// <param name="parameterName">The name of the parameter to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the parameter is not found.</param>
        /// <returns>The parameter value if found; otherwise, the specified default value.</returns>
        public static string Parameter(string parameterName, string defaultValue = "")
        {
            var value = TestContext.Parameters.Get(parameterName);
            return value ?? defaultValue;
        }        
    }

    public static class Delete
    {
        public static void DeleteFile(string fileName, string hostAddress = null)
        {
            // Gets the hostname or IP in the event the string is a URL and/or includes port info
            string hostName = new UriBuilder(hostAddress).Host;
            string remoteServerPath = $@"\\{hostName}\C$";

            if (hostName != null)
            {                
                ServerAuthCredentialStore credentials = new ServerAuthCredentialStore();
                using (new NetworkConnection(remoteServerPath, new NetworkCredential(credentials.UserName, credentials.Password, credentials.UserDomain)))
                    File.Delete($@"{remoteServerPath}\{fileName}");
            }
            else
            {
                File.Delete(fileName);
            }
        }

    }
}
