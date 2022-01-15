using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStationForList
    {
        public int BaseStationId { get; set; }
        public string StationName { get; set; }
        public int AvailableChargingS { get; set; }
        public int UnAvailableChargingS { get; set; }
        public Location StationLocation { get; set; }
        public IEnumerable<DroneInCharge> DInChargeList { get; set; }
        public override string ToString()
        {
            return $"id = {BaseStationId}, name = {StationName}, location = {StationLocation}, available charging stations = {AvailableChargingS},not available charging stations = {UnAvailableChargingS}, drones in charge = {DInChargeList}";
        }
        public void RemoveDInCharge(int id) { }

    }
}
