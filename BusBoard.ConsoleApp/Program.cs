using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusBoard.ConsoleApp
{
  class Program
  {
    private static TfLAPI tflAPI = new TfLAPI();
    
    static void Main(string[] args)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      
      Console.WriteLine("Please enter a postcode: ");

      string postcode = Console.ReadLine();
      
      postcode = postcode.Replace(" ", String.Empty);
      postcode = postcode.ToUpper();

      List<StopInfo> stops = findStops(postcode, 2);

      foreach (StopInfo stop in stops)
      {
        string busList = tflAPI.Request(stop.id + "/Arrivals");

        List<BusInfo> busInfoList = JsonConvert.DeserializeObject<List<BusInfo>>(busList);
        busInfoList.Sort();
        List<BusInfo> fiveBuses = busInfoList.Take(5).ToList();
        
        Console.WriteLine("Stop: " + stop.commonName);
        Console.WriteLine("Distance: " + Math.Round(stop.distance, 0) + "m");
        Console.WriteLine("Buses: ");
        foreach (BusInfo bus in fiveBuses)
        {
          Console.WriteLine("Line: " + bus.lineId + ", Destination: " + bus.destinationName + ", Expected arrival: " + bus.expected.ToString("HH:mm"));
        }

        Console.WriteLine();
      } 
    }

    private static List<StopInfo> findStops(string postcode, int num)
    {
        PostcodeAPI postAPI = new PostcodeAPI();

        string postResponse = postAPI.Execute(postcode);
        JObject json = JObject.Parse(postResponse);

        string result = json["result"].ToString();

        PostcodeInfo postInfo = JsonConvert.DeserializeObject<PostcodeInfo>(result);

        string tflResponse = tflAPI.stopTypes("NaptanPublicBusCoachTram", postInfo.latitude, postInfo.longitude);
        JObject jStops = JObject.Parse(tflResponse);

        string array = jStops["stopPoints"].ToString();

        List<StopInfo> stops = JsonConvert.DeserializeObject<List<StopInfo>>(array);
        stops.Sort();

        return stops.Take(num).ToList();
    }
  }
}
