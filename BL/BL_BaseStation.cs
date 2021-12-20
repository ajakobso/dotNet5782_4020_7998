using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;

namespace BL
{
    internal sealed partial class BL
    {
        public void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            if ((myDalObject.CopyLongitudeRange()[0] > location.Long) || (myDalObject.CopyLongitudeRange()[1] < location.Long) || (myDalObject.CopyLattitudeRange()[0] > location.Lat) || (myDalObject.CopyLattitudeRange()[1] < location.Lat))
            {
                throw new LocationOutOfRangeException();
            }
            try { location = AddLocation(location.Long, location.Lat); }
            catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//catch this
            try { myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, numOfAvailableDCharge, location.Long, location.Lat); }//we have no drone who is charging here yet
            catch (DAL.DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
            //BaseStationForList nBs = new BaseStationForList { BaseStationId = num, StationLocation = location, StationName = name, AvailableChargingS = numOfAvailableDCharge, UnAvailableChargingS = 0, DInChargeList = new List<DroneInCharge>() };
            //baseStations.Add(nBs);
        }
        public void removeBaseStation(int id)
        {
            try { myDalObject.RemoveBaseStation(id); }
            catch(DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
        }
        public void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            // bool Check = false;
            foreach (var baseStation in myDalObject.CopyBaseStations())
            {
                if (baseStation.Id == Id)
                {
                    //Check = true;
                    DAL.DO.BaseStation nBaseStation = new DAL.DO.BaseStation();
                    nBaseStation = baseStation;

                    if (!(Name == " "))
                    {
                        nBaseStation.Name = Name;////////remove the old bs, and add the updated one.
                    }
                    else
                        nBaseStation.Name = baseStation.Name;
                    if (!(NumOfChargeSlots == -1))
                    {
                        if (nBaseStation.AvailableChargeSlots < nBaseStation.ChargeSlots)
                        {
                            nBaseStation.AvailableChargeSlots = (NumOfChargeSlots - (nBaseStation.ChargeSlots - nBaseStation.AvailableChargeSlots));
                            nBaseStation.ChargeSlots = NumOfChargeSlots;
                        }
                        else
                        {
                            nBaseStation.AvailableChargeSlots = NumOfChargeSlots;
                            nBaseStation.ChargeSlots = NumOfChargeSlots;
                        }
                    }
                    else
                        nBaseStation.ChargeSlots = baseStation.ChargeSlots;
                    try
                    {
                        myDalObject.RemoveBaseStation(baseStation.Id);
                        myDalObject.AddBaseStation(nBaseStation.Id, nBaseStation.Name, nBaseStation.ChargeSlots, nBaseStation.AvailableChargeSlots, baseStation.Longitude, baseStation.Lattitude);
                    }
                    catch (DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                    return;
                }
            }
            //foreach (BaseStationForList baseStation1 in baseStations)
            //{
            //    if (baseStation1.BaseStationId == Id)
            //    {
            //        BaseStationForList nbaseStation = new BaseStationForList();
            //        nbaseStation = baseStation1;

            //        if (!(Name == " "))
            //        {
            //            nbaseStation.StationName = Name;
            //        }
            //        if (!(NumOfChargeSlots == -1))
            //        {
            //            nbaseStation.AvailableChargingS = (NumOfChargeSlots - nbaseStation.UnAvailableChargingS);
            //        }
            //    }
            ///////not sure which exceptions to de here or what you did here, i think maby you need to add the nBaseStation somewhere? or not? 
            // }
            //if (Check == 0)
            throw new BaseStationNotFoundException();
        }
        public BaseStationForList DisplayBaseStation(int id)
        {

            foreach (var baseStation in myDalObject.CopyBaseStations())
            {
                if (baseStation.Id == id)
                {
                    Location sLocation = new Location(baseStation.Longitude, baseStation.Lattitude);
                    BaseStationForList nBaseStation = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = sLocation };
                    return nBaseStation;
                }
            }
            throw new BaseStationNotFoundException();
        }//
        public IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate)
        {
            List<BaseStationForList> nStationsList = new List<BaseStationForList>();
            foreach (DAL.DO.BaseStation baseStation in myDalObject.CopyBaseStations())
            {
                Location ForListLocation = new Location(baseStation.Longitude, baseStation.Lattitude);
                BaseStationForList baseStationForList = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = ForListLocation };
                nStationsList.Add(baseStationForList);
            }
            //List<BaseStationForList> nStationsList = baseStations.FindAll(predicate);
            return nStationsList;
        }
        public IEnumerable<int> DisplayBaseStationsId()
        {
            List<int> responce = new();
            foreach (var bs in myDalObject.CopyBaseStations())
            {
                responce.Add(bs.Id);
            }
            return responce;
        }
        public IEnumerable<BaseStationForList> DisplayAvailableChargingStation()
        {
            List<BaseStationForList> list = new();
            foreach (var bs in myDalObject.CopyBaseStations())
            {
                if (bs.AvailableChargeSlots > 0)
                {
                    Location bsLocation = new Location(bs.Longitude, bs.Lattitude);
                    BaseStationForList baseStationForList = new BaseStationForList { BaseStationId = bs.Id, StationName = bs.Name, AvailableChargingS = bs.AvailableChargeSlots, UnAvailableChargingS = (bs.ChargeSlots - bs.AvailableChargeSlots), StationLocation = bsLocation };
                    list.Add(baseStationForList);
                }
            }
            return list;
        }
    }

}