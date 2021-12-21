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
        static readonly DependencyProperty LongitudeProperty = DependencyProperty.Register("Longitude", typeof(DAL.DO.Coordinate), typeof(Location));
        static readonly DependencyProperty LattitudeProperty = DependencyProperty.Register("Lattitude", typeof(DAL.DO.Coordinate), typeof(Location));
        //static readonly DependencyProperty LongProperty = DependencyProperty.Register("Longitude", typeof(double), typeof(Location));
        //static readonly DependencyProperty LatProperty = DependencyProperty.Register("Lattitude", typeof(double), typeof(Location));
        public DAL.DO.Coordinate Longitude { get => (DAL.DO.Coordinate)GetValue(LongitudeProperty); set => SetValue(LongitudeProperty, value); }
        public DAL.DO.Coordinate Latitude { get => (DAL.DO.Coordinate)GetValue(LattitudeProperty); set => SetValue(LattitudeProperty, value); }
        public double Long { /*get => (double)GetValue(LongProperty); set => SetValue(LongProperty, value);*/get; set; }
        public double Lat {/*get => (double)GetValue(LatProperty); set => SetValue(LatProperty, value);*/get; set; }
    }
}
