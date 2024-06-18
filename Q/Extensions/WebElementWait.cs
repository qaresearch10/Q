using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Polly.Retry;
using Selenium.WebDriver.WaitExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Q.Web
{
    public class WebElementWait
    {
        private readonly IWebElement element;
        private readonly IWebDriver driver = Q.driver;
        public const int Timeout = 25;
        private const int MaxRetries = 10;

        public WebElementWait(IWebElement element)
        {
            this.element = element;
        }               

        /// <summary>
        /// Waits until the specified attribute of an element equals the given value within the specified timeout period.
        /// </summary>
        /// <param name="attrName">The name of the attribute to check.</param>
        /// <param name="attrValue">The value that the attribute should equal.</param>
        /// <param name="timeout">The maximum time to wait for the attribute to equal the specified value, in seconds.</param>
        /// <returns>True if the attribute equals the specified value within the timeout period, otherwise false.</returns>
        public bool UntilElementAttributeValueEquals(string attrName, string attrValue, int timeout = Timeout)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Element cannot be null.");
            }

            try
            {
                return element.Wait(timeout * 1000).ForAttributes().ToContainWithValue(attrName, attrValue);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the attribute {attrName} equals {attrValue}: {ex.Message}.");                
            }
            return false;
        }

        /// <summary>
        /// Waits until the specified attribute of an element does not equal the given value within the specified timeout period.
        /// </summary>
        /// <param name="attrName">The name of the attribute to check.</param>
        /// <param name="attrValue">The value that the attribute should not equal.</param>
        /// <param name="timeout">The maximum time to wait for the attribute to not equal the specified value, in seconds.</param>
        /// <returns>True if the attribute does not equal the specified value within the timeout period, otherwise false.</returns>
        public bool UntilElementAttributeValueDoesNotEqual(string attrName, string attrValue, int timeout = Timeout)
        {
            try
            {
                return element.Wait(timeout * 1000).ForAttributes().ToContainWithoutValue(attrName, attrValue);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the attribute {attrName} does not equal {attrValue}: {ex.Message}.");                
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
                return element.Wait(timeout * 1000).ForClasses().ToContain(text);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element class contains {text}: {ex.Message}.");                
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
                return element.Wait(timeout * 1000).ForClasses().ToNotContain(text);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element class does not contain {text}: {ex.Message}.");                
            }
            return false;
        }

        /// <summary>
        /// Checks if the specified element is visible.
        /// </summary>               
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is visible, otherwise false.</returns>
        public bool UntilElementIsVisible(int timeout = Timeout)
        {
            return element.Is().Visible(timeout);
        }

        /// <summary>
        /// Checks if the specified element is not visible.
        /// </summary>  
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is not visible, otherwise false.</returns>
        public bool UntilElementIsNotVisible(int timeout = Timeout)
        {
            return element.Is().NotVisible(timeout);
        }

        /// <summary>
        /// Checks if the specified element is enabled.
        /// </summary>    
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is enabled, otherwise false.</returns>
        public bool UntilElementIsEnabled(int timeout = Timeout)
        {
            return element.Is().Enabled(timeout);
        }

        /// <summary>
        /// Checks if the specified element is disabled.
        /// </summary>   
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is disabled, otherwise false.</returns>
        public bool UntilElementIsDisabled(int timeout = Timeout)
        {
            return element.Is().Disabled(timeout);
        }

        /// <summary>
        /// Checks if the specified element is selected.
        /// </summary>      
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is selected, otherwise false.</returns>
        public bool IsSelected(int timeout = Timeout)
        {
            return element.Is().Selected(timeout);
        }

        /// <summary>
        /// Checks if the specified element is not selected.
        /// </summary>    
        /// <param name="timeout">The maximum time to wait for the class attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the element is not selected, otherwise false.</returns>
        public bool IsNotSelected(int timeout = Timeout)
        {
            return element.Is().NotSelected(timeout);
        }        

        /// <summary>
        /// Waits until the text content of an element equals the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that the element's text content should equal.</param>
        /// <param name="timeout">The maximum time to wait for the text content to equal the specified text, in seconds.</param>
        /// <returns>True if the element's text content equals the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementTextEquals(string text, int timeout = Timeout)
        {
            try
            {
                return element.Wait(timeout * 1000).ForText().ToEqual(text);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element text equals {text}: {ex.Message}.");                
            }
            return false;
        }

        /// <summary>
        /// Waits until the text content of an element does not equal the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that the element's text content should not equal.</param>
        /// <param name="timeout">The maximum time to wait for the text content to not equal the specified text, in seconds.</param>
        /// <returns>True if the element's text content does not equal the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementTextDoesNotEqual(string text, int timeout = Timeout)
        {            
            try
            {
                return element.Wait(timeout * 1000).ForText().ToNotEqual(text);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element text does not equal {text}: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// Waits until the text content of an element contains the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that the element's text content should contain.</param>
        /// <param name="timeout">The maximum time to wait for the text content to contain the specified text, in seconds.</param>
        /// <returns>True if the element's text content contains the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementTextContains(string text, int timeout = Timeout)
        {
            try
            {
                return  element.Wait(timeout * 1000).ForText().ToContain(text);                
            }            
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element contains text: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// Waits until the text content of an element does not contain the specified text within the specified timeout period.
        /// </summary>
        /// <param name="text">The text that the element's text content should not contain.</param>
        /// <param name="timeout">The maximum time to wait for the text content to not contain the specified text, in seconds.</param>
        /// <returns>True if the element's text content does not contain the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementTextDoesNotContain(string text, int timeout = Timeout)
        {
            try
            {
                return element.Wait(timeout * 1000).ForText().ToNotContain(text);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element does not contain text: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// Waits until the value attribute of an element contains the specified text within the specified timeout period.
        /// </summary>
        /// <param name="value">The text that should be contained in the value attribute.</param>
        /// <param name="timeout">The maximum time to wait for the value attribute to contain the specified text, in seconds.</param>
        /// <returns>True if the value attribute contains the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementValueContains(string value, int timeout = Timeout)
        {
            try
            {
                return element.Wait(timeout * 1000).ForAttributes().ToContainWithValue("value", value);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element value contains text: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// Waits until the value attribute of an element does not contain the specified text within the specified timeout period.
        /// </summary>
        /// <param name="value">The text that should not be contained in the value attribute.</param>
        /// <param name="timeout">The maximum time to wait for the value attribute to not contain the specified text, in seconds.</param>
        /// <returns>True if the value attribute does not contain the specified text within the timeout period, otherwise false.</returns>
        public bool UntilElementValueDoesNotContain(string value, int timeout = Timeout)
        {
            try
            {
                return element.Wait(timeout * 1000).ForAttributes().ToContainWithoutValue("value", value);
            }
            catch (Exception ex)
            {
                Q.logger.Error($"An unexpected error occurred while checking if the element value does not contain text: {ex.Message}.");
                throw;
            }
        }

        /// <summary>
        /// Waits until the specified element has finished its animation.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the animation to complete, in seconds.</param>
        /// <returns>The web element after the animation is done.</returns>
        public IWebElement UntilElementAnimationDone(int timeout = Timeout)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Q.driver;
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));

                bool isAnimationDone = wait.Until(driver =>
                {
                    return (bool)js.ExecuteScript(@"
                    var elem = arguments[0];
                    var hasAnimation = window.getComputedStyle(elem).animationName !== 'none' || window.getComputedStyle(elem).transitionProperty !== 'none';
                    if (!hasAnimation) return true;
                    var animations = elem.getAnimations ? elem.getAnimations() : [];
                    var transitions = elem.getTransitions ? elem.getTransitions() : [];
                    return animations.length === 0 && transitions.length === 0;
                ", element);
                });

                if (isAnimationDone)
                {
                    Q.logger.Info("Element animation is done.");
                }
                else
                {
                    Q.logger.Warn("Element animation did not complete within the specified timeout.");
                    throw new WebDriverTimeoutException("Element animation did not complete within the specified timeout.");
                }
            }            
            catch (Exception ex)
            {
                Q.logger.Error($"An error occurred while waiting for the element animation to complete: {ex.Message}.");
                throw;
            }

            return element;
        }

        /// <summary>
        /// Waits until the animation of the element is completed within the specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the animation to complete, in seconds.</param>
        /// <returns>Web element after animation is done</returns>
        public IWebElement UntilElementAnimationDone2(int timeout = Timeout)
        {
            try
            {
                new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout))
                .Until(WebDriverExtensions.AnimationToComplete(element));
                return element;
            }            
            catch (Exception ex)
            {
                Q.logger.Error($"An error occurred while waiting for the element animation to complete: {ex.Message}.");
                throw;
            }            
        }
    }
}
