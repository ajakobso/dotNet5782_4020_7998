using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
namespace DAL
{
    namespace IDAL
    {
        interface IDal
        {
            public static void AddBaseStation(int id, string name, int chargeSlots, double longitude, double lattitude);
            public static void AddDrone(int id, WeightCategories maxW, string model);
            public static void AddCustomer(int id, string name, string phone, double longitude, double lattitude);
            public static void AddParcel(int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime requested, DateTime scheduled, DateTime pickedUp, DateTime delivered);
            public static void AscriptionPtoD(int parcelId, int droneId);
            public static void PickUpParcel(int parcelId);
            public static void PaclerDelivering(int parcelId);
            public static void DroneCharging(int droneId, int baseStationId);
            public static void DroneRelease(int droneId, int baseStationId);
            public static BaseStation CopyBaseStation(int baseStationId);
            public static Drone CopyDrone(int droneId);
            public static Customer CopyCustomer(int customerId);
            public static Parcel CopyParcel(int parcelId);
            public static IEnumerable<BaseStation> CopyBaseStations();
            public static IEnumerable<Drone> CopyDronesList();
            public static IEnumerable<Customer> CopyCustomersList();
            public static IEnumerable<Parcel> CopyParcelsList();
            public static IEnumerable<Parcel> UnAscriptedParcels();
            public static IEnumerable<BaseStation> AvailableBaseStation();
        }
    }
}
