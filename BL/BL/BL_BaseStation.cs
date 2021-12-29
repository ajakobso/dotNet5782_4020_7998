﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

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
            catch (DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
            //BaseStationForList nBs = new BaseStationForList { BaseStationId = num, StationLocation = location, StationName = name, AvailableChargingS = numOfAvailableDCharge, UnAvailableChargingS = 0, DInChargeList = new List<DroneInCharge>() };
            //baseStations.Add(nBs);
        }
        public void removeBaseStation(int id)
        {
            try { myDalObject.RemoveBaseStation(id); }
            catch(DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
        }
        public void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            foreach (var baseStation in myDalObject.CopyBaseStations().Where(baseStation => baseStation.Id == Id))///////////////////////////////////kind of linq
            {
                DO.BaseStation nBaseStation = new DO.BaseStation();
                nBaseStation = baseStation;
                if (!(Name == " "))
                {
                    nBaseStation.Name = Name;////////remove the old bs, and add the updated one.
                }
                //else
                //    nBaseStation.Name = baseStation.Name;
                if (!(NumOfChargeSlots == -1))
                {
                    if (baseStation.AvailableChargeSlots < baseStation.ChargeSlots)
                    {
                        nBaseStation.AvailableChargeSlots += NumOfChargeSlots - baseStation.ChargeSlots;
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                    }
                    else
                    {
                        nBaseStation.AvailableChargeSlots = NumOfChargeSlots;
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                    }
                }
                //else
                //    nBaseStation.ChargeSlots = baseStation.ChargeSlots;
                try
                {
                    myDalObject.RemoveBaseStation(baseStation.Id);
                    myDalObject.AddBaseStation(nBaseStation.Id, nBaseStation.Name, nBaseStation.ChargeSlots, nBaseStation.AvailableChargeSlots, baseStation.Longitude, baseStation.Lattitude);
                }
                catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                return;
            }
                  throw new BaseStationNotFoundException();
        }
        public BaseStationForList DisplayBaseStation(int id)
        {
            foreach (var nBaseStation in from baseStation in myDalObject.CopyBaseStations()//linq
                                         where baseStation.Id == id
                                         let sLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                         let nBaseStation = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = baseStation.ChargeSlots - baseStation.AvailableChargeSlots, StationLocation = sLocation }
                                         select nBaseStation)
            {
                return nBaseStation;
            }

            throw new BaseStationNotFoundException();
        }//
        
        public IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate)
        {
            IEnumerable<BaseStationForList> nStationList = new List<BaseStationForList>();
            nStationList = (from DO.BaseStation baseStation in myDalObject.CopyBaseStations()//linq, not foreach
                                                      let ForListLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                                      let baseStationForList = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = ForListLocation }
                                                      select baseStationForList).ToList();
            nStationList = (nStationList as List<BaseStationForList>).FindAll(predicate);
            return nStationList;
        }
        public IEnumerable<int> DisplayBaseStationsId()
        {
            IEnumerable<int> responce = new List<int>();
            (responce as List<int>).AddRange(from bs in myDalObject.CopyBaseStations()//linq, not for each
                              select bs.Id);
            return responce;
        }
        public IEnumerable<BaseStationForList> DisplayAvailableChargingStation()
        {
            IEnumerable<BaseStationForList> BsList = new List<BaseStationForList>();
            (BsList as List<BaseStationForList>).AddRange(from bs in myDalObject.CopyBaseStations()//linq, not foreach
                          where bs.AvailableChargeSlots > 0
                          let bsLocation = new Location(bs.Longitude, bs.Lattitude)
                          let baseStationForList = new BaseStationForList { BaseStationId = bs.Id, StationName = bs.Name, AvailableChargingS = bs.AvailableChargeSlots, UnAvailableChargingS = (bs.ChargeSlots - bs.AvailableChargeSlots), StationLocation = bsLocation }
                          select baseStationForList);
            return BsList;
        }
    }

}