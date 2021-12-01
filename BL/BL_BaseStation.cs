using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL : IBL.BO.IBL
    {

        void IBL.BO.IBL.AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            //myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
            public IEnumerable<IBL.BO.DroneInCharge> DronesInCharge = new IEnumerable<IBL.BO.DroneInCharge>();
        myDalObject.DataSource.Config.BaseStations.Add(new dal.BaseStation { Id = num, Name = name, ChargeSlots = numOfAvailableDCharge, Longitude = location.Longitude, Lattitude = location.Latitude });
        }

    void IBL.BO.IBL.UpdateBaseStation(int Id, string Name, int NumOfChargeSlots) { }//
    void IBL.BO.IBL.DisplayBaseStation(int id) { }//
    void IBL.BO.IBL.DisplayBaseStationsList() { }//
    void IBL.BO.IBL.DisplayAvailableChargingStation() { }//
}
}

