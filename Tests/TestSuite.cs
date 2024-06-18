using OpenQA.Selenium;
using Q.PageObjects;
using Q.Web;
using static Q.Web.Q;
using static Q.PageObjects.TestPO;
using static Q.PageObjects.HerokuAppPO;

namespace Q.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class TestSuite : BaseTest
    {
        [Test]
        public void TestZero()
        {
            logger.ArrangeSection("Navigating to the Home page.");
            string wdioUrl = "https://the-internet.herokuapp.com";
            driver.Navigate().GoToUrl(wdioUrl);

            logger.ActSection("Clicking on the Checkboxes button");
            string text = HerokuAppPO.checkboxes.Get().ElementText();            
            
            navigateToMenu(HerokuAppPO.Navigation.Checkboxes);

            IWebElement el1 = driver.FindElement(checkbox1);
            IWebElement el2 = driver.FindElement(checkbox2);

            el1.UnSelect();
            el2.UnSelect();

            bool checkbox1Selected = el1.Is().NotSelected();
            bool checkbox2Selected = el2.Is().NotSelected();

            //Thread.Sleep(1000);
            Assert.Multiple(() =>
            {                
                Assert.That(checkbox1Selected, Is.True);
                Assert.That(checkbox2Selected, Is.True);
                Assert.That(text.Equals("Checkboxes"), Is.True);
            });           
        }

        [Test]
        [Ignore("")]
        public void TestOne()
        {
            string wdioUrl = "https://webdriver.io/";

            driver.Navigate().GoToUrl(wdioUrl);

            //search("Alex");
            string buttonText = getStartedLink.Get().ElementText();
            string attribute = getStartedLink.Get().Attribute("class");
            string cssValue = getStartedLink.Get().CssValue("font-weight");
            bool isVisible = getStartedLink.Wait().UntilElementIsVisible();

            navigateToMenu(TestPO.Navigation.getStarted);
            assumeUrl("https://webdriver.io/docs/gettingstarted");
        }

        [Test]
        public void TestTwo()
        {
            logger.ArrangeSection("Setting up the Test Two");
            string wdioUrl = "https://webdriver.io/";
            string expectedUrl = "https://webdriver.io/docs/why-webdriverio";

            //Interceptor interceptor = new Interceptor(driver);
            //await interceptor.InterceptAsync();
            By locator = By.Id("Test");
            
            driver.Navigate().GoToUrl(wdioUrl);

            logger.ActSection("Navigating to the test page");
            navigateToMenu(TestPO.Navigation.whyWdio);

            
            logger.AssertSection("Asserting results");
            //assumeUrl(expectedUrl);
            bool isUrlCorrect = Wait.UntilUrlToBe(expectedUrl, 10);
            Assert.That(isUrlCorrect, Is.True, $"Expected URL: {expectedUrl} but was ...");
        }

        [Test]        
        public void TestThree()
        {
            logger.ArrangeSection("Setting up the Test Three");
            string url = "https://the-internet.herokuapp.com/?ref=hackernoon.com";
                  
            driver.Navigate().GoToUrl(url);
            
            logger.ActSection("Navigating to the test page");
            dynamicLoadingLink.Click();
            example2Link.Click();
            startButton.Click();

            logger.AssertSection("Asserting results");
            bool isVisible = helloWorldDiv.Wait().UntilElementIsVisible();
            Assert.That(isVisible, Is.True);
        }
    }    
}