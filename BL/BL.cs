using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL;
using DalObject;
using IBL.BO;
namespace IBL
{
    public partial class BL :IBL
    {
        public IDal dal;
        private List<DroneForList> drones;
        private static Random r = new Random();
        public BL()
        {
            dal = new DalObject.DalObject();
            drones = new List<DroneForList>();//drones list
            foreach(var drone in dal.GetDrone())
            {
                drones.Add(new DroneForList {DroneId = drone.Id, Model = drone.Model, MaxWeight = (Enums.WeightCategories)drone.MaxWeight });

            }
            foreach (var drone in drones)
            {
                if (isDroneInDelivering(drone))///////////////////////////
                {
                    drone.DroneStatus = Enums.DroneStatuses.Shipping;
                }
            }
          
        }
    }
}
