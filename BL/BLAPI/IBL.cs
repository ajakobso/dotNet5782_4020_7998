using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
namespace BlApi
{
    public interface IBL
    {
        void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge);
        void removeBaseStation(int id);
        void AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation);
        void AddCustomer(int Id, string Name, string PhoneNum, Location Location);
        void AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority);
        Location AddLocation(double longitude, double lat);
        void UpdateDrone(int Id, string Model);
        void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots);
        void UpdateCustomer(int Id, string Name, string PhoneNum);
        void DroneToCharge(int Id);
        void ReleaseDroneFromCharge(int Id, double TimeInCharge);
        void AscriptionParcelToDrone(int Id);
        void PickUpParcel(int DId);
        void DeliveringParcelByDrone(int Id);
        public void RemoveParcel(int id);
        DateTime GetInsertionTime(int droneID);
        BaseStation DisplayBaseStation(int id);
        DroneForList DisplayDrone(int id);
        Drone GetDrone(int id);
        Customer DisplayCustomer(int id);
        Parcel DisplayParcel(int id);
        IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate);
        IEnumerable<DroneForList> DisplayDronesList(Predicate<DroneForList> predicate);
        IEnumerable<int> DisplayBaseStationsId();
        IEnumerable<CustomerForList> DisplayCustomersList(Predicate<CustomerForList> predicate);
        IEnumerable<ParcelToList> DisplayParcelsList(Predicate<ParcelToList> predicate);
        IEnumerable<ParcelToList> DisplayUnAscriptedParcelsList();//predict
        IEnumerable<BaseStationForList> DisplayAvailableChargingStation();//predict
        void SimulatorActivation(int droneId, Action updateDisplay, Func<bool> stopCheck);
    }
}

