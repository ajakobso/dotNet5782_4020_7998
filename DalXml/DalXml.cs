﻿using System;
using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using DalApi;
namespace Dal
{
    sealed class DalXml : IDAL
    {

        #region singelton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }// static ctor to ensure instance init is done just before first usage
        DalXml() {} // default => private
        public static DalXml Instance { get => instance; }// The public Instance property to use
        #endregion

        #region xml files
        //string BaseStationsPath = @"..\BaseStationsXml.xml"; //if there is a problem with the path we can try this or maybe the full path idk
        string BaseStationsPath = @"BaseStationsXml.xml";//XMLSerializer
        string CustomersPath = @"CustomersXml.xml";//XElement
        string CoordinatePath = @"CoordinatesXml.xml";//XMLSerializer
        string DronesPath = @"DronesXml.xml";//XMLSerializer
        string DronesInChargePath = @"DronesInChargeXml.xml";//XMLSerializer
        string ParcelsPath = @"ParcelsXml.xml";//XMLSerializer
        string ConfigPath = @"config.xml";
        #endregion

        #region Base Station
        public void AddBaseStation(int id, string name, int chargeSlots, int availableChargeSlots, double longitude, double lattitude)
        {
            List<BaseStation> ListBaseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            foreach (var _ in from BaseStation bStation in ListBaseStations
                              where bStation.Id == id
                              select new { })
            {
                throw new AddExistingBaseStationException();
            }
            ListBaseStations.Add(new BaseStation { Id = id, Name = name, ChargeSlots = chargeSlots, AvailableChargeSlots = availableChargeSlots, Longitude = longitude, Lattitude = lattitude });
            XmlTools.SaveListToXmlSerializer(ListBaseStations, BaseStationsPath);
        }
        public void RemoveBaseStation(int id)
        {
            List<BaseStation> ListBaseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            foreach (var bs in from bs in ListBaseStations
                               where bs.Id == id
                               select bs)
            {
                ListBaseStations.Remove(bs);
                return;
            }
            throw new BaseStationNotFoundException();
        }
        public BaseStation CopyBaseStation(int baseStationId)
        {
            List<BaseStation> ListBaseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            foreach (var baseStation in
            from BaseStation baseStation in ListBaseStations
            where baseStation.Id == baseStationId
            select baseStation)
            {
                return baseStation;
            }
            throw new BaseStationNotFoundException();
        }
        public IEnumerable<BaseStation> CopyBaseStations()
        {
            List<BaseStation> ListBaseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            return from bs in ListBaseStations
                   select bs;
        }
        public IEnumerable<BaseStation> AvailableBaseStation()
        {
            List<BaseStation> ListBaseStations = XmlTools.LoadListFromXmlSerializer<BaseStation>(BaseStationsPath);
            return from BaseStation baseStation in ListBaseStations
                   where baseStation.ChargeSlots > 0
                   select baseStation;
        }
        #endregion

        #region Customer XElement
        public void AddCustomer(int id, string name, string phone, double longitude, double lattitude)
        {
            XElement CustomersRootElem = XmlTools.LoadListFromXmlElement(CustomersPath);
            XElement cus = (from c in CustomersRootElem.Elements()
                             where int.Parse(c.Element("Id").Value) == id
                             select c).FirstOrDefault();
            if (cus != null)
                throw new DO.AddExistingCustomerException();
            XElement CustomerElem = new XElement("Customer",
                                  new XElement("Id", id),
                                  new XElement("Name", name),
                                  new XElement("Phone", phone),
                                  new XElement("Longitute", longitude),
                                  new XElement("Lattitude", lattitude));
            CustomersRootElem.Add(CustomerElem);
            XmlTools.SaveListToXmlElement(CustomersRootElem, CustomersPath);
        }
        public void RemoveCustomer(int id)
        {
            XElement customersRootElem = XmlTools.LoadListFromXmlElement(CustomersPath);

            XElement cus = (from c in customersRootElem.Elements()
                            where int.Parse(c.Element("Id").Value) == id
                            select c).FirstOrDefault();
            if (cus != null)
            {
                cus.Remove(); //remove the cutomer frome the list

                XmlTools.SaveListToXmlElement(customersRootElem, CustomersPath);
            }
            else
                throw new DO.CustomerNotFoundException();
        }
        public Customer CopyCustomer(int customerId)
        {
            XElement customersRootElem = XmlTools.LoadListFromXmlElement(CustomersPath);
            Customer cus = (from c in customersRootElem.Elements()
                            where int.Parse(c.Element("Id").Value) == customerId
                            select new Customer()
                            {
                                Id = Int32.Parse(c.Element("Id").Value),
                                Name = c.Element("Name").Value,
                                Phone = c.Element("Phone").Value,
                                Longitude = double.Parse(c.Element("Longitude").Value),
                                Lattitude = double.Parse(c.Element("Lattitude").Value)
                            }
                                ).FirstOrDefault();
            return cus;
        }
        public IEnumerable<Customer> CopyCustomersList()
        {
            XElement customersRootElem = XmlTools.LoadListFromXmlElement(CustomersPath);
            return from c in customersRootElem.Elements()
                   select new Customer()
                   {
                       Id = Int32.Parse(c.Element("Id").Value),
                       Name = c.Element("Name").Value,
                       Phone = c.Element("Phone").Value,
                       Longitude = double.Parse(c.Element("Longitude").Value), //idk what to do here
                       Lattitude = double.Parse(c.Element("Lattitude").Value)
                   };
        }
        #endregion

        #region Drone
        public void AddDrone(int id, double Battery, WeightCategories maxW, string model)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            foreach (var _ in from Drone drone in ListDrones
                              where drone.Id == id
                              select new { })
            {
                throw new AddExistingDroneException();
            }
            ListDrones.Add(new Drone { Id = id, Battery = Battery, MaxWeight = maxW, Model = model });//Battery = battery, Status = status
            XmlTools.SaveListToXmlSerializer(ListDrones, DronesPath);
        }
        public void RemoveDrone(int id)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            foreach (var drone in from drone in ListDrones
                                  where drone.Id == id
                                  select drone)
            {
                ListDrones.Remove(drone);
                XmlTools.SaveListToXmlSerializer(ListDrones, DronesPath);
                return;
            }
            throw new DroneIdNotFoundException();
        }
        public Drone CopyDrone(int droneId)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            foreach (var Drone in
            from Drone Drone in ListDrones
            where Drone.Id == droneId
            select Drone)
            {
                return Drone;
            }
            throw new DroneIdNotFoundException();
        }
        public IEnumerable<Drone> CopyDronesList()
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            return from d in ListDrones
                   select d;
        }
        public void DroneCharging(int droneId, int baseStationId)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            List<DroneCharge> ListDronesInCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(DronesInChargePath);
            foreach (var (drone, newDrone) in from Drone drone in ListDrones
                                              where drone.Id == droneId
                                              let newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model }// Status = DroneStatuses.Maintenance, Battery = drone.Battery
                                              select (drone, newDrone))
            {
                ListDrones.Remove(drone);
                ListDrones.Add(newDrone);
                DroneCharge newDCharge = new DroneCharge { DroneId = droneId, StationId = baseStationId };//what is this for??? and if we need to add it somewhere - then where??
                ListDronesInCharge.Add(newDCharge);
                XmlTools.SaveListToXmlSerializer(ListDrones, DronesPath);
                XmlTools.SaveListToXmlSerializer(ListDronesInCharge, DronesInChargePath);
                return;
            }
            throw new DroneIdNotFoundException();
        }
        public void DroneRelease(int droneId, int baseStationId)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            List<DroneCharge> ListDronesInCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(DronesInChargePath);
            foreach (var (drone, newDrone) in from Drone drone in ListDrones
                                              where drone.Id == droneId
                                              let newDrone = new Drone { Id = drone.Id, MaxWeight = drone.MaxWeight, Model = drone.Model }//Status = DroneStatuses.Available, Battery = drone.Battery
                                              select (drone, newDrone))
            {
                ListDrones.Remove(drone);
                ListDrones.Add(newDrone);
                XmlTools.SaveListToXmlSerializer(ListDrones, DronesPath);
                foreach (var charger in from DroneCharge charger in ListDronesInCharge
                                        where charger.DroneId == droneId && charger.StationId == baseStationId
                                        select charger)
                {
                    ListDronesInCharge.Remove(charger);
                    XmlTools.SaveListToXmlSerializer(ListDronesInCharge, DronesInChargePath);
                    return;
                }
            }
            throw new DroneIdNotFoundException();//if the drone exists then the program wont get here
        }
        public double[] DronePowerConsumingPerKM()
        {
            List<string> config = XmlTools.LoadListFromXmlSerializer<string>(ConfigPath);
            double[] DPC = new double[5];
            _ = double.TryParse(config[0], out DPC[0]);
            _ = double.TryParse(config[1], out DPC[1]);
            _ = double.TryParse(config[2], out DPC[2]);
            _ = double.TryParse(config[3], out DPC[3]);
            _ = double.TryParse(config[4], out DPC[4]);
            return DPC;
        }
        #endregion

        #region Parcel
        public void AddParcel(int id, int droneId, int senderId, int targetId, Priorities priority, WeightCategories weight, DateTime? requested, DateTime? scheduled, DateTime? pickedUp, DateTime? delivered)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            List<string> config = XmlTools.LoadListFromXmlSerializer<string>(ConfigPath);
            int parcelId;
            if (id != -1)
                parcelId = id;
            else
            {
                #region running number update - parcel id
            _ = Int32.TryParse(config[5], out parcelId);
            parcelId++;
            config[5] = parcelId.ToString();
            XmlTools.SaveListToXmlSerializer(config, ConfigPath);
                #endregion
            }
            foreach (var _ in from Parcel parcel in ListParcels
                              where parcel.DroneId == droneId
                              select new { })
            {
                ListParcels.Add(new Parcel { Id = parcelId, DroneId = 0, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
                XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
                throw new AddParcelToAnAsscriptedDroneException();
            }
            ListParcels.Add(new Parcel { Id = parcelId, DroneId = droneId, SenderId = senderId, TargetId = targetId, Priority = priority, Weight = weight, Requested = requested, Scheduleded = scheduled, PickedUp = pickedUp, Delivered = delivered });
            XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
        }
        public void RemoveParcel(int id)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);

            foreach (var parcel in from parcel in ListParcels
                                   where parcel.Id == id
                                   select parcel)
            {
                ListParcels.Remove(parcel);
                XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
                return;
            }
            throw new ParcelIdNotFoundException();
        }
        public Parcel CopyParcel(int parcelId)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            foreach (var parcel in from Parcel parcel in ListParcels
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                return parcel;
            }
            throw new ParcelIdNotFoundException();
        }
        public IEnumerable<Parcel> CopyParcelsList()
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            return from p in ListParcels
                   select p;
        }
        public void AscriptionPtoD(int parcelId, int droneId)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(DronesPath);
            Parcel p = new Parcel();
            bool droneExsists = false;
            bool parcelExsists = false;
            foreach (var _ in from Drone drone in ListDrones
                              where droneId == drone.Id
                              select new { })
            {
                droneExsists = true;
            }
            if (droneExsists == false)
                throw new DroneIdNotFoundException();
            foreach (var parcel in from Parcel parcel in ListParcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                p = parcel;
                parcelExsists = true;
            }
            if (parcelExsists)
            {
                ListParcels.Remove(p);
                p.DroneId = droneId;
                p.Scheduleded = DateTime.Now;
                ListParcels.Add(p);
                XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
            }
            else
                throw new ParcelIdNotFoundException();
        }
        public void PickUpParcel(int parcelId)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            foreach (var parcel in from Parcel parcel in ListParcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                if (parcel.DroneId == 0)
                    throw new DroneIdNotFoundException();
                else
                {
                    Parcel nParcel = new Parcel { Id = parcel.Id, DroneId = parcel.DroneId, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, PickedUp = DateTime.Now, Delivered = parcel.Delivered };
                    ListParcels.Remove(parcel);
                    ListParcels.Add(nParcel);
                    XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
                    return;
                }
            }
            throw new ParcelIdNotFoundException();
        }
        public void ParcelDelivering(int parcelId)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            foreach (var parcel in from Parcel parcel in ListParcels//finding our parcel
                                   where parcel.Id == parcelId
                                   select parcel)
            {
                if (parcel.DroneId == 0)
                    throw new DroneIdNotFoundException();
                else
                {
                    Parcel nParcel = new Parcel { Id = parcel.Id, DroneId = parcel.DroneId, SenderId = parcel.SenderId, TargetId = parcel.TargetId, Weight = parcel.Weight, Priority = parcel.Priority, Requested = parcel.Requested, Scheduleded = parcel.Scheduleded, PickedUp = parcel.PickedUp, Delivered = DateTime.Now };
                    ListParcels.Remove(parcel);
                    ListParcels.Add(nParcel);
                    XmlTools.SaveListToXmlSerializer(ListParcels, ParcelsPath);
                    return;
                }
            }
            throw new ParcelIdNotFoundException();
        }
        public IEnumerable<Parcel> UnAscriptedParcels()
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(ParcelsPath);
            return from p in ListParcels
                   where p.DroneId == 0
                   select p;
        }
        #endregion

        #region Coordinate
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
        public double[] CopyLongitudeRange()
        {
            List<string> config = XmlTools.LoadListFromXmlSerializer<string>(ConfigPath);

            double[] responce = new double[2];
            _ = double.TryParse(config[6], out responce[0]);
            _ = double.TryParse(config[7], out responce[1]);
            return responce;
        }
        public double[] CopyLattitudeRange()
        {
            List<string> config = XmlTools.LoadListFromXmlSerializer<string>(ConfigPath);
            double[] responce = new double[2];
            _ = double.TryParse(config[8], out responce[0]);
            _ = double.TryParse(config[9], out responce[1]); return responce;
        }
        #endregion
    }
}
