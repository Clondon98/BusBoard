﻿using System;
using RestSharp;
using RestSharp.Authenticators;

namespace BusBoard.ConsoleApp
{
    public class TfLAPI
    {
        private const string BaseUrl = "https://api.tfl.gov.uk/StopPoint/";

        private readonly string _accountSid = "4a396ec2";
        private readonly string _secretKey = "15bd42c732995a153c17373b8d093d3c";

        public T Execute<T>(String req) where T : new()
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(req, Method.GET);
            request.AddQueryParameter("app_id", _accountSid);
            request.AddQueryParameter("app_key", _secretKey);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var TfLException = new ApplicationException(message, response.ErrorException);
                throw TfLException;
            }
            
            return response.Data;
        }
    }
}