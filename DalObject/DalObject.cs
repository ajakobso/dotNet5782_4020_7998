﻿using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;
namespace Dal
{
    internal sealed class DalObject : IDAL
    {

        #region singelton & initialization
        static readonly DalObject instance = new DalObject();
        public static DalObject Instance { get => instance; }
        static DalObject() { }
        DalObject()
        {
            DataSource.Initialize();
        }
        #endregion

        #region Base Station
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(int id, string name, int chargeSlots, int availableChargeSlots, double longitude, double lattitude)
        {
            foreach (var _ in from BaseStation bStation in DataSource.Config.BaseStations
                              where bStation.Id == id
                              select new { })
            {
                throw new AddExistingBaseStationException();
            }
            DataSource.Config.BaseStations.Add(new BaseStation { Id = id, Name = name, ChargeSlots = chargeSlots, AvailableChargeSlots = availableChargeSlots, Longitude = longitude, Lattitude = lattitude });
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveBaseStation(int id)
        {
            foreach (var bs in from bs in DataSource.Config.BaseStations
                               where bs.Id == id
                               select bs)
            {
                DataSource.Config.BaseStations.Remove(bs);
                return;
            }
            //foreach (var bs in DataSource.Config.BaseStations)-not linq
            //{
            //    if (bs.Id == id)
            //    {
            //        DataSource.Config.BaseStations.Remove(bs);
            //        return;
            //    }

            //}
            throw new BaseStationNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation CopyBaseStation(int baseStationId)//return copy of a base station
        {
            foreach (var baseStation in
            //bool BSExsists = false;
            //BaseStation nBStation = new BaseStation();
            from BaseStation baseStation in DataSource.Config.BaseStations
            where baseStation.Id == baseStationId
            select baseStation)
            {
                return baseStation;
            }
            //foreach (BaseStation baseStation in DataSource.Config.BaseStations)
            //{
            //    if (baseStation.Id == baseStationId)
            //    {
            //        return baseStation;
            //    }
            //}
            // if (BSExsists == false)
            throw new BaseStationNotFoundException();
            // return nBStation;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> CopyBaseStations()//return copy of the base stations's list
        {
            IEnumerable<BaseStation> copyBS = DataSource.Config.BaseStations;
            return copyBS;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> AvailableBaseStation()//return new list with the base stations who have available charge slots.
        {
            return from BaseStation baseStation in DataSource.Config.BaseStations
                   where baseStation.ChargeSlots > 0
                   select baseStation;
            //foreach (BaseStation baseStation in DataSource.Config.BaseStations)-not linq
            //{
            //    if (baseStation.ChargeSlots > 0)
            //    {
            //        nList.Add(baseStation);
            //    }
            //}
        }
        #endregion

        #region Customer
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            foreach (var _ in from Customer customer in DataSource.Config.Customers
                              where customer.Id == id
                              select new { })
            {
                throw new AddExistingCustomerException();
            }
            //foreach (Customer customer in DataSource.Config.Customers)-not linq
            //{
            //    if (customer.Id == id)
            //    {
            //        throw new AddExistingCustomerException();
            //    }
            //}
            DataSource.Config.Customers.Add(new Customer { Id = id, Name = name, Phone = phone, Longitude = longitude, Lattitude = lattitude });
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveCustomer(int id)
        {
            foreach (var customer in from customer in DataSource.Config.Customers
                                     where customer.Id == id
                                     select customer)
            {
                DataSource.Config.Customers.Remove(customer);
                return;
            }
            //foreach (var customer in DataSource.Config.Customers)
            //{
            //    if (customer.Id == id)
            //    { DataSource.Config.Customers.Remove(customer); return; }
            //}
            throw new CustomerNotFoundException();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer CopyCustomer(int customerId)//return copy of a customer
        {
            bool customerExists = false;
            Customer nCustomer = new Customer();
            foreach (var customer in from Customer customer in DataSource.Config.Customers
                                     where customer.Id == customerId
                                     select customer)
            {
                return customer;
            }
            //foreach (Customer customer in DataSource.Config.Customers)-not linq
            //{
            //    if (customer.Id == customerId)
            //    {
            //        return customer;
            //    }
            //}
            if (customerExists == false)
                throw new CustomerNotFoundException();
            return nCustomer;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> CopyCustomersList()//return copy of the customer's list
        {
            IEnumerable<Customer> copyC = DataSource.Config.Customers;
            return copyC;
        }
        #endregion

        #region Drone 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(int id, WeightCategories maxW, string model)//double battery, DroneStatuses status
        {
            foreach (var _ in from Drone drone in DataSource.Config.Drones
                              where drone.Id == id
                              select new { })
            {
                throw new AddExistingDroneException();
            }
            //foreach (Drone drone in DataSource.Config.Drones)
            //{
            //    if (drone.Id == id)
            //    {
            //        throw new AddExistingDroneException();
            //    }

            //}
            DataSource.Config.Drones.Add(new Drone { Id = id, MaxWeight = maxW, Model = model });//Battery = battery, Status = status
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDrone(int id)
        {
            foreach (var drone in from drone in DataSource.Config.Drones
                                  where drone.Id == id
                                  select drone)
            {
                DataSource.Config.Drones.Remove(drone);
                return;
            }
            //foreach (var drone in DataSource.Config.Drones)-not linq
            //{
            //    if (drone.Id == id)
            //    {
            //        DataSource.Config.Drones.Remove(drone);
            //        return;
            //    }
            //}
            throw new DroneIdNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(int droneId, int stationId)
        {
            DataSource.Config.DroneCharges.Add(new DroneCharge { DroneId = droneId, StationId = stationId, InsertionTime = DateTime.Now });
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(int droneId)
        {
            DataSource.Config.DroneCharges.Remove(DataSource.Config.DroneCharges.Find(x => x.DroneId == droneId));
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] DronePowerConsumingPerKM()
        {
            double[] DPC = new double[5];
            DPC[0] = DataSource.Config.Available;
            DPC[1] = DataSource.Config.LightWheight;
            DPC[2] = DataSource.Config.MiddleWheight;
            DPC[3] = DataSource.Config.HeavyWeight;
            DPC[4] = DataSource.Config.BatteryPerHour;
            return DPC;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneCharging(int droneId, int baseStationId)//inserting a drone into a charging station in order to charge his battery
        {
            bool droneExists = false;
            foreach (var (drone, newDrone) in from Drone drone in DataSource.Config.Drones
                                              where drone.Id == droneId
                                              let newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model }// Status = DroneStatuses.Maintenance, Battery = drone.Battery
                                              select (drone, newDrone))
            {
                //DataSource.Config.Drones.Remove(drone);
                //DataSource.Config.Drones.Add(newDrone);
                DroneCharge newDCharge = new DroneCharge { DroneId = droneId, StationId = baseStationId, InsertionTime = DateTime.Now };//what is this for??? and if we need to add it somewhere - then where??
                DataSource.Config.DroneCharges.Add(newDCharge);
                droneExists = true;
                return;
            }
            if (droneExists == false)
                throw new DroneIdNotFoundException();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DroneRelease(int droneId)//Release the drone from the charging station                                                                 
        {
            foreach (var charger in from DroneCharge charger in DataSource.Config.DroneCharges
                                    where charger.DroneId == droneId
                                    select charger)
            {
                DataSource.Config.DroneCharges.Remove(charger);
                return;
            }
            throw new DroneIdNotFoundException();
            //foreach (Drone drone in DataSource.Config.Drones)-not linq
            //{
            //    if (drone.Id == droneId)
            //    {
            //        Drone newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model };//Status = DroneStatuses.Available, Battery = drone.Battery
            //        DataSource.Config.Drones.Remove(drone);
            //        DataSource.Config.Drones.Add(newDrone);
            //        foreach (var charger in from DroneCharge charger in DataSource.Config.DroneCharges
            //                                where charger.DroneId == droneId && charger.StationId == baseStationId
            //                                select charger)
            //        {
            //            DataSource.Config.DroneCharges.Remove(charger);
            //        }
            //        //foreach (DroneCharge charger in DataSource.Config.DroneCharges)///remove the matching charging station from the list-not linq
            //        //{
            //        //    if (charger.DroneId == droneId && charger.StationId == baseStationId)
            //        //    {
            //        //        DataSource.Config.DroneCharges.Remove(charger);
            //        //    }
            //        //}
            //        droneExists = true;
            //    }

            //}

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone CopyDrone(int droneId)//return copy of a drone
        {
            Drone nDrone = new Drone();
            bool droneExists = false;
            foreach (var drone in from Drone drone in DataSource.Config.Drones
                                  where drone.Id == droneId
                                  select drone)
            {
                return drone;
            }
            //foreach (Drone drone in DataSource.Config.Drones)-not linq
            //{
            //    if (drone.Id == droneId)
            //    {
            //        return drone;
            //    }
            //}
            if (droneExists == false)
                throw new DroneIdNotFoundException();
            return nDrone;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> CopyDronesInCharge()
        {
            return from DroneCharge drone in DataSource.Config.DroneCharges
                   select drone;
        }
        #endregion

        #region Parcel
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(int id, int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime? requested, DateTime? scheduled, DateTime? pickedUp, DateTime? delivered)
        {
            int parcelID;
            if (id != -1)
            { parcelID = id; }
            else { parcelID = DataSource.Config.RunningParcelId++; }
            //foreach (var _ in from Parcel parcel in DataSource.Config.Parcels
            //                  where parcel.DroneId == droneId
            //                  select new { })
            //{
            //    DataSource.Config.Parcels.Add(new Parcel { Id = parcelID, DroneId = 0, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
            //    throw new AddParcelToAnAsscriptedDroneException();
            //}
            DataSource.Config.Parcels.Add(new Parcel { Id = parcelID, DroneId = droneId, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            foreach (var parcel in from parcel in DataSource.Config.Parcels
                                   where parcel.Id == id
                                   select parcel)
            {
                DataSource.Config.Parcels.Remove(parcel);
                return;
            }
            //foreach (var parcel in DataSource.Config.Parcels)-not linq
            //{
            //    if (parcel.Id == id)
            //    { DataSource.Config.Parcels.Remove(parcel); return; }

            //}

            throw new ParcelIdNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AscriptionPtoD(int parcelId, int droneId)// ascription a parcel with drone
        {
            Parcel p = new Parcel();
            bool droneExsists = false;
            bool parcelExsists = false;
            foreach (var _ in from Drone drone in DataSource.Config.Drones
                              where/*drone.Status == DroneStatuses.Available &&*/droneId == drone.Id
                              select new { })
            {
                droneExsists = true;
            }
            //foreach (Drone drone in DataSource.Config.Drones)-not linq
            //{
            //    if (/*drone.Status == DroneStatuses.Available &&*/ droneId == drone.Id)
            //    {
            //        droneExsists = true;
            //    }
            //}
            if (droneExsists == false)
                throw new DroneIdNotFoundException();
            foreach (var parcel in from Parcel parcel in DataSource.Config.Parcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                p = parcel;
                parcelExsists = true;
            }
            //foreach (Parcel parcel in DataSource.Config.Parcels)//finding our parcel-not linq
            //{
            //    if (parcel.Id == parcelId)
            //    {
            //        p = parcel;
            //        parcelExsists = true;
            //    }
            //}
            if (parcelExsists)
            {
                DataSource.Config.Parcels.Remove(p);
                p.DroneId = droneId;
                p.Scheduleded = DateTime.Now;
                DataSource.Config.Parcels.Add(p);
            }
            else
                throw new ParcelIdNotFoundException();

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickUpParcel(int parcelId)
        {
            foreach (var parcel in from Parcel parcel in DataSource.Config.Parcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                if (parcel.DroneId == 0)
                    throw new DroneIdNotFoundException();
                else
                {
                    Parcel nParcel = new Parcel { Id = parcel.Id, DroneId = parcel.DroneId, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, PickedUp = DateTime.Now, Delivered = parcel.Delivered };
                    DataSource.Config.Parcels.Remove(parcel);
                    DataSource.Config.Parcels.Add(nParcel);
                    return;
                }
            }
            throw new ParcelIdNotFoundException();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelDelivering(int parcelId)//אם קלט הפונקציה זה איבר מסוג חבילה אז אפשר למחוק את הלולאה של פוראיצ הראשונה, העיקרון שעשיתי פה אבל ישמש אותנו בפונקציות של ההצגה של איבר/רשימה.
        {
            foreach (var parcel in from Parcel parcel in DataSource.Config.Parcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                if (parcel.DroneId == 0)
                    throw new DroneIdNotFoundException();
                else
                {
                    Parcel nParcel = new Parcel { Id = parcel.Id, DroneId = parcel.DroneId, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, PickedUp = parcel.PickedUp, Delivered = DateTime.Now };
                    DataSource.Config.Parcels.Remove(parcel);
                    DataSource.Config.Parcels.Add(nParcel);
                    return;
                }
            }
            throw new ParcelIdNotFoundException();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel CopyParcel(int parcelId)//return copy of a parcel
        {
            bool parcelExists = false;
            Parcel nParcel = new Parcel();
            foreach (var parcel in from Parcel parcel in DataSource.Config.Parcels
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                return parcel;
            }
            //foreach (Parcel parcel in DataSource.Config.Parcels)-not linq!
            //{
            //    if (parcel.Id == parcelId)
            //    {
            //        return parcel;
            //    }
            //}
            if (parcelExists == false)
                throw new ParcelIdNotFoundException();
            return nParcel;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> CopyDronesList()//return copy of the drones's list
        {
            IEnumerable<Drone> copyD = DataSource.Config.Drones;
            return copyD;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> CopyParcelsList()//return copy of the parcels's list
        {
            IEnumerable<Parcel> copyP = DataSource.Config.Parcels;
            return copyP;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> UnAscriptedParcels()//return new list with all the un-ascripted parcels.
        {
            return from Parcel parcel in DataSource.Config.Parcels
                    where parcel.DroneId == 0
                    select parcel;
            //foreach (Parcel parcel in DataSource.Config.Parcels)-not linq
            //{
            //    if (parcel.DroneId == 0)
            //    {
            //        nList.Add(parcel);
            //    }
            //}
        }
        #endregion

        #region Coordinate 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] CopyLongitudeRange()
        {
            double[] responce = { DataSource.Config.LongitudeRange[0], DataSource.Config.LongitudeRange[1] };
            return responce;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] CopyLattitudeRange()
        {
            double[] responce = { DataSource.Config.LattitudeRange[0], DataSource.Config.LattitudeRange[1] };
            return responce;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Coordinate Fromdouble(double angleInDegrees)
        {
            //ensure the value will fall within the primary range [-180.0..+180.0]
            while (angleInDegrees < -180.0)
                angleInDegrees += 360.0;

            while (angleInDegrees > 180.0)
                angleInDegrees -= 360.0;

            var result = new Coordinate();

            //switch the value to positive
            result.IsNegative = angleInDegrees < 0;
            angleInDegrees = Math.Abs(angleInDegrees);

            //gets the degree
            result.Degrees = (int)Math.Floor(angleInDegrees);
            var delta = angleInDegrees - result.Degrees;

            //gets minutes and seconds
            var seconds = (int)Math.Floor(3600.0 * delta);
            result.Seconds = seconds % 60;
            result.Minutes = (int)Math.Floor(seconds / 60.0);
            delta = delta * 3600.0 - seconds;

            //gets fractions
            result.Milliseconds = (int)(1000.0 * delta);

            return result;
        }
        #endregion

    }
}

