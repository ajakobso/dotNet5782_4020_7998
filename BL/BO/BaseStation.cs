﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStation
    {
        public int BaseStationId { get; set; }
        public string StationName { get; set; }
        public Location StationLocation { get; set; }
        public int AvailableChargingS { get; set; }
        public IEnumerable<DroneInCharge> DInChargeList { get; set; }
        public override string ToString()
        {
            return $"id = {BaseStationId}, name = {StationName}, location = {StationLocation}, available charging stations = {AvailableChargingS}, drones in charge = {DInChargeList}";
        }
       
    }
}
