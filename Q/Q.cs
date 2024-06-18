using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Q.Log;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;
using System.Reflection;
using static Q.Web.Retry;

namespace Q.Web
{
    public static class Q
    {
        public const int Timeout = 25;
        public static MethodBase? method;

        [ThreadStatic]
        public static IWebDriver driver;
        public static Logging logger = new Logging();

        //To do: Create Accept, Dismiss, Send Keys Alert methods

        // public static IWebElement CurrentlyFocusedElement()
        // public static string NodeIP()
        // public static void UntilAjaxComplete(int timeout)
        // public static void WaitForPageToLoad
        // public static void DragAndDrop(IWebElement source, IWebElement target)
        // public static void DragAndDrop(By source, By target, int timeout)
        // public static void SendKeyboardKeys(int timeout, params Keyboard.Key[] KeysToCHORD)
        // public static void SwitchToIFrame(By bylocator, int timeout)
        // public static void SwitchToIFrame(string FrameNameOrID, int timeout)
        // public static void SwitchToDefaultContent()
        // public static void NavigateToUrl(string Url)
        // public static void NavigateBackForwardRefresh(NavigationCommands command)
        // public static T ExecuteJavaScript<T>(string javaScriptText)
        // public static T ExecuteJavaScript<T>(string javaScriptText, params object[] args)
        // public static void ExecuteJavaScript(string javaScriptText)
        // public static void ExecuteJavaScript(string javaScriptText, params object[] args)

        /// <summary>
        /// Switches to the specified window or tab by its index.
        /// </summary>        
        /// <param name="tabWindowIndex">The index of the window or tab to switch to.</param>
        /// <param name="maxRetryCount">The maximum number of retry attempts. Default is 5.</param>
        public static void SwitchToWindowOrTab(int tabWindowIndex, int maxRetryCount = 5)
        {
            if (Q.driver == null)
            {
                throw new ArgumentNullException(nameof(Q.driver), "Driver cannot be null.");
            }

            for (int attempt = 1; attempt <= maxRetryCount; attempt++)
            {
                try
                {
                    var windowHandles = Q.driver.WindowHandles;
                    if (tabWindowIndex < 0 || tabWindowIndex >= windowHandles.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(tabWindowIndex), "Tab or window index is out of range.");
                    }

                    Q.driver.SwitchTo().Window(windowHandles[tabWindowIndex]);
                    Q.logger.Info($"Switched to window/tab with index {tabWindowIndex}.");
                    return;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Q.logger.Error($"Attempt {attempt}: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Q.logger.Warn($"Attempt {attempt}: Failed to switch to window/tab with index {tabWindowIndex}. Retrying...");
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                }
            }

            throw new Exception($"Failed to switch to window/tab with index {tabWindowIndex} after {maxRetryCount} attempts.");
        }

        /// <summary>
        /// Returns a locator that finds an element by its visible text.
        /// </summary>
        /// <param name="text">The visible text to use for locating the element.</param>
        /// <returns>A locator that can be used to find an element by its visible text.</returns>
        public static By GetLocatortByVisibleText(string text)
        {
            return By.XPath("//*[text()='" + text + "']");
        }


        public static By SomeValue(string value)
        {
            return By.XPath("//*[@somevalue='" + value + "']");
        }

        /// <summary>
        /// Returns a locator that finds an element by a partial match of its visible text within the specified node type.
        /// </summary>
        /// <param name="text">The partial visible text to use for locating the element.</param>
        /// <param name="nodeName">The node type to search within. Default is '*' which means any node type.</param>
        /// <returns>A locator that can be used to find an element by a partial match of its visible text within the specified node type.</returns>
        public static By FindElementByPartialText(string text, string nodeName = "*")
        {
            return By.XPath("//" + nodeName + "[text()[contains(.,'" + text + "')]]");
        }

        /// <summary>
        /// Returns the type of the current test method.
        /// </summary>
        /// <param name="adapter">The test context adapter instance.</param>
        /// <returns>The type of the current test method.</returns>
        public static Type TestType(this TestContext.TestAdapter adapter)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var callerAssemblies = new StackTrace().GetFrames().Select(x => x.GetMethod().ReflectedType.Assembly).Distinct().Where(x => x.GetReferencedAssemblies().Any(y => y.FullName == assembly.FullName));
            var initialAssembly = callerAssemblies.Last();
            var types = initialAssembly.GetTypes();
            var type = types.Where(x => x.FullName == adapter.ClassName).First();
            return type;
        }

        /// <summary>
        /// Waits until an element located by the specified locator exists within the given timeout period.
        /// </summary>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to exist, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <returns>The found web element if it exists within the timeout period.</returns>
        public static IWebElement GetElementExists(By locator, int timeout = Timeout)
        {
            return TryCatch(() =>
            {
                IWebElement element = Q.driver.FindElement(locator, timeout = Timeout);
                return element;
            });
        }

        /// <summary>
        /// Waits until an element located by the specified locator is visible within the given timeout period.
        /// </summary>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be visible, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <returns>The found web element if it is visible within the timeout period.</returns>
        public static IWebElement GetElementVisible(By locator, int timeout = Timeout)
        {
            return TryCatch(() =>
            {
                IWebElement element = Q.driver.FindElementVisible(locator, timeout);
                return element;
            });
        }

        /// <summary>
        /// Waits until an element located by the specified locator is enabled within the given timeout period.
        /// </summary>
        /// <param name="locator">The locator to use for finding the element.</param>
        /// <param name="timeout">The maximum time to wait for the element to be enabled, in seconds. Default is <see cref="Timeout"/>.</param>
        /// <returns>The found web element if it is enabled within the timeout period.</returns>
        public static IWebElement GetElementEnabled(By locator, int timeout = Timeout)
        {
            return TryCatch(() =>
            {
                IWebElement element = Q.driver.FindElementEnabled(locator, timeout);
                return element;
            });
        }

        /// <summary>
        /// Scrolls the web page until the specified element is in the visible area of the web browser viewport.        
        /// </summary>
        /// <param name="element">The web element to scroll into view.</param>
        public static void ScrollIntoView(IWebElement element)
        {
            // Using JavaScript to scroll the element into view
            ((IJavaScriptExecutor)Q.driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            //Just in case there is a floating menu at the top
            ScrollBy(0, -150);
        }

        /// <summary>
        /// Scrolls the current web page view by a specified number of pixels along the X (horizontal) and Y (vertical) axes.        
        /// </summary>
        /// <param name="x">The distance to scroll along the X axis (horizontal).</param>
        /// <param name="y">The distance to scroll along the Y axis (vertical).</param>
        public static void ScrollBy(int x = 0, int y = 0)
        {
            // Scroll by x, y pixels
            ((IJavaScriptExecutor)Q.driver).ExecuteScript($"window.scrollBy({x}, {y});");
        }

        /// <summary>
		/// Class representing special keys on a keyboard. All non letter and number keys
		/// </summary>
		public static class Keyboard
        {
            internal static string GetKey(Key key)
            {
                switch (key)
                {
                    case Key.Add:
                        return Keys.Add;
                    case Key.Alt:
                        return Keys.Alt;
                    case Key.ArrowDown:
                        return Keys.ArrowDown;
                    case Key.ArrowLeft:
                        return Keys.ArrowLeft;
                    case Key.ArrowRight:
                        return Keys.ArrowRight;
                    case Key.ArrowUp:
                        return Keys.ArrowUp;
                    case Key.Backspace:
                        return Keys.Backspace;
                    case Key.Cancel:
                        return Keys.Cancel;
                    case Key.Clear:
                        return Keys.Clear;
                    case Key.Command:
                        return Keys.Command;
                    case Key.Control:
                        return Keys.Control;
                    case Key.Decimal:
                        return Keys.Decimal;
                    case Key.Delete:
                        return Keys.Delete;
                    case Key.Divide:
                        return Keys.Divide;
                    case Key.Down:
                        return Keys.Down;
                    case Key.End:
                        return Keys.End;
                    case Key.Enter:
                        return Keys.Enter;
                    case Key.Equal:
                        return Keys.Equal;
                    case Key.Escape:
                        return Keys.Escape;
                    case Key.F1:
                        return Keys.F1;
                    case Key.F2:
                        return Keys.F2;
                    case Key.F3:
                        return Keys.F3;
                    case Key.F4:
                        return Keys.F4;
                    case Key.F5:
                        return Keys.F5;
                    case Key.F6:
                        return Keys.F6;
                    case Key.F7:
                        return Keys.F7;
                    case Key.F8:
                        return Keys.F8;
                    case Key.F9:
                        return Keys.F9;
                    case Key.F10:
                        return Keys.F10;
                    case Key.F11:
                        return Keys.F11;
                    case Key.F12:
                        return Keys.F12;
                    case Key.Help:
                        return Keys.Help;
                    case Key.Home:
                        return Keys.Home;
                    case Key.Insert:
                        return Keys.Insert;
                    case Key.Left:
                        return Keys.Left;
                    case Key.LeftAlt:
                        return Keys.LeftAlt;
                    case Key.LeftControl:
                        return Keys.LeftControl;
                    case Key.LeftShift:
                        return Keys.LeftShift;
                    case Key.Meta:
                        return Keys.Meta;
                    case Key.Multiply:
                        return Keys.Multiply;
                    case Key.Null:
                        return Keys.Null;
                    case Key.NumberPad0:
                        return Keys.NumberPad0;
                    case Key.NumberPad1:
                        return Keys.NumberPad1;
                    case Key.NumberPad2:
                        return Keys.NumberPad2;
                    case Key.NumberPad3:
                        return Keys.NumberPad3;
                    case Key.NumberPad4:
                        return Keys.NumberPad4;
                    case Key.NumberPad5:
                        return Keys.NumberPad5;
                    case Key.NumberPad6:
                        return Keys.NumberPad6;
                    case Key.NumberPad7:
                        return Keys.NumberPad7;
                    case Key.NumberPad8:
                        return Keys.NumberPad8;
                    case Key.NumberPad9:
                        return Keys.NumberPad9;
                    case Key.PageDown:
                        return Keys.PageDown;
                    case Key.PageUp:
                        return Keys.PageUp;
                    case Key.Pause:
                        return Keys.Pause;
                    case Key.Return:
                        return Keys.Return;
                    case Key.Right:
                        return Keys.Right;
                    case Key.Simicolon:
                        return Keys.Right;
                    case Key.Seperator:
                        return Keys.Separator;
                    case Key.Shift:
                        return Keys.Shift;
                    case Key.Space:
                        return Keys.Space;
                    case Key.Subtract:
                        return Keys.Subtract;
                    case Key.Tab:
                        return Keys.Tab;
                    case Key.Up:
                        return Keys.Up;
                    default:
                        return null;
                }
            }

            /// <summary>
            /// Enum representing Keys on a keyboard
            /// </summary>
            public enum Key
            {
#pragma warning disable 1591
                Add,
                Alt,
                ArrowDown,
                ArrowLeft,
                ArrowRight,
                ArrowUp,
                Backspace,
                Cancel,
                Clear,
                Command,
                Control,
                Decimal,
                Delete,
                Divide,
                Down,
                End,
                Enter,
                Equal,
                Escape,
                F1,
                F2,
                F3,
                F4,
                F5,
                F6,
                F7,
                F8,
                F9,
                F10,
                F11,
                F12,
                Help,
                Home,
                Insert,
                Left,
                LeftAlt,
                LeftControl,
                LeftShift,
                Meta,
                Multiply,
                Null,
                NumberPad0,
                NumberPad1,
                NumberPad2,
                NumberPad3,
                NumberPad4,
                NumberPad5,
                NumberPad6,
                NumberPad7,
                NumberPad8,
                NumberPad9,
                PageDown,
                PageUp,
                Pause,
                Return,
                Right,
                Simicolon,
                Seperator,
                Shift,
                Space,
                Subtract,
                Tab,
                Up
            }
        }

        /// <summary>
        /// Enum representing Browser Navigation Commands
        /// </summary>
        public enum NavigationCommands
        {
            /// <summary>
            /// Navigate Back
            /// </summary>
            Back,
            /// <summary>
            /// Navigate Forward
            /// </summary>
            Forward,
            /// <summary>
            /// Refresh Page
            /// </summary>
            Refresh
        }

        internal static void LogStart(this MethodBase method)
        {
            LogStart(method, "");
        }

        internal static void LogStart(this MethodBase method, params object[] args)
        {
            Q.logger.Info("");
        }

        internal static void LogEnd(this MethodBase method)
        {
            Q.logger.Info("");
        }

        internal static void LogEnd(string method)
        {
            Q.logger.Info("");
        }

        public static class Wait
        {
            /// <summary>
            /// Waits until an alert is present within the specified timeout period.
            /// </summary>
            /// <param name="timeout">The maximum time to wait for an alert to be present, in seconds.</param>
            /// <returns>True if an alert is present within the timeout period, otherwise false.</returns>
            public static bool UntilAlertIsPresent(int timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.AlertIsPresent());
                    return true;
                }
                catch (NoAlertPresentException)
                {                    
                    Q.logger.Info("No alerts present.");
                }
                return false;
            }

            /// <summary>
            /// Waits until the alert state matches the specified state within the given timeout period.
            /// </summary>
            /// <param name="state">The expected state of the alert (true for present, false for not present).</param>
            /// <param name="timeout">The maximum time to wait for the alert state to match the specified state, in seconds.</param>
            /// <returns>True if the alert state matches the specified state within the timeout period, otherwise false.</returns>
            public static bool UntilAlertStateIs(bool state, int timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.AlertState(state));
                    return state;
                }
                catch (NoAlertPresentException)
                {                    
                    Q.logger.Info("No alert present.");
                }
                return !state;
            }

            /// <summary>
            /// Waits until an element located by the specified locator exists within the given timeout period.
            /// </summary>
            /// <param name="locator">The locator to use for finding the element.</param>
            /// <param name="timeout">The maximum time to wait for the element to exist, in seconds.</param>
            /// <returns>True if the element exists within the timeout period, otherwise false.</returns>
            public static bool UntilElementExists(By locator, int timeout = Timeout)
            {
                method = MethodBase.GetCurrentMethod();
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    IWebElement element = wait.Until(ExpectedConditions.ElementExists(locator));

                    Q.logger.Info($"Element {locator} exists.");
                    return true;
                }
                catch (WebDriverTimeoutException ex)
                {
                    Q.logger.Error($"Timeout Exception in method {method}: The element {locator} does not exist after {timeout} seconds.", ex);
                }
                catch (StaleElementReferenceException ex)
                {
                    Q.logger.Error($"Stale Element: The element {locator} became stale.", ex);
                }
                Q.logger.Error($"Timeout Exception in method {method}: The element {locator} does not exist after {timeout} seconds.");
                return false;
            }

            /// <summary>
            /// Waits until the page title contains the specified text within the given timeout period.
            /// </summary>
            /// <param name="title">The text that should be contained in the page title.</param>
            /// <param name="timeout">The maximum time to wait for the title to contain the specified text, in seconds.</param>
            /// <returns>True if the page title contains the specified text within the timeout period, otherwise false.</returns>
            public static bool UntilTitleContains(string title, double timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.TitleContains(title));
                    return true;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception {ex} occurred.");
                }
                return false;
            }

            /// <summary>
            /// Waits until the page title matches the specified text within the given timeout period.
            /// </summary>
            /// <param name="title">The text that the page title should match.</param>
            /// <param name="timeout">The maximum time to wait for the title to match the specified text, in seconds.</param>
            /// <returns>True if the page title matches the specified text within the timeout period, otherwise false.</returns>
            public static bool UntilTitleIs(string title, double timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.TitleIs(title));
                    return true;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception {ex} occurred.");
                }
                return false;
            }

            /// <summary>
            /// Waits until the current URL contains the specified text within the given timeout period.
            /// </summary>
            /// <param name="urlPart">The text that should be contained in the URL.</param>
            /// <param name="timeout">The maximum time to wait for the URL to contain the specified text, in seconds.</param>
            /// <returns>True if the URL contains the specified text within the timeout period, otherwise false.</returns>
            public static bool UntilUrlContains(string urlPart, double timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.UrlContains(urlPart));
                    return true;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception {ex} occurred.");
                }
                return false;
            }

            /// <summary>
            /// Waits until the current URL matches the specified regular expression within the given timeout period.
            /// </summary>
            /// <param name="urlRegex">The regular expression that the URL should match.</param>
            /// <param name="timeout">The maximum time to wait for the URL to match the specified regular expression, in seconds. Default is <see cref="Timeout"/>.</param>
            /// <returns>True if the URL matches the specified regular expression within the timeout period, otherwise false.</returns>
            public static bool UntilUrlMatches(string urlRegex, double timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.UrlMatches(urlRegex));
                    return true;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception {ex} occurred.");
                }
                return false;
            }

            /// <summary>
            /// Waits until the current URL matches the specified URL within the given timeout period.
            /// </summary>
            /// <param name="url">The URL that the current URL should match.</param>
            /// <param name="timeout">The maximum time to wait for the URL to match the specified URL, in seconds. Default is <see cref="Timeout"/>.</param>
            /// <returns>True if the URL matches the specified URL within the timeout period, otherwise false.</returns>
            public static bool UntilUrlToBe(string url, double timeout = Timeout)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(Q.driver, TimeSpan.FromSeconds(timeout));
                    wait.Until(ExpectedConditions.UrlToBe(url));
                    return true;
                }
                catch (Exception ex)
                {
                    Q.logger.Error($"Exception {ex} occurred.");
                }
                Q.logger.Info($"Expected {url} but was {Q.driver.Url}.");
                return false;
            }
        }

        internal class Log
        {
            private static event EventHandler<SeleniunEventArgs> AssertEvent = LoggingEventServices.OnAssert;
            private static event EventHandler<SeleniunEventArgs> ActionEvent = LoggingEventServices.OnAction;
            private static event EventHandler<SeleniunEventArgs> HelperEvent = LoggingEventServices.OnHelper;
            private static event EventHandler<SeleniunEventArgs> FindElementEvent = LoggingEventServices.OnFindElement;
            internal static void OnAssertEvent(MethodBase method, params object[] AdditionalInfo)
            {
                AssertEvent(method, new SeleniunEventArgs() { additionalInfo = AdditionalInfo });
            }

            internal static void OnActionEvent(MethodBase method, params object[] AdditionalInfo)
            {
                ActionEvent(method, new SeleniunEventArgs() { additionalInfo = AdditionalInfo });
            }

            internal static void OnHelperEvent(MethodBase method, params object[] AdditionalInfo)
            {
                HelperEvent(method, new SeleniunEventArgs() { additionalInfo = AdditionalInfo });
            }

            internal static void OnFindElementEvent(MethodBase method, By ByLocator)
            {
                FindElementEvent(method, new SeleniunEventArgs() { byLocator = ByLocator });
            }
        }
    }
}
