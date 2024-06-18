using OpenQA.Selenium;
using Q.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Mobile
{
    public static class Drivers
    {
        /// <summary>
        /// A method to create a Appium Driver
        /// </summary>
        /// <returns>Appium Driver</returns>
        /*public static AndroidDriver<AppiumWebElement> CreateAppiumDriver()
        {
            AppiumOptions options = new AppiumOptions();

            // Apply headless mode if specified
            if (bool.TryParse(CommonUtils.GetParameter("Headless"), out bool headless) && headless)
            {
                options.AddAdditionalCapability("avdArgs", "-no-window");
            }

            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "14");
            options.AddAdditionalCapability(MobileCapabilityType.DeviceName, CommonUtils.GetParameter("deviceName"));
            options.AddAdditionalCapability(MobileCapabilityType.App, CommonUtils.GetParameter("appPath"));

            Uri appiumUrl = new Uri("http://127.0.0.1:4723/wd/hub");

            // Create an Appium Android Driver
            return new AndroidDriver<AppiumWebElement>(appiumUrl, options);
        }*/
    }
}
