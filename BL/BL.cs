﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DalApi;
using BL.BO;
using BL.BlApi;
namespace BL
{
    internal sealed partial class BL : IBL
    {
        static readonly BL instance = new BL();
        public static BL Instance { get => instance; }
        static BL() { }
        private IDAL myDalObject;
        public List<DroneForList> drones;
        public List<BaseStationForList> baseStations;
        private static Random rand = new Random();
        BL()
        {
            myDalObject = DalFactory.GetDal("1");//initialize myDalObject
            drones = new List<DroneForList>();//drones list
            baseStations = new();
            initializeDrones();
        }
        private int droneWhileShipping(int droneId)//check if there is a parcel that the drone is ascripted to
        {
            foreach (var parcel in myDalObject.CopyParcelsList())
            {
                if (parcel.DroneId == droneId)
                {
                    return parcel.Id;
                }
            }
            return -1;
        }
        private Location RandomCustomerLocation(int num)//location is Random between customers that parcels has just delivered to them
        {
            int counter = 0;
            foreach (var l in customersWithReceivedParcelsList())
            {
                if (counter == num)
                {
                    return l;
                }
            }
            throw new Exception();
        }
        private Location RandomBSLocation(int num)//location is Random between the basestations
        {
            int counter = 0;
            Location bsLocation;
            foreach (var bs in myDalObject.CopyBaseStations())
            {
                if (counter == num)
                {
                    bsLocation = AddLocation(bs.Longitude, bs.Lattitude);
                    return bsLocation;
                }
            }
            throw new Exception();
        }
        private List<Location> customersWithReceivedParcelsList()//return a list of customers that received parcels
        {
            Customer customer;
            List<Location> list = new List<Location>();
            foreach (var c in myDalObject.CopyParcelsList())
            {
                customer = DisplayCustomer(c.Id);
                if (customer.ParcelsFromCustomer != null)
                    list.Add(customer.Place);
            }
            return list;
        }
        private void initializeDrones()
        {

            foreach (var dalDrone in myDalObject.CopyDronesList())
            {
                DroneForList drone = new DroneForList
                {
                    DroneId = dalDrone.Id,
                    Model = dalDrone.Model,
                    MaxWeight = WeightParcel(dalDrone.MaxWeight)
                };
                int parcelId = droneWhileShipping(drone.DroneId);
                if (parcelId != -1)
                {
                    var parcel = myDalObject.CopyParcel(parcelId);
                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    drone.InDeliveringParcelId = parcelId;
                    var c = myDalObject.CopyCustomer(parcel.SenderId);
                    Location sLocation = AddLocation(c.Longitude, c.Lattitude);
                    if (parcel.PickedUp == null)
                    {
                        var bs = myDalObject.CopyBaseStation((int)distanceFromBS(sLocation)[1]);
                        Location bsLocation = AddLocation(bs.Longitude, bs.Lattitude);
                        drone.CurrentLocation = bsLocation;
                    }
                    else
                    {
                        if (parcel.PickedUp != null)
                        {
                            drone.CurrentLocation = sLocation;
                        }
                    }
                    double minBattery = minimumBattery(drone, parcel);//Random.NextDouble() * (maximum - minimum) + minimum
                    drone.Battery = rand.NextDouble() * (100 - minBattery) + minBattery;
                }
                else
                {
                    drone.DroneState = (Enums.DroneStatuses)rand.Next(1, 2);
                    if (drone.DroneState == Enums.DroneStatuses.Maintenance)
                    {
                        int random = rand.Next(0, myDalObject.CopyBaseStations().Count());
                        Location randomBS = RandomBSLocation(random);
                        drone.CurrentLocation = randomBS;
                        drone.Battery = rand.NextDouble() * 20;
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