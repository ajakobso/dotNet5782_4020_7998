using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        
        public IDAL.DO.Coordinate Longitude { get; set; }
        public IDAL.DO.Coordinate Latitude { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public Location(double longitude, double lat) 
        {
            Long = longitude;
            Lat = lat;
            Longitude = DalObject.DalObject.FromDouble(longitude); 
            Latitude = DalObject.DalObject.FromDouble(lat); }//initalize the location
        public override string ToString()
        {
            return $"{Longitude.ToString("WE")} {Latitude.ToString("NS")}";
        }
    }
}
