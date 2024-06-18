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
    /// WebElement Get Extensions
    /// </summary>
    public class WebElementGet
    {
        private readonly IWebElement element;
        public const int Timeout = 25;
        private const int MaxRetries = 10;

        public WebElementGet(IWebElement element)
        {
            this.element = element;
        }        

        /// <summary>
        /// Retrieves the value of a specified attribute from an element.
        /// </summary>
        /// <param name="attributeName">The attribute to be retrieved.</param>
        /// <returns>The attribute's current value</returns>        
        public string Attribute(string attributeName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element), "Element cannot be null.");
            }

            return TryCatchSimple(() =>            
            {
                string attribute = element.GetAttribute(attributeName);

                Q.logger.Info($"Retrieved value of {attributeName} from element.");
                return attribute;
            });            
        }

        /// <summary>
        /// Retrieves the value of a specified CSS property from an element.
        /// </summary>
        /// <param name="cssProperty">The CSS property name whose value is to be retrieved.</param>        
        /// <returns>The value of the specified CSS property as a string.</returns>
        public string CssValue(string cssProperty)
        {
            return TryCatchSimple(() =>
            {
                string value = element.GetCssValue(cssProperty);

                Q.logger.Info($"Retrieved CSS value: '{value}' for property '{cssProperty}' from element.");
                return value;
            });            
        }

        /// <summary>
        /// Rretrieves the value of a specified DOM attribute for an element.
        /// </summary>
        /// <param name="domAttributeName">The name of the DOM attribute for which the value is to be retrieved.</param>
        /// <returns>The HTML attribute's current value</returns> 
        public string DomAttribute(string domAttributeName)
        {
            return TryCatchSimple(() =>
            {
                string domAttribute = element.GetDomAttribute(domAttributeName);

                Q.logger.Info($"Retrieved DOM Attribute: {domAttribute} from element.");
                return domAttribute;
            });
        }

        /// <summary>
        /// Retrieves the Dom property of the specified element.
        /// </summary>
        /// <param name="propertyName">The Dom property to be retrieved.</param>
        /// <returns>The JavaScript property's current value</returns> 
        public string DomProperty(string propertyName)
        {
            return TryCatchSimple(() =>
            {
                string property = element.GetDomProperty(propertyName);

                Q.logger.Info($"Retrieved Dom property: '{property}' from element.");
                return property;
            });            
        }

        /// <summary>
        /// Retrieves the text content of the specified element.
        /// </summary>               
        /// <returns>The text content of the specified element as a string.</returns>
        public string ElementText()
        {
            return TryCatchSimple(() =>
            {
                string text = element.Text;

                Q.logger.Info($"Retrieved text: '{text}' from element.");
                return text;
            });            
        }

        /// <summary>
        /// Retrieves the value of the specified element's value attribute.
        /// </summary>        
        /// <returns>The value of the specified element's value attribute as a string.</returns>
        public string ElementValue()
        {
            return TryCatchSimple(() =>
            {
                string value = element.GetAttribute("value");

                Q.logger.Info($"Retrieved value: '{value}' from element.");
                return value;
            });            
        }       

        /// <summary>
        /// Retrieves the Shadow Root of the specified element.
        /// </summary>        
        /// <returns>An <see cref="ISearchContext"/> representing the shadow root of the targeted element.</returns>    
        public ISearchContext ShadowRoot()
        {
            return TryCatchSimple(() =>
            {                
                return element.GetShadowRoot();
            });
        }

        /// <summary>
        /// Retrieves the Location of the specified element.
        /// </summary>        
        /// <returns>A <see cref="System.Drawing.Point"/> representing the X and Y coordinates of the top-left corner of the element relative to the
        /// top-left corner of the page.</returns>        
        public System.Drawing.Point Location()
        {
            return TryCatchSimple(() =>
            {
                return element.Location;
            });
        }

        /// <summary>
        /// Retrieves the Size of the specified element.
        /// </summary>        
        /// <returns>A <see cref="System.Drawing.Size"/> object representing the width and height of the element.</returns>  
        public System.Drawing.Size Size()
        {
            return TryCatchSimple(() =>
            {
                return element.Size;
            });
        }

        /// <summary>
        /// Retrieves the Tag name of the specified element.
        /// </summary>        
        /// <returns>The Tag name of the element.</returns>
        public string Tagname()
        {
            return TryCatchSimple(() =>
            {
                return element.TagName;
            });
        }
    }
}
