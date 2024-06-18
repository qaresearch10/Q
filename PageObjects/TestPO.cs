using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Q.Web;
using static Q.Web.Q;

namespace Q.PageObjects
{
    public class TestPO
    {
        const int Timeout = 25;
        #region By        
        public static By
            getStartedLink = By.XPath("//div[@class=\"buttons_pzbO\"]/a[1]"),
            whyWdioLink = By.XPath("//div[@class=\"buttons_pzbO\"]/a[2]"),
            navButtons = By.XPath("//div[@class='buttons_pzbO']"),
            searchB = By.CssSelector(".navbarSearchContainer_Bca1 button"),
            searchI = By.CssSelector("form input"),

            dynamicLoadingLink = By.XPath("//li[14]/a"),
            example2Link = By.PartialLinkText("Example 2"),
            startButton = By.XPath("//button"),
            loadingDiv = By.Id("loading"),
            helloWorldDiv = By.Id("finish")
            ;
        #endregion

        public enum Navigation
        {
            getStarted,
            whyWdio
        }

        #region Methods
        /// <summary>
        /// Clicks on the specified menu option
        /// </summary>
        /// <param name="option"></param>
        public static void navigateToMenu(Navigation option)
        {            
            switch (option)
            {
                case Navigation.getStarted:
                    getStartedLink.Click();
                    break;
                case Navigation.whyWdio:
                    whyWdioLink.Click();
                    break;
            }
        }

        /// <summary>
        /// Search for text
        /// </summary>
        /// <param name="text"></param>
        public static void search(string text)
        {
            IWebElement el = driver.FindElement(getStartedLink, Timeout);
            el.Click();
            getStartedLink.Click(); ;
            searchB.Click();
            searchI.Type(text);
            searchI.Type(Keys.Enter);
        }

        /// <summary>
        /// Assert the expected url equals to actual url
        /// </summary>
        /// <param name="url"></param>
        public static void assumeUrl(String url)
        {
            string actualUrl = driver.Url;
            Assume.That(actualUrl, Is.EqualTo(url), $"The URL is incorrect.");
        }        
        #endregion
    }
}
