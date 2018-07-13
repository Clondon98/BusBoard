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
    public string PostCode { get; set; }
    public List<ConsoleApp.BusInfo> bigBusList = new List<ConsoleApp.BusInfo>();
    
    public BusInfo(string postcode, List<ConsoleApp.BusInfo> buses)
    {
      PostCode = postcode.Replace(" ", String.Empty).ToUpper();
      bigBusList = buses;
    }  
  }
}