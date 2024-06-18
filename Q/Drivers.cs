using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Q.Common;
using OpenQA.Selenium.Internal;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Remote;
using static Q.Common.Get;


namespace Q.Web
{
    /// <summary>
    /// Drivers Class provides methods for creating and configuring instances of Selenium WebDriver 
    /// </summary>
    public static class Drivers
    {
        private static bool _local;
        private static Uri _hub = new Uri("http://localhost:4444/wd/hub");

        public static bool RunLocally
        {
            get
            {
                return _local;
            }
        }

        public static Uri HubURL
        {
            get
            {
                return _hub;
            }
            set
            {
                _hub = value;
            }
        }

        static Drivers()
        {
            if (bool.TryParse(Parameter("RunLocally"), out bool runLocally) && runLocally)
            {
                _local = true;
            }
            else
            {
                _local = false;
            }

            var hub = string.IsNullOrEmpty(Parameter("HubUrl")) ? null : Parameter("HubUrl");
            if (hub != null)
            {
                HubURL = new Uri(hub);
            }
        }

        /// <summary>
        /// A method to create a Chrome Driver
        /// </summary>
        /// <returns>Chrome Driver</returns>
        public static IWebDriver CreateChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();

            // Retrieve and apply Chrome driver path
            string chromeDriverPath = Parameter("Chrome_DriverPath", "C:\\Downloads");

            // Apply headless mode if specified
            if (bool.TryParse(Parameter("Headless"), out bool headless) && headless)
            {
                options.AddArgument("--headless");
            }

            // Retrieve and apply Chrome options Arguments
            var arguments = Parameter("Chrome_Arguments", "").Split(';');
            foreach (var argument in arguments)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    options.AddArgument(argument);
                }
            }

            // Retrieve and apply Chrome UserProfilePreferences
            var preferences = Parameter("Chrome_UserProfilePreferences", "").Split(';');
            foreach (var preference in preferences)
            {
                var keyValue = preference.Split('=');
                if (keyValue.Length == 2)
                {
                    // Assuming all values are strings; adjust parsing as necessary for specific types
                    options.AddUserProfilePreference(keyValue[0], keyValue[1]);
                }
            }

            // Create a ChromeDriverService with custom configurations
            if (_local)
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true; // Hide the command prompt window on Windows
                return new ChromeDriver(service, options);
            }
            return new RemoteWebDriver(_hub, options.ToCapabilities());
        }

        /// <summary>
        /// A method to create a Edge Driver
        /// </summary>
        /// <returns>Edge Driver</returns>
        public static IWebDriver CreateEdgeDriver()
        {
            EdgeOptions options = new EdgeOptions();

            // Apply headless mode if specified
            if (bool.TryParse(Parameter("Headless"), out bool headless) && headless)
            {
                options.AddArgument("--headless");
            }

            // Retrieve and apply Edge options Arguments
            var arguments = Parameter("Edge_Arguments", "").Split(';');
            foreach (var argument in arguments)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    options.AddArgument(argument);
                }
            }

            // Create an EdgeDriverService with custom configurations
            if (_local)
            {
                EdgeDriverService service = EdgeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true; // Hide the command prompt window on Windows
                return new EdgeDriver(service, options);
            }
            return new RemoteWebDriver(_hub, options.ToCapabilities());
        }

        /// <summary>
        /// A method to create a Mobile Emulator Driver
        /// </summary>
        /// <returns>Mobile Emulator Driver</returns>
        public static IWebDriver CreateChromeMobileEmulatorDriver()
        {
            ChromeOptions options = new ChromeOptions();

            // Retrieve and apply Chrome driver path
            string chromeDriverPath = Parameter("Chrome_DriverPath", "C:\\Downloads");

            // Apply headless mode if specified
            if (bool.TryParse(Parameter("Headless"), out bool headless) && headless)
            {
                options.AddArgument("--headless");
            }

            // Retrieve and apply Chrome options Arguments
            var arguments = Parameter("Chrome_Arguments", "").Split(';');
            foreach (var argument in arguments)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    options.AddArgument(argument);
                }
            }

            // Retrieve and apply Chrome UserProfilePreferences
            var preferences = Parameter("Chrome_UserProfilePreferences", "").Split(';');
            foreach (var preference in preferences)
            {
                var keyValue = preference.Split('=');
                if (keyValue.Length == 2)
                {
                    // Assuming all values are strings; adjust parsing as necessary for specific types
                    options.AddUserProfilePreference(keyValue[0], keyValue[1]);
                }
            }
            options.AddArguments("use-fake-ui-for-media-stream");
            options.EnableMobileEmulation(Parameter("Device"));

            // Create a ChromeDriverService with custom configurations
            if (_local)
            {
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true; // Hide the command prompt window on Windows
                return new ChromeDriver(service, options);
            }
            return new RemoteWebDriver(_hub, options.ToCapabilities());
        }

        /// <summary>
        /// Supported Browsers
        /// </summary>
        public enum BrowserType
        {
            Chrome,
            Edge,
            MobileEmulator
        }

        /// <summary>
        /// A method to create a driver for the particular browser type
        /// </summary>
        /// <param name="browserType"></param>
        /// <returns>Driver</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IWebDriver CreateDriver()
        {
            BrowserType browserType;
            // Retrieve the browser type from the .runsettings file
            var browserName = Parameter("Browser", "Chrome"); // Default to Chrome if not specified

            // Safely parse the browser name to the BrowserType enum
            if (!Enum.TryParse(browserName, true, out browserType))
            {
                throw new ArgumentException($"Unsupported browser type: {browserName}");
            }

            return browserType switch
            {
                BrowserType.Chrome => CreateChromeDriver(),
                BrowserType.Edge => CreateEdgeDriver(),
                _ => throw new ArgumentOutOfRangeException(nameof(browserType), $"Not supported browser type: {browserType}"),
            };
        }
    }
}
