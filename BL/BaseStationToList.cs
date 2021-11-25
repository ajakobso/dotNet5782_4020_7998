using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationToList
    {
        public int BaseStationId { get; set; }
        public string StationName { get; set; }
        public int AvailableChargingS { get; set; }
        public int UnAvailableChargingS { get; set; }
    }
}
