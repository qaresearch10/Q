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
    /// By locator Get Extensions Class
    /// </summary>
    public class ByLocatorGet
    {
        private readonly By locator;        
        public const int Timeout = 25;
        private const int MaxRetries = 5;

        public ByLocatorGet(By locator)
        {
            this.locator = locator;
        }

        /// <summary>
        /// Retrieves the value of a specified attribute from an element identified by a predefined locator.
        /// </summary>
        /// <param name="attributeName">The name of the attribute whose value is to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>The value of the specified attribute of the element. If the element does not have the specified attribute, returns null.</returns>        
        public string Attribute(string attributeName, int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                Q.logger.Info($"Retrieved value of {attributeName} from element with locator: {locator}.");
                return element.GetAttribute(attributeName);
            });
        }

        /// <summary>
        /// Retrieves the value of a specified CSS property from an element identified by a predefined locator.
        /// </summary>
        /// <param name="cssProperty">The CSS property name whose value is to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>The value of the specified CSS property of the element. If the element does not have the specified CSS property, returns an empty string.</returns>
        public string CssValue(string cssProperty, int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                string value = element.GetCssValue(cssProperty);

                Q.logger.Info($"Retrieved CSS value: {value} for property '{cssProperty}' from element with locator: {locator}.");
                return value;
            });           
        }

        /// <summary>
        /// Rretrieves the value of a specified DOM attribute for an element identified by a predefined locator.
        /// </summary>
        /// <param name="domAttributeName">The name of the DOM attribute for which the value is to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>The HTML attribute's current value</returns> 
        public string DomAttribute(string domAttributeName, int timeout = Timeout)
        {            
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                string domAttribute = element.GetDomAttribute(domAttributeName);

                Q.logger.Info($"Retrieved DOM Attribute: {domAttribute} from element with locator: {locator}.");
                return domAttribute;
            });            
        }

        /// <summary>
        /// Retrieves the Dom property of the specified element.
        /// </summary>
        /// <param name="propertyName">The Dom property to be retrieved.</param>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>The JavaScript property's current value</returns> 
        public string DomProperty(string propertyName, int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                string property = element.GetDomProperty(propertyName);

                Q.logger.Info($"Retrieved Dom property: '{property}' from element with locator: {locator}.");
                return property;
            });
        }

        /// <summary>
        /// Retrieves the text content of the specified element.
        /// </summary>         
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>The text content of the specified element as a string.</returns>
        public string ElementText(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                string text = element.Text;

                Q.logger.Info($"Retrieved text: '{text}' from element with locator: {locator}.");
                return text;
            });
        }

        /// <summary>
        /// Retrieves the value of an element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be visible, in seconds.</param>
        /// <returns>The value of the specified element as a string. Returns null if value is not set.</returns>
        public string ElementValue(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatchSimple(() =>
            {
                string value = element.GetAttribute("value");

                Q.logger.Info($"Retrieved value: '{value}' from element with locator: {locator}.");
                return value;
            });            
        }        

        /// <summary>
        /// Attempts to access the shadow root of an element specified by a predefined locator within the given timeout period. 
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>An <see cref="ISearchContext"/> representing the shadow root of the targeted element.</returns>        
        public ISearchContext ShadowRoot(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);

            return TryCatchSimple(() =>
            {                
                return element.GetShadowRoot();
            });            
        }

        /// <summary>
        /// Retrieves the on-screen location (coordinates) of an element specified by a predefined locator, within the given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A <see cref="System.Drawing.Point"/> representing the X and Y coordinates of the top-left corner of the element relative to the
        /// top-left corner of the page.</returns>        
        public System.Drawing.Point Location(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);

            return TryCatchSimple(() =>
            {                
                return element.Location;
            });
        }

        /// <summary>
        /// Retrieves the size (dimensions) of an element specified by a predefined locator, within the given timeout period.            
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A <see cref="System.Drawing.Size"/> object representing the width and height of the element.</returns>        
        public System.Drawing.Size Size(int timeout = Timeout)
        {            
            IWebElement element = GetElementExists(locator, timeout);

            return TryCatchSimple(() =>
            {
                return element.Size;
            });
        }

        /// <summary>
        /// Retrieves the tag name of an element specified by a predefined locator, ensuring the element is accessible within the given timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
        /// <returns>A string representing the tag name of the located element (e.g., 'div', 'span', 'input'). </returns>        
        public string TagName(int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);

            return TryCatchSimple(() =>
            {
                return element.TagName;
            });
        }             
    }
}
