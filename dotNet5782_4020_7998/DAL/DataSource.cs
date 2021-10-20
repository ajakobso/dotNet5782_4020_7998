using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    class DataSource
    {
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<BaseStation> BaseStations = new List<BaseStation>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
       internal class Config//fields of the first empty element in every array.
        {
            //internal static int FDroneAvailable = 0;
            //internal static int FBaseStationAvailable = 0;
            //internal static int FCustomerAvailable = 0;
            //internal static int FParcelavailable = 0;

            //שדה ליצירת מספר מזהה עבור חבילות
        }
        //מתודה סטטית  לאיתחול מופעי הישויות עם נתונים וכו
        static void Initialize ()
        {
            Random r = new Random();
            BaseStations.Add(new BaseStation { Id = 1001, Name = "station1", ChargeSlots = 3, Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });//initializing stations around Jerusalem
            BaseStations.Add(new BaseStation { Id = 1002, Name = "station2", ChargeSlots = 5, Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Drones.Add(new Drone { Id = r.Next(1, 11), Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Light, Model = "10A", Status = DroneStatuses.Maintenance });
            Drones.Add(new Drone { Id = r.Next(1, 11), Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "5A", Status = DroneStatuses.Available });
            Drones.Add(new Drone { Id = r.Next(1, 11), Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Middle, Model = "10B", Status = DroneStatuses.Available });
            Drones.Add(new Drone { Id = r.Next(1, 11), Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Light, Model = "15C", Status = DroneStatuses.Shipping });
            Drones.Add(new Drone { Id = r.Next(1, 11), Battery = r.NextDouble() + r.Next(1, 100), MaxWeight = WeightCategories.Heavy, Model = "10D", Status = DroneStatuses.Shipping });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Avital", Phone = "0548651821", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Avi", Phone = "0548679821", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Lulu", Phone = "0548768521", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Talya", Phone = "0548619897", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Phoebe", Phone = "0548679846", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Moti", Phone = "0548489651", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Ben", Phone = "0548498147", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Nati", Phone = "0589756121", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Batty", Phone = "0542235829", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });
            Customers.Add(new Customer { Id = r.Next(100000000, 1000000000), Name = "Rachel", Phone = "0548645679", Longitude = 31 + r.NextDouble(), Lattitude = 35 + r.NextDouble() });


            //for (int i = 0; i < 5; i++)
            //{
            //    WeightCategories maxW = WeightCategories.Light;
            //    int weight = r.Next(0, 3);
            //    if (weight == 0) { maxW = WeightCategories.Light; } 
            //    if (weight == 1) { maxW = WeightCategories.Middle; }
            //    if (weight == 2) { maxW = WeightCategories.Heavy; }
            //    Drones.Add(new Drone { Id = r.Next(1,11), Battery = r.NextDouble() + r.Next(1,100), MaxWeight = maxW, Model =  })


            //}
        }
    }
}
