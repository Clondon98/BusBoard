using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusBoard.ConsoleApp;
using BusBoard.Web.Models;
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
      string postcode = selection.Postcode;
      List<ConsoleApp.BusInfo> buses = new List<ConsoleApp.BusInfo>();

      try
      {
        buses = busInfoList(postcode);
      }
      catch (NullReferenceException)
      {
        Console.WriteLine("Error caught");
        postcode = "Error error error";
      }
      
      var info = new BusInfo(postcode, buses);
      
      return View(info);        
    }

    private List<ConsoleApp.BusInfo> busInfoList(string PostCode)
    {
      List<StopInfo> stops = findStops(PostCode, 2);
      List<ConsoleApp.BusInfo> bigBusList = new List<ConsoleApp.BusInfo>();
      
      foreach (StopInfo stop in stops)
      {
        string busList = tflAPI.Request(stop.id + "/Arrivals");

        List<ConsoleApp.BusInfo> busInfoList = JsonConvert.DeserializeObject<List<ConsoleApp.BusInfo>>(busList);
        busInfoList.Sort();
        List<ConsoleApp.BusInfo> fiveBuses = busInfoList.Take(5).ToList();

        bigBusList.AddRange(fiveBuses);

      }
      
      bigBusList.Sort();

      return bigBusList;
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