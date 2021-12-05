using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL.DO;
namespace IDAL
{
    public interface IDal//in the intire interface i changed every function to not be public and static, and it solved some errors, i think its ok cause its public static in dalobject
    {
        void AddBaseStation(int id, string name, int chargeSlots, double longitude, double lattitude);
        void AddDrone(int id,double Battery, WeightCategories maxW, string model);
        void AddCustomer(int id, string name, string phone, double longitude, double lattitude);
        void AddParcel(int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime requested, DateTime scheduled, DateTime pickedUp, DateTime delivered);
        void RemoveCustomer(int id);
        void RemoveParcel(int id);
        void RemoveDrone(int id);
        void RemoveBaseStation(int id);
        void AscriptionPtoD(int parcelId, int droneId);
        void PickUpParcel(int parcelId);
        void ParcelDelivering(int parcelId);
        void DroneCharging(int droneId, int baseStationId);
        void DroneRelease(int droneId, int baseStationId);
        BaseStation CopyBaseStation(int baseStationId);
        Drone CopyDrone(int droneId);
        Customer CopyCustomer(int customerId);
        Parcel CopyParcel(int parcelId);
        double[] DronePowerConsumingPerKM();//check that
        IEnumerable<BaseStation> CopyBaseStations();
        IEnumerable<Drone> CopyDronesList();
        IEnumerable<Customer> CopyCustomersList();
        IEnumerable<Parcel> CopyParcelsList();
        IEnumerable<Parcel> UnAscriptedParcels();
        IEnumerable<BaseStation> AvailableBaseStation();
    }

}

