using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        static Get()
        {
            LoadParameters();
        }
        private static Dictionary<string, string> _parameters;

        /// <summary>
        /// Retrieves a parameter value from the RunSettings file
        /// </summary>
        /// <param name="parameterName">The name of the parameter to retrieve.</param>
        /// <param name="defaultValue">The default value to return if the parameter is not found.</param>
        /// <returns>The parameter value if found; otherwise, the specified default value.</returns>
        public static string Parameter(string parameterName, string defaultValue = "")
        {
            return _parameters.TryGetValue(parameterName, out var value) ? value : defaultValue;
        }

        // Method to load parameters from an XML file
        private static void LoadParameters()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".RunSettings");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"RunSettings file not found at {filePath}");
            }

            var doc = XDocument.Load(filePath);
            // Initialize the _parameters dictionary with values from the XML
            _parameters = doc.Descendants("Parameter")
                             .ToDictionary(
                                 x => (string)x.Attribute("name"),
                                 x => (string)x.Attribute("value")
                             );
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
