using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace PL.PO
{
    public class DroneInCharge: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(DroneInCharge));
        static readonly DependencyProperty BatteryProperty = DependencyProperty.Register("Battery", typeof(double), typeof(DroneInCharge));
        static readonly DependencyProperty InsertionTimeProperty = DependencyProperty.Register("Insertion time", typeof(DateTime), typeof(DroneInCharge));
        public int DroneId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public double Battery { get => (double)GetValue(BatteryProperty); set => SetValue(BatteryProperty, value); }
        public DateTime InsertionTime { get=>(DateTime)GetValue(InsertionTimeProperty); set=>SetValue(InsertionTimeProperty, value); }
    }
}
