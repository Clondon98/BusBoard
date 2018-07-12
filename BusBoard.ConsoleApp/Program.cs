using System.Net;
using RestSharp;
using SimpleJson;

namespace BusBoard.ConsoleApp
{
  class Program
  {
    static void Main(string[] args)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      TfLAPI api = new TfLAPI();
      
      string request = "490008660N/Arrivals";
      
      JsonArray response = api.Execute<JsonArray>(request);
    }
  }
}
