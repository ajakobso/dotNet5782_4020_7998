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
            foreach (var baseStation in myDalObject.CopyBaseStations().Where(baseStation => baseStation.Id == Id))///////////////////////////////////
            {
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
            //foreach (BaseStationForList baseStation1 in baseStations) -dont have this list anymore
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
            foreach (var nBaseStation in from baseStation in myDalObject.CopyBaseStations()
                                         where baseStation.Id == id
                                         let sLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                         let nBaseStation = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = sLocation }
                                         select nBaseStation)
            {
                return nBaseStation;
            }

            throw new BaseStationNotFoundException();
        }//
        public IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate)
        {
            IEnumerable<BaseStationForList> nStationList = new List<BaseStationForList>();
            nStationList = (from DAL.DO.BaseStation baseStation in myDalObject.CopyBaseStations()
                                                      let ForListLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                                      let baseStationForList = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = ForListLocation }
                                                      select baseStationForList).ToList();
            nStationList = (nStationList as List<BaseStationForList>).FindAll(predicate);
            return nStationList;
        }
        public IEnumerable<int> DisplayBaseStationsId()
        {
            IEnumerable<int> responce = new List<int>();
            (responce as List<int>).AddRange(from bs in myDalObject.CopyBaseStations()
                              select bs.Id);
            return responce;
        }
        public IEnumerable<BaseStationForList> DisplayAvailableChargingStation()
        {
            IEnumerable<BaseStationForList> BsList = new List<BaseStationForList>();
            (BsList as List<BaseStationForList>).AddRange(from bs in myDalObject.CopyBaseStations()
                          where bs.AvailableChargeSlots > 0
                          let bsLocation = new Location(bs.Longitude, bs.Lattitude)
                          let baseStationForList = new BaseStationForList { BaseStationId = bs.Id, StationName = bs.Name, AvailableChargingS = bs.AvailableChargeSlots, UnAvailableChargingS = (bs.ChargeSlots - bs.AvailableChargeSlots), StationLocation = bsLocation }
                          select baseStationForList);
            return BsList;
        }
    }

}