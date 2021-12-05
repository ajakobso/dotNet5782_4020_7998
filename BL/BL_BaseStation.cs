﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    public partial class BL
    {
        public void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            try { myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Long, location.Lat); }
            catch (IDAL.DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
        }
        public void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            int Check = 0;
            foreach(var baseStation in myDalObject.CopyBaseStations())
            {
                if(baseStation.Id==Id)
                {
                    Check++;
                    IDAL.DO.BaseStation nBaseStation = new IDAL.DO.BaseStation();
                    nBaseStation = baseStation;

                    if (!(Name == " "))
                    {
                        nBaseStation.Name = Name;////////remove the old bs, and add the updated one.
                    }
                    else
                        nBaseStation.Name = baseStation.Name;
                    if (!(NumOfChargeSlots == -1))
                    {
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                    }
                    else
                        nBaseStation.ChargeSlots = baseStation.ChargeSlots;
                    try
                    {
                        myDalObject.RemoveBaseStation(baseStation.Id);
                        myDalObject.AddBaseStation(baseStation.Id, nBaseStation.Name, nBaseStation.ChargeSlots, baseStation.Longitude, baseStation.Lattitude);
                    }
                    catch (IDAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                    
                    return;
                }
            }
            foreach(BaseStationForList baseStation1 in baseStations)
            {
                if(baseStation1.BaseStationId==Id)
                {
                    BaseStationForList nbaseStation = new BaseStationForList();
                    nbaseStation = baseStation1;

                    if (!(Name==" "))
                    {
                        nbaseStation.StationName = Name;
                    }
                    if (!(NumOfChargeSlots==-1))
                    {
                        nbaseStation.AvailableChargingS = (NumOfChargeSlots - nbaseStation.UnAvailableChargingS);
                    }
                }
                ///////not sure which exceptions to de here or what you did here, i think maby you need to add the nBaseStation somewhere? or not? 
            }
            if (Check == 0)
                throw new BaseStationNotFoundException();
        }//
        public BaseStationForList DisplayBaseStation(int id)
        {
            BaseStationForList nBaseStation = new BaseStationForList();
            foreach(BaseStationForList baseStation in baseStations)
            {
                if(baseStation.BaseStationId==id)
                {
                    nBaseStation = baseStation;
                    return nBaseStation;
                }
            }
            throw new BaseStationNotFoundException();
        }//
        public IEnumerable<BaseStationForList> DisplayBaseStationsList()
        {
            List<BaseStationForList> nStationsList = new();
            nStationsList = baseStations;
            return nStationsList;
        }
        public void DisplayAvailableChargingStation()
        { 

        }
}
}


