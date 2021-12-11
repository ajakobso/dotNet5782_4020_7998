using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public partial class BL
    {
        public void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            if ((myDalObject.CopyLongitudeRange()[0] > location.Long) || (myDalObject.CopyLongitudeRange()[1] < location.Long) || (myDalObject.CopyLattitudeRange()[0] > location.Lat) || (myDalObject.CopyLattitudeRange()[1] < location.Lat))
            {
                throw new LocationOutOfRangeException();
            }
            try { location = AddLocation(location.Long, location.Lat); }
            catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//catch this
            try { myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Long, location.Lat); }
            catch (DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }


            BaseStationForList nBs = new BaseStationForList { BaseStationId = num, StationLocation = location, StationName = name, AvailableChargingS = numOfAvailableDCharge, UnAvailableChargingS = 0, DInChargeList = new List<DroneInCharge>() };
            baseStations.Add(nBs);
        }
        public void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            int Check = 0;
            foreach (var baseStation in myDalObject.CopyBaseStations())
            {
                if (baseStation.Id == Id)
                {
                    Check++;
                    DO.BaseStation nBaseStation = new DO.BaseStation();
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
                    catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                    return;
                }
            }
            foreach (BaseStationForList baseStation1 in baseStations)
            {
                if (baseStation1.BaseStationId == Id)
                {
                    BaseStationForList nbaseStation = new BaseStationForList();
                    nbaseStation = baseStation1;

                    if (!(Name == " "))
                    {
                        nbaseStation.StationName = Name;
                    }
                    if (!(NumOfChargeSlots == -1))
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
            foreach (var baseStation in baseStations)
            {
                if (baseStation.BaseStationId == id)
                {
                    nBaseStation = baseStation;
                    return nBaseStation;
                }
            }
            throw new BaseStationNotFoundException();
        }//
        public IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate)
        {
            List<BaseStationForList> nStationsList = baseStations.FindAll(predicate);
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
            foreach (var bs in baseStations)
            {
                if (bs.AvailableChargingS > 0)
                {
                    list.Add(bs);
                }
            }
            return list;
        }
    }
}
