using System;

namespace BusBoard.ConsoleApp
{
    public class StopInfo : IComparable
    {
        public string naptanId;
        public string commonName;
        public double distance;
        
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
                return String.Compare(this.naptanId, it.naptanId);
            }
        }

        public string toString()
        {
            return naptanId;
        }
    }    
}