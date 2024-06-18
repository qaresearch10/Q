using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Serilog;
using Serilog.Sinks.SystemConsole;
using NUnit.Framework;
using System.Threading;
using static Q.Web.Q;

namespace Q.Web
{
    public class BaseTest
    {
        [SetUp]
        public virtual void SetupBase()
        {
            logger.Info("Creating a WebDriver");
            driver = Drivers.CreateDriver();
        }

        [TearDown]
        public virtual void TearDown()
        {
            logger.Info("Disposing the Driver");
            driver.Dispose();
        }
    }
}
