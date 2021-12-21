using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class DroneInParcel: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(DroneInParcel));
        static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(DroneInParcel));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(DroneInParcel));
        public int DroneId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public double BatteryState { get => (double)GetValue(BatteryProperty); set => SetValue(BatteryProperty, value);}
        public Location CurrentLocation { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value);}
    }
}
