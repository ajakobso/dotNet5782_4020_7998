using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL.PO
{
    public static class BoPoAdapter
    {
        public static BaseStation BaseStationBoPo(BL.BO.BaseStation boBaseStation)
        {
            BaseStation nBaseStation = new();
            boBaseStation.DeepCopyTo(nBaseStation);
            return nBaseStation;
        }
        public static BaseStationForList BaseStationForListBoPo(BL.BO.BaseStationForList boBaseStationForList)
        {
            BaseStationForList nBaseStationForList = new();
            boBaseStationForList.DeepCopyTo(nBaseStationForList);
            return nBaseStationForList;
        }
        public static Customer CustomerBoPo(BL.BO.Customer boCustomer)
        {
            Customer nCustomer = new();
            boCustomer.DeepCopyTo(nCustomer);
            return nCustomer;
        }
        public static CustomerForList CustomerForListBoPo(BL.BO.CustomerForList boCustomerForList)
        {
            CustomerForList nCustomerForList = new();
            boCustomerForList.DeepCopyTo(nCustomerForList);
            return nCustomerForList;
        }
        public static CustomerInParcel CustomerInParcelBoPo(BL.BO.CustomerInParcel boCustomerInParcel)
        {
            CustomerInParcel nCustomerInParcel = new();
            boCustomerInParcel.DeepCopyTo(nCustomerInParcel);
            return nCustomerInParcel;
        }
        public static Drone DroneBoPo(BL.BO.Drone boDrone)
        {
            Drone nDrone = new();
            boDrone.DeepCopyTo(nDrone);
            return nDrone;
        }
        public static DroneForList DroneForListBoPo(BL.BO.DroneForList boDroneForList)
        {
            DroneForList nDroneForList = new();
            boDroneForList.DeepCopyTo(nDroneForList);
            return nDroneForList;
        }
        public static DroneInCharge DroneInChargeBoPo(BL.BO.DroneInCharge boDroneInCharge)
        {
            DroneInCharge nDroneInCharge = new();
            boDroneInCharge.DeepCopyTo(nDroneInCharge);
            return nDroneInCharge;
        }
        public static DroneInParcel DroneInParcelBoPo(BL.BO.DroneInParcel boDroneInParcel)
        {
            DroneInParcel nDroneInParcel = new();
            boDroneInParcel.DeepCopyTo(nDroneInParcel);
            return nDroneInParcel;
        }
        public static Location  LocationBoPo(BL.BO.Location  boLocation )
        {
            Location  nLocation  = new();
            boLocation .DeepCopyTo(nLocation );
            return nLocation ;
        }
        public static Parcel ParcelBoPo(BL.BO.Parcel boParcel)
        {
            Parcel nParcel = new();
            boParcel.DeepCopyTo(nParcel);
            return nParcel;
        }
        public static ParcelInCustomer ParcelInCustomerBoPo(BL.BO.ParcelInCustomer boParcelInCustomer)
        {
            ParcelInCustomer nParcelInCustomer = new();
            boParcelInCustomer.DeepCopyTo(nParcelInCustomer);
            return nParcelInCustomer;
        }
        public static ParcelInDelivering ParcelInDeliveringBoPo(BL.BO.ParcelInDelivering boParcelInDelivering)
        {
            ParcelInDelivering nParcelInDelivering = new();
            boParcelInDelivering.DeepCopyTo(nParcelInDelivering);
            return nParcelInDelivering;
        }
        public static ParcelToList ParcelToListBoPo(BL.BO.ParcelToList boParcelToList)
        {
            ParcelToList nParcelToList = new();
            boParcelToList.DeepCopyTo(nParcelToList);
            return nParcelToList;
        }
    }
}
