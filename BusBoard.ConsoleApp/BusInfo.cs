using System;

namespace BusBoard.ConsoleApp
{
    public class BusInfo : IComparable
    {
        public string id { get; set; }
        public string vehicleId { get; set; }
        public string naptanId { get; set; }
        public string stationName { get; set; }
        public string lineId { get; set; }
        public string platformName { get; set; }
        public string destinationName { get; set; }
        public string expectedArrival { get; set; }
        public int timeToStation { get; set; }

        public string toString()
        {
            return id + " " + timeToStation;
        }
        
        public int CompareTo(object obj)
        {
            BusInfo it = (BusInfo) obj;
            if (this.timeToStation < it.timeToStation)
                return -1;
            else if (this.timeToStation > it.timeToStation)
            {
                return 1;
            }
            else
            {
                return String.Compare(this.id, it.id);
            }
        }
    }
}