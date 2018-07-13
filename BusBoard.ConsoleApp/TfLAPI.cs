using System;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    public class TfLAPI
    {
        private const string BaseUrl = "https://api.tfl.gov.uk/StopPoint/";

        private readonly string _accountSid = "4a396ec2";
        private readonly string _secretKey = "15bd42c732995a153c17373b8d093d3c";
        private RestClient client = new RestClient(BaseUrl);

        public string Request(string req)
        {
            var request = new RestRequest(req, Method.GET);
            request.AddQueryParameter("app_id", _accountSid);
            request.AddQueryParameter("app_key", _secretKey);

            var response = client.Execute(request);

            return Execute(request);
        }

        public string stopTypes(string stopType, double lat, double lon)
        {
            var request = new RestRequest();
            request.AddQueryParameter("stopTypes", stopType);
            request.AddQueryParameter("lat", lat.ToString());
            request.AddQueryParameter("lon", lon.ToString());
            request.AddQueryParameter("app_id", _accountSid);
            request.AddQueryParameter("app_key", _secretKey);

            return Execute(request);
        }

        public string Execute(RestRequest request)
        {
            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var TfLException = new ApplicationException(message, response.ErrorException);
                throw TfLException;
            }
            
            return response.Content;
        }
    }
}