using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        
        public void AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation)
        {
            Random r = new Random();//אין לי שמץ של מושג אם ככה מגדירים רנדום
            r.Next(20, 41);//מגריל מספר בין 20 ל-40 לפי מה שהבנתי
            Location BStationLocation;
            foreach(var baseStation in myDalObject.CopyBaseStations())//כרגיל לא עובד
            {
                if (baseStation.Id == Bstation)
                {
                    try { BStationLocation = AddLocation(baseStation.Longitude, baseStation.Lattitude); }
                    catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//add throw of location exception in all the references of the AddLocation
                    try { myDalObject.AddDrone(Id, (double)r.Next(20, 40) / 100, (IDAL.DO.WeightCategories)MaxWeight, Model); }
                    catch (IDAL.DO.AddExistingDroneException) { throw new AddExistingDroneException(); }
                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = MaxWeight, DroneState = Enums.DroneStatuses.Maintenance, Battery = (double)r.Next(20, 40) / 100, CurrentLocation = BStationLocation });
                    if ((myDalObject.CopyLongitudeRange()[0] > location.Long) || (myDalObject.CopyLongitudeRange()[1] < location.Long) || (myDalObject.CopyLattitudeRange()[0] > location.Lat) || (myDalObject.CopyLattitudeRange()[1] < location.Lat))
                    {
                        throw new LocationOutOfRangeException();
                    }
                    return;
                }
            }
            throw new BaseStationNotFoundException();
        }
        public void UpdateDrone(int Id, string Model)
        {
            int Check = 0;
            foreach(var drone in myDalObject.CopyDronesList())
            {
                if(drone.Id==Id)
                {
                    myDalObject.RemoveDrone(drone.Id);
                    myDalObject.AddDrone(drone.Id, drone.Battery, drone.MaxWeight, Model);
                    Check++;
                    break;
                }
            }
            if (Check == 0)
                throw new IDAL.DO.DroneIdNotFoundException();
            foreach(DroneForList droneForList in drones)
            {
                if (droneForList.DroneId == Id)
                {
                    RemoveDroneForList(droneForList.DroneId);
                    drones.Add(new DroneForList { DroneId = Id, Model = Model, MaxWeight = droneForList.MaxWeight, Battery = droneForList.Battery, CurrentLocation = droneForList.CurrentLocation, DroneState = droneForList.DroneState, InDeliveringParcelId = droneForList.InDeliveringParcelId });
                    break;
                }
            }
        }
        public void DroneToCharge(int Id)
        {
            double NBattery = 20.0;//minimum value as a default, in the meanwhile until we insert value.
            foreach (DroneForList drone in drones)
            {
                double Battery;
                if (drone.DroneState == Enums.DroneStatuses.Available)
                {
                    Location droneLocation = AddLocation(drone.CurrentLocation.Long, drone.CurrentLocation.Lat);
                    Location location = AddLocation(myDalObject.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Longitude, myDalObject.CopyBaseStation((int)distanceFromBS(droneLocation)[1]).Lattitude);
                    Battery = myDalObject.DronePowerConsumingPerKM()[0] * distanceFromBS(drone.CurrentLocation)[0];
                    if (Battery > drone.Battery)
                    {
                        throw new IDAL.DO.DroneOutOfBatteryException();//////////////////////////////צריך להגדיר חריגה מתאימה
                        
                    }
                    foreach (var dalDrone in myDalObject.CopyDronesList())
                    {
                        if (dalDrone.Id == drone.DroneId)
                        {
                            myDalObject.RemoveDrone(dalDrone.Id);
                            myDalObject.AddDrone(dalDrone.Id, dalDrone.Battery - Battery, dalDrone.MaxWeight, dalDrone.Model);
                            NBattery = dalDrone.Battery - Battery;
                            //dalDrone.Longitude = location.Longitude;
                            //dalDrone.Latittude = location.Latitude;
                            drone.DroneState = Enums.DroneStatuses.Maintenance;
                        }
                    }
                    foreach (var baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((baseStation.Longitude == location.Long) && (baseStation.Lattitude == location.Lat))
                        {
                            try { myDalObject.RemoveBaseStation(baseStation.Id); }
                            catch (IDAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            try { myDalObject.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots - 1, baseStation.Longitude, baseStation.Lattitude); }
                            catch (IDAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                           
                        }

                    }
                    foreach (var baseStation1 in baseStations)
                    {
                        if (baseStation1.StationLocation==location)
                        {
                            baseStation1.DInChargeList.Add(new DroneInCharge { DroneId = drone.DroneId, Battery = NBattery });
                        }
                    }
            
                }
            }
        }//לממש
        public void ReleaseDroneFromCharge(int Id, double TimeInCharge)
        {
            DroneForList nDrone = new DroneForList();
            foreach (DroneForList drone in drones)
            {
                if(drone.DroneId==Id)
                {
                    if(!(drone.DroneState==Enums.DroneStatuses.Maintenance))
                    {
                        throw new DroneIdNotFoundException();
                    }
                    nDrone = drone;
                    nDrone.Battery += TimeInCharge*myDalObject.DronePowerConsumingPerKM()[4];
                    nDrone.DroneState = Enums.DroneStatuses.Available;
                    foreach (var baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((nDrone.CurrentLocation.Long == baseStation.Longitude) && (nDrone.CurrentLocation.Lat == baseStation.Lattitude)) 
                        {
                            try { myDalObject.RemoveBaseStation(baseStation.Id); }
                            catch (IDAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            try { myDalObject.AddBaseStation(baseStation.Id, baseStation.Name, baseStation.ChargeSlots + 1, baseStation.Longitude, baseStation.Lattitude); }
                            catch (IDAL.DO.BaseStationNotFoundException) { throw new BaseStationNotFoundException(); }
                            catch (IDAL.DO.AddExistingBaseStationException) { throw new AddExistingBaseStationException(); }
                        }
                    }
                    foreach (var baseStation1 in baseStations)
                    {
                        if (baseStation1.StationLocation == nDrone.CurrentLocation)
                        {

                            baseStation1.DInChargeList.Remove(new DroneInCharge { DroneId = nDrone.DroneId, Battery = nDrone.Battery });
                        }
                    }
                    drones.Add(nDrone);

                }
            }
        }//לממש
        public DroneForList DisplayDrone(int id)
        {
            DroneForList nDrone = new DroneForList();
            foreach (DroneForList drone in drones)
            {
                if (drone.DroneId == id)
                {
                    nDrone = drone;
                    return nDrone;
                }
            }
             throw new DroneIdNotFoundException();
         //the function demend us to return a value, and because the return is inside a condition it cause an error
        }
        public IEnumerable<DroneForList> DisplayDronesList(Predicate<DroneForList> predicate)
        {
            IEnumerable<DroneForList> DronesList = drones.FindAll(predicate);
            return DronesList;
        }
        
        public void RemoveDroneForList(int Id)
        {
            int Check = 0;
            foreach(var drone in drones)
            {
                if(drone.DroneId==Id)
                {
                    drones.Remove(drone);
                    Check++;
                    break;
                }
            }
            if (Check==0)
            {
                throw new DroneIdNotFoundException();
            }
        }
    }
}
