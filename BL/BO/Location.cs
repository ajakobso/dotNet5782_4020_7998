﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        
        public DO.Coordinate Longitude { get; set; }
        public DO.Coordinate Latitude { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public Location(double longitude, double lat) 
        {
            Long = longitude;
            Lat = lat;
            Longitude = Fromdouble(longitude); 
            Latitude = Fromdouble(lat); }//initalize the location
        public override string ToString()
        {
            return $"{Longitude.ToString("WE")} {Latitude.ToString("NS")}";
        }
        private DO.Coordinate Fromdouble(double angleInDegrees)
        {
            //ensure the value will fall within the primary range [-180.0..+180.0]
            while (angleInDegrees < -180.0)
                angleInDegrees += 360.0;

            while (angleInDegrees > 180.0)
                angleInDegrees -= 360.0;

            var result = new DO.Coordinate();

            //switch the value to positive
            result.IsNegative = angleInDegrees < 0;
            angleInDegrees = Math.Abs(angleInDegrees);

            //gets the degree
            result.Degrees = (int)Math.Floor(angleInDegrees);
            var delta = angleInDegrees - result.Degrees;

            //gets minutes and seconds
            var seconds = (int)Math.Floor(3600.0 * delta);
            result.Seconds = seconds % 60;
            result.Minutes = (int)Math.Floor(seconds / 60.0);
            delta = delta * 3600.0 - seconds;

            //gets fractions
            result.Milliseconds = (int)(1000.0 * delta);

            return result;
        }
    }
}
