using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.PO
{
    public class BaseStation: DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(BaseStation));
        static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(BaseStation));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(BaseStation));
        static readonly DependencyProperty AvailableChargingSProperty = DependencyProperty.Register("Available charging slots", typeof(int), typeof(BaseStation));
        static readonly DependencyProperty DInChargeListProperty = DependencyProperty.Register("Drone in charge", typeof(IEnumerable<DroneInCharge>), typeof(BaseStation));      
        public int BaseStationId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string StationName { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
        public Location StationLocation { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value); }
        public int AvailableChargingS { get => (int)GetValue(AvailableChargingSProperty); set => SetValue(AvailableChargingSProperty, value); }
        public IEnumerable<DroneInCharge> DInChargeList { get => (IEnumerable<DroneInCharge>)GetValue(DInChargeListProperty); set => SetValue(DInChargeListProperty, value); }
    }
}
