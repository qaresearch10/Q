using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static Q.Web.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;
using System.Threading;

namespace Q.Web
{
    /// <summary>
    /// WebElement Is Extensions
    /// </summary>
    public class WebElementIs
    {
        private readonly IWebElement element;
        public const int Timeout = 25;        

        public WebElementIs(IWebElement element)
        {
            this.element = element;
        }

        /// <summary>
        /// Checks if the specified element is visible.
        /// </summary>   
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is visible, otherwise false.</returns>
        public bool Visible(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isEnabled = retryPolicy.Execute(() =>
            {
                return element.Displayed;
            });
            return isEnabled;
        }

        /// <summary>
        /// Checks if the specified element is invisible.
        /// </summary>  
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is invisible, otherwise false.</returns>
        public bool NotVisible(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isDisplayed = retryPolicy.Execute(() =>
            {
                try
                {
                    // Check if the element is not displayed
                    return !element.Displayed;
                }
                catch (NoSuchElementException)
                {
                    // Return false to trigger retry
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    // Return false to trigger retry
                    return true;
                }
            });
            return isDisplayed;
        }

        /// <summary>
        /// Checks if the specified element is enabled.
        /// </summary>   
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is enabled, otherwise false.</returns>
        public bool Enabled(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isEnabled = retryPolicy.Execute(() =>
            {
                return element.Enabled;
            });
            return isEnabled;
        }

        /// <summary>
        /// Checks if the specified element is disabled.
        /// </summary>    
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is disabled, otherwise false.</returns>
        public bool Disabled(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isEnabled = retryPolicy.Execute(() =>
            {
                return !element.Enabled;
            });
            return isEnabled;
        }

        /// <summary>
        /// Checks if the specified element is selected.
        /// </summary>          
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is selected, otherwise false.</returns>
        public bool Selected(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isSelected = retryPolicy.Execute(() =>
            {
                return element.Selected;
            });
            return isSelected;
        }

        /// <summary>
        /// Checks if the specified element is not selected.
        /// </summary>   
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>True if the element is not selected, otherwise false.</returns>
        public bool NotSelected(int timeout = Timeout)
        {
            // Define a retry policy
            RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(timeout);

            bool isSelected = retryPolicy.Execute(() =>
            {
                return !element.Selected;
            });
            return isSelected;
        }

    }
}