﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    class DataSource
    {
       internal class Config//fields of the first empty element in every array.
        {
            internal static int FstDroneAvailable = 0;///
            internal static int FstBaseStationAvailable = 0;
            internal static int FstCustomerAvailable = 0;
            internal static int FstParcelavailable = 0;
            //שדה ליצירת מספר מזהה עבור חבילות
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<BaseStation> BaseStations = new List<BaseStation>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
       internal class Config
        {
            internal static int RunningParcelId = 1000;//running number for the parcels id
        }
        //מתודה סטטית  לאיתחול מופעי הישויות עם נתונים וכו
        static void Initialize ()
        {
            Random r = new Random();
            BaseStations.Add(new BaseStation { Id = 101, Name = "station1", ChargeSlots = 3, Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });//initializing stations around Jerusalem
            BaseStations.Add(new BaseStation { Id = 102, Name = "station2", ChargeSlots = 5, Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Drones.Add(new Drone { Id = 1, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Light, Model = "10A", Status = DroneStatuses.Maintenance });
            Drones.Add(new Drone { Id = 2, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "5A", Status = DroneStatuses.Available });
            Drones.Add(new Drone { Id = 3, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Middle, Model = "10B", Status = DroneStatuses.Available });
            Drones.Add(new Drone { Id = 4, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "15C", Status = DroneStatuses.Shipping });
            Drones.Add(new Drone { Id = 5, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "10D", Status = DroneStatuses.Shipping });
            Drones.Add(new Drone { Id = 6, Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "15D", Status = DroneStatuses.Shipping });
            Customers.Add(new Customer { Id = 326456189, Name = "Avital", Phone = "0548651821", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 204168946, Name = "Avi", Phone = "0548679821", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 425891358, Name = "Lulu", Phone = "0548768521", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 702594863, Name = "Talya", Phone = "0548619897", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 203459782, Name = "Phoebe", Phone = "0548679846", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 306894751, Name = "Moti", Phone = "0548489651", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 451620785, Name = "Ben", Phone = "0548498147", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 410258943, Name = "Nati", Phone = "0589756121", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 327498510, Name = "Batty", Phone = "0542235829", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = 610845302, Name = "Rachel", Phone = "0548645679", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 4, SenderId = 425891358, TargetId = 326456189, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 5, SenderId = 425891358, TargetId = 610845302, Priority = Priorities.Standart, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 4, SenderId = 610845302, TargetId = 326456189, Priority = Priorities.Fast, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 6, SenderId = 425891358, TargetId = 203459782, Priority = Priorities.Urgent, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 6, SenderId = 203459782, TargetId = 425891358, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 5, SenderId = 425891358, TargetId = 326456189, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 4, SenderId = 204168946, TargetId = 327498510, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 5, SenderId = 702594863, TargetId = 326456189, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 5, SenderId = 425891358, TargetId = 204168946, Priority = Priorities.Standart, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });
            Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 4, SenderId = 327498510, TargetId = 702594863, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = DateTime.Now, Delivered = new DateTime(2021, 10, 22) });

        }
    }
}
