using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IDAL;
using DalObject;
using IBL.BO;
namespace IBL.BO
{
    public partial class BL /*:IBL*/
    {
        private IDAL dal;
        private List<DroneForList> drones;
        private static Random r = new Random();
        public BL()
        {
            dal = new DalObject.DalObject();
            drones = new List<DroneForList>();
            foreach(var drone in dal.GetDrones())
            {
                drones.Add(new DroneForList { Id = drone.Id, Model = drone.Model, MaxWeight = (WeightCategories)drone.MaxWeight });

            }
            foreach(var drone in drones)
            {
                if (isDroneInDelivering(drone))///////////////////////////
                {
                    drone.DroneStatus = Enums.DroneStatuses.Shipping;
                }
            }
        }
    }
}
