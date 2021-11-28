using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneForList
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public Enums.WeightCategories MaxWeight { get; set; }
        public Enums.DroneStatuses DroneState { get; set; }
        public double Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public int InDeliveringParcelId { get; set; }//of course just in case there is a parcel in delivering

    }
}
