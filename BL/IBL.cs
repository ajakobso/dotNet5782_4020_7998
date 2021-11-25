using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
namespace IBL.BO
{
    interface IBL
    {
        public:
            void AddBaseStation(int num, string name, string location, int numOfAvailableDCharge);
            void AddDrone(int Id, char Model, WeightCategories MaxWeight, int Bstation);
            void AddCustomer(int Id, string Name, int PhoneNum, string Location);
            void AddParcelToDeliver(SCustomerId, SCustomerId, WeightCategories Weight, Priorities Priority);
            void UpdateDrone(int Id, string Model);
            void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots);
            void UpdateCustomer(int Id, string Name, int PhoneNum);
            void DroneToCharge(int Id);
            void ReleaseDroneFromCharge(int Id, DateTime TimeInCharge);
            void AscriptionParcelToDrone(int Id);
            void PickUpParcel(int DId);
            void DeliveringParcelByDrone(int Id);
            void DisplayBaseStation();
            void DisplayDrone();
            void DisplayCustomer();
            void DisplayParcel();
            void DisplayBaseStationsList();
            void DisplayDronesList();
            void DisplayCustomersList();
            void DisplayParcelsList();
            void DisplayUnAscriptedParcelsList();
            void DisplayAvailableChargingStation();

    }
}
