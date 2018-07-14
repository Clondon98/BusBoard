using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusBoard.ConsoleApp;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusInfo = BusBoard.Web.ViewModels.BusInfo;

namespace BusBoard.Web.Controllers
{
  public class HomeController : Controller
  {
    private static TfLAPI tflAPI = new TfLAPI();

    public ActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public ActionResult BusInfo(PostcodeSelection selection)
    {
      // Add some properties to the BusInfo view model with the data you want to render on the page.
      // Write code here to populate the view model with info from the APIs.
      // Then modify the view (in Views/Home/BusInfo.cshtml) to render upcoming buses.
      string postcode;
      List<ConsoleApp.BusInfo> buses;

      try
      {
        postcode = selection.Postcode.Replace(" ", String.Empty).ToUpper();
        buses = busInfoList(postcode);
      }
      catch (NullReferenceException)
      {
        return View("Error");
      }

      var info = new BusInfo(postcode, buses);

      return View(info);
    }

    [HttpGet]
    public ActionResult TubeView(PostcodeSelection selection)
    {
      string postcode;
      List<ConsoleApp.TubeInfo> tubes;

      try
      {
        postcode = selection.Postcode.Replace(" ", String.Empty).ToUpper();
        tubes = tubeInfoList(postcode);
      }
      catch (NullReferenceException)
      {
        return View("Error");
      }

      var info = new TubeView(postcode, tubes);

      return View(info);
    }

    public List<TubeInfo> tubeInfoList(string PostCode)
    {
      List<StopInfo> stops = findStops(PostCode, "NaptanMetroStation", Int32.MaxValue);
      List<ConsoleApp.TubeInfo> bigTubeList = new List<ConsoleApp.TubeInfo>();

      foreach (StopInfo stop in stops)
      {
        string tubeList = tflAPI.Request(stop.naptanId + "/Arrivals");
        
        List<ConsoleApp.TubeInfo> tubeInfoList;

        try
        {
          tubeInfoList = JsonConvert.DeserializeObject<List<ConsoleApp.TubeInfo>>(tubeList);
        }
        catch (JsonSerializationException)
        {
          tubeInfoList = new List<TubeInfo>();
          foreach (TubeInfo tube in tubeInfoList) tube.stopDistance = stop.distance;
        }

        bigTubeList.AddRange(tubeInfoList);
      }

      bigTubeList.Sort();

      return bigTubeList.Take(20).ToList();
    }

    public List<ConsoleApp.BusInfo> busInfoList(string PostCode)
    {
      List<StopInfo> stops = findStops(PostCode, "NaptanPublicBusCoachTram", Int32.MaxValue);
      
      List<ConsoleApp.BusInfo> bigBusList = new List<ConsoleApp.BusInfo>();

      foreach (StopInfo stop in stops)
      {
        string busList = tflAPI.Request(stop.naptanId + "/Arrivals");

        List<ConsoleApp.BusInfo> fiveBuses;
        
        try
        {
          List<ConsoleApp.BusInfo> busInfoList = JsonConvert.DeserializeObject<List<ConsoleApp.BusInfo>>(busList);
          busInfoList.Sort();
          fiveBuses = busInfoList.Take(5).ToList();

          foreach (ConsoleApp.BusInfo bus in fiveBuses) bus.stopDistance = stop.distance;
          }
            catch (JsonSerializationException)
          {
            fiveBuses = new List<ConsoleApp.BusInfo>();
          }

        bigBusList.AddRange(fiveBuses);

      }

      bigBusList.Sort();

      return bigBusList;
    }


    private static List<StopInfo> findStops(string postcode, string type, int num)
    {
      PostcodeAPI postAPI = new PostcodeAPI();

      string postResponse = postAPI.Execute(postcode);
      JObject json = JObject.Parse(postResponse);

      string result = json["result"].ToString();

      PostcodeInfo postInfo = JsonConvert.DeserializeObject<PostcodeInfo>(result);

      int range = 100;
      
      string tflResponse = tflAPI.stopTypes(type, range, postInfo.latitude, postInfo.longitude);
      JObject jStops = JObject.Parse(tflResponse);

      string array = jStops["stopPoints"].ToString();
      
      List<StopInfo> stops = JsonConvert.DeserializeObject<List<StopInfo>>(array);

      while (stops.Count == 0 && range <= 1000)
      {
        range += 100;
        tflResponse = tflAPI.stopTypes(type, range, postInfo.latitude, postInfo.longitude);
        jStops = JObject.Parse(tflResponse);

        array = jStops["stopPoints"].ToString();
        
        stops = JsonConvert.DeserializeObject<List<StopInfo>>(array);
      }
      
      stops.Sort();

      return stops;
    }

  public ActionResult About()
    {
      ViewBag.Message = "Information about this site";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Contact us!";

      return View();
    }
  }
}