using System;

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return $"id = ID, name = Name, charge slot = ChargeSlot, longitude = Longitude, latitude = Latitude ";
            }
        }
    }
}
