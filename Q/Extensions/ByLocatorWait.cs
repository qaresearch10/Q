using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using static Q.Web.Q;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using Serilog;
using System.Collections.ObjectModel;
using Q.Mobile;
using System.Xml.Linq;
using Polly.Retry;
using Selenium.WebDriver.WaitExtensions;

namespace Q.Web
{
    /// <summary>
    /// By locator Wait Extensions Class
    /// </summary>
    public class ByLocatorWait
    {
        private readonly By locator;        
        public const int Timeout = 25;
        private const int MaxRetries = 10;

        public ByLocatorWait(By locator)
        {
            this.locator = locator;
        }

        // Define a retry policy
        RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(MaxRetries);

        /// <summary>
        /// Waits until the animation of the element is completed within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the animation to complete, in seconds.</param>
        /// <returns>Web element after animation is done</returns>
        public IWebElement UntilElementAnimationDone(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout))
                .Until(WebDriverExtensions.AnimationToComplete(element));
            return element;
        }

        /// <summary>
        /// Waits until the specified attribute of an element equals the provided value within the specified timeout period.
        /// </summary>
        /// <param name="AttrName">The name of the attribute to check.</param>
        /// <param name="AttrValue">The value of the attribute to check against.</param>
        /// <param name="timeout">The maximum time to wait for the attribute value to change, in seconds.</param>
        /// <returns>True if the attribute value equals the provided value within the timeout period, otherwise false.</returns>
        public bool UntilElementAttributeValueEquals(string AttrName, string AttrValue, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                bool result = wait.Until(driver =>
                {
                    IWebElement element = driver.FindElement(locator);
                    string actualValue = element.GetAttribute(AttrName);
                    return actualValue == AttrValue;
                });

                Q.logger.Info($"Element {locator} attribute '{AttrName}' equals '{AttrValue}'.");
                return result;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementAttributeValueEquals: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified attribute of an element does not equal the provided value within the specified timeout period.
        /// </summary>
        /// <param name="AttrName">The name of the attribute to check.</param>
        /// <param name="AttrValue">The value of the attribute to check against.</param>
        /// <param name="timeout">The maximum time to wait for the attribute value to change, in seconds.</param>
        /// <returns>True if the attribute value does not equal the provided value within the timeout period, otherwise false.</returns>
        public bool UntilElementAttributeValueDoesNotEquals(string AttrName, string AttrValue, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                bool result = wait.Until(driver =>
                {
                    IWebElement element = driver.FindElement(locator);
                    string actualValue = element.GetAttribute(AttrName);
                    return actualValue != AttrValue;
                });

                Q.logger.Info($"Element {locator} attribute '{AttrName}' does not equal '{AttrValue}'.");
                return result;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementAttributeValueNotEquals: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the class attribute of an element contains the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that should be contained in the class attribute.</param>
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the class attribute contains the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementClassContains(string text, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                bool result = wait.Until(driver =>
                {
                    IWebElement element = driver.FindElement(locator);
                    string classAttribute = element.GetAttribute("class");
                    return classAttribute.Contains(text);
                });

                Q.logger.Info($"Element {locator} class attribute contains '{text}'.");
                return result;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementClassContains: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the class attribute of an element does not contain the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that should not be contained in the class attribute.</param>
        /// <param name="timeout">The maximum time to wait for the class attribute to not contain the specified text, in seconds.</param>
        /// <returns>True if the class attribute does not contain the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementClassDoesNotContain(string text, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                bool result = wait.Until(driver =>
                {
                    IWebElement element = driver.FindElement(locator);
                    string classAttribute = element.GetAttribute("class");
                    return !classAttribute.Contains(text);
                });

                Q.logger.Info($"Element {locator} class attribute does not contain '{text}'.");
                return result;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementClassNotContains: {ex.Message}.");
            }
            return false;
        }        

        /// <summary>
        /// Waits until the specified element exists within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element exists within the timeout period, otherwise false.</returns>
        public bool UntilElementExists(int timeout = Timeout)
        {
            return locator.Is().Present();
        }

        /// <summary>
        /// Waits until the specified element does not exist within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to not exist, in seconds.</param>
        /// <returns>True if the element does not exist within the timeout period, otherwise false.</returns>
        public bool UntilElementDoesNotExist(int timeout = Timeout)
        {
            try
            {
                Q.driver.Wait(timeout * 1000).ForElement(locator).ToNotExist();

                Q.logger.Info($"The element {locator} is not present in DOM.");
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementDoesNotExist: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified element is visible within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be visible, in seconds.</param>
        /// <returns>True if the element is visible within the timeout period, otherwise false.</returns>
        public bool UntilElementIsVisible(int timeout = Timeout)
        {
            return locator.Is().Visible();
        }

        /// <summary>
        /// Waits until the specified element is not visible within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to not be visible, in seconds.</param>
        /// <returns>True if the element is not visible within the timeout period, otherwise false.</returns>
        public bool UntilElementIsNotVisible(int timeout = Timeout)
        {
            return locator.Is().NotVisible();
        }

        /// <summary>
        /// Waits until the selection state of the specified element is equal to the given state within the specified timeout period.
        /// </summary>
        /// <param name="state">The desired selection state of the element (true for selected, false for not selected).</param>
        /// <param name="timeout">The maximum time to wait for the element's selection state to be the specified state, in seconds.</param>
        /// <returns>True if the element's selection state matches the specified state within the timeout period, otherwise false.</returns>
        public bool UntilElementSelectionStateToBe(bool state, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementSelectionStateToBe(locator, state));

                Q.logger.Info($"Element {locator} Selection state is {state}.");
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementSelectionStateToBe: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified element is enabled within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <returns>True if the element is enabled within the timeout period, otherwise false.</returns>
        public bool UntilElementIsEnabled(int timeout = Timeout)
        {
            return locator.Is().Enabled();
        }

        /// <summary>
        /// Waits until the specified element is disabled within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be disabled, in seconds.</param>
        /// <returns>True if the element is disabled within the timeout period, otherwise false.</returns>
        public bool UntilElementIsDisabled(int timeout = Timeout)
        {
            return locator.Is().Disabled();
        }

        /// <summary>
        /// Waits until the specified element is selected within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be selected, in seconds.</param>
        /// <returns>True if the element is selected within the timeout period, otherwise false.</returns>
        public bool UntilElementIsSelected(int timeout = Timeout)
        {
            return locator.Is().Selected();
        }

        /// <summary>
        /// Waits until the specified element is not selected within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be not selected, in seconds.</param>
        /// <returns>True if the element is not selected within the timeout period, otherwise false.</returns>
        public bool UntilElementIsNotSelected(int timeout = Timeout)
        {
            return locator.Is().NotSelected();
        }        

        /// <summary>
        /// Waits until an element with the specified text is not visible within the specified timeout period.
        /// </summary>
        /// <param name="text">The text to search for within the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to not be visible, in seconds.</param>
        /// <returns>True if the element with the specified text is not visible within the timeout period, otherwise false.</returns>
        public bool UntilElementWithTextIsNotVisible(string text, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.InvisibilityOfElementWithText(locator, text));
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilElementWithTextIsNotVisible: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until all specified elements are present within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for all elements to be present, in seconds.</param>
        /// <returns>True if all specified elements are present within the timeout period, otherwise false.</returns>
        public bool UntilAllElementsArePresent(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilAllElementsArePresent: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified element is stale within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to become stale, in seconds.</param>
        public void ForStalenessOf(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));
                wait.Until(ExpectedConditions.StalenessOf(element));
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in ForStalenessOf: {ex.Message}.");
            }            
        }

        /// <summary>
        /// Waits until the specified text is present in the element within the specified timeout period.
        /// </summary>
        /// <param name="text">The text to wait for within the element.</param>
        /// <param name="timeout">The maximum time to wait for the text to be present, in seconds.</param>
        /// <returns>True if the text is present within the element within the timeout period, otherwise false.</returns>
        public bool UntilTextIsPresentInElement(string text, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred in UntilTextIsPresentInElement: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified text is present in the value attribute of the element within the specified timeout period.
        /// </summary>
        /// <param name="text">The text to wait for within the value attribute of the element.</param>
        /// <param name="timeout">The maximum time to wait for the text to be present in the value attribute, in seconds.</param>
        /// <returns>True if the text is present in the value attribute within the timeout period, otherwise false.</returns>
        public bool UntilTextIsPresentInElementValue(string text, int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.TextToBePresentInElementValue(locator, text));
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred UntilTextIsPresentInElementValue: {ex.Message}.");
            }
            return false;
        }

        /// <summary>
        /// Waits until all specified elements are visible within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for all elements to be visible, in seconds.</param>
        /// <returns>True if all specified elements are visible within the timeout period, otherwise false.</returns>
        public bool UntilVisibilityOfAllElements(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
                return true;
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred UntilVisibilityOfAllElements: {ex.Message}.");
            }
            return false;
        }
    }
}
