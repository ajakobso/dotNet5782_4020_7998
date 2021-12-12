using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DalApi;
using DAL.DO;
namespace DAL.DalObject//add exception of id that didnt found
{
    internal sealed class DalObject : IDAL
    {
        static readonly DalObject instance = new DalObject();
        public static DalObject Instance { get => instance; }
        static DalObject() { }
        DalObject()
        {
            DataSource.Initialize();
        }
        public void AddBaseStation(int id, string name, int chargeSlots, double longitude, double lattitude)
        {
            foreach (BaseStation bStation in DataSource.Config.BaseStations)
            {

                if (bStation.Id == id)
                {
                    throw new AddExistingBaseStationException();
                }
            }
            //if ((longitude > 35.3) || (longitude < 35.1) || (lattitude > 3.9) || (lattitude < 3.7))
            //{
            //    throw new LocationOutOfRangeException();
            //}

            DataSource.Config.BaseStations.Add(new BaseStation { Id = id, Name = name, ChargeSlots = chargeSlots, Longitude = longitude, Lattitude = lattitude });
        }
        public void AddDrone(int id, double Battery, WeightCategories maxW, string model)//double battery, DroneStatuses status
        {
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == id)
                {
                    throw new AddExistingDroneException();
                }

            }
            DataSource.Config.Drones.Add(new Drone { Id = id, Battery = Battery, MaxWeight = maxW, Model = model });//Battery = battery, Status = status
        }
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            foreach (Customer customer in DataSource.Config.Customers)
            {
                if (customer.Id == id)
                {
                    throw new AddExistingCustomerException();
                }
                //if ((longitude > 35.3) || (longitude < 35.1) || (lattitude > 3.9) || (lattitude < 3.7))
                //{
                //    throw new LocationOutOfRangeException();
                //}
            }
            DataSource.Config.Customers.Add(new Customer { Id = id, Name = name, Phone = phone, Longitude = longitude, Lattitude = lattitude });
        }
        public void AddParcel(int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime? requested, DateTime? scheduled, DateTime? pickedUp, DateTime? delivered)
        {
            foreach (Parcel parcel in DataSource.Config.Parcels)
            {

                if (parcel.DroneId == droneId)
                {
                    DataSource.Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = 0, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
                    throw new AddParcelToAnAsscriptedDroneException();
                }
            }


            DataSource.Config.Parcels.Add(new Parcel { Id = DataSource.Config.RunningParcelId++, DroneId = droneId, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
        }

        public void RemoveCustomer(int id)
        {
            foreach (var customer in DataSource.Config.Customers)
            {
                if (customer.Id == id)
                    DataSource.Config.Customers.Remove(customer);
            }
            throw new CustomerNotFoundException();
        }
        public void RemoveParcel(int id)
        {
            foreach (var parcel in DataSource.Config.Parcels)
            {
                if (parcel.Id == id)
                    DataSource.Config.Parcels.Remove(parcel);
            }
            throw new ParcelIdNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        public void RemoveDrone(int id)
        {
            foreach (var drone in DataSource.Config.Drones)
            {
                if (drone.Id == id)
                    DataSource.Config.Drones.Remove(drone);
                return;
            }
            throw new DroneIdNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        public void RemoveBaseStation(int id)
        {
            foreach (var bs in DataSource.Config.BaseStations)
            {
                if (bs.Id == id)
                {
                    DataSource.Config.BaseStations.Remove(bs);
                    return;
                }

            }
            throw new BaseStationNotFoundException();//probably best to add new exception for attemp to remove unexists element bet i have no power
        }
        public void AscriptionPtoD(int parcelId, int droneId)// ascription a parcel with drone
        {
            Parcel p = new Parcel();
            bool droneExsists = false;
            bool parcelExsists = false;
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (/*drone.Status == DroneStatuses.Available &&*/ droneId == drone.Id)
                {
                    droneExsists = true;
                }
            }
            if (droneExsists == false)
                throw new DroneIdNotFoundException();
            foreach (Parcel parcel in DataSource.Config.Parcels)//finding our parcel
            {
                if (parcel.Id == parcelId)
                {
                    p = parcel;
                    parcelExsists = true;
                }
            }
            if (parcelExsists)
            { p.DroneId = droneId; p.Scheduleded = DateTime.Now; }
            else
                throw new ParcelIdNotFoundException();

        }
        public void PickUpParcel(int parcelId)
        {
            Parcel p = new Parcel();
            bool parcelExists = false;
            bool droneExists = false;
            foreach (Parcel parcel in DataSource.Config.Parcels)//finding our parcel
            {
                if (parcel.Id == parcelId)
                {
                    p = parcel;
                    parcelExists = true;
                }
            }
            if (parcelExists)
            {
                foreach (Drone drone in DataSource.Config.Drones)
                {
                    if (drone.Id == p.DroneId)
                    {
                        Drone newDrone = new Drone { Id = drone.Id, /*Status = DroneStatuses.Shipping,*/ Battery = drone.Battery, MaxWeight = drone.MaxWeight, Model = drone.Model };
                        DataSource.Config.Drones.Remove(drone);
                        DataSource.Config.Drones.Add(newDrone);
                        droneExists = true;
                    }
                }
                if (droneExists == false)
                    throw new DroneIdNotFoundException();
            }
            else
                throw new ParcelIdNotFoundException();
        }
        public void ParcelDelivering(int parcelId)//אם קלט הפונקציה זה איבר מסוג חבילה אז אפשר למחוק את הלולאה של פוראיצ הראשונה, העיקרון שעשיתי פה אבל ישמש אותנו בפונקציות של ההצגה של איבר/רשימה.
        {
            bool parcelExists = false;
            bool droneExists = false;
            foreach (Parcel parcel in DataSource.Config.Parcels)///find the pacler by its id
            {
                if (parcel.Id == parcelId)
                {
                    parcelExists = true;
                    foreach (Drone drone in DataSource.Config.Drones)///find the drone that ascribed to the pacler
                    {
                        if (parcelExists)
                        {
                            if (drone.Id == parcel.DroneId)
                            {
                                Drone newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model };//Status = DroneStatuses.Available, Battery = drone.Battery
                                DataSource.Config.Drones.Remove(drone);
                                DataSource.Config.Drones.Add(newDrone);///change the status of the drone into available because he finish the shipment.
                                Parcel newParcel = new Parcel { Id = parcel.Id, Delivered = DateTime.Now, DroneId = parcel.DroneId, PickedUp = parcel.PickedUp, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight };
                                DataSource.Config.Parcels.Remove(parcel);
                                DataSource.Config.Parcels.Add(newParcel);//updating the delivering time of the parcel
                                droneExists = true;
                            }
                        }
                        else
                            throw new ParcelIdNotFoundException();
                    }
                    if (droneExists == false)
                        throw new DroneIdNotFoundException();
                }
            }
        }
        public void DroneCharging(int droneId, int baseStationId)//inserting a drone into a charging station in order to charge his battery
        {
            bool droneExists = false;
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model };// Status = DroneStatuses.Maintenance, Battery = drone.Battery
                    DataSource.Config.Drones.Remove(drone);
                    DataSource.Config.Drones.Add(newDrone); ///change the status of the drone into maintenance because he need to charge.
                    DroneCharge newDCharge = new DroneCharge { DroneId = droneId, StationId = baseStationId }; ///
                    droneExists = true;
                }
            }
            if (droneExists == false)
                throw new DroneIdNotFoundException();
        }
        public void DroneRelease(int droneId, int baseStationId)//Release the drone from the charging station                                                                 
        {
            bool droneExists = false;
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    Drone newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model };//Status = DroneStatuses.Available, Battery = drone.Battery
                    DataSource.Config.Drones.Remove(drone);
                    DataSource.Config.Drones.Add(newDrone); ///change the status of the drone into available because the user's request
                    foreach (DroneCharge charger in DataSource.Config.DroneCharges)///remove the matching charging station from the list
                    {
                        if (charger.DroneId == droneId && charger.StationId == baseStationId)
                        {
                            DataSource.Config.DroneCharges.Remove(charger);
                        }
                    }
                    droneExists = true;
                }

            }
            if (droneExists == false)
                throw new DroneIdNotFoundException();
        }
        public BaseStation CopyBaseStation(int baseStationId)//return copy of a base station
        {
            bool BSExsists = false;
            BaseStation nBStation = new BaseStation();
            foreach (BaseStation baseStation in DataSource.Config.BaseStations)
            {
                if (baseStation.Id == baseStationId)
                {
                    return baseStation;
                }
            }
            if (BSExsists == false)
                throw new BaseStationNotFoundException();
            return nBStation;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public Drone CopyDrone(int droneId)//return copy of a drone
        {
            Drone nDrone = new Drone();
            bool droneExists = false;
            foreach (Drone drone in DataSource.Config.Drones)
            {
                if (drone.Id == droneId)
                {
                    return drone;
                }
            }
            if (droneExists == false)
                throw new DroneIdNotFoundException();
            return nDrone;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public Customer CopyCustomer(int customerId)//return copy of a customer
        {
            bool customerExists = false;
            Customer nCustomer = new Customer();
            foreach (Customer customer in DataSource.Config.Customers)
            {
                if (customer.Id == customerId)
                {
                    return customer;
                }
            }
            if (customerExists == false)
                throw new CustomerNotFoundException();
            return nCustomer;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public Parcel CopyParcel(int parcelId)//return copy of a parcel
        {
            bool parcelExists = false;
            Parcel nParcel = new Parcel();
            foreach (Parcel parcel in DataSource.Config.Parcels)
            {
                if (parcel.Id == parcelId)
                {
                    return parcel;
                }
            }
            if (parcelExists == false)
                throw new ParcelIdNotFoundException();
            return nParcel;//the function demend us to return a value, and because the return is inside a condition it cause an error
        }
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
        public double[] CopyLongitudeRange()
        {
            double[] responce = { DataSource.Config.LongitudeRange[0], DataSource.Config.LongitudeRange[1] };
            return responce;
        }
        public double[] CopyLattitudeRange()
        {
            double[] responce = { DataSource.Config.LattitudeRange[0], DataSource.Config.LattitudeRange[1] };
            return responce;
        }
        public IEnumerable<BaseStation> CopyBaseStations()//return copy of the base stations's list
        {
            IEnumerable<BaseStation> copyBS = DataSource.Config.BaseStations;
            return copyBS;
        }
        public IEnumerable<Drone> CopyDronesList()//return copy of the drones's list
        {
            IEnumerable<Drone> copyD = DataSource.Config.Drones;
            return copyD;
        }
        public IEnumerable<Customer> CopyCustomersList()//return copy of the customer's list
        {
            IEnumerable<Customer> copyC = DataSource.Config.Customers;
            return copyC;
        }
        public IEnumerable<Parcel> CopyParcelsList()//return copy of the parcels's list
        {
            IEnumerable<Parcel> copyP = DataSource.Config.Parcels;
            return copyP;
        }
        public IEnumerable<Parcel> UnAscriptedParcels()//return new list with all the un-ascripted parcels.
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
        public IEnumerable<BaseStation> AvailableBaseStation()//return new list with the base stations who have available charge slots.
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
        public static Coordinate Fromdouble(double angleInDegrees)
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

    }
}

