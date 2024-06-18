using OpenQA.Selenium;
using Q.Log;
using Q.Web;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;

namespace Q.Log
{
    public class Logging
    {
        private ILogger _logger;

        public Logging() 
        {
            _logger = CreateLog();
        }

        public ILogger CreateLog()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With<ClassNameEnricher>()
                .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Debug,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} | {Level}] {ClassName}: {Message}{NewLine}{Exception}")
                //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void Verbose(string message)
        {
            _logger.Verbose(message);
        }

        public void Verbose(string message, params object[] args)
        {
            _logger.Verbose(message, args);
        }

        public void Debug(string message)
        {
         _logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Info(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void Warn(string message)
        {
            _logger.Warning(message);
        }

        public void Warn(string message, params object[] args)
        {
            _logger.Warning(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception e)
        {
            _logger.Error(e.Message);
        }

        public void Error(Exception exception, string message)
        {
            _logger.Error(exception, message);
        }

        public void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void Error(Exception exception, string message, params object[] args)
        {
            _logger.Error(exception, message, args);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception exception, string message)
        {
            _logger.Fatal(exception, message);
        }

        public void ArrangeSection(string message)
        {
            string prefix = Environment.NewLine + "--- ARRANGE: ";
            _logger.Information(prefix + message);
        }

        public void ActSection(string message)
        {
            string prefix = Environment.NewLine + "--- ACT: ";
            _logger.Information(prefix + message);
        }

        public void AssertSection(string message)
        {
            string prefix = Environment.NewLine + "--- ASSERT: ";
            _logger.Information(prefix + message);
        }

        public void CleanupSection(string message)
        {
            string prefix = Environment.NewLine + "--- CLEANUP: ";
            _logger.Information(prefix + message);
        }
    }

    public class ClassNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var stackTrace = new StackTrace();
            string className = "UnknownClass";

            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i).GetMethod();
                if (method.DeclaringType != typeof(Logger) && method.DeclaringType != typeof(Logging))
                {
                    className = method.DeclaringType.Name;
                    break;
                }
            }

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ClassName", className));
        }
    }

    /// <summary>
    /// Event Driven Logging class for SeleniumUtils methods
    /// </summary>
    public class LoggingEventServices
    {
        /// <summary>
        /// Method to call for SeleniumUtils Assert Inner class methods
        /// </summary>
        /// <param name="source">Method base <see cref="MethodBase"/></param>
        /// <param name="args">SeleniumEventArgs instance <seealso cref="SeleniunEventArgs"/></param>
        public static void OnAssert(object source, SeleniunEventArgs args)
        {
            var src = (MethodBase)source;

            if (args.additionalInfo != null)
            {
                src.LogStart(args.additionalInfo);
            }
            else
            {
                src.LogStart();
            }
        }

        /// <summary>
        /// Method to call for SeleniumUtils Action Inner class methods
        /// </summary>
        /// <param name="source">Method base <see cref="MethodBase"/></param>
        /// <param name="args">SeleniumEventArgs instance <seealso cref="SeleniunEventArgs"/></param>
        public static void OnAction(object source, SeleniunEventArgs args)
        {
            var src = (MethodBase)source;

            if (args.additionalInfo != null)
            {
                src.LogStart(args.additionalInfo);
            }
            else
            {
                src.LogStart();
            }
        }

        /// <summary>
        /// Method to call for SeleniumUtils Helper Inner class methods
        /// </summary>
        /// <param name="source">Method base <see cref="MethodBase"/></param>
        /// <param name="args">SeleniumEventArgs instance <seealso cref="SeleniunEventArgs"/></param>
        public static void OnHelper(object source, SeleniunEventArgs args)
        {
            var src = (MethodBase)source;

            if (args.additionalInfo != null)
            {
                src.LogStart(args.additionalInfo);
            }
            else
            {
                src.LogStart();
            }
        }

        /// <summary>
        /// Method to call for SeleniumUtils overload methods which take By Locators
        /// </summary>
        /// <param name="source">Method base <see cref="MethodBase"/></param>
        /// <param name="args">SeleniumEventArgs instance <seealso cref="SeleniunEventArgs"/></param>
        public static void OnFindElement(object source, SeleniunEventArgs args)
        {
            var src = (MethodBase)source;
            Web.Q.logger.Info("Locating IWebElement for " + src.ReflectedType.Name + "." + src.Name + " with By: " + args.byLocator);
        }
    }

    /// <summary>
    /// Custom class to inherit EventArgs with additional Selenium specific details
    /// <see cref="EventArgs"/>
    /// </summary>
    public class SeleniunEventArgs : EventArgs
    {
        /// <summary>
        /// Any additional information to log
        /// </summary>
        public object[] additionalInfo { get; set; }

        /// <summary>
        /// By Locators
        /// </summary>
        public By byLocator { get; set; }

    }
}

