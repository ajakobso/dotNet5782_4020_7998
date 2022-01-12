using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    internal sealed partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            lock (myDal)
            {
                if ((myDal.CopyLongitudeRange()[0] > location.Long) || (myDal.CopyLongitudeRange()[1] < location.Long) || (myDal.CopyLattitudeRange()[0] > location.Lat) || (myDal.CopyLattitudeRange()[1] < location.Lat))
                {
                    throw new LocationOutOfRangeException();
                }
                try { location = AddLocation(location.Long, location.Lat); }
                catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//catch this
                try { myDal.AddBaseStation(num, name, numOfAvailableDCharge, numOfAvailableDCharge, location.Long, location.Lat); }//we have no drone who is charging here yet
                catch (DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
            }
            //BaseStationForList nBs = new BaseStationForList { BaseStationId = num, StationLocation = location, StationName = name, AvailableChargingS = numOfAvailableDCharge, UnAvailableChargingS = 0, DInChargeList = new List<DroneInCharge>() };
            //baseStations.Add(nBs);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void removeBaseStation(int id)
        {
            lock (myDal)
            {
                try { myDal.RemoveBaseStation(id); }
                catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            lock (myDal)
            {
                foreach (var baseStation in myDal.CopyBaseStations().Where(baseStation => baseStation.Id == Id))///////////////////////////////////kind of linq
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
                        myDal.RemoveBaseStation(baseStation.Id);
                        myDal.AddBaseStation(nBaseStation.Id, nBaseStation.Name, nBaseStation.ChargeSlots, nBaseStation.AvailableChargeSlots, baseStation.Longitude, baseStation.Lattitude);
                    }
                    catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                    return;
                }
                throw new BaseStationNotFoundException();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStationForList DisplayBaseStation(int id)
        {
            lock (myDal)
            {
                IEnumerable<DroneInCharge> dronesInCharge = from drone in myDal.CopyDronesInCharge()
                                                            let nDrone = new DroneInCharge { DroneId = drone.DroneId, StationId = drone.StationId, InsertionTime = drone.InsertionTime }
                                                            select nDrone;
                foreach (var nBaseStation in from baseStation in myDal.CopyBaseStations()//linq
                                             where baseStation.Id == id
                                             let sLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                             let dronesInC = dronesInCharge.ToList().FindAll(x => x.StationId == baseStation.Id)
                                             let nBaseStation = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = baseStation.ChargeSlots - baseStation.AvailableChargeSlots, StationLocation = sLocation, DInChargeList = dronesInC }
                                             select nBaseStation)
                {
                    return nBaseStation;
                }

                throw new BaseStationNotFoundException();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationForList> DisplayBaseStationsList(Predicate<BaseStationForList> predicate)
        {
            lock (myDal)
            {
                IEnumerable<BaseStationForList> nStationList = new List<BaseStationForList>();
                IEnumerable<DroneInCharge> dronesInCharge = from drone in myDal.CopyDronesInCharge()
                                                            let nDrone = new DroneInCharge { DroneId = drone.DroneId, StationId = drone.StationId, InsertionTime = drone.InsertionTime }
                                                            select nDrone;
                nStationList = (from DO.BaseStation baseStation in myDal.CopyBaseStations()//linq, not foreach
                                let ForListLocation = new Location(baseStation.Longitude, baseStation.Lattitude)
                                let dronesInC = dronesInCharge.ToList().FindAll(x => x.StationId == baseStation.Id)
                                let baseStationForList = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = (baseStation.ChargeSlots - baseStation.AvailableChargeSlots), StationLocation = ForListLocation, DInChargeList = dronesInC.ToList() }
                                select baseStationForList).ToList();
                nStationList = (nStationList as List<BaseStationForList>).FindAll(predicate);
                return nStationList;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<int> DisplayBaseStationsId()
        {
            lock (myDal)
            {
                IEnumerable<int> responce = new List<int>();
                (responce as List<int>).AddRange(from bs in myDal.CopyBaseStations()//linq, not for each
                                                 select bs.Id);
                return responce;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationForList> DisplayAvailableChargingStation()
        {
            lock (myDal)
            {
                IEnumerable<BaseStationForList> BsList = new List<BaseStationForList>();
                (BsList as List<BaseStationForList>).AddRange(from bs in myDal.CopyBaseStations()//linq, not foreach
                                                              where bs.AvailableChargeSlots > 0
                                                              let bsLocation = new Location(bs.Longitude, bs.Lattitude)
                                                              let baseStationForList = new BaseStationForList { BaseStationId = bs.Id, StationName = bs.Name, AvailableChargingS = bs.AvailableChargeSlots, UnAvailableChargingS = (bs.ChargeSlots - bs.AvailableChargeSlots), StationLocation = bsLocation }
                                                              select baseStationForList);
                return BsList;
            }
        }
    }

}