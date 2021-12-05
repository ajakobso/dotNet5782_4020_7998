using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IBL.BO;
namespace IBL
{
    public partial class BL : IBL
    {
        private IDal myDalObject;
        public List<DroneForList> drones;
        public List<BaseStationForList> baseStations;
        private static Random rand = new Random();
        public BL()
        {
            myDalObject = new DalObject.DalObject();//initialize myDalObject
            drones = new List<DroneForList>();//drones list
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
                    bsLocation = new Location(bs.Longitude, bs.Lattitude);
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
                    double minBattery = minimumBattery(drone, parcel);
                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    drone.Battery = rand.Next(((int)minBattery) + 1, 100) + rand.NextDouble();
                    drone.InDeliveringParcelId = parcelId;
                    var c = myDalObject.CopyCustomer(parcel.SenderId);
                    Location sLocation = new Location(c.Longitude, c.Lattitude);
                    if (parcel.PickedUp==DateTime.MinValue)
                    {
                        var bs = myDalObject.CopyBaseStation((int)distanceFromBS(sLocation)[1]);
                        Location bsLocation = new Location(bs.Longitude, bs.Lattitude);
                        drone.CurrentLocation = bsLocation;
                    }
                    else
                    {
                        if (parcel.PickedUp > DateTime.MinValue)
                        {
                            drone.CurrentLocation = sLocation;
                        }
                    }
                }
                else
                {
                    drone.DroneState = (Enums.DroneStatuses)rand.Next(1, 2);
                    if (drone.DroneState == Enums.DroneStatuses.Maintenance)
                    {
                        int random = rand.Next(0, myDalObject.CopyBaseStations().Count());
                        Location randomBS = randomBSLocation(random);
                        drone.CurrentLocation = randomBS;
                        drone.Battery = rand.NextDouble() + rand.Next(0, 20);
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
                            drone.Battery = rand.Next((int)minBattery + 1, 100) + rand.NextDouble();
                        }
                    }


                }
                drones.Add(drone);
            }

        }
    }
}
