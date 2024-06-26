using RestSharp;
using Newtonsoft.Json;
using Q.Models;

namespace Q.Services
{
    public class DogService : BaseApiClient
    {
        public DogService(string baseUrl) : base(baseUrl) { }

        public RestResponse GetById(string dogId)
        {
            var request = new RestRequest($"breeds/{dogId}", Method.Get);
            request.RequestFormat = DataFormat.Json;
            var response = ExecuteRequest(request);

            // Check if the response is null
            if (response == null)
            {
                throw new ApplicationException("No response received from the server.");
            }

            return response;
        }

        public Dog GetById(RestResponse response, string dogId)
        {
            var data = JsonConvert.DeserializeObject<DogData>(response.Content);

            // Check if the deserialized object or the Data property is null
            if (data == null || data.Data == null)
            {
                throw new ApplicationException("No data found in the response.");
            }

            return data?.Data;
        }
    }
}