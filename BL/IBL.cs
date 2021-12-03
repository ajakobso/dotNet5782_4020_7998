using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    public interface IBL
    {
            void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge);
            void AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation);
            void AddCustomer(int Id, string Name, string PhoneNum, Location Location);
            void AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority);
            void UpdateDrone(int Id, string Model);
            void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots);
            void UpdateCustomer(int Id, string Name, int PhoneNum);
            void DroneToCharge(int Id);
            void ReleaseDroneFromCharge(int Id, DateTime TimeInCharge);
            void AscriptionParcelToDrone(int Id);
            void PickUpParcel(int DId);
            void DeliveringParcelByDrone(int Id);
            void DisplayBaseStation(int id);
            void DisplayDrone(int id);
            void DisplayCustomer(int id);
            void DisplayParcel(int id);
            void DisplayBaseStationsList();
            void DisplayDronesList();
            void DisplayCustomersList();
            void DisplayParcelsList();
            void DisplayUnAscriptedParcelsList();
            void DisplayAvailableChargingStation();

    }
}
