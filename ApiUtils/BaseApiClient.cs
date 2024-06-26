using System;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Services
{
    public abstract class BaseApiClient
    {
        protected readonly RestClient _client;

        protected BaseApiClient(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        protected RestResponse ExecuteRequest(RestRequest request)
        {
            var response = _client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new ApplicationException($"Error: {response.ErrorMessage}");
            }
            return response;
        }
    }
}
