using OpenQA.Selenium;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using static Q.Web.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using static Q.Web.Q;

namespace Q.Web
{
    /// <summary>
    /// WebElement Extensions Class
    /// </summary>
    public static class WebElementExtensions
    {
        public const int Timeout = 25;        

        public static WebElementGet Get(this IWebElement element)
        {
            return new WebElementGet(element);
        }

        public static WebElementIs Is(this IWebElement element)
        {
            return new WebElementIs(element);
        }

        public static WebElementWait Wait(this IWebElement element)
        {
            return new WebElementWait(element);
        }

        public static WebElementAssert Assert(this IWebElement element)
        {
            return new WebElementAssert(element);
        }
        
        /// <summary>
        /// Performs a drag-and-drop action from the source element to the target element within the specified timeout period.
        /// </summary>
        /// <param name="source">The source IWebElement.</param>
        /// <param name="target">The target IWebElement.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be interactable, in seconds.</param>
        public static void DragAndDrop(this IWebElement source, IWebElement target, int timeout = Timeout)
        {
            // Perform the drag-and-drop action
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(driver);
                actions.DragAndDrop(source, target).Perform();                
            });
        }

        /// <summary>
        /// Performs a drag-and-drop action from the source element to the target element within the specified timeout period.
        /// </summary>
        /// <param name="source">The source IWebElement.</param>
        /// <param name="target">The locator of the target element.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be interactable, in seconds.</param>
        public static void DragAndDrop(this IWebElement source, By target, int timeout = Timeout)
        {          
            IWebElement targetElement = GetElementEnabled(target, timeout);

            // Perform the drag-and-drop action
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(driver);
                actions.DragAndDrop(source, targetElement).Perform();                
            });
        }        

        /// <summary>
        /// Selects the element identified by a predefined locator within the specified timeout period.         
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>        
        public static void Select(this IWebElement element, int timeout = Timeout, bool scrollIntoView = false)
        {           
            if (element.Selected != true)
            {
                element.Click(timeout, scrollIntoView);
            }
        }

        /// <summary>
        /// Unselects the element identified by a predefined locator within the specified timeout period.         
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>        
        public static void UnSelect(this IWebElement element, int timeout = Timeout, bool scrollIntoView = false)
        {            
            if (element.Selected)
            {
                element.Click(timeout, scrollIntoView);
            }
        }

        /// <summary>
        /// Clears the text from an input field identified by a predefined web element within the specified timeout period. 
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Clear(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                element.Clear();                
            });            
        }

        /// <summary>
        /// Performs a click action on an element with optional scrolling behaviors to ensure the element is visible.        
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <param name="scrollIntoView">Specifies whether to automatically scroll the element into view before attempting the click.</param>
        /// <param name="scrollBy">Indicates whether to perform a custom scroll based on the specified x and y offsets before the click.</param>
        /// <param name="x">The horizontal offset in pixels to scroll by when scrollBy is true.</param>
        /// <param name="y">The vertical offset in pixels to scroll by when scrollBy is true.</param>   
        public static void Click(this IWebElement element, int timeout = Timeout, bool scrollIntoView = true, bool scrollBy = false, int x = 0, int y = 0)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                if (scrollIntoView)
                {
                    ScrollToElement(element, timeout);
                }

                if (scrollBy)
                {
                    ScrollBy(element, x, y);
                }
                element.Click();
            });            
        }

        /// <summary>
        /// Performs a double-click action on an element after ensuring it is enabled.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void DoubleClick(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.DoubleClick(element).Perform();
            });           
        }

        /// <summary>
        /// Performs a hover action over the specified element after ensuring it is enabled.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Hover(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.MoveToElement(element).Perform();                
            });            
        }

        /// <summary>
        /// Performs a long-click (click and hold) on the specified element after ensuring it is enabled.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void LongClick(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.MoveToElement(element).ClickAndHold(element).Release().Build().Perform();                
            });            
        }

        /// <summary>
        /// Sends a sequence of keyboard keys (as a chord) to an element within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <param name="keysToCHORD">The sequence of keyboard keys to send as a chord.</param>
        public static void SendKeyboardKeys(this IWebElement element, int timeout = Timeout, params Keyboard.Key[] keysToCHORD)
        {
            WaitForElementToBeEnabled(element, timeout);
            string text = string.Empty;
            
            foreach (var key in keysToCHORD)
            {
                text += Keyboard.GetKey(key);
            }
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.Click(element).SendKeys(text).Perform();
            });            
        }

        /// <summary>
        /// Types the specified text into the web element after ensuring it is enabled.
        /// Optionally clears the existing text before typing.
        /// </summary>        
        /// <param name="text">The text to type into the element.</param>
        /// <param name="clear">If true, clears the existing text in the element before typing. Default is true.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Type(this IWebElement element, string text, bool clear = true, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            TryCatchSimple(() =>
            {
                if (clear)
                {
                    element.Clear();
                }
                Actions action = new Actions(Q.driver);
                action.MoveToElement(element).SendKeys(text).Perform();
            });
        }

        /// <summary>
        /// Finds a child element within the specified web element, after ensuring it is enabled.
        /// </summary>        
        /// <param name="locator">The locator to use for finding the child element.</param>
        /// <param name="timeout">The maximum time to wait for the parent element to be enabled, in seconds.</param>
        /// <returns>The found child web element.</returns>
        public static IWebElement FindElement(this IWebElement element, By locator, int timeout = Timeout)
        {
            WaitForElementToExist(locator, timeout);
            return TryCatch(() =>            
            {                
                IWebElement  _element = element.FindElement(locator);               
                return _element;
            });            
        }

        /// <summary>
        /// Finds all child elements within the specified web element using the given locator, after ensuring it is available.
        /// </summary>        
        /// <param name="locator">The locator to use for finding the child elements.</param>
        /// <param name="timeout">The maximum time to wait for the parent element to be available, in seconds.</param>
        /// <returns>A list of found child web elements.</returns>
        public static IList<IWebElement> FindElements(this IWebElement element, By locator, int timeout = Timeout)
        {
            IList<IWebElement> elements = new List<IWebElement>();
            WaitForElementToExist(locator, timeout);
            TryCatch(() =>
            {                
                elements = element.FindElements(locator);                
            });

            return elements;
        }

        /// <summary>
        /// Finds all displayed child elements within the specified web element using the given locator, after ensuring the parent element is interactable.
        /// </summary>        
        /// <param name="locator">The locator to use for finding the displayed child elements.</param>
        /// <param name="timeout">The maximum time to wait for the parent element to be interactable, in seconds.</param>
        /// <returns>A list of displayed child web elements.</returns>
        public static IList<IWebElement> FindElementsDisplayed(this IWebElement element, By locator, int timeout = Timeout)
        {
            IList<IWebElement> displayedElements = new List<IWebElement>();
            IList<IWebElement> allElements = element.FindElements(locator, timeout);

            for (int i = 0; i < allElements.Count; i++)
            {
                IWebElement el = allElements.ElementAt(i);
                if (el.Displayed)
                    displayedElements.Add(el);
            }

            return displayedElements;
        }

        /// <summary>
        /// Submits a form associated with the specified element within the given timeout period. 
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Submit(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeEnabled(element, timeout);
            element.Submit();
        }

        /// <summary>
        /// Scrolls the specified web element into view.
        /// </summary>        
        /// <returns>The web element that was scrolled into view.</returns>
        public static IWebElement ScrollIntoView(this IWebElement element, int timeout = Timeout)
        {
            WaitForElementToBeVisible(element, timeout);
            TryCatchSimple(() =>
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Q.driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);                
            });            

            return element;
        }

        /// <summary>
        /// Scrolls the specified web element into view, with an option to align it to the top or bottom of the visible area.
        /// </summary>        
        /// <param name="alignTop">If true, aligns the element to the top of the visible area; otherwise, aligns to the bottom. Default is true.</param>
        /// <returns>The web element that was scrolled into view.</returns>
        public static IWebElement ScrollToElement(this IWebElement element, int timeout = Timeout, bool alignTop = true)
        {
            WaitForElementToBeVisible(element, timeout);
            TryCatchSimple(() =>
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Q.driver;
                js.ExecuteScript($"arguments[0].scrollIntoView({alignTop.ToString().ToLower()});", element);

                // Adjust the scroll position to account for the menu height (e.g., 50 pixels)
                int menuHeight = 100; 
                if (alignTop)
                {
                    js.ExecuteScript($"window.scrollBy(0, -{menuHeight});");
                }
            });            

            return element;
        }

        /// <summary>
        /// Clicks the specified web element using JavaScript.
        /// </summary>   
        /// <param name="timeout">The maximum time to wait for the element to be displayed, in seconds.</param>
        /// <returns>The web element that was clicked.</returns>
        public static IWebElement JavaScriptClick(this IWebElement element,int timeout = Timeout)
        {
            TryCatch(() =>
            {                
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(driver => element.Displayed);
            });
            TryCatchSimple(() =>
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Q.driver;
                js.ExecuteScript("arguments[0].click();", element);                
            });            

            return element;
        }

        /// <summary>
        /// Scrolls the current web page view by a specified number of pixels along the X (horizontal) and Y (vertical) axes.        
        /// </summary>
        /// <param name="x">The distance to scroll along the X axis (horizontal).</param>
        /// <param name="y">The distance to scroll along the Y axis (vertical).</param>
        public static void ScrollBy(this IWebElement element, int x = 0, int y = 0, int timeout = Timeout)
        {
            WaitForElementToBeVisible(element, timeout);
            TryCatchSimple(() =>
            {
                // Scroll by x, y pixels
                ((IJavaScriptExecutor)Q.driver).ExecuteScript($"window.scrollBy({x}, {y});");
            });
        }

        private static void WaitForElementToBeEnabled(IWebElement element, int timeout = Timeout)
        {
            TryCatch(() =>
            {
                new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout))
                .Until(ElementToBeClickable(element));
            });            
        }

        private static void WaitForElementToBeVisible(IWebElement element, int timeout = Timeout)
        {
            TryCatch(() =>
            {
                WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(driver => element.Displayed);
            });
        }

        private static void WaitForElementToExist(By locator, int timeout = Timeout)
        {
            TryCatch(() =>
            {
                WebDriverWait wait = new(Q.driver, TimeSpan.FromSeconds(timeout));
                wait.Until(ElementExists(locator));
            });
        }

        private static void WaitForPageToLoad(IWebDriver driver, int timeout = Timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(drv => (drv as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));
        }
        
        private static void WaitForAnimationToComplete(IWebDriver driver, IWebElement element, int timeout = Timeout)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeout))
                .Until(AnimationToComplete(element));
        }
        
        private static Func<IWebDriver, bool> AnimationToComplete(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    // Ensure jQuery is available
                    var jsExecutor = (IJavaScriptExecutor)driver;
                    bool jQueryLoaded = (bool)jsExecutor.ExecuteScript("return typeof jQuery != 'undefined';");
                    if (!jQueryLoaded)
                    {
                        jsExecutor.ExecuteScript("var script = document.createElement('script'); script.src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js'; document.head.appendChild(script);");
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                        wait.Until(d => (bool)((IJavaScriptExecutor)d).ExecuteScript("return typeof jQuery != 'undefined';"));
                    }

                    jsExecutor = (IJavaScriptExecutor)driver;
                    bool isAnimating = (bool)jsExecutor.ExecuteScript("return $(arguments[0]).is(':animated');", element);
                    return !isAnimating;
                }                
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception in AnimationToComplete: {ex.Message}");
                    return true;
                }
            };
        }
    }
}
