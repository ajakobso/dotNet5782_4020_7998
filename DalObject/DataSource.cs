using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
namespace Dal
{
    public class DataSource
    {
        internal class Config//fields of the first empty element in every array.
        {
            internal static List<Drone> Drones = new List<Drone>();
            internal static List<BaseStation> BaseStations = new List<BaseStation>();
            internal static List<Customer> Customers = new List<Customer>();
            internal static List<Parcel> Parcels = new List<Parcel>();
            internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
            internal static double Available = 1;//100 KM for a full battery
            internal static double LightWheight = 3;//33+- KM for a full battery
            internal static double MiddleWheight = 4;//25 KM for a full battery
            internal static double HeavyWeight = 5;//20 KM for a full battery
            internal static double BatteryPerHour = 60;//we have dicided that the delivering company will operate only in jerusalem, so 1 precent per minut make sense.
            internal static int RunningParcelId = 1001;//running number for the parcels id
            internal static double[] LongitudeRange = { 35.1252, 35.2642 };//longitude coordinates of jerusalem
            internal static double[] LattitudeRange = { 31.7082, 31.8830 };//lattitude coordinates of jerusalem
        }
        //מתודה סטטית  לאיתחול מופעי הישויות עם נתונים וכו
        public static void Initialize()
        {
            Random r = new();
            Config.BaseStations.Add(new BaseStation { Id = 101, Name = "station1", ChargeSlots = 3, Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });//initializing stations around Jerusalem
            Config.BaseStations.Add(new BaseStation { Id = 102, Name = "station2", ChargeSlots = 5, Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Drones.Add(new Drone { Id = 1, MaxWeight = WeightCategories.Light, Model = "10A" });// Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Maintenance
            Config.Drones.Add(new Drone { Id = 2, MaxWeight = WeightCategories.Heavy, Model = "5A" });//Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Available
            Config.Drones.Add(new Drone { Id = 3, MaxWeight = WeightCategories.Middle, Model = "10B" });//Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Available
            Config.Drones.Add(new Drone { Id = 4, MaxWeight = WeightCategories.Heavy, Model = "15C" });//Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Shipping
            Config.Drones.Add(new Drone { Id = 5, MaxWeight = WeightCategories.Heavy, Model = "10D" });//Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Shipping
            Config.Drones.Add(new Drone { Id = 6, MaxWeight = WeightCategories.Heavy, Model = "15D" });//Battery = r.NextDouble() + r.Next(1, 100), Status = DroneStatuses.Shipping
            Config.Customers.Add(new Customer { Id = 326456189, Name = "Avital", Phone = "0548651821", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 204168946, Name = "Yosi", Phone = "0548679821", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 425891358, Name = "Lulu", Phone = "0548768521", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 702594863, Name = "Talya", Phone = "0548619897", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 203459782, Name = "Monica", Phone = "0548679846", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 306894751, Name = "Moti", Phone = "0548489651", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 451620785, Name = "Ben", Phone = "0548498147", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 410258943, Name = "Nati", Phone = "0589756121", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 327498510, Name = "Ayelet", Phone = "0542235829", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Customers.Add(new Customer { Id = 610845302, Name = "Rachel", Phone = "0548645679", Longitude = (r.NextDouble() * (DataSource.Config.LongitudeRange[1] - DataSource.Config.LongitudeRange[0])) + DataSource.Config.LongitudeRange[0], Lattitude = (r.NextDouble() * (DataSource.Config.LattitudeRange[1] - DataSource.Config.LattitudeRange[0])) + DataSource.Config.LattitudeRange[0] });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 6, SenderId = 425891358, TargetId = 326456189, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 425891358, TargetId = 610845302, Priority = Priorities.Standart, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 610845302, TargetId = 326456189, Priority = Priorities.Fast, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 5, SenderId = 425891358, TargetId = 203459782, Priority = Priorities.Urgent, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 203459782, TargetId = 425891358, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 425891358, TargetId = 326456189, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 204168946, TargetId = 327498510, Priority = Priorities.Fast, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 4, SenderId = 702594863, TargetId = 326456189, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = DateTime.Now, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 425891358, TargetId = 204168946, Priority = Priorities.Standart, Weight = WeightCategories.Heavy, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = 327498510, TargetId = 702594863, Priority = Priorities.Standart, Weight = WeightCategories.Middle, Requested = DateTime.Now, Scheduleded = null, PickedUp = null, Delivered = null });
            //Config.DroneCharges.Add(new DroneCharge { DroneId = 1, StationId = 102 });
        }
    }
}
