using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class DroneForList: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(DroneForList));
        static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model", typeof(string), typeof(DroneForList));
        static readonly DependencyProperty MaxWeightProperty = DependencyProperty.Register("Weight", typeof(Enums.WeightCategories), typeof(DroneForList));
        static readonly DependencyProperty DroneForListStatusProperty = DependencyProperty.Register("Status", typeof(Enums.DroneStatuses), typeof(DroneForList));
        static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(DroneForList));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(DroneForList));
        static readonly DependencyProperty InDeliveryParcelProperty = DependencyProperty.Register("Parcel Id", typeof(int), typeof(DroneForList));
        public int DroneId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value);}
        public string Model { get => (string)GetValue(ModelProperty); set => SetValue(ModelProperty, value); }
        public Enums.WeightCategories MaxWeight { get => (Enums.WeightCategories)GetValue(MaxWeightProperty); set => SetValue(MaxWeightProperty, value); }
        public Enums.DroneStatuses DroneState { get => (Enums.DroneStatuses)GetValue(DroneForListStatusProperty); set => SetValue(DroneForListStatusProperty, value); }
        public Location CurrentLocation { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value); }
        public double Battery { get => (double)GetValue(BatteryProperty); set => SetValue(BatteryProperty, value); }
        public int InDeliveringParcelId { get => (int)GetValue(InDeliveryParcelProperty); set => SetValue(InDeliveryParcelProperty, value); }//of course just in case there is a parcel in delivering
    }
}
