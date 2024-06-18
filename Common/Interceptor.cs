using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V125.Fetch;

public class Interceptor
{
    private readonly IWebDriver driver;

    public Interceptor(IWebDriver webDriver)
    {
        driver = webDriver;
    }

    public async Task InterceptAsync()
    {
        if (driver is IDevTools devTools)
        {
            var session = devTools.GetDevToolsSession();

            var fetch = new FetchAdapter(session);

            // Enable the Fetch domain
            await fetch.Enable(new EnableCommandSettings());

            // Set up event listener for paused requests
            fetch.RequestPaused += async (sender, e) =>
            {
                var request = e.Request;
                string url = request.Url;
                string method = request.Method;

                // Check if it's a POST request
                if (method == "POST")
                {
                    // Access and modify request data here (optional)
                    // You can access request headers, body, etc.

                    // Allow the request to proceed
                    await fetch.ContinueRequest(new ContinueRequestCommandSettings
                    {
                        RequestId = e.RequestId
                    });
                }
                else
                {
                    // Allow the request to proceed for non-POST requests
                    await fetch.ContinueRequest(new ContinueRequestCommandSettings
                    {
                        RequestId = e.RequestId
                    });
                }
            };
        }
        else
        {
            throw new NotSupportedException("Driver does not support DevTools protocol");
        }
    }
}
