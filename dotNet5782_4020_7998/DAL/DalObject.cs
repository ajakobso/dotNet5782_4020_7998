using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public static void AddBaseStation(int id, string name, int chargeSlots, double longitude, double lattitude)
        {
            DataSource.BaseStations.Add(new BaseStation { Id = id, Name = name, ChargeSlots = chargeSlots, Longitude = longitude, Lattitude = lattitude });
        }
        public static void AddDrone(int id, double battery, WeightCategories maxW, string model, DroneStatuses status)
        {
            DataSource.Drones.Add(new Drone { Id = id, Battery = battery, MaxWeight = maxW, Model = model, Status = status });
        }
        public static void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            DataSource.Customers.Add(new Customer { Id = id, Name = name, Phone = phone, Longitude = longitude, Lattitude = lattitude });
        }
        public static void AddParcel(int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime requested, DateTime scheduled, DateTime pickedUp, DateTime delivered)
        {
            DataSource.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = droneId, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
        }
        public static void AscriptionPtoD(Parcel parcel)// ascription a parcel with an available drone
        {
            foreach (Drone drone in DataSource.Drones)
            {
                if(drone.Status==DroneStatuses.Available)
                {
                    parcel.DroneId = drone.Id;
                    return;
                }
            }
        }
        public static void PickUpParcel(Parcel parcel)
        {
            
            foreach (Drone drone in DataSource.Drones)
            {
                if (drone.Id == parcel.DroneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Shipping, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                    DataSource.Drones.Remove(drone);
                    DataSource.Drones.Add(newDrone);
                    return;
                }
            }
        }
        public static void PaclerDelivering(int parcelId)//אם קלט הפונקציה זה איבר מסוג חבילה אז אפשר למחוק את הלולאה של פוראיצ הראשונה, העיקרון שעשיתי פה אבל ישמש אותנו בפונקציות של ההצגה של איבר/רשימה.
        {
            foreach (Parcel parcel in DataSource.Parcels)///find the pacler by its id
            {
                if(parcel.Id == parcelId)
                {
                    foreach (Drone drone in DataSource.Drones)///find the drone that ascribed to the pacler
                    {
                        if (drone.Id == parcel.DroneId)
                        {
                            Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Available, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                            DataSource.Drones.Remove(drone);
                            DataSource.Drones.Add(newDrone);///change the status of the drone into available because he finish the shipment.
                            Parcel newParcel = new Parcel { Id = parcel.Id, Delivered = DateTime.Now, DroneId = parcel.DroneId, PickedUp = parcel.PickedUp, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight };
                            DataSource.Parcels.Remove(parcel);
                            DataSource.Parcels.Add(newParcel);//updating the delivering time of the parcel
                            return;
                        }
                    }
                }
            }
        }

    }
}
