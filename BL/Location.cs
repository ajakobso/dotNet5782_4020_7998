using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location
    {
        
        public IDAL.DO.Coordinate Longitude { get; set; }
        public IDAL.DO.Coordinate Latitude { get; set; }
        public Location(double longitude, double lat) { Longitude = IDAL.DO.Coordinate.FromDouble(longitude); Latitude = IDAL.DO.Coordinate.FromDouble(lat); }//initalize the location
        //}
    }
}
