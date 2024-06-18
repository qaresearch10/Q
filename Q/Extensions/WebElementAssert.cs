using OpenQA.Selenium;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q.Web
{
    /// <summary>
    /// WebElement Assertions Class
    /// </summary>
    public class WebElementAssert
    {
        private readonly IWebElement element;
        public const int Timeout = 25;
        public const int RetryAttempts = 10;

        public WebElementAssert(IWebElement element)
        {
            this.element = element;
        }

        // Define a retry policy
        RetryPolicy<bool> retryPolicy = Policies.GetRetryPolicy(RetryAttempts);

        /// <summary>
        /// Asserts that the current URL contains the specified text.
        /// </summary>
        /// <param name="expectedText">The text that is expected to be found in the current URL.</param>
        /// <param name="message">The message to display if the assertion fails. Optional.</param>
        public void UrlContains(string expectedText, string message = "")
        {            
            bool isUrlCorrect = Q.driver.Url.Contains(expectedText);
            Assert.That(isUrlCorrect, Is.True, message);
        }

        /// <summary>
        /// Asserts that the specified element is visible on the page.
        /// </summary>
        /// <param name="message">The message to display if the assertion fails. Optional.</param>
        public void IsVisible(string message = "")
        {
            bool isVisible = retryPolicy.Execute(() =>
            {
                return element.Displayed;
            });
            Assert.That(isVisible, Is.True, message);
        }
    }
}
