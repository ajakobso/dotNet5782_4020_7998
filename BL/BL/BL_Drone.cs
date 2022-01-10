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
        public void AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation)
        {
            lock (myDal)
            {
                Random r = new Random();//אין לי שמץ של מושג אם ככה מגדירים רנדום
                r.Next(20, 41);//מגריל מספר בין 20 ל-40 לפי מה שהבנתי
                Location BStationLocation;
                foreach (var baseStation in from baseStation in myDal.CopyBaseStations()//כרגיל לא עובד
                                            where baseStation.Id == Bstation
                                            select baseStation)
                {
                    try { BStationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude); }
                    catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//add throw of location exception in all the references of the AddLocation

                    double battery = r.NextDouble() * (40 - 20) + 20;
                    try { myDal.AddDrone(Id, battery, (DO.WeightCategories)MaxWeight, Model); }
                    catch (DO.AddExistingDroneException) { throw new AddExistingDroneException(); }

                    try { myDal.DroneCharging(Id, Bstation); }
                    catch (DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }

                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = MaxWeight, DroneState = Enums.DroneStatuses.Maintenance, Battery = r.NextDouble() * (40 - 20) + 20, CurrentLocation = BStationLocation });
                    myDal.RemoveBaseStation(Bstation);
                    myDal.AddBaseStation(Bstation, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude);
                    return;
                }
                throw new BaseStationNotFoundException();
            }
            #region notes
            //foreach (var baseStation in myDal.CopyBaseStations())//כרגיל לא עובד//the not-linq
            //{
            //    if (baseStation.Id == Bstation)
            //    {
            //        try { BStationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude); }
            //        catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//add throw of location exception in all the references of the AddLocation
            //        double battery = r.NextDouble() * (40 - 20) + 20;
            //        try { myDal.AddDrone(Id, battery, (DO.WeightCategories)MaxWeight, Model); }
            //        catch (DO.AddExistingDroneException) { throw new AddExistingDroneException(); }
            //        try { myDal.DroneCharging(Id, Bstation); }
            //        catch (DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
            //        drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = MaxWeight, DroneState = Enums.DroneStatuses.Maintenance, Battery = r.NextDouble() * (40 - 20) + 20, CurrentLocation = BStationLocation });
            //        myDal.RemoveBaseStation(Bstation);
            //        myDal.AddBaseStation(Bstation, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude);
            //        // #region add drone to first charge in base station
            //        //DroneInCharge nDIC = new DroneInCharge { DroneId = Id, Battery = battery, InsertionTime = DateTime.Now };
            //        // List<DroneInCharge> dicList = new();
            //        // BaseStationForList oldBs = new(), nBsForList;
            //        //try { oldBs = DisplayBaseStation(baseStation.Id); }
            //        // catch (BaseStationNotFoundException)
            //        // {
            //        //     dicList.Add(nDIC);
            //        //    nBsForList = new BaseStationForList { BaseStationId = baseStation.Id, StationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude), StationName = baseStation.Name, DInChargeList = dicList, AvailableChargingS = baseStation.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
            //        //   baseStations.Add(nBsForList);
            //        //   return;
            //        // }
            //        // dicList = oldBs.DInChargeList;
            //        // dicList.Add(nDIC);
            //        //nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
            //        //baseStations.Remove(oldBs);
            //        //baseStations.Add(nBsForList);
            //        // #endregion
            //        return;
            //    }
            //}
            #endregion
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int Id, string Model)
        {
            lock (myDal)
            {
                bool Check = false;
                foreach (var drone in from drone in myDal.CopyDronesList()
                                      where drone.Id == Id
                                      select drone)
                {
                    myDal.RemoveDrone(drone.Id);
                    myDal.AddDrone(drone.Id, drone.Battery, drone.MaxWeight, Model);
                    Check = true;
                    break;
                }
                //foreach (var drone in myDal.CopyDronesList())//the not-linq
                //{
                //    if (drone.Id == Id)
                //    {
                //        myDal.RemoveDrone(drone.Id);
                //        myDal.AddDrone(drone.Id, drone.Battery, drone.MaxWeight, Model);
                //        Check = true;
                //        break;
                //    }
                //}
                if (!Check)
                    throw new DO.DroneIdNotFoundException();
                foreach (var droneForList in from DroneForList droneForList in drones
                                             where droneForList.DroneId == Id
                                             select droneForList)
                {
                    RemoveDroneForList(droneForList.DroneId);
                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = droneForList.MaxWeight, Battery = droneForList.Battery, CurrentLocation = droneForList.CurrentLocation, DroneState = droneForList.DroneState, InDeliveringParcelId = droneForList.InDeliveringParcelId });
                    break;
                }
            }
            //foreach (DroneForList droneForList in drones)//the not-linq
            //{
            //    if (droneForList.DroneId == Id)
            //    {
            //        RemoveDroneForList(droneForList.DroneId);
            //        drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = droneForList.MaxWeight, Battery = droneForList.Battery, CurrentLocation = droneForList.CurrentLocation, DroneState = droneForList.DroneState, InDeliveringParcelId = droneForList.InDeliveringParcelId });
            //        break;
            //    }
            //}
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneToCharge(int Id)
        {
            lock (myDal)
            {
                double NBattery = 20.0;//minimum value as a default, in the meanwhile until we insert value.
                bool check = false;
                foreach (DroneForList drone in drones)
                {
                    double Battery;
                    if (drone.DroneState == Enums.DroneStatuses.Available && drone.DroneId == Id)
                    {
                        Location droneLocation = AddLocation(drone.CurrentLocation.Long, drone.CurrentLocation.Lat);
                        Location location = AddLocation(myDal.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Longitude, myDal.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Lattitude);
                        Battery = myDal.DronePowerConsumingPerKM()[0] * distanceFromBS(drone.CurrentLocation)[0];
                        if (Battery > drone.Battery)
                        {
                            throw new DroneOutOfBatteryException();//////////////////////////////צריך להגדיר חריגה מתאימה

                        }

                        foreach (var dalDrone in from dalDrone in myDal.CopyDronesList()//linq
                                                 where dalDrone.Id == drone.DroneId
                                                 select dalDrone)
                        {
                            myDal.RemoveDrone(dalDrone.Id);
                            myDal.AddDrone(dalDrone.Id, dalDrone.Battery - Battery, dalDrone.MaxWeight, dalDrone.Model);
                            NBattery = dalDrone.Battery - Battery;
                            //dalDrone.Longitude = location.Longitude;
                            //dalDrone.Latittude = location.Latitude;
                            drone.DroneState = Enums.DroneStatuses.Maintenance;
                            break;
                        }

                        foreach (var baseStation in from baseStation in myDal.CopyBaseStations()
                                                    where (baseStation.Longitude == location.Long) && (baseStation.Lattitude == location.Lat) && (baseStation.ChargeSlots > 0)
                                                    select baseStation)//linq
                        {
                            try { myDal.RemoveBaseStation(baseStation.Id); }
                            catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                            try { myDal.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude); }
                            catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                            check = true;
                            break;
                        }

                        if (!check)
                        {
                            throw new NoChargingSlotIsAvailableException();
                        }
                    }
                }
            }
        }//לממש
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromCharge(int Id, double TimeInCharge)
        {
            lock (myDal)
            {
                DroneForList nDrone = new DroneForList();
                foreach (var drone in from DroneForList drone in drones
                                      where drone.DroneId == Id
                                      select drone)//linq
                {
                    if (!(drone.DroneState == Enums.DroneStatuses.Maintenance))
                    {
                        throw new DroneIdNotFoundException();
                    }

                    nDrone = drone;
                    nDrone.Battery += TimeInCharge * myDal.DronePowerConsumingPerKM()[4];
                    if (nDrone.Battery > 100)
                    { nDrone.Battery = 100; }

                    nDrone.DroneState = Enums.DroneStatuses.Available;
                    foreach (var baseStation in from baseStation in myDal.CopyBaseStations()//linq
                                                where (nDrone.CurrentLocation.Long == baseStation.Longitude) && (nDrone.CurrentLocation.Lat == baseStation.Lattitude)
                                                select baseStation)
                    {
                        try { myDal.RemoveBaseStation(baseStation.Id); }
                        catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }

                        try { myDal.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots, baseStation.AvailableChargeSlots + 1, baseStation.Longitude, baseStation.Lattitude); }
                        catch (DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                        catch (DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }

                        break;
                    }

                    drones.Add(nDrone);
                    return;
                }
            }
        }//לממש
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneForList DisplayDrone(int id)
        {
            lock (myDal)
            {
                DroneForList nDrone = new DroneForList();
                foreach (var drone in from DroneForList drone in drones
                                      where drone.DroneId == id
                                      select drone)//linq
                {
                    nDrone = drone;
                    return nDrone;
                }
                throw new DroneIdNotFoundException();
            }
            //the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            lock (myDal)
            {
                DroneForList nDrone = new DroneForList();
                foreach (var drone in from DroneForList drone in drones
                                      where drone.DroneId == id
                                      select drone)//linq
                {
                    ParcelInDelivering p = null;
                    if (drone.InDeliveringParcelId != 0)
                    {
                        foreach (var parcel in from DO.Parcel parcel in myDal.CopyParcelsList()
                                               where drone.InDeliveringParcelId == parcel.Id
                                               select parcel)
                        {
                            Parcel boParcel = DisplayParcel(parcel.Id);
                            Customer sender = DisplayCustomer(boParcel.SCIParcel.CustomerId);
                            Customer target = DisplayCustomer(boParcel.DCIParcel.CustomerId);
                            if (drone.DroneState == Enums.DroneStatuses.Shipping || drone.InDeliveringParcelId > 1000)
                            {
                                return new Drone
                                {
                                    DroneId = drone.DroneId,
                                    Model = drone.Model,
                                    MaxWeight = drone.MaxWeight,
                                    CurrentLocation = drone.CurrentLocation,
                                    DeliveryParcel = new ParcelInDelivering
                                    {
                                        ParcelId = boParcel.ParcelId,
                                        ParcelPriority = boParcel.ParcelPriority,
                                        ParcelWC = boParcel.ParcelWC,
                                        Distance = Distance(sender.Place.Long, sender.Place.Lat, target.Place.Long, target.Place.Lat),
                                        SenderLocation = sender.Place,
                                        TargetLocation = target.Place,
                                        Sender = new CustomerInParcel { CustomerId = sender.CustomerId, CustomerName = sender.CustomerName },
                                        Target = new CustomerInParcel { CustomerId = target.CustomerId, CustomerName = target.CustomerName },
                                        ParcelState = boParcel.ParcelPickUpTime != null && boParcel.ParcelDeliveringTime == null
                                    },
                                    DroneStatus = drone.DroneState,
                                    Battery = drone.Battery
                                };

                            }

                        }
                    }
                    else
                    {
                        return new Drone
                        {
                            DroneId = drone.DroneId,
                            Model = drone.Model,
                            MaxWeight = drone.MaxWeight,
                            CurrentLocation = drone.CurrentLocation,
                            DeliveryParcel = new ParcelInDelivering(),
                            DroneStatus = drone.DroneState,
                            Battery = drone.Battery
                        };
                    }
                }
                throw new DroneIdNotFoundException();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneForList> DisplayDronesList(Predicate<DroneForList> predicate)
        {
            lock (myDal)
            {
                IEnumerable<DroneForList> DronesList = drones.FindAll(predicate);
                return DronesList;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneForList(int Id)
        {
            lock (myDal)
            {
                bool Check = false;
                foreach (var drone in from drone in drones
                                      where drone.DroneId == Id
                                      select drone)//linq
                {
                    try
                    { myDal.RemoveDrone(Id); }
                    catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }

                    drones.Remove(drone);
                    Check = true;
                    break;
                }

                if (!Check)
                {
                    throw new DroneIdNotFoundException();
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DateTime GetInsertionTime(int droneID)
        {
            lock (myDal)
            {
                foreach (var bs in DisplayBaseStationsList(x => x.BaseStationId == x.BaseStationId))//not linq
                {
                    foreach (var dic in from dic in bs.DInChargeList
                                        where dic.DroneId == droneID
                                        select dic)//linq
                    {
                        return dic.InsertionTime;
                    }
                    throw new DroneIdNotFoundException();
                }
                throw new BaseStationNotFoundException();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SimulatorActivation(int droneId, Action updateDisplay, Func<bool> stopCheck)
        {
            _= new Simulator(this, droneId, updateDisplay, stopCheck);
        }//implement this
    }
}

