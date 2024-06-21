using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Q.Web;
using static Q.Web.Q;

namespace Q.PageObjects
{
    public class HerokuAppPO
    {
        #region By
        public static By
            addRemoveElements = By.PartialLinkText("Add/Remove Elements"),
            checkboxes = By.PartialLinkText("Checkboxes"),
            checkbox1 = By.XPath("//form[@id='checkboxes']//input[1]"),
            checkbox2 = By.XPath("//form[@id='checkboxes']//input[2]"),
            dynamicLoadingLink = By.XPath("//li[14]/a"),
            example1Link = By.PartialLinkText("Example 1"),
            example2Link = By.PartialLinkText("Example 2"),
            startButton = By.XPath("//button"),
            loadingDiv = By.Id("loading"),
            helloWorldDiv = By.Id("finish"),
            addElement = By.CssSelector(".example > button"),
            deleteElement = By.CssSelector(".example div#elements > button")
            ;
        #endregion


        public enum Navigation
        {
            AddRemoveElements,
            Checkboxes,
            DynamicLoadingLink
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
                case Navigation.AddRemoveElements:
                    addRemoveElements.Click();
                    break;
                case Navigation.Checkboxes:
                    checkboxes.Click();
                    break;
                case Navigation.DynamicLoadingLink:
                    dynamicLoadingLink.Click();
                    break;
            }
            driver.WaitForPageToLoad();
        }
    }
    #endregion
}