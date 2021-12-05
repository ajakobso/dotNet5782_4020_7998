using System;

namespace IBL.BO
{

    public class Enums
    {
        //partial class BL//מימוש הממשק IBL
        //{
        public enum DroneStatuses { Available, Maintenance, Shipping };
        public enum ParcelState { Created, Ascripted, PickedUp, Delivered };
        public enum WeightCategories { Light, Middle, Heavy };
        //public enum DroneStatuses { Available, Maintenance, Shipping };
        public enum Priorities { Standart, Fast, Urgent };
        public enum Inputs { a, p, d, l, e };
        public enum Adding { nBaseStation, nDrone, nCustomer, nParcel };//for the main
        public enum Updating { AscPtoD, PUParcel, Pdelivering, DCharging, DRelease };//for the main
        public enum Displaying { DBaseStation, DDrone, DCustomer, DParcel };//for the main
        public enum ListsDisplaying { BaseStationsList, DronesList, CustomersList, ParcelsList, UnAscriptedParcelsLict, AvailableChargingStationsList };//for the main
        public enum PowerPerKM { Available, LightWheight, MiddleWheight, HeavyWeight };
        public enum NewUpdating { DroneModel, BaseStation, Customer, DroneToCharge, DroneRealese, AscriptionPToD, PickUpParcel, DeliveringPByD };//for the main
        
    }
}


