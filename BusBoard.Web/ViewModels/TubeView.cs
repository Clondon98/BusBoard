using System.Collections.Generic;

namespace BusBoard.Web.ViewModels
{
    public class TubeView
    {
        public string PostCode { get; set; }
        public List<ConsoleApp.TubeInfo> bigTubeList = new List<ConsoleApp.TubeInfo>();
    
        public TubeView(string postcode, List<ConsoleApp.TubeInfo> tubes)
        {
            PostCode = postcode;
            bigTubeList = tubes;
        }  
    }
}