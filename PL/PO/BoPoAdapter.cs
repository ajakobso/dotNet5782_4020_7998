using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace PL.PO
{
    public static class BoPoAdapter
    {
        public static ObservableCollection<BaseStation> BaseStationAdapter(IEnumerable<BL.BO.BaseStation> list)
        {
            ObservableCollection<BaseStation> nBaseStations = new();
            list.ToList().ForEach(x => nBaseStations.Add(BaseStationBoPo(x)));
            return nBaseStations;
        }
        public static ObservableCollection<BaseStationForList> BaseStationForListAdapter(IEnumerable<BL.BO.BaseStationForList> list)
        {
            ObservableCollection<BaseStationForList> nBaseStationForLists = new();
            list.ToList().ForEach(x => nBaseStationForLists.Add(BaseStationForListBoPo(x)));
            return nBaseStationForLists;
        }
        public static ObservableCollection<DroneForList> DroneForListAdapter(IEnumerable<BL.BO.DroneForList> list)
        {
            ObservableCollection<DroneForList> nDroneForLists = new();
            list.ToList().ForEach(x => nDroneForLists.Add(DroneForListBoPo(x)));
            return nDroneForLists;
        }
        public static ObservableCollection<Customer> CustomerAdapter(IEnumerable<BL.BO.Customer> list)
        {
            ObservableCollection<Customer> nCustomers = new();
            list.ToList().ForEach(x => nCustomers.Add(CustomerBoPo(x)));
            return nCustomers;
        }
        public static ObservableCollection<CustomerForList> CustomerForListAdapter(IEnumerable<BL.BO.CustomerForList> list)
        {
            ObservableCollection<CustomerForList> nCustomerForLists = new();
            list.ToList().ForEach(x => nCustomerForLists.Add(CustomerForListBoPo(x)));
            return nCustomerForLists;
        }
        public static ObservableCollection<CustomerInParcel> CustomerInParcelAdapter(IEnumerable<BL.BO.CustomerInParcel> list)
        {
            ObservableCollection<CustomerInParcel> nCustomerInParcels = new();
            list.ToList().ForEach(x => nCustomerInParcels.Add(CustomerInParcelBoPo(x)));
            return nCustomerInParcels;
        }
        public static ObservableCollection<Drone> DroneAdapter(IEnumerable<BL.BO.Drone> list)
        {
            ObservableCollection<Drone> nDrones = new();
            list.ToList().ForEach(x => nDrones.Add(DroneBoPo(x)));
            return nDrones;
        }
        public static ObservableCollection<DroneInCharge> DroneInChargeAdapter(IEnumerable<BL.BO.DroneInCharge> list)
        {
            ObservableCollection<DroneInCharge> nDroneInCharges = new();
            list.ToList().ForEach(x => nDroneInCharges.Add(DroneInChargeBoPo(x)));
            return nDroneInCharges;
        }
        public static ObservableCollection<DroneInParcel> DroneInParcelAdapter(IEnumerable<BL.BO.DroneInParcel> list)
        {
            ObservableCollection<DroneInParcel> nDroneInParcels = new();
            list.ToList().ForEach(x => nDroneInParcels.Add(DroneInParcelBoPo(x)));
            return nDroneInParcels;
        }
        public static ObservableCollection<Location> LocationAdapter(IEnumerable<BL.BO.Location> list)
        {
            ObservableCollection<Location> nLocations = new();
            list.ToList().ForEach(x => nLocations.Add(LocationBoPo(x)));
            return nLocations;
        }
        public static ObservableCollection<Parcel> ParcelAdapter(IEnumerable<BL.BO.Parcel> list)
        {
            ObservableCollection<Parcel> nParcels = new();
            list.ToList().ForEach(x => nParcels.Add(ParcelBoPo(x)));
            return nParcels;
        }
        public static ObservableCollection<ParcelInCustomer> ParcelInCustomerAdapter(IEnumerable<BL.BO.ParcelInCustomer> list)
        {
            ObservableCollection<ParcelInCustomer> nParcelInCustomers = new();
            list.ToList().ForEach(x => nParcelInCustomers.Add(ParcelInCustomerBoPo(x)));
            return nParcelInCustomers;
        }
        public static ObservableCollection<ParcelInDelivering> ParcelInDeliveringAdapter(IEnumerable<BL.BO.ParcelInDelivering> list)
        {
            ObservableCollection<ParcelInDelivering> nParcelInDeliverings = new();
            list.ToList().ForEach(x => nParcelInDeliverings.Add(ParcelInDeliveringBoPo(x)));
            return nParcelInDeliverings;
        }
        public static ObservableCollection<ParcelToList> ParcelToListAdapter(IEnumerable<BL.BO.ParcelToList> list)
        {
            ObservableCollection<ParcelToList> nParcelToLists = new();
            list.ToList().ForEach(x => nParcelToLists.Add(ParcelToListBoPo(x)));
            return nParcelToLists;
        }
        public static BaseStation BaseStationBoPo(BL.BO.BaseStation boBaseStation)
        {
            BaseStation nBaseStation = new();
            Cloning.Clone(boBaseStation,nBaseStation);
            nBaseStation.StationLocation = LocationBoPo(boBaseStation.StationLocation);
            return nBaseStation;
        }
        public static BaseStationForList BaseStationForListBoPo(BL.BO.BaseStationForList boBaseStationForList)
        {
            BaseStationForList nBaseStationForList = new();
            Cloning.Clone(boBaseStationForList,nBaseStationForList);
            nBaseStationForList.StationLocation = LocationBoPo(boBaseStationForList.StationLocation);
            return nBaseStationForList;
        }
        public static Customer CustomerBoPo(BL.BO.Customer boCustomer)
        {
            Customer nCustomer = new();
            Cloning.Clone(boCustomer, nCustomer);
            nCustomer.Place = LocationBoPo(boCustomer.Place);
            return nCustomer;
        }
        public static CustomerForList CustomerForListBoPo(BL.BO.CustomerForList boCustomerForList)
        {
            CustomerForList nCustomerForList = new();
            Cloning.Clone(boCustomerForList, nCustomerForList);
            return nCustomerForList;
        }
        public static CustomerInParcel CustomerInParcelBoPo(BL.BO.CustomerInParcel boCustomerInParcel)
        {
            CustomerInParcel nCustomerInParcel = new();
            Cloning.Clone(boCustomerInParcel, nCustomerInParcel);
            return nCustomerInParcel;
        }
        public static Drone DroneBoPo(BL.BO.Drone boDrone)
        {
            Drone nDrone = new();
            Cloning.Clone(boDrone, nDrone);
            nDrone.CurrentLocation = LocationBoPo(boDrone.CurrentLocation);
            return nDrone;
        }
        public static DroneForList DroneForListBoPo(BL.BO.DroneForList boDroneForList)
        {
            DroneForList nDroneForList = new();
            Cloning.Clone(boDroneForList, nDroneForList);
            nDroneForList.CurrentLocation = LocationBoPo(boDroneForList.CurrentLocation);
            return nDroneForList;
        }
        public static DroneInCharge DroneInChargeBoPo(BL.BO.DroneInCharge boDroneInCharge)
        {
            DroneInCharge nDroneInCharge = new();
            Cloning.Clone(boDroneInCharge, nDroneInCharge);
            return nDroneInCharge;
        }
        public static DroneInParcel DroneInParcelBoPo(BL.BO.DroneInParcel boDroneInParcel)
        {
            DroneInParcel nDroneInParcel = new();
            Cloning.Clone(boDroneInParcel, nDroneInParcel);
            return nDroneInParcel;
        }
        public static Location LocationBoPo(BL.BO.Location  boLocation )
        {
           return new(boLocation.Long, boLocation.Lat);
        }
        public static Parcel ParcelBoPo(BL.BO.Parcel boParcel)
        {
            Parcel nParcel = new();
            Cloning.Clone(boParcel, nParcel);
            return nParcel;
        }
        public static ParcelInCustomer ParcelInCustomerBoPo(BL.BO.ParcelInCustomer boParcelInCustomer)
        {
            ParcelInCustomer nParcelInCustomer = new();
            Cloning.Clone(boParcelInCustomer, nParcelInCustomer);
            return nParcelInCustomer;
        }
        public static ParcelInDelivering ParcelInDeliveringBoPo(BL.BO.ParcelInDelivering boParcelInDelivering)
        {
            ParcelInDelivering nParcelInDelivering = new();
            Cloning.Clone(boParcelInDelivering, nParcelInDelivering);
            return nParcelInDelivering;
        }
        public static ParcelToList ParcelToParcelToListBoPo(BL.BO.Parcel boParcelToList)
        {
            ParcelToList nParcelToList = new();
            Cloning.Clone(boParcelToList, nParcelToList);
            return nParcelToList;
        }
        public static ParcelToList ParcelToListBoPo(BL.BO.ParcelToList boParcelToList)
        {
            ParcelToList nParcelToList = new();
            Cloning.Clone(boParcelToList, nParcelToList);
            return nParcelToList;
        }
    }
}
