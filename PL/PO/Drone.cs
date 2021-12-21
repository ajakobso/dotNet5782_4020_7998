using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class Drone: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(Drone));
        static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(string), typeof(Drone));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(Drone));
        static readonly DependencyProperty DroneStatusProperty = DependencyProperty.Register("Status", typeof(Enums.DroneStatuses), typeof(Drone));
        static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(Drone));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(Drone));
        static readonly DependencyProperty DeliveryParcelProperty = DependencyProperty.Register("Parcel", typeof(ParcelInDelivering), typeof(Drone));
        public int DroneId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string Model { get => (string)GetValue(ModelProperty); set => SetValue(ModelProperty, value); }
        public Enums.WeightCategories MaxWeight { get=> (Enums.WeightCategories)GetValue(MaxWeightProperty); set=>SetValue(MaxWeightProperty, value); }
        public Enums.DroneStatuses DroneStatus { get => (Enums.DroneStatuses)GetValue(DroneStatusProperty); set => SetValue(DroneStatusProperty, value); }
        public double Battery { get => (double)GetValue(BatteryProperty); set => SetValue(BatteryProperty, value); }
        public ParcelInDelivering DeliveryParcel { get => (ParcelInDelivering)GetValue(DeliveryParcelProperty); set=>SetValue(DeliveryParcelProperty, value); }
        public Location CurrentLocation { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value); }
    }
}
