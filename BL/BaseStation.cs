using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int BaseStationId { get; set; }
        public string StationName { get; set; }
        public Location StationLocation { get; set; }
        public int AvailableChargingS { get; set; }
        public IEnumerable<List<DroneInCharge>> DInChargeList { get; set; }

       
    }
}
