﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DalApi
{
    public interface IDAL//in the intire interface i changed every function to not be public and static, and it solved some errors, i think its ok cause its public static in dalobject
    {
        void AddBaseStation(int id, string name, int chargeSlots, int availableChargeSlots, double longitude, double lattitude);
        void AddDrone(int id, WeightCategories maxW, string model);
        void AddCustomer(int id, string name, string phone, double longitude, double lattitude);
        void AddParcel(int id, int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime? requested, DateTime? scheduled, DateTime? pickedUp, DateTime? delivered);
        void AddDroneCharge(int id, int sId);
        void RemoveDroneCharge(int id);
        void RemoveCustomer(int id);
        void RemoveParcel(int id);
        void RemoveDrone(int id);
        void RemoveBaseStation(int id);
        void AscriptionPtoD(int parcelId, int droneId);
        void PickUpParcel(int parcelId);
        void ParcelDelivering(int parcelId);
        void DroneCharging(int droneId, int baseStationId);
        void DroneRelease(int droneId);
        Coordinate Fromdouble(double angleInDegrees);
        BaseStation CopyBaseStation(int baseStationId);
        Drone CopyDrone(int droneId);
        Customer CopyCustomer(int customerId);
        Parcel CopyParcel(int parcelId);
        double[] DronePowerConsumingPerKM();
        double[] CopyLongitudeRange();
        double[] CopyLattitudeRange();
        IEnumerable<BaseStation> CopyBaseStations();
        IEnumerable<Drone> CopyDronesList();
        IEnumerable<Customer> CopyCustomersList();
        IEnumerable<Parcel> CopyParcelsList();
        IEnumerable<DroneCharge> CopyDronesInCharge();
        IEnumerable<Parcel> UnAscriptedParcels();//change to predict
        IEnumerable<BaseStation> AvailableBaseStation();//change to predict
    }

}

