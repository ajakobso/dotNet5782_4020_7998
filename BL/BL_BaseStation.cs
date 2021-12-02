﻿using System;
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
            public IEnumerable<IBL.BO.DroneInCharge> DronesInCharge; //= new IEnumerable<IBL.BO.DroneInCharge>();
            // DalObject.DataSource.Config.BaseStations.Add(new IDAL.DO.BaseStation {Id = num, Name = name, ChargeSlots = numOfAvailableDCharge, Longitude = location.Longitude, Lattitude = location.Latitude });//חייב לבדוק מה הבעיה עם זה..זה פשוט תוקע את כל המחלקה
            //myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
            // myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
        }


        void IBL.BO.IBL.UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            int Check = 0;
            foreach(BL.myDalObject.BaseStation baseStation in BL.myDalObject.DataSource.Config.BaseStations)
            {
                if(baseStation.Id==Id)
                {
                    Check++;
                    if(!(Name==null))
                    {
                        baseStation.Name = Name;
                    }
                    if(!(NumOfChargeSlots==-1))
                    {
                        baseStation.ChargeSlots = NumOfChargeSlots;
                    }
                    break;
                }
            }
            if (Check == 0)
                throw new myDalObject.BaseStationNotFoundException();
        }//
        void IBL.BO.IBL.DisplayBaseStation(int id) { }//
        void IBL.BO.IBL.DisplayBaseStationsList() { }//
        void IBL.BO.IBL.DisplayAvailableChargingStation() { }//
    public Location findCloseBaseStationLocation(Ilocatable fromLocation)
    {
        List<BaseStation> Locations = new List<BaseStation>();
        foreach (var baseStation in myDalObject.DataSource.Config.BaseStations)
        {
            Locations.Add(new BaseStation
            {
                Location = new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude }
            });
        }
        Location location = Locations[0].Location;
        ///////////////////////
    }
}


}


