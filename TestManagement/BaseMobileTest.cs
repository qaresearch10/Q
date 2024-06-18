using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using static Q.Web.Q;

namespace Q.Web
{
    public class BaseMobileTest
    {
        [SetUp]
        public virtual void SetupBase()
        {
            Size emulatorSize = new Size(375, 812);

            driver = Drivers.CreateChromeMobileEmulatorDriver();
            driver.Manage().Window.Size = emulatorSize;
            CenterWindow(Q.driver, emulatorSize);
        }

        [TearDown]
        public virtual void TearDown()
        {
            logger.Info("Disposing the Driver");
            driver.Dispose();
        }

        static void CenterWindow(IWebDriver driver, Size windowSize)
        {
            Size screenSize = GetScreenSize();
            int x = (screenSize.Width - windowSize.Width) / 2;
            int y = (screenSize.Height - windowSize.Height) / 2;
            driver.Manage().Window.Position = new Point(x, y);
        }

        static Size GetScreenSize()
        {
            int screenWidth = GetSystemMetrics(0);
            int screenHeight = GetSystemMetrics(1);
            return new Size(screenWidth, screenHeight);
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);
    }
}
