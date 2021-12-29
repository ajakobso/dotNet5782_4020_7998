﻿using System;
using System.Collections.Generic;
using System.Linq;
using DalApi;
using BO;
using BlApi;
namespace BL
{
    internal sealed partial class BL : IBL
    {
        static readonly BL instance = new BL();
        public static BL GetInstance() { return instance; }
        static BL() { }
        private IDAL myDalObject;
        public List<DroneForList> drones;
        //public List<BaseStationForList> baseStations;
        private Random rand = new Random();
        BL()
        {
            myDalObject = DalFactory.GetDal();//initialize myDalObject
            drones = new List<DroneForList>();//drones list
            //baseStations = new();
            initializeDrones();
        }
        private int droneWhileShipping(int droneId)//check if there is a parcel that the drone is ascripted to
        {
            IEnumerable<DO.Parcel> parcels = new List<DO.Parcel>();
            //(parcels as List<DO.Parcel>).ForEach();
            DO.Parcel selectedParcel=new();
            bool Check = false;
            parcels = myDalObject.CopyParcelsList();
            selectedParcel=(parcels as List<DO.Parcel>).Find(x => (x.DroneId == droneId) /* Check = true) */);//if ERROR- do .FirstOrDefault!!
            ////foreach (var parcel in from parcel in myDalObject.CopyParcelsList()
            ////                       where parcel.DroneId == droneId
            ////                       select parcel)
            ////{
            ////    selectedParcel = parcel;
            ////    Check = true;
            ////   // return parcel.Id;
            ////}
            if (Check)
                return selectedParcel.Id;
            return -1;
        }
        private Location RandomCustomerLocation(int num)//location is Random between customers that parcels has just delivered to them
        {
            int counter = 0;
            IEnumerable<Location> locations = new List<Location>();
            locations = customersWithReceivedParcelsList();
            //(locations as List<Location>).ForEach()
            foreach (var l in locations)/////////////////////////////not linq, doesnt working
            {
                if (counter == num)
                {
                    return l;
                }
                else
                {
                    counter++;
                }
            }
            throw new Exception();
        }
        private DO.BaseStation RandomBSLocation(int num)//location is Random between the basestations
        {
            int counter = 0;//mistake maybe here
            IEnumerable<DO.BaseStation> baseStations = new List<DO.BaseStation>();
            baseStations = myDalObject.CopyBaseStations();
            //(baseStations as List<DO.BaseStation>).ForEach(from bs in myDalObject.CopyBaseStations() where (counter == num) 
            //                                                   let baseStation=myDalObject.CopyBaseStation(bs.Id) select baseStation);
            //baseStations.ToList().ForEach(from bs in baseStations where counter == num
              //                                                 let baseStation = myDalObject.CopyBaseStation(bs.Id) select baseStation);
            foreach (var bs in baseStations)////////////////////////////////////not linq, doesnt working
            {
                if (counter == num)
                {
                    //bsLocation = AddLocation(bs.Longitude, bs.Lattitude);
                    return /*bsLocation;*/ bs;
                }
                else { counter++; }
            }
            throw new Exception();
        }
        private IEnumerable<Location> customersWithReceivedParcelsList()//return a list of customers that received parcels
        {
            Customer customer=new();
            IEnumerable<Location> LocList = new List<Location>();
            IEnumerable<DO.Parcel> ParceList = new List<DO.Parcel>();
            //(ParceList as List<DO.Parcel>).ForEach(x => customer = DisplayCustomer(x.TargetId), (customer.ParcelsFromCustomer
             //    != null) ? (LocList as List<Location>).Add(customer.Place));/////////////////////////////
            foreach (DO.Parcel p in myDalObject.CopyParcelsList())////////////////////////not linq, doesnt working
            {
                customer = DisplayCustomer(p.TargetId);
                if (customer.ParcelsFromCustomer != null)
                    (LocList as List<Location>).Add(customer.Place);
            }
            return LocList;
        }
        private void initializeDrones()
        {

            foreach (var dalDrone in myDalObject.CopyDronesList())/////////////not linq, doesnt working
            {
                DroneForList drone = new DroneForList
                {
                    DroneId = dalDrone.Id,
                    Model = dalDrone.Model,
                    MaxWeight = WeightParcel(dalDrone.MaxWeight)
                };
                // DroneInCharge nDIC;
                List<DroneInCharge> dicList = new();
                //BaseStationForList nBsForList;
                //BaseStationForList oldBs = new();
                int parcelId = droneWhileShipping(drone.DroneId);
                if (parcelId != -1)//the drone is ascripted to a parcel
                {
                    var parcel = myDalObject.CopyParcel(parcelId);
                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    drone.InDeliveringParcelId = parcelId;
                    var c = myDalObject.CopyCustomer(parcel.SenderId);
                    Location sLocation = AddLocation(c.Longitude, c.Lattitude);

                    if (parcel.PickedUp == null)//the drone didnt pick up the parcel yet - location: the closest base station to the sender customer
                    {
                        var bs = new DO.BaseStation();
                        try { bs = myDalObject.CopyBaseStation((int)distanceFromBS(sLocation)[1]); }
                        catch (BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                        Location bsLocation = AddLocation(bs.Longitude, bs.Lattitude);
                        drone.CurrentLocation = bsLocation;
                        #region add drone to first charge in the closest base station to the customer
                        //nDIC = new DroneInCharge { DroneId = drone.DroneId, Battery = rand.NextDouble() * (100 - minimumBattery(drone, parcel)) + minimumBattery(drone, parcel), InsertionTime = DateTime.Now };
                        //try { oldBs = DisplayBaseStation(bs.Id); }
                        //catch (BaseStationNotFoundException) 
                        //{
                        //    dicList.Add(nDIC);

                        //nBsForList = new BaseStationForList { BaseStationId = bs.Id, StationLocation = bsLocation, StationName = bs.Name, DInChargeList = dicList, AvailableChargingS = bs.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
                        //baseStations.Add(nBsForList);
                        //}
                        //if (oldBs.DInChargeList != null)
                        //{ dicList = oldBs.DInChargeList; }
                        //dicList.Add(nDIC);
                        //nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
                        //baseStations.Remove(oldBs);
                        //baseStations.Add(nBsForList);
                        #endregion
                    }
                    else
                    {
                        if (parcel.PickedUp != null)//the drone picked up the parcel - his location is at the customer
                        {
                            drone.CurrentLocation = sLocation;
                        }
                    }
                    double minBattery = minimumBattery(drone, parcel);//Random.NextDouble() * (maximum - minimum) + minimum
                    double r1 = rand.NextDouble();
                    double r2 = 100 - minBattery;
                    drone.Battery = r1 * r2 + minBattery;
                }
                else
                {
                    drone.DroneState = (Enums.DroneStatuses)rand.Next(0, 2);
                    if (drone.DroneState == Enums.DroneStatuses.Maintenance)
                    {
                        int random = rand.Next(0, myDalObject.CopyBaseStations().Count());
                        var randomBS = RandomBSLocation(random);
                        drone.CurrentLocation = AddLocation(randomBS.Longitude, randomBS.Lattitude);
                        drone.Battery = rand.NextDouble() * 20;
                        //#region add drone to first charge in base station
                        //nDIC = new DroneInCharge { DroneId = drone.DroneId, Battery = drone.Battery, InsertionTime = DateTime.Now };
                        //try 
                        //{ 
                        //oldBs = DisplayBaseStation(randomBS.Id);
                        //dicList = oldBs.DInChargeList;// the list contains nothingfor some reason
                        //dicList.Add(nDIC);
                        // nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
                        // baseStations.Remove(oldBs);
                        // baseStations.Add(nBsForList);
                        // }
                        //catch (BaseStationNotFoundException)
                        //{
                        //    dicList.Add(nDIC);
                        //    nBsForList = new BaseStationForList { BaseStationId = randomBS.Id, StationLocation = drone.CurrentLocation, StationName = randomBS.Name, DInChargeList = dicList, AvailableChargingS = randomBS.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
                        //    baseStations.Add(nBsForList);
                        //}
                        // #endregion
                    }
                    else
                    {
                        if (drone.DroneState == Enums.DroneStatuses.Available)
                        {
                            int random = rand.Next(0, customersWithReceivedParcelsList().Count());
                            Location randomCustomer = RandomCustomerLocation(random);
                            drone.CurrentLocation = randomCustomer;
                            var bs = myDalObject.CopyBaseStation((int)distanceFromBS(randomCustomer)[1]);
                            double minBattery = myDalObject.DronePowerConsumingPerKM()[0] * Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, bs.Longitude, bs.Lattitude);
                            drone.Battery = rand.NextDouble() * (100 - minBattery) + minBattery;
                        }
                    }
                }
                drones.Add(drone);
            }

        }
        public Location AddLocation(double longitude, double lat)
        {
            _ = new double[2];
            double[] temp1 = myDalObject.CopyLongitudeRange();
            _ = new double[2];
            double[] temp2 = myDalObject.CopyLattitudeRange();
            Location location = new Location(longitude, lat);
            return (temp1[0] < longitude && longitude < temp1[1] && temp2[0] < lat && lat < temp2[1])
                ? location
                : throw new LocationOutOfRangeException();
        }
    }

}