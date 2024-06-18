using OpenQA.Selenium;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Q.Web
{
    public static class Policies
    {
        public static void Execute(Action action)
        {
            var context = new Context
            {
                ["Action"] = action
            };

            generalExceptionPolicy.Execute((ctx) =>
            {
                action();
            }, context);
        }

        public static Policy staleElementPolicy = Policy
            .Handle<StaleElementReferenceException>()
            .WaitAndRetry(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {                    
                    Q.logger.Error($"Retry {retryCount} due to Exception: {exception.Message}. Waiting {timeSpan} before next retry.");
                });

        public static Policy generalExceptionPolicy = Policy
           .Handle<Exception>()
           .Fallback(
               fallbackAction: (context) =>
               {
                   throw new Exception("General exception occurred after retries.");
               },
               onFallback: (exception, context) =>
               {
                   var action = context["Action"] as Action;
                   var methodName = action.Method.Name;
                   Q.logger.Error($"General exception caught in method {methodName}: {exception.Message}");                   
               });

        public static RetryPolicy<bool> GetRetryPolicy(int retryCount = 10)
        {
            return Policy<bool>
                .HandleResult(result => result == false) // Retry if the result is false
                .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(1), (result, timeSpan, retryAttempt, context) =>
                {
                    Q.logger.Warn($"Retrying to check the element state, attempt {retryAttempt}");
                });            
        }        
    }
}
