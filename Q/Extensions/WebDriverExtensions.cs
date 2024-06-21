using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Q.Web.Retry;

namespace Q.Web
{
    public static class WebDriverExtensions
    {
        private const int Timeout = 25;

        /// <summary>
        /// Finds an element within the specified timeout period, optionally waiting for animations to complete.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be located, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <param name="waitForAnimationDone">If true, waits for animations to complete after finding the element. Default is false.</param>
        /// <returns>The found web element.</returns>
        public static IWebElement FindElement(this IWebDriver driver, By locator, int timeout = Timeout, bool waitForAnimationDone = false)
        {
            //WaitForPageToLoad(driver, timeout);
            return TryCatch(() =>
            {
                WaitForElementToExist(driver, locator, timeout);
                IWebElement element = driver.FindElement(locator);
                if (waitForAnimationDone)
                {
                    WaitForAnimationToComplete(driver, element, timeout);
                    return driver.FindElement(locator);
                }
                return element;
            });
        }

        /// <summary>
        /// Finds an element that is visible within the specified timeout period, optionally waiting for animations to complete.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be visible, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <param name="waitForAnimationDone">If true, waits for animations to complete after finding the element. Default is false.</param>
        /// <returns>The found and visible web element.</returns>
        public static IWebElement FindElementVisible(this IWebDriver driver, By locator, int timeout = Timeout, bool waitForAnimationDone = false)
        {
            //WaitForPageToLoad(driver, timeout);
            return TryCatch(() =>
            {
                WaitForElementToBeVisible(driver, locator, timeout);
                IWebElement element = driver.FindElement(locator);
                if (waitForAnimationDone)
                {
                    WaitForAnimationToComplete(driver, element, timeout);
                    return driver.FindElement(locator);
                }
                return element;
            });
        }

        /// <summary>
        /// Finds an element that is enabled within the specified timeout period, optionally waiting for animations to complete.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <param name="waitForAnimationDone">If true, waits for animations to complete after finding the element. Default is false.</param>
        /// <returns>The found and enabled web element.</returns>
        public static IWebElement FindElementEnabled(this IWebDriver driver, By locator, int timeout = Timeout, bool waitForAnimationDone = false)
        {
            //WaitForPageToLoad(driver, timeout);
            return TryCatch(() =>
            {
                WaitForElementToBeVisible(driver, locator, timeout);
                IWebElement element = driver.FindElement(locator);
                WaitForElementToBeEnabled(driver, element, timeout);
                if (waitForAnimationDone)
                {
                    WaitForAnimationToComplete(driver, element, timeout);
                    return driver.FindElement(locator);
                }
                return element;
            });
        }

        /// <summary>
        /// Finds all elements that match the specified locator within the given timeout period.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to use for finding the elements.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be located, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <returns>A list of found web elements.</returns>
        public static IList<IWebElement> FindElements(this IWebDriver driver, By locator, int timeout = Timeout)
        {
            //WaitForPageToLoad(driver, timeout);
            IList<IWebElement> elements = new List<IWebElement>();
            TryCatch(() =>
            {
                WaitForElementToExist(driver, locator, timeout);
                elements = driver.FindElements(locator);
            });

            return elements;
        }

        /// <summary>
        /// Checks if an element that matches the specified locator is present within the given timeout period.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be located, in seconds.</param>
        /// <returns>True if the element is present within the timeout period, otherwise false.</returns>
        public static bool IsElementPresent(this IWebDriver driver, By locator, int timeout = Timeout)
        {
            TryCatch(() =>
            {
                WaitForElementToExist(driver, locator, timeout);
            });

            IWebElement element = driver.FindElement(locator, timeout);

            if (element.Displayed)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Waits for the web page loading to complete
        /// </summary>       
        /// <param name="timeout">The maximum time to wait for the page loading to complete, in seconds.</param>
        public static void WaitForPageToLoad(this IWebDriver driver, int timeout = Timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(drv => (drv as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));
        }

        private static void WaitForAnimationToComplete(IWebDriver driver, By locator, int timeout = Timeout)
        {
            IWebElement element = driver.FindElement(locator, timeout);
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(AnimationToComplete(element));
        }

        private static void WaitForAnimationToComplete(IWebDriver driver, IWebElement element, int timeout = Timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(AnimationToComplete(element));
        }

        private static void WaitForElementToExist(IWebDriver driver, By locator, int timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(ElementExists(locator));
        }

        private static void WaitForElementToBeVisible(IWebDriver driver, By locator, int timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(ElementIsVisible(locator));
        }

        private static void WaitForElementToBeEnabled(IWebDriver driver, IWebElement element, int timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(ElementToBeClickable(element));
        }

        /// <summary>
        /// Returns a function that waits for the specified element's animation to complete.
        /// </summary>
        /// <param name="element">The web element whose animation to wait for.</param>
        /// <returns>A function that waits for the element's animation to complete and returns true if the animation has completed, otherwise false.</returns>
        public static Func<IWebDriver, bool> AnimationToComplete(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    var jsExecutor = (IJavaScriptExecutor)driver;
                    bool isAnimating = (bool)jsExecutor.ExecuteScript("return $(arguments[0]).is(':animated')", element);
                    return !isAnimating;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception in AnimationToComplete: {ex.Message}");
                    return true;
                }
            };
        }

        /// <summary>
        /// Returns a function that waits for an element to contain the specified text.
        /// </summary>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="text">The text that the element should contain.</param>
        /// <returns>A function that waits for the element to contain the specified text.</returns>
        public static Func<IWebDriver, IWebElement> ElementContainsText(By locator, string text)
        {
            return driver =>
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
                    return wait.Until(drv =>
                    {
                        var element = drv.FindElement(locator);
                        return element.Text.Contains(text) ? element : null;
                    });
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }
            };
        }

        /// <summary>
        /// Returns a function that waits for an element located by the specified locator to be refreshed.
        /// </summary>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <returns>A function that waits for the element to be refreshed and returns the element if found, otherwise null.</returns>
        public static Func<IWebDriver, IWebElement> ElementRefreshed(By locator)
        {
            return driver =>
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
                    return wait.Until(drv =>
                    {
                        try
                        {
                            // Attempt to locate the element again to ensure it's refreshed
                            var element = drv.FindElement(locator);
                            return element;
                        }
                        catch (StaleElementReferenceException)
                        {
                            // If a stale element reference exception occurs, wait until the element is located again
                            return null;
                        }
                    });
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"An error occurred while waiting for the element to be refreshed: {ex.Message}.");
                    throw;
                }
            };
        }

        /// <summary>
        /// Returns a function that waits for the specified element to be visible.
        /// </summary>
        /// <param name="element">The web element to wait for visibility.</param>
        /// <returns>A function that waits for the element to be visible and returns the element if visible, otherwise null.</returns>        
        public static Func<IWebDriver, IWebElement> IsVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Timeout));
                    return wait.Until(d =>
                    {
                        try
                        {
                            return element.Displayed ? element : null;
                        }
                        catch (StaleElementReferenceException)
                        {
                            return null;
                        }
                    });
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (WebDriverTimeoutException)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"An error occurred while waiting for the element to be visible: {ex.Message}.", ex);
                    throw;
                }
            };

            // public static Func<IWebDriver, IWebElement> ElementHasCssAttrValue(IWebElement element, string cssAttribute, string cssValue)
            // public static Func<IWebDriver, IWebElement> ElementHasNoCssAttrValue(IWebElement element, string cssAttribute, string cssValue)
            // public static Func<IWebDriver, IWebElement> ElementHasText(By locator, string text)
            // public static Func<IWebDriver, IWebElement> ElementHasValue(By locator, string value)
            // public static Func<IWebDriver, IWebElement> ElementHasValue(IWebElement element, string value)
        }
    }
}
