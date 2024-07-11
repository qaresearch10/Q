using OpenQA.Selenium;
using Polly.Retry;
using static Q.Web.Q;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q.Web
{
    /// <summary>
    /// By locator Assertions Class
    /// </summary>
    public class ByLocatorAssert
    {
        private readonly By locator;        
        public const int Timeout = 25;
        public const int RetryAttempts = 10;

        public ByLocatorAssert(By locator)
        {
            this.locator = locator;
        }

        // Define a retry policy
        RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(RetryAttempts);

        /// <summary>
        /// Asserts that an element specified by the locator is visible within a specified timeout period.
        /// </summary>
        /// <param name="message">The message to display if the assertion fails. Optional.</param>
        /// <param name="timeout">The maximum time to wait for the element to become visible, in seconds.</param>
        public void IsVisible(string message = "", int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            bool isVisible = retryPolicy.Execute(() =>
            {
                return element.Displayed;
            });
            Assert.That(isVisible, Is.True, message);
        }        
    }
}