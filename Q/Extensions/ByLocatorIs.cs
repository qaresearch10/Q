using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using static Q.Web.Q;
using static Q.Web.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using Q.Mobile;
using System.Xml.Linq;
using System.Threading;
using Polly.Retry;

namespace Q.Web
{
    /// <summary>
    /// By locator Is Extensions Class
    /// </summary>
    public class ByLocatorIs
    {
        private readonly By locator;
        public const int Timeout = 25;
        private const int MaxRetries = 5;

        public ByLocatorIs(By locator)
        {
            this.locator = locator;
        }

        // Define a retry policy
        RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(MaxRetries);

        /// <summary>
        /// Determines if an element specified by a predefined locator is present in the DOM within a given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A boolean value indicating whether the element is present within the timeout period.</returns>        
        public bool Present(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementExists(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Q.logger.Warn($"Element {locator} not found within {timeout} seconds.");
                return false;
            }
        }

        /// <summary>
        /// Determines if an element specified by a predefined locator is displayed on the web page within a given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A boolean value indicating whether the element is displayed within the timeout period.</returns>        
        public bool Visible(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Q.logger.Warn($"Element {locator} not visible after {timeout} seconds.");
                return false;
            }            
        }

        /// <summary>
        /// Determines if an element specified by a predefined locator is not displayed on the web page within a given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A boolean value indicating whether the element is displayed within the timeout period.</returns>        
        public bool NotVisible(int timeout = Timeout)
        {            
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Q.logger.Warn($"Element {locator} is still visible after {timeout} seconds.");
                return false;
            }
        }

        /// <summary>
        /// Checks if an element specified by a predefined locator is enabled and interactable on the web page within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <returns>A boolean value indicating whether the element is enabled within the timeout period.</returns>        
        public bool Enabled(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementToBeClickable(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Q.logger.Warn($"Element {locator} is not enabled after {timeout} seconds.");
                return false;
            }
        }

        /// <summary>
        /// Checks if an element specified by a predefined locator is not enabled and not interactable on the web page within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <returns>A boolean value indicating whether the element is enabled within the timeout period.</returns>        
        public bool Disabled(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);

            bool isEnabled = retryPolicy.Execute(() =>
            {
                return !element.Enabled;
            });
            return isEnabled;
        }

        /// <summary>
        /// Determines if an element specified by a predefined locator is selected on the web page within a given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A boolean vlaue indicating whether the element is selected within the timeout period.</returns>
        public bool Selected(int timeout = Timeout)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ExpectedConditions.ElementToBeSelected(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                Q.logger.Warn($"Element {locator} is not selected after {timeout} seconds.");
                return false;
            }
        }

        /// <summary>
        /// Determines if an element specified by a predefined locator is not selected on the web page within a given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A boolean vlaue indicating whether the element is selected within the timeout period.</returns>
        public bool NotSelected(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);

            bool isSelected = retryPolicy.Execute(() =>
            {
                return !element.Selected;
            });
            return isSelected;
        }
    }
}