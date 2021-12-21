using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL.PO
{
    public class BaseStationForList : DependencyObject
    {
        static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(int), typeof(BaseStationForList));
        static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(BaseStationForList));
        static readonly DependencyProperty LocationProperty = DependencyProperty.Register("Location", typeof(Location), typeof(BaseStationForList));
        static readonly DependencyProperty UnAvailableChargingSProperty = DependencyProperty.Register("Un Available charging slots", typeof(int), typeof(BaseStationForList));
        static readonly DependencyProperty AvailableChargingSProperty = DependencyProperty.Register("Available charging slots", typeof(int), typeof(BaseStationForList));
        static readonly DependencyProperty DInChargeListProperty = DependencyProperty.Register("Drone in charge", typeof(IEnumerable<DroneInCharge>), typeof(BaseStationForList));
        public int BaseStationId { get => (int)GetValue(IdProperty); set => SetValue(IdProperty, value); }
        public string StationName { get => (string)GetValue(NameProperty); set => SetValue(NameProperty, value); }
        public int AvailableChargingS { get => (int)GetValue(AvailableChargingSProperty); set => SetValue(AvailableChargingSProperty, value); }
        public int UnAvailableChargingS { get => (int)GetValue(UnAvailableChargingSProperty); set => SetValue(UnAvailableChargingSProperty, value); }
        public Location StationLocation { get => (Location)GetValue(LocationProperty); set => SetValue(LocationProperty, value); }
        public IEnumerable<DroneInCharge> DInChargeList { get => (IEnumerable<DroneInCharge>)GetValue(DInChargeListProperty); set => SetValue(DInChargeListProperty, value); }
    }
}
