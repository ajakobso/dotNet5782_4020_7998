using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInParcel
    {
        public int DroneId { get; set; }
        public double BatteryState { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            return $"id = {DroneId}, battery = {BatteryState}, location = {CurrentLocation}";
        }
    }
}
