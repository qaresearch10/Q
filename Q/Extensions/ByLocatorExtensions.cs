using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static Q.Web.Retry;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Q.Mobile;
using static Q.Web.Q;
using System.Xml.Linq;

namespace Q.Web
{
    /// <summary>
    /// By locator Extensions Class
    /// </summary>
    public static class ByLocatorExtensions
    {
        public const int Timeout = 25;

        public static ByLocatorGet Get(this By locator)
        {
            return new ByLocatorGet(locator);
        }

        public static ByLocatorIs Is(this By locator)
        {
            return new ByLocatorIs(locator);
        }

        public static ByLocatorWait Wait(this By locator)
        {
            return new ByLocatorWait(locator);
        }

        public static ByLocatorAssert Assert(this By locator)
        {
            return new ByLocatorAssert(locator);
        }

        /// <summary>
        /// Performs a drag-and-drop action from the source element to the target element within the specified timeout period.
        /// </summary>
        /// <param name="source">The locator of the source element.</param>
        /// <param name="target">The locator of the target element.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be interactable, in seconds.</param>
        public static void DragAndDrop(this By source, By target, int timeout = Timeout)
        {            
            IWebElement sourceElement = GetElementEnabled(source, timeout);
            IWebElement targetElement = GetElementEnabled(target, timeout);

            // Perform the drag-and-drop action
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(driver);
                actions.DragAndDrop(sourceElement, targetElement).Perform();                
            });            
        }

        /// <summary>
        /// Performs a drag-and-drop action from the source element to the target element within the specified timeout period.
        /// </summary>
        /// <param name="source">The locator of the source element.</param>
        /// <param name="target">The target IWebElement.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be interactable, in seconds.</param>
        public static void DragAndDrop(this By source, IWebElement target, int timeout = Timeout)
        {
            IWebElement sourceElement = GetElementEnabled(source, timeout);

            // Perform the drag-and-drop action
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(driver);
                actions.DragAndDrop(sourceElement, target).Perform();                
            });
        }

        /// <summary>
        /// Selects the element identified by a predefined locator within the specified timeout period.         
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>        
        public static void Select(this By locator, int timeout = Timeout, bool scrollIntoView = false)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            if(element.Selected != true)
            {
                element.Click(timeout, scrollIntoView);
            }
        }

        /// <summary>
        /// Unselects the element identified by a predefined locator within the specified timeout period.         
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>        
        public static void UnSelect(this By locator, int timeout = Timeout, bool scrollIntoView = false)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            if (element.Selected)
            {
                element.Click(timeout, scrollIntoView);
            }
        }

        /// <summary>
        /// Clears the text from an input field identified by a predefined locator within the specified timeout period.         
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>        
        public static void Clear(this By locator, int timeout = Timeout)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            TryCatchSimple(() =>
            {
                element.Clear();
            });
        }

        /// <summary>
        /// Performs a click action on an element identified by a predefined locator, with optional scrolling behaviors to ensure the element is visible.        
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <param name="scrollIntoView">Specifies whether to automatically scroll the element into view before attempting the click.</param>
        /// <param name="scrollBy">Indicates whether to perform a custom scroll based on the specified x and y offsets before the click.</param>
        /// <param name="x">The horizontal offset in pixels to scroll by when scrollBy is true.</param>
        /// <param name="y">The vertical offset in pixels to scroll by when scrollBy is true.</param>        
        public static By Click(this By locator, int timeout = Timeout, bool scrollIntoView = true, bool scrollBy = false, int x = 0, int y = 0)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            TryCatchSimple(() =>
            {
                if (scrollIntoView)
                {
                    ScrollToElement(locator, timeout);
                }

                if (scrollBy)
                {
                    ScrollBy(locator, x, y, timeout);
                }            
                element.Click();
            });
            return locator;
        }

        /// <summary>
        /// Double-clicks on an element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void DoubleClick(this By locator, int timeout = Timeout)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.DoubleClick(element).Perform();
            });            
        }

        /// <summary>
        /// Hovers over an element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be visible, in seconds.</param>
        public static void Hover(this By locator, int timeout = Timeout)
        {
            IWebElement element = GetElementVisible(locator, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.MoveToElement(element).Perform();
            });           
        }

        /// <summary>
        /// Performs a long click (click and hold) on an element located by the given locator within the specified timeout period.
        /// </summary>       
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void LongClick(this By locator, int timeout = Timeout)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            TryCatchSimple(() =>
            {
                Actions actions = new Actions(Q.driver);
                actions.MoveToElement(element).ClickAndHold(element).Release().Build().Perform();
            });            
        }

        /// <summary>
        /// Sends a sequence of keyboard keys (as a chord) to an element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// <param name="keysToCHORD">The sequence of keyboard keys to send as a chord.</param>
        public static void SendKeyboardKeys(this By locator, int timeout = Timeout, params Keyboard.Key[] keysToCHORD)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
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
        /// Inputs the specified text into an element identified by a predefined locator, ensuring the element is accessible and interactable
        /// within the given timeout period.
        /// </summary>        
        /// <param name="text">The text to be input into the target element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Type(this By locator, string text, bool clear = true, int timeout = Timeout)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
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
        /// Selects an option from a combo box (dropdown) by the visible text of the option within the specified timeout period.
        /// </summary>        
        /// <param name="optionText">The visible text of the option to select.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        /// It may need adjustment for a custom drop-down
        public static void SelectFromComboByText(this By locator, string optionText, int timeout = Timeout)
        {
            TryCatchSimple(() =>
            {
                IWebElement comboBoxElement = GetElementEnabled(locator, timeout);
                comboBoxElement.Click();

                // Find the option by its visible text and click it
                IWebElement optionElement = GetElementEnabled(By.XPath($"//li[contains(text(), '{optionText}')]"), timeout);
                optionElement.Click();
            });            

            Q.logger.Info($"Successfully selected option '{optionText}' from combobox with locator: {locator}");
        }

        /// <summary>
        /// Selects a sub-option from a combo box by the visible text of the first option and the sub-option within the specified timeout period.
        /// </summary>        
        /// <param name="firstOptionText">The visible text of the first option to select.</param>
        /// <param name="subOptionText">The visible text of the sub-option to select.</param>
        /// <param name="timeout">The maximum time to wait for the elements to be enabled, in seconds.</param>
        /// It may need adjustment for a custom drop-down
        public static void SelectSubOptionFromComboByText(this By locator, string firstOptionText, string subOptionText, int timeout = Timeout)
        {
            TryCatchSimple(() =>
            {
                IWebElement comboBoxElement = GetElementEnabled(locator, timeout);
                comboBoxElement.Click();

                // Find the first option by its visible text and click it
                IWebElement firstOptionElement = GetElementEnabled(By.XPath($"//li[contains(text(), '{firstOptionText}')]"), timeout);
                firstOptionElement.Click();

                // Wait for the sub-option to be visible and click it
                IWebElement subOptionElement = GetElementEnabled(By.XPath($"//li[contains(text(), '{subOptionText}')]"), timeout);
                subOptionElement.Click();
            });            
        }

        /// <summary>
        /// Selects an item from a combo box (dropdown) by locating the item element within the specified timeout period.
        /// </summary>        
        /// <param name="comboItem">The locator of the item to select within the combo box.</param>
        /// <param name="timeout">The maximum time to wait for the combo box and item to be enabled, in seconds.</param>
        public static void SelectFromComboByElement(this By locator, By comboItem, int timeout = Timeout)
        {
            TryCatchSimple(() =>
            {
                LongClick(locator, timeout);
                LongClick(comboItem, timeout);
            });            
        }        

        /// <summary>
        /// Selects multiple options by their values from a dropdown element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="values">The values of the options to be selected.</param>
        /// <param name="timeout">The maximum time to wait for the dropdown to be visible, in seconds.</param>
        public static void SelectMultipleByValue(this By locator, IEnumerable<string> values, int timeout = Timeout)
        {
            IWebElement element = GetElementVisible(locator, timeout);
            SelectElement selectElement = new SelectElement(element);

            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    selectElement.SelectByValue(value);                    
                }
            }
        }

        /// <summary>
        /// Selects an option by value from a dropdown element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="value">The value of the option to be selected.</param>
        /// <param name="timeout">The maximum time to wait for the dropdown to be visible, in seconds.</param>
        public static void SelectByValue(this By locator, string value, int timeout = Timeout)
        {
            IWebElement element = GetElementVisible(locator, timeout);

            SelectElement selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);            
        }

        /// <summary>
        /// Switches the WebDriver context to the specified iframe using the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the iframe to be available, in seconds.</param>
        public static void SwitchToIFrame(this By locator, int timeout = Timeout)
        {
            WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(locator));            
        }

        /// <summary>
        /// Waits for the staleness of an element located by the given locator within the specified timeout period.
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to become stale, in seconds.</param>
        /// <returns>The locator of the element after it becomes stale.</returns>
        public static By AfterStalenessOf(this By locator, int timeout = Timeout)
        {
            WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
            IWebElement element = GetElementExists(locator, timeout);
            bool isStale = wait.Until(ExpectedConditions.StalenessOf(element));
            if(isStale)
                Q.logger.Info($"Element with locator: {locator} is now stale after waiting for {timeout} seconds.");

            return locator;
        }

        /// <summary>
        /// Finds a child element within the specified web element, after ensuring it is available.        
        /// </summary>        
        /// <param name="locator2">The Selenium By selector used to locate the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be available, in seconds.</param>
        /// <returns>An <see cref="IWebElement"/> representing the located element. 
        public static IWebElement FindElement(this By locator, By locator2, int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            return TryCatch(() =>
            {                
                return element.FindElement(locator2, timeout);
            });
        }

        /// <summary>
        /// Locates and returns a read-only collection of web elements on the page that match the specified selector criteria, within the given timeout period.       
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be available, in seconds.</param>
        /// <returns>A list of <see cref="IWebElement"/> elements. 
        public static IList<IWebElement> FindElements(this By locator, int timeout = Timeout)
        {
            IList<IWebElement> elements = new List<IWebElement>();
            IWebElement element = GetElementExists(locator, timeout);
            TryCatch(() =>
            {
                elements = element.FindElements(locator);
            });

            return elements;
        }

        /// <summary>
        /// Finds all displayed child elements within the specified web element using the given locator, after ensuring the parent element is interactable.
        /// </summary>        
        /// <param name="locator2">The locator to use for finding the displayed child elements.</param>
        /// <param name="timeout">The maximum time to wait for the parent element to be interactable, in seconds.</param>
        /// <returns>A list of displayed child web elements.</returns>
        public static IList<IWebElement> FindElementsDisplayed(this By locator, By locator2, int timeout = Timeout)
        {
            IList<IWebElement> displayedElements = new List<IWebElement>();
            IWebElement element = GetElementExists(locator, timeout);
            IList<IWebElement> allElements = element.FindElements(locator2, timeout);

            for (int i = 0; i < allElements.Count; i++)
            {
                IWebElement el = allElements.ElementAt(i);
                if (el.Displayed)
                    displayedElements.Add(el);
            }

            return displayedElements;
        }

        /// <summary>
        /// Submits a form within the given timeout period. 
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds.</param>
        public static void Submit(this By locator, int timeout = Timeout)
        {
            IWebElement element = GetElementEnabled(locator, timeout);
            element.Submit();                       
        }            

        /// <summary>
        /// Scrolls the web page until the element is in the visible area of the web browser viewport.        
        /// </summary>        
        /// <param name="timeout">The maximum time to wait for the element to be available, in seconds.</param>
        public static void ScrollIntoView(this By locator, int timeout = Timeout)
        {            
            IWebElement element = GetElementVisible(locator, timeout);
            TryCatchSimple(() =>
            {
                // Using JavaScript to scroll the element into view
                ((IJavaScriptExecutor)Q.driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);                
            });
        }

        /// <summary>
        /// Scrolls the web element into view, with an option to align it to the top or bottom of the visible area.
        /// </summary>        
        /// <param name="alignTop">If true, aligns the element to the top of the visible area; otherwise, aligns to the bottom. Default is true.</param>
        /// /// <param name="timeout">The maximum time to wait for the element to be available, in seconds.</param>
        /// <returns>The web element that was scrolled into view.</returns>
        public static IWebElement ScrollToElement(this By locator, int timeout = Timeout, bool alignTop = true)
        {
            IWebElement element = GetElementVisible(locator, timeout);
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
        /// Scrolls the current web page view by a specified number of pixels along the X (horizontal) and Y (vertical) axes.        
        /// </summary>
        /// <param name="x">The distance to scroll along the X axis (horizontal).</param>
        /// <param name="y">The distance to scroll along the Y axis (vertical).</param>
        /// <param name="timeout">The maximum time to wait for the element to be available, in seconds.</param>
        public static void ScrollBy(this By locator, int x = 0, int y = 0, int timeout = Timeout)
        {
            IWebElement element = GetElementExists(locator, timeout);
            TryCatchSimple(() =>
            {
                // Scroll by x, y pixels
                ((IJavaScriptExecutor)Q.driver).ExecuteScript($"window.scrollBy({x}, {y});");
            });
        }
    }
}
