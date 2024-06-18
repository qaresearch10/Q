using Q.PageObjects;
using Q.Web;
using static Q.PageObjects.TestPO;
using static Q.Web.Q;
using NUnit.Framework;
using System.Threading;

namespace Q.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class MobileTestSuite : BaseMobileTest
    {
        [Test]
        public void TestOne()
        {
            logger.ArrangeSection("Setting up Mobile Test One");
            string wdioUrl = "https://webdriver.io/";
            
            driver.Navigate().GoToUrl(wdioUrl);

            logger.ActSection("Navigating to the test page");
            navigateToMenu(Navigation.whyWdio);

            logger.AssertSection("Asserting results");
            bool isUrlCorrect = Wait.UntilUrlToBe("https://webdriver.io/docs/why-webdriverio", 5);
            Assert.That(isUrlCorrect, Is.True);            
        }
    }
}