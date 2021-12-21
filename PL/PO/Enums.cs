using System;

namespace PL.PO
{

    public class Enums
    {
        public enum DroneStatuses { Available, Maintenance, Shipping};
        public enum ParcelState { Created, Ascripted, PickedUp, Delivered };
        public enum WeightCategories { Light, Middle, Heavy};        
        public enum Priorities { Standart, Fast, Urgent };
        public enum PowerPerKM { Available, LightWheight, MiddleWheight, HeavyWeight, BatteryPercentPerHour };  
    }
}


