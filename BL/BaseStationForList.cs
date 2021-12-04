using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationForList
    {
        public int BaseStationId { get; set; }
        public string StationName { get; set; }
        public int AvailableChargingS { get; set; }
       public int UnAvailableChargingS { get; set; }
        public Location StationLocation { get; set; }
        public IEnumerable<DroneInCharge> DInChargeList { get; set; }

    }
}
