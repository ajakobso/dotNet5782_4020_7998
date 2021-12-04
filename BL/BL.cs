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
        private static Random r = new Random();
        bool isDroneInDelivering(DroneForList drone)
        {
            return true;
        }
        public BL()
        {
            myDalObject = new DalObject.DalObject();//initialize myDalObject
            drones = new List<DroneForList>();//drones list
            //foreach(var drone in myDalObject.CopyDronesList())

            //{
            //    drones.Add(new DroneForList {DroneId = drone.Id, Model = drone.Model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight });
           
            //}
            foreach (var drone in drones)
            {
                if (isDroneInDelivering(drone))
                {
                    drone.DroneStatus = Enums.DroneStatuses.Shipping;
                }
            }
          
        }
    }
}
