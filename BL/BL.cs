using System;
using System.Collections.Generic;
using System.Linq;
using IDAL;
using IBL.BO;
namespace IBL
{
    public partial class BL : Ibl
    {
        private IDal myDalObject;
        public List<DroneForList> drones;
        public List<BaseStationForList> baseStations;
        private static Random rand = new Random();
        public BL()
        {
            myDalObject = new DalObject.DalObject();//initialize myDalObject
            drones = new List<DroneForList>();//drones list
            baseStations = new();
            initializeDrones();
        }
        private int droneWhileShipping(int droneId)//check if there is a parcel that the drone is ascripted to
        {
            foreach (var parcel in myDalObject.CopyParcelsList())
            {
                if (parcel.DroneId==droneId)
                {
                    return parcel.Id;
                }
            }
            return -1;
        }
        private Location randomCustomerLocation(int num)//location is random between customers that parcels has just delivered to them
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
        private Location randomBSLocation(int num)//location is random between the basestations
        {
            int counter = 0;
            Location bsLocation;
            foreach(var bs in myDalObject.CopyBaseStations())
            {
                if (counter==num)
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
                if (parcelId!=-1)
                {
                    var parcel = myDalObject.CopyParcel(parcelId);
                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    drone.InDeliveringParcelId = parcelId;
                    var c = myDalObject.CopyCustomer(parcel.SenderId);
                    Location sLocation = AddLocation(c.Longitude, c.Lattitude);
                    if (parcel.PickedUp==null)
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
                    double minBattery = minimumBattery(drone, parcel);//random.NextDouble() * (maximum - minimum) + minimum
                    drone.Battery = rand.NextDouble() * (100 - minBattery) + minBattery;
                }
                else
                {
                    drone.DroneState = (Enums.DroneStatuses)rand.Next(1, 2);
                    if (drone.DroneState == Enums.DroneStatuses.Maintenance)
                    {
                        int random = rand.Next(0, myDalObject.CopyBaseStations().Count());
                        Location randomBS = randomBSLocation(random);
                        drone.CurrentLocation = randomBS;
                        drone.Battery = rand.NextDouble() * 20;
                    }
                    else
                    { 
                        if (drone.DroneState == Enums.DroneStatuses.Available)
                        {
                            int random = rand.Next(0, customersWithReceivedParcelsList().Count());
                            Location randomCustomer = randomCustomerLocation(random);
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
            double[] temp1 = myDalObject.CopyLongitudeRange();
            double[] temp2 = myDalObject.CopyLattitudeRange();
            return temp1[0] < longitude && longitude < temp1[1] && temp2[0] < lat && lat < temp2[1]
                ? AddLocation(longitude, lat)
                : throw AddLocationOutOfRangeException();
        }
    }
}
