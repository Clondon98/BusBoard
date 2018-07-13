using System;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    public class PostcodeAPI
    {
        private const string BaseUrl = "https://api.postcodes.io/postcodes/";
        
        public string Execute(string req)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(req, Method.GET);

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