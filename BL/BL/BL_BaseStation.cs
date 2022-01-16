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
                    if (!(Name == " "))//" " is the default value
                    {
                        nBaseStation.Name = Name;
                    }
                    if (!(NumOfChargeSlots == -1))//-1 is the default value
                    {
                        int unAvailableChargeSlots = baseStation.ChargeSlots - baseStation.AvailableChargeSlots;
                        nBaseStation.AvailableChargeSlots = NumOfChargeSlots - unAvailableChargeSlots;
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                        nBaseStation.AvailableChargeSlots = NumOfChargeSlots;
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                    }
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
        public BaseStation DisplayBaseStation(int id)
        {
            lock (myDal)
            {

                IEnumerable<DroneInCharge> dronesInCharge = from drone in myDal.CopyDronesInCharge()
                                                            let nDrone = new DroneInCharge { DroneId = drone.DroneId, StationId = drone.StationId, InsertionTime = drone.InsertionTime }
                                                            select nDrone;
                dronesInCharge = from drone in dronesInCharge
                                 where drone.StationId == id
                                 let nDrone = new DroneInCharge { DroneId = drone.DroneId, StationId = drone.StationId, InsertionTime = drone.InsertionTime }
                                 select nDrone;

                {
                    foreach (var baseStation in from baseStation in myDal.CopyBaseStations()
                                                where baseStation.Id == id
                                                select baseStation)
                    {
                        return new BaseStation{ BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, StationLocation = new Location(baseStation.Longitude, baseStation.Lattitude), DInChargeList = dronesInCharge };
                    }
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
                                let dronesInC = dronesInCharge.Where(x => x.StationId == baseStation.Id)
                                let baseStationForList = new BaseStationForList { BaseStationId = baseStation.Id, StationName = baseStation.Name, AvailableChargingS = baseStation.AvailableChargeSlots, UnAvailableChargingS = baseStation.ChargeSlots - baseStation.AvailableChargeSlots, StationLocation = ForListLocation, DInChargeList = dronesInC.ToList() }
                                select baseStationForList);
                Func<BaseStationForList, bool> func = new(predicate);//non-direct convertion between the predicate to Fucn in order to use the Where method
                return nStationList.Where(func);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<int> DisplayBaseStationsId()
        {
            lock (myDal)
            {
                IEnumerable<int> responce = new List<int>();
                responce = from bs in myDal.CopyBaseStations()
                           select bs.Id;
                return responce;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStationForList> DisplayAvailableChargingStation()
        {
            lock (myDal)
            {
                IEnumerable<BaseStationForList> BsList = new List<BaseStationForList>();
                BsList = (from bs in myDal.CopyBaseStations()
                          where bs.AvailableChargeSlots > 0
                          let bsLocation = new Location(bs.Longitude, bs.Lattitude)
                          let baseStationForList = new BaseStationForList { BaseStationId = bs.Id, StationName = bs.Name, AvailableChargingS = bs.AvailableChargeSlots, UnAvailableChargingS = (bs.ChargeSlots - bs.AvailableChargeSlots), StationLocation = bsLocation }
                          select baseStationForList);
                return BsList;
            }
        }
    }
}