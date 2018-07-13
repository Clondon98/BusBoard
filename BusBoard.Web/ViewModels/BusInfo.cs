using System;
using System.Collections.Generic;
using System.Linq;
using BusBoard.ConsoleApp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusBoard.Web.ViewModels
{
  public class BusInfo
  {
    private static TfLAPI tflAPI = new TfLAPI();
    public List<ConsoleApp.BusInfo> bigBusList = new List<ConsoleApp.BusInfo>();
    
    public BusInfo(string postcode)
    {
      PostCode = postcode;
      busInfoList();
    }

    public string PostCode { get; set; }

    public void busInfoList()
    {
      PostCode = PostCode.Replace(" ", String.Empty);
      PostCode = PostCode.ToUpper();

      List<StopInfo> stops = findStops(PostCode, 2);
      
      foreach (StopInfo stop in stops)
      {
        string busList = tflAPI.Request(stop.id + "/Arrivals");

        List<ConsoleApp.BusInfo> busInfoList = JsonConvert.DeserializeObject<List<ConsoleApp.BusInfo>>(busList);
        busInfoList.Sort();
        List<ConsoleApp.BusInfo> fiveBuses = busInfoList.Take(5).ToList();

        bigBusList.AddRange(fiveBuses);

      }
      
      bigBusList.Sort();
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