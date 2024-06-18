using System.Collections.Concurrent;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.EventArguments;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using static Q.Web.Drivers;
using Response = Titanium.Web.Proxy.Http.Response;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Web;

namespace Q.Common
{
    public class WebProxy
    {
        private static ProxyServer _proxyServer;
        private static readonly IDictionary<int, Request> _requestsHistory =
            new ConcurrentDictionary<int, Request>();
        private static readonly IDictionary<int, Response> _responsesHistory =
            new ConcurrentDictionary<int, Response>();
        private static bool _recordTraffic; // If enabled details on requests sent and responses receieved will be stored
        private static string _proxyPort = "18882";

        public static string? WatchPath { get; set; }
        public static string? WatchBody { get; set; }

        public static void Start()
        {
            _proxyServer = new ProxyServer(true);
            ExplicitProxyEndPoint explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, Convert.ToInt32(_proxyPort), true);
            _proxyServer.AddEndPoint(explicitEndPoint);
            _proxyServer.Start();
            _proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            _proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);
            _proxyServer.BeforeRequest += OnRequestCaptureTrafficEventHandler;
            _proxyServer.BeforeResponse += OnResponseCaptureTrafficEventHandler;
            _recordTraffic = false;
        }

        public static void Stop()
        {
            if (_proxyServer != null)
            {
                _proxyServer.Stop();
                _proxyServer.Dispose();
                _proxyServer = null;
            }
            ClearHistory();
        }

        public static DriverOptions GetDriverOptions()
        {
            string serverAddress = RunLocally ? $"http://localhost:{_proxyPort}" : $"http://{GetLocalIPAddress()}:{_proxyPort}";
            Proxy proxy = new Proxy
            {
                HttpProxy = serverAddress,
                SslProxy = serverAddress,
                FtpProxy = serverAddress
            };

            return GetBrowserOptionsWithProxy(proxy);
        }

        public static void EnableTrafficCapture()
        {
            _recordTraffic = true;
            WatchBody = null;
        }

        public static void DisableTrafficCapture()
        {
            _recordTraffic = false;
        }

        public static void ClearHistory()
        {
            _requestsHistory.Clear();
            _responsesHistory.Clear();
        }

        private static DriverOptions GetBrowserOptionsWithProxy(Proxy proxy)
        {
            string browserName = Common.Get.Parameter("Browser");
            BrowserType browser = (BrowserType)Enum.Parse(typeof(BrowserType), browserName);

            switch (browser)
            {
                case BrowserType.Chrome:
                    return new ChromeOptions() { Proxy = proxy };
                case BrowserType.Edge:
                    return new EdgeOptions() { Proxy = proxy };
                default:
                    return new ChromeOptions() { Proxy = proxy };
            }
        }

        private static async Task OnRequestCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
        {
            if (_recordTraffic)
            {
                if (!_requestsHistory.ContainsKey(e.HttpClient.Request.GetHashCode()) && e.HttpClient != null && e.HttpClient.Request != null)
                {
                    _requestsHistory.Add(e.HttpClient.Request.GetHashCode(), e.HttpClient.Request);
                }

                string[] urlParts = e.HttpClient.Request.Url.Split('?');
                if (WatchPath != null && urlParts.First().ToLower().Contains(WatchPath.ToLower()))
                {
                    WatchBody = e.GetRequestBodyAsString().Result;

                }
            }
        });

        private static void OnRequestBlockResourceEventHandler(object sender, SessionEventArgs e)
        {
            if (e.HttpClient.Request.RequestUri.ToString().Contains(""))
            {
                string customBody = string.Empty;
                e.Ok(Encoding.UTF8.GetBytes(customBody));
            }
        }

        private static async Task OnResponseCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(() =>
        {
            if (_recordTraffic)
            {
                if (!_responsesHistory.ContainsKey(e.HttpClient.Response.GetHashCode()) && e.HttpClient != null && e.HttpClient.Response != null)
                {
                    _responsesHistory.Add(e.HttpClient.Response.GetHashCode(), e.HttpClient.Response);
                }
            }
        });

        public static bool HasSentMatchingRequest(string path, Dictionary<string, string> queryParams, bool dumpOnMismatch = false)
        {
            string[] expectedParamKeys = queryParams.Keys.ToArray();
            bool expectedRequestSent = _requestsHistory.Values.Any(request => RequestHasTargetPathAndParams(request));

            if (dumpOnMismatch && !expectedRequestSent)
            {
                DumpRequestHistory();
            }

            return expectedRequestSent;

            bool RequestHasTargetPathAndParams(Request request)
            {
                string[] urlParts = request.Url.Split('?');
                if (!urlParts.First().Contains(path))
                    return false;

                string query = urlParts.Length == 2 ? urlParts.Last() : "";
                NameValueCollection requestQueryParams = HttpUtility.ParseQueryString(query);

                foreach (string key in requestQueryParams.AllKeys)
                {
                    string value = string.Join(",", requestQueryParams.GetValues(key));                    
                }

                return Array.TrueForAll(expectedParamKeys, key => RequestHasMatchingValueForKey(requestQueryParams, key));
            }

            bool RequestHasMatchingValueForKey(NameValueCollection requestQueryParams, string key)
            {
                string expectedParamValue = queryParams[key];
                string? actualParamValue = requestQueryParams.GetValues(key)?.First();
                return actualParamValue == expectedParamValue;
            }
        }

        private static void DumpRequestHistory()
        {
            foreach (var request in _requestsHistory.Values)
            {
                Web.Q.logger.Info($"Request: {request.Url}");
            }
        }

        private static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
