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
        bool isDroneInDelivering(DroneForList drone)
        {
            return true;
        }
        public BL()
        {
            myDalObject = new DalObject.DalObject();//initialize myDalObject
            drones = new List<DroneForList>();//drones list
            initializeDrones();
        }
        private void initializeDrones()
        {
            foreach (var drone in myDalObject.CopyDronesList())
            {
                drones.Add(new DroneForList
                {
                    DroneId = drone.Id,
                    Model = drone.Model,
                    MaxWeight = WeightParcel(drone.MaxWeight)
                });
            }
            foreach (var drone in drones)
            {
                if(droneWhileShipping(drone))
                {
                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    //drone.Battery=rand.Next(MinBattery,100)
                    if (ParcelNotPikedUpYet)
                    {
                        drone.CurrentLocation = findCloseBaseStationLocation(parcel.senderLocation);
                    }
                    else
                    {
                        if (ParcelPickedUpButNotDeliveredYet)
                        {
                            drone.CurrentLocation = parcel.senderLocation
                        }
                    }
                }
                else
                {
                    drone.DroneState = (Enums.DroneStatuses)rand.Next(1, 2);
                    if (drone.DroneState == Enums.DroneStatuses.Maintenance)
                    {
                        //drone.CurrentLocation=rand.Next(baseStations)//location is random between the basestations
                        drone.Battery = rand.NextDouble() + rand.Next(0, 20);
                    }
                    else
                    { 
                        if (drone.DroneState == Enums.DroneStatuses.Available)
                        { 
                            //drone.CurrentLocation=rand.Next(customers)//location is random between customers that parcels has just delivered to them
                            //drone.Battery=rand.Next(MinBattery,100)
                        }
                    }


                }
            }
        }
    }
}
