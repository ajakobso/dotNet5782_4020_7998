﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;
namespace BL
{
    internal sealed partial class BL
    {

        public void AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation)
        {
            Random r = new Random();//אין לי שמץ של מושג אם ככה מגדירים רנדום
            r.Next(20, 41);//מגריל מספר בין 20 ל-40 לפי מה שהבנתי
            Location BStationLocation;
            foreach (var baseStation in myDalObject.CopyBaseStations())//כרגיל לא עובד
            {
                if (baseStation.Id == Bstation)
                {
                    try { BStationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude); }
                    catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//add throw of location exception in all the references of the AddLocation
                    double battery = r.NextDouble() * (40 - 20) + 20;
                    try { myDalObject.AddDrone(Id, battery, (DAL.DO.WeightCategories)MaxWeight, Model) ; }
                    catch (DAL.DO.AddExistingDroneException) { throw new AddExistingDroneException(); }
                    try { myDalObject.DroneCharging(Id, Bstation); }
                    catch (DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = MaxWeight, DroneState = Enums.DroneStatuses.Maintenance, Battery = r.NextDouble() * (40 - 20) + 20, CurrentLocation = BStationLocation });
                    myDalObject.RemoveBaseStation(Bstation);
                    myDalObject.AddBaseStation(Bstation, baseStation.Name,baseStation.ChargeSlots, baseStation.AvailableChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude);
                   // #region add drone to first charge in base station
                    //DroneInCharge nDIC = new DroneInCharge { DroneId = Id, Battery = battery, InsertionTime = DateTime.Now };
                   // List<DroneInCharge> dicList = new();
                   // BaseStationForList oldBs = new(), nBsForList;
                    //try { oldBs = DisplayBaseStation(baseStation.Id); }
                   // catch (BaseStationNotFoundException)
                   // {
                   //     dicList.Add(nDIC);
                    //    nBsForList = new BaseStationForList { BaseStationId = baseStation.Id, StationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude), StationName = baseStation.Name, DInChargeList = dicList, AvailableChargingS = baseStation.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
                     //   baseStations.Add(nBsForList);
                     //   return;
                   // }
                   // dicList = oldBs.DInChargeList;
                   // dicList.Add(nDIC);
                    //nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
                    //baseStations.Remove(oldBs);
                    //baseStations.Add(nBsForList);
                   // #endregion
                    return;
                }
            }
            throw new BaseStationNotFoundException();
        }
        public void UpdateDrone(int Id, string Model)
        {
            bool Check = false;
            foreach (var drone in myDalObject.CopyDronesList())
            {
                if (drone.Id == Id)
                {
                    myDalObject.RemoveDrone(drone.Id);
                    myDalObject.AddDrone(drone.Id, drone.Battery, drone.MaxWeight, Model);
                    Check=true;
                    break;
                }
            }
            if (!Check)
                throw new DAL.DO.DroneIdNotFoundException();
            foreach (DroneForList droneForList in drones)
            {
                if (droneForList.DroneId == Id)
                {
                    RemoveDroneForList(droneForList.DroneId);
                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = droneForList.MaxWeight, Battery = droneForList.Battery, CurrentLocation = droneForList.CurrentLocation, DroneState = droneForList.DroneState, InDeliveringParcelId = droneForList.InDeliveringParcelId });
                    break;
                }
            }
        }
        public void DroneToCharge(int Id)
        {
            double NBattery = 20.0;//minimum value as a default, in the meanwhile until we insert value.
            bool check = false;
            foreach (DroneForList drone in drones)
            {
                double Battery;
                if (drone.DroneState == Enums.DroneStatuses.Available && drone.DroneId == Id)
                {
                    Location droneLocation = AddLocation(drone.CurrentLocation.Long, drone.CurrentLocation.Lat);
                    Location location = AddLocation(myDalObject.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Longitude, myDalObject.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Lattitude);
                    Battery = myDalObject.DronePowerConsumingPerKM()[0] * distanceFromBS(drone.CurrentLocation)[0];
                    if (Battery > drone.Battery)
                    {
                        throw new DroneOutOfBatteryException();//////////////////////////////צריך להגדיר חריגה מתאימה

                    }
                    foreach (var dalDrone in myDalObject.CopyDronesList())
                    {
                        if (dalDrone.Id == drone.DroneId)
                        {
                            myDalObject.RemoveDrone(dalDrone.Id);
                            myDalObject.AddDrone(dalDrone.Id, dalDrone.Battery - Battery, dalDrone.MaxWeight, dalDrone.Model);
                            NBattery = dalDrone.Battery - Battery;
                            //dalDrone.Longitude = location.Longitude;
                            //dalDrone.Latittude = location.Latitude;
                            drone.DroneState = Enums.DroneStatuses.Maintenance;
                            break;
                        }
                    }
                    foreach (var baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((baseStation.Longitude == location.Long) && (baseStation.Lattitude == location.Lat) && (baseStation.ChargeSlots > 0))
                        {
                            try { myDalObject.RemoveBaseStation(baseStation.Id); }
                            catch (DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            try { myDalObject.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude); }
                            catch (DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            check = true;
                            break;
                        }

                    }
                    if (!check)
                    {
                        throw new NoChargingSlotIsAvailableException();
                    }
                    //foreach (var baseStation1 in baseStations)
                    //{
                    //    if (baseStation1.StationLocation.Long == location.Long && baseStation1.StationLocation.Lat == location.Lat) 
                    //    {
                    //        baseStation1.DInChargeList.Add(new DroneInCharge { DroneId = drone.DroneId, Battery = NBattery, InsertionTime = DateTime.Now });
                    //        return;
                    //    }
                    //}

                }
            }
        }//לממש
        public void ReleaseDroneFromCharge(int Id, double TimeInCharge)
        {
            DroneForList nDrone = new DroneForList();
            foreach (DroneForList drone in drones)
            {
                if (drone.DroneId == Id)
                {
                    if (!(drone.DroneState == Enums.DroneStatuses.Maintenance))
                    {
                        throw new DroneIdNotFoundException();
                    }
                    nDrone = drone;
                    nDrone.Battery += TimeInCharge * myDalObject.DronePowerConsumingPerKM()[4];
                    if (nDrone.Battery > 100)
                    { nDrone.Battery = 100; }
                    nDrone.DroneState = Enums.DroneStatuses.Available;
                    foreach (var baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((nDrone.CurrentLocation.Long == baseStation.Longitude) && (nDrone.CurrentLocation.Lat == baseStation.Lattitude))
                        {
                            try { myDalObject.RemoveBaseStation(baseStation.Id); }
                            catch (DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            try { myDalObject.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots + 1, baseStation.Longitude, baseStation.Lattitude); }
                            catch (DAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            catch (DAL.DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
                            break;
                        }
                    }
                    //foreach (var baseStation1 in baseStations)
                    //{
                    //    if (baseStation1.StationLocation == nDrone.CurrentLocation)//he deleted the second BS from basestations i think bc the add drone op
                    //    {
                    //        baseStation1.DInChargeList.Remove(new DroneInCharge { DroneId = nDrone.DroneId, Battery = nDrone.Battery, InsertionTime=DateTime.Now });
                    //        break;
                    //    }
                    //}
                    drones.Add(nDrone);
                    return;
                }
            }
        }//לממש
        public DroneForList DisplayDrone(int id)
        {
            
            DroneForList nDrone = new DroneForList();
            foreach (DroneForList drone in drones)
            {
                if (drone.DroneId == id)
                {
                    nDrone = drone;
                    return nDrone;
                }
            }
            throw new DroneIdNotFoundException();
            //the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public IEnumerable<DroneForList> DisplayDronesList(Predicate<DroneForList> predicate)
        {
            IEnumerable<DroneForList> DronesList = drones.FindAll(predicate);
            return DronesList;
        }

        public void RemoveDroneForList(int Id)
        {
            bool Check=false;
            foreach (var drone in drones)
            {
                if (drone.DroneId == Id)
                {
                    try
                    { myDalObject.RemoveDrone(Id); }
                    catch ( DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    drones.Remove(drone);
                    Check = true;
                    break;
                }
            }
            if (!Check)
            {
                throw new DroneIdNotFoundException();
            }
        }
        public DateTime GetInsertionTime(int droneID)
        { 
            foreach(var bs in DisplayBaseStationsList(x=>x.BaseStationId==x.BaseStationId))
            {
                foreach(var dic in bs.DInChargeList)
                {
                    if (dic.DroneId==droneID)
                    {
                        return dic.InsertionTime;
                    }
                }
                throw new DroneIdNotFoundException();
            }
            throw new BaseStationNotFoundException();
        }
    }
    
}

