using System;
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
        private Random rand = new Random();

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
                else
                {
                    counter++;
                }
            }
            throw new Exception();
        }
        private DAL.DO.BaseStation RandomBSLocation(int num)//location is Random between the basestations
        {
            int counter = 0;//mistake maybe here
            foreach (var bs in myDalObject.CopyBaseStations())
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
        private List<Location> customersWithReceivedParcelsList()//return a list of customers that received parcels
        {
            Customer customer;
            List<Location> list = new List<Location>();
            foreach (var p in myDalObject.CopyParcelsList())
            {
                customer = DisplayCustomer(p.TargetId);
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
                DroneInCharge nDIC;
                List<DroneInCharge> dicList = new();
                BaseStationForList nBsForList;
                BaseStationForList oldBs = new();
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
                        var bs = myDalObject.CopyBaseStation((int)distanceFromBS(sLocation)[1]);
                        Location bsLocation = AddLocation(bs.Longitude, bs.Lattitude);
                        drone.CurrentLocation = bsLocation;
                        #region add drone to first charge in the closest base station to the customer
                        nDIC = new DroneInCharge { DroneId = drone.DroneId, Battery = rand.NextDouble() * (100 - minimumBattery(drone, parcel)) + minimumBattery(drone, parcel), InsertionTime = DateTime.Now };
                        try { oldBs = DisplayBaseStation(bs.Id); }
                        catch (BaseStationNotFoundException) 
                        {
                            dicList.Add(nDIC);
                            nBsForList = new BaseStationForList { BaseStationId = bs.Id, StationLocation = bsLocation, StationName = bs.Name, DInChargeList = dicList, AvailableChargingS = bs.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
                            baseStations.Add(nBsForList);
                        }
                        if (oldBs.DInChargeList != null)
                        { dicList = oldBs.DInChargeList; }
                        dicList.Add(nDIC);
                        nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
                        baseStations.Remove(oldBs);
                        baseStations.Add(nBsForList);
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
                        #region add drone to first charge in base station
                        nDIC = new DroneInCharge { DroneId = drone.DroneId, Battery = drone.Battery, InsertionTime = DateTime.Now };
                        try 
                        { 
                            oldBs = DisplayBaseStation(randomBS.Id);
                            dicList = oldBs.DInChargeList;// the list contains nothingfor some reason
                            dicList.Add(nDIC);
                            nBsForList = new BaseStationForList { BaseStationId = oldBs.BaseStationId, StationLocation = oldBs.StationLocation, StationName = oldBs.StationName, DInChargeList = dicList, AvailableChargingS = oldBs.AvailableChargingS - 1, UnAvailableChargingS = oldBs.UnAvailableChargingS++ };
                            baseStations.Remove(oldBs);
                            baseStations.Add(nBsForList);
                        }
                        catch (BaseStationNotFoundException)
                        {
                            dicList.Add(nDIC);
                            nBsForList = new BaseStationForList { BaseStationId = randomBS.Id, StationLocation = drone.CurrentLocation, StationName = randomBS.Name, DInChargeList = dicList, AvailableChargingS = randomBS.AvailableChargeSlots - 1, UnAvailableChargingS = 1 };
                            baseStations.Add(nBsForList);
                        }
                        #endregion
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