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
        public IEnumerable<DroneInCharge> DInChargeList { get; set; }

       public void removeDInCharge(int id)
        {
            foreach(var drone in DInChargeList)
            {
                if(drone.DroneId==id)
                {
                    DInChargeList.remove(drone);
                }
            }
        }
    }
}
