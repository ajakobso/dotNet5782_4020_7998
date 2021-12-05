using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories MaxWeight { get; set; }
        public Enums.DroneStatuses DroneStatus { get; set; }
        public double Battery { get; set; }
        public ParcelInDelivering DeliveryParcel { get; set; }
        public Location CurrentLocation { get; set; }

    }
}
