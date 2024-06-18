using OpenQA.Selenium;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Web
{
    public static class Retry
    {
        public static TResult TryCatch<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }
            catch (NoSuchElementException ex)
            {
                action.Method.HandleException(ex, "NoSuchElementException Caught in: " + action.Method.Name);
                throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                action.Method.HandleException(ex, "WebDriverTimeoutException Caught in: " + action.Method.Name);
                throw;
            }
            catch (StaleElementReferenceException ex)
            {
                if (ex.Message.Contains("stale element not found"))
                {
                    action.Method.HandleException(ex, "Stale element not found Caught in: " + action.Method.Name);
                    throw;
                }

                Q.logger.Info($"StaleElementReferenceException Caught in TryCatch: {ex.Message}");
                return Retry.Execute(() => action(), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                action.Method.HandleException(ex, "Exception Caught in: " + action.Method.Name);
                throw;
            }
        }

        public static void TryCatch(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (NoSuchElementException ex)
            {
                action.Method.HandleException(ex, "NoSuchElementException Caught in: " + action.Method.Name);
                throw;
            }
            catch (WebDriverTimeoutException ex)
            {
                action.Method.HandleException(ex, "WebDriverTimeoutException Caught in: " + action.Method.Name);
                throw;
            }
            catch (StaleElementReferenceException ex)
            {
                if (ex.Message.Contains("stale element not found"))
                {
                    action.Method.HandleException(ex, "Stale element not found Caught in: " + action.Method.Name);
                    throw;
                }

                Q.logger.Info($"StaleElementReferenceException Caught in TryCatch: {ex.Message}");
                Retry.Execute(() => action.Invoke(), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                action.Method.HandleException(ex, "Exception Caught in: " + action.Method.Name);
                throw;
            }
        }

        public static TResult TryCatchSimple<TResult>(Func<TResult> action)
        {
            try
            {
                return action();
            }            
            catch (Exception ex)
            {
                action.Method.HandleException(ex, "Exception Caught in: " + action.Method.Name);
                throw;
            }
        }

        public static void TryCatchSimple(Action action)
        {
            try
            {
                action.Invoke();
            }            
            catch (Exception ex)
            {
                action.Method.HandleException(ex, "Exception Caught in: " + action.Method.Name);
                throw;
            }
        }

        private static void Execute(Action action, TimeSpan retryInterval, int retryCount = 10)
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(retryCount, _ => retryInterval);

            policy.Execute(action);
        }

        private static T Execute<T>(Func<T> func, TimeSpan retryInterval, int retryCount = 10)
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(retryCount, _ => retryInterval);

            return policy.Execute(func);
        }        
    }
}
