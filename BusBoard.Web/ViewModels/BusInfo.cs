using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
  public class BusInfo
  {
    public string PostCode { get; set; }
    public List<ConsoleApp.BusInfo> bigBusList = new List<ConsoleApp.BusInfo>();
    
    public BusInfo(string postcode, List<ConsoleApp.BusInfo> buses)
    {
      PostCode = postcode;
      bigBusList = buses;
    }  
  }
}