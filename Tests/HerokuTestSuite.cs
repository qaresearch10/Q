using OpenQA.Selenium;
using Q.PageObjects;
using static Q.PageObjects.HerokuAppPO;
using Q.Web;
using static Q.Web.Q;
using Q.Common;

namespace Q.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class HerokuTestSuite : BaseTest
    {
        [Test]
        public void SelectUnselectCheckboxes()
        {
            logger.ArrangeSection("Setting up the Test SelectUnselectCheckboxes.");
            string wdioUrl = Get.Parameter("Heroku");

            driver.Navigate().GoToUrl(wdioUrl);
            string text = checkboxes.Get().ElementText();

            logger.ActSection("Navigating to the test page.");            
            navigateToMenu(Navigation.Checkboxes);

            logger.ActSection("Unselecting Checkboxes.");
            IWebElement el1 = driver.FindElement(checkbox1, 5);
            IWebElement el2 = driver.FindElement(checkbox2, 5);

            //checkbox1.UnSelect();
            //checkbox1.Is().Selected();
            //can be done as well

            el1.UnSelect();
            el2.UnSelect();

            bool checkbox1NotSelected = el1.Is().NotSelected();
            bool checkbox2NotSelected = el2.Is().NotSelected();

            logger.ActSection("Selecting Checkboxes.");
            el1.Select();
            el2.Select();

            bool checkbox1Selected = el1.Is().Selected();
            bool checkbox2Selected = el2.Is().Selected();

            logger.AssertSection("Asserting results.");
            Assert.Multiple(() =>
            {
                Assert.That(checkbox1NotSelected, Is.True, "Checkbox 1 is selected.");
                Assert.That(checkbox2NotSelected, Is.True, "Checkbox 2 is selected.");
                Assert.That(checkbox1Selected, Is.True, "Checkbox 1 is not selected.");
                Assert.That(checkbox2Selected, Is.True, "Checkbox 2 is not selected.");
                Assert.That(text.Equals("Checkboxes"), Is.True);
            });           
        }        

        [Test]
        public void AddRemoveElements()
        {
            logger.ArrangeSection("Setting up the Test ddRemoveElements.");
            string url = Common.Get.Parameter("Heroku");

            driver.Navigate().GoToUrl(url);

            logger.ActSection("Navigating to the test page.");
            navigateToMenu(Navigation.AddRemoveElements);

            logger.ActSection("Adding and Deleting element.");
            addElement.Click();
            bool isDeleteButtonVisible = deleteElement.Is().Visible();
            deleteElement.Click();
            bool isDeleteButtonNotVisible = deleteElement.Is().NotVisible();

            logger.AssertSection("Asserting results.");
            Assert.Multiple(() =>
            {
                Assert.That(isDeleteButtonVisible, Is.True, "Delete button is not visible.");
                Assert.That(isDeleteButtonNotVisible, Is.True, "Delete button is still visible.");
            });
        }

        [Test]
        public void DynamicLoading1()
        {
            logger.ArrangeSection("Setting up the Test DynamicLoading2");
            string url = Common.Get.Parameter("Heroku");

            driver.Navigate().GoToUrl(url);

            logger.ActSection("Navigating to the test page");
            navigateToMenu(Navigation.DynamicLoadingLink);

            logger.ActSection("Selecting Start.");
            example1Link.Click();
            startButton.Click();

            logger.AssertSection("Asserting results");
            string expectedText = "Hello World!";
            bool isHelloWorldVisible = helloWorldDiv.Is().Visible();
            string actualText = helloWorldDiv.Get().ElementText();

            Assert.Multiple(() =>
            {
                Assert.That(isHelloWorldVisible, Is.True, "'Hello World' is not visible.");
                Assert.That(actualText, Is.EqualTo(expectedText), "The text is not correct.");
            });
        }

        [Test]        
        public void DynamicLoading2()
        {
            logger.ArrangeSection("Setting up the Test DynamicLoading2");
            string url = Common.Get.Parameter("Heroku");
                  
            driver.Navigate().GoToUrl(url);
            
            logger.ActSection("Navigating to the test page");
            navigateToMenu(Navigation.DynamicLoadingLink);

            logger.ActSection("Selecting Start.");
            example2Link.Click();
            startButton.Click();

            logger.AssertSection("Asserting results");
            string expectedText = "Hello World!";
            bool isHelloWorldVisible = helloWorldDiv.Is().Visible();
            string actualText = helloWorldDiv.Get().ElementText();

            Assert.Multiple(() =>
            {
                Assert.That(isHelloWorldVisible, Is.True, "'Hello World' is not visible.");
                Assert.That(actualText, Is.EqualTo(expectedText), "The text is not correct.");
            });            
        }

        [Test]
        public void BasicAuth()
        {
            logger.ArrangeSection("Setting up the Test DynamicLoading2");
            string username = "admin";
            string password = "admin";
            string url = $"https://{username}:{password}@the-internet.herokuapp.com/basic_auth";
            driver.Navigate().GoToUrl(url);            

            logger.AssertSection("Asserting results");
            bool isTitleVisible = contentTitle.Is().Visible();
            bool isTextVisible = contentText.Is().Visible();
            string title = contentTitle.Get().ElementText();
            string text = contentText.Get().ElementText();
            
            Assert.Multiple(() =>
            {
                Assert.That(isTitleVisible, Is.True, "Title is not visible.");
                Assert.That(isTextVisible, Is.True, "Text is not visible.");
                Assert.That(title, Is.EqualTo("Basic Auth"), "Title is not correct.");
                Assert.That(text, Is.EqualTo("Congratulations! You must have the proper credentials."), "Text is not correct.");

            });
        }
    }    
}