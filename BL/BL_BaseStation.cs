using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {

        void AddBaseStation(int num, string name, IBL.BO.Location location, int numOfAvailableDCharge)
        {
            dal.DalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
                    public List<IBL.BO.DroneInCharge> DronesInCharge = new List<IBL.BO.DroneInCharge>();
        //dal.DataSource.Config.BaseStations.Add(new dal.BaseStation { Id = num, Name = name, ChargeSlots = numOfAvailableDCharge, Longitude = location.Longitude, Lattitude = location.Latitude });
    }

    void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots);
        void DisplayBaseStation(int id);
        void DisplayBaseStationsList();
        void DisplayAvailableChargingStation();

    }
}
