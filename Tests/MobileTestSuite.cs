using Q.PageObjects;
using Q.Web;
using static Q.PageObjects.HerokuAppPO;
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
            string herokuUrl = Common.Get.Parameter("Heroku");
            
            driver.Navigate().GoToUrl(herokuUrl);

            logger.ActSection("Navigating to the test page");
            navigateToMenu(Navigation.Checkboxes);

            logger.AssertSection("Asserting results");
            string expectedUrl = "https://the-internet.herokuapp.com/checkboxes";
            bool isUrlCorrect = Wait.UntilUrlToBe(expectedUrl, 5);
            Assert.That(isUrlCorrect, Is.True, "Url is not correct.");            
        }
    }
}