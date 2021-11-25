using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//add descriptions??????????????????????????????????????????????????
namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories { Light, Middle, Heavy };
        //public enum DroneStatuses { Available, Maintenance, Shipping };
        public enum Priorities { Standart, Fast, Urgent };
        public enum Inputs { a, p, d, l, e };
        public enum Adding { nBaseStation, nDrone, nCustomer, nParcel };//for the main
        public enum Updating { AscPtoD, PUParcel, Pdelivering, DCharging, DRelease };//for the main
        public enum Displaying { DBaseStation, Ddrone, DCustomer, DParcel };//for the main
        public enum ListsDisplaying { BaseStationsList, DronesList, CustomersList, ParcelsList, UnAscriptedParcelsLict, AvailableChargingStationsList };//for the main
        public enum PowerPerKM { Available, LightWheight, MiddleWheight, HeavyWeight};
    }
}



