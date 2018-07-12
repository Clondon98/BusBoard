using System;

namespace BusBoard.ConsoleApp
{
    public class StopInfo : IComparable
    {
        public string id;
        public string[] modes;
        public string commonName;
        public double distance;
        public string placeType;
        
        public int CompareTo(object obj)
        {
            StopInfo it = (StopInfo) obj;
            if (this.distance < it.distance)
                return -1;
            else if (this.distance > it.distance)
            {
                return 1;
            }
            else
            {
                return String.Compare(this.id, it.id);
            }
        }

        public string toString()
        {
            return id;
        }
    }    
}