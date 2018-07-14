using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BusBoard.ConsoleApp
{
    public class TubeInfo : IComparable
    {
        public string id;
        public string naptanID;
        public string lineName;
        public string stationName;
        public string platformName;
        public string destinationName;
        public string currentLocation;
        public string expectedArrival;
        public string towards;
        public double timeToStation;
        
        [JsonIgnore]
        public DateTime expected { get; set; }
        
        [JsonIgnore]
        public double stopDistance { get; set; }

        [OnDeserialized]
        internal void setUp(StreamingContext streamingContext)
        {
            expected = DateTime.Parse(expectedArrival);
            if (towards.Equals("")) towards = destinationName;
        }
        
        public string toString()
        {
            return id + " " + timeToStation;
        }
        
        public int CompareTo(object obj)
        {
            TubeInfo it = (TubeInfo) obj;
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