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
            DataSource.Config.BaseStations.Add(new BaseStation { Id = id, Name = name, ChargeSlots = chargeSlots, Longitude = longitude, Lattitude = lattitude });
        }
        public static void AddDrone(int id, double battery, WeightCategories maxW, string model, DroneStatuses status)
        {
            DataSource.Config.Drones.Add(new Drone { Id = id, Battery = battery, MaxWeight = maxW, Model = model, Status = status });
        }
        public static void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            DataSource.Config.Customers.Add(new Customer { Id = id, Name = name, Phone = phone, Longitude = longitude, Lattitude = lattitude });
        }
        public static void AddParcel(int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime requested, DateTime scheduled, DateTime pickedUp, DateTime delivered)
        {
            DataSource.Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = droneId, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
        }
        public static void AscriptionPtoD(int parcelId, int droneId)// ascription a parcel with drone
        {
            Parcel p = new Parcel();
            foreach (Parcel parcel in DataSource.Config.Parcels)//finding our parcel
            {
                if (parcel.Id == parcelId)
                {
                    p = parcel;
                }
            }
            p.DroneId = droneId;
            //foreach (Drone drone in DataSource.Config.Drones)
            //{
            //    if (drone.Status == DroneStatuses.Available)
            //    {
            //        parcel.DroneId = drone.Id;
            //        return;
            //    }
            //}
        }
        public static void PickUpParcel(int parcelId)
        {
            Parcel p = new Parcel();
            foreach (Parcel parcel in DataSource.Config.Parcels)//finding our parcel
            {
                if (parcel.Id == parcelId)
                {
                    p = parcel;
                }
            }
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == p.DroneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Shipping, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                    DataSource.Config.Drones.Remove(drone);
                    DataSource.Config.Drones.Add(newDrone);
                    return;
                }
            }
        }
        public static void PaclerDelivering(int parcelId)//אם קלט הפונקציה זה איבר מסוג חבילה אז אפשר למחוק את הלולאה של פוראיצ הראשונה, העיקרון שעשיתי פה אבל ישמש אותנו בפונקציות של ההצגה של איבר/רשימה.
        {
            foreach (Parcel parcel in DataSource.Config.Parcels)///find the pacler by its id
            {
                if (parcel.Id == parcelId)
                {
                    foreach (Drone drone in DataSource.Config.Drones)///find the drone that ascribed to the pacler
                    {
                        if (drone.Id == parcel.DroneId)
                        {
                            Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Available, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                            DataSource.Config.Drones.Remove(drone);
                            DataSource.Config.Drones.Add(newDrone);///change the status of the drone into available because he finish the shipment.
                            Parcel newParcel = new Parcel { Id = parcel.Id, Delivered = DateTime.Now, DroneId = parcel.DroneId, PickedUp = parcel.PickedUp, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight };
                            DataSource.Config.Parcels.Remove(parcel);
                            DataSource.Config.Parcels.Add(newParcel);//updating the delivering time of the parcel
                            return;
                        }
                    }
                }
            }
        }
        public static void DroneCharging(int droneId, int baseStationId)//inserting a drone into a charging station in order to charge his battery
        {
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Maintenance, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                    DataSource.Config.Drones.Remove(drone);
                    DataSource.Config.Drones.Add(newDrone); ///change the status of the drone into maintenance because he need to charge.
                    DroneCharge newDCharge = new DroneCharge { DroneId = droneId, StationId = baseStationId }; ///
                }

            }
        }
        public static void DroneRelease(int droneId, int baseStationId)//Release the drone from the charging station                                                                 
        {
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, Status = DroneStatuses.Available, Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                    DataSource.Config.Drones.Remove(drone);
                    DataSource.Config.Drones.Add(newDrone); ///change the status of the drone into available because the user's request
                    foreach (DroneCharge charger in DataSource.Config.DroneCharges)///remove the matching charging station from the list
                    {
                        if (charger.DroneId == droneId && charger.StationId == baseStationId)
                        {
                            DataSource.Config.DroneCharges.Remove(charger);
                        }
                    }
                }

            }
        }
        public static BaseStation CopyBaseStation(int baseStationId)//return copy of a base station
        {
            BaseStation nBStation = new BaseStation();
            foreach (BaseStation baseStation in DataSource.Config.BaseStations)
            {
                if (baseStation.Id == baseStationId)
                {
                    return baseStation;
                }
            }
            return nBStation;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public static Drone CopyDrone(int droneId)//return copy of a drone
        {
            Drone nDrone = new Drone();
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    return drone;
                }
            }
            return nDrone;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public static Customer CopyCustomer(int customerId)//return copy of a customer
        {
            Customer nCustomer = new Customer();
            foreach (Customer customer in DataSource.Config.Customers)
            {
                if (customer.Id == customerId)
                {
                    return customer;
                }
            }
            return nCustomer;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public static Parcel CopyParcel(int parcelId)//return copy of a parcel
        {
            Parcel nParcel = new Parcel();
            foreach (Parcel parcel in DataSource.Config.Parcels)
            {
                if (parcel.Id == parcelId)
                {
                    return parcel;
                }
            }
            return nParcel;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public static List<BaseStation> CopyBaseStations()//return copy of the base stations's list
        {
            return DataSource.Config.BaseStations;
        }
        public static List<Drone> CopyDronesList()//return copy of the drones's list
        {
            return DataSource.Config.Drones;
        }
        public static List<Customer> CopyCustomersList()//return copy of the customer's list
        {
            return DataSource.Config.Customers;
        }
        public static List<Parcel> CopyParcelsList()//return copy of the parcels's list
        {
            return DataSource.Config.Parcels;
        }
        public static List<Parcel> UnAscriptedParcels()//return new list with all the un-ascripted parcels.
        {
            List<Parcel> nList = new List<Parcel>();
            foreach (Parcel parcel in DataSource.Config.Parcels)
            {
                if (parcel.DroneId == 0)
                {
                    nList.Add(parcel);
                }
            }
            return nList;
        }
        public static List<BaseStation> AvailableBaseStation()//return new list with the base stations who have available charge slots.
        {
            List<BaseStation> nList = new List<BaseStation>();
            foreach (BaseStation baseStation in DataSource.Config.BaseStations)
            {
                if (baseStation.ChargeSlots > 0)
                {
                    nList.Add(baseStation);
                }
            }
            return nList;
        }


    }
}
