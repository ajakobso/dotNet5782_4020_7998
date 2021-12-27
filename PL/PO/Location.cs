using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class Location : DependencyObject
    {
        //static readonly DependencyProperty LongitudeProperty = DependencyProperty.Register("Longitude", typeof(string), typeof(Location));
        // static readonly DependencyProperty LattitudeProperty = DependencyProperty.Register("Lattitude", typeof(string), typeof(Location));

        //static readonly DependencyProperty LongProperty = DependencyProperty.Register("Longitude", typeof(double), typeof(Location));
        //static readonly DependencyProperty LatProperty = DependencyProperty.Register("Lattitude", typeof(double), typeof(Location));
        //public Coordinate Longitude { get => (Coordinate)GetValue(LongitudeProperty); set => SetValue(LongitudeProperty, value); }
        //public Coordinate Latitude { get => (Coordinate)GetValue(LattitudeProperty); set => SetValue(LattitudeProperty, value); }
        public Coordinate Longitude { get; set; }
        public Coordinate Lattitude { get; set; }
        public double Long { /*get => (double)GetValue(LongProperty); set => SetValue(LongProperty, value);*/get; set; }
        public double Lat {/*get => (double)GetValue(LatProperty); set => SetValue(LatProperty, value);*/get; set; }


        public Location(double longitude, double lat)
        {
            Long = longitude;
            Lat = lat;
            Longitude = Fromdouble(longitude);
            Lattitude = Fromdouble(lat);
        }//initalize the location
        public override string ToString()
        {
            return $"{Longitude.ToString("WE")} {Lattitude.ToString("NS")}";
        }
        private Coordinate Fromdouble(double angleInDegrees)
        {
            //ensure the value will fall within the primary range [-180.0..+180.0]
            while (angleInDegrees < -180.0)
                angleInDegrees += 360.0;

            while (angleInDegrees > 180.0)
                angleInDegrees -= 360.0;

            var result = new Coordinate();

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

    public class Coordinate
    {
        public bool IsNegative { get; set; }
        public int Degrees { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public int Milliseconds { get; set; }

        public override string ToString()
        {
            var degrees = this.IsNegative
                ? -this.Degrees
                : this.Degrees;

            return string.Format(
                "{0}° {1:00}' {2:00}\"",
                degrees,
                this.Minutes,
                this.Seconds);
        }
        public string ToString(string format)
        {
            switch (format)
            {
                case "NS":
                    return string.Format(
                        "{0}° {1:00}' {2:00}.{3:000}\" {4}",
                        this.Degrees,
                        this.Minutes,
                        this.Seconds,
                        this.Milliseconds,
                        this.IsNegative ? 'S' : 'N');

                case "WE":
                    return string.Format(
                        "{0}° {1:00}' {2:00}.{3:000}\" {4}",
                        this.Degrees,
                        this.Minutes,
                        this.Seconds,
                        this.Milliseconds,
                        this.IsNegative ? 'W' : 'E');

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
