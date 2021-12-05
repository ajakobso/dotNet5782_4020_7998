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
            int Check = 0;
            foreach(BaseStation baseStation in myDalObject.DataSource.Config.BaseStations)//כרגיל לא עובד
            {
                if (baseStation.BaseStationId == Bstation)
                {
                    BStationLocation = baseStation.StationLocation;
                    Check++;
                    break;
                }
            }
            if (Check == 0)
                throw new IDAL.DO.BaseStationNotFoundException();

            myDalObject.AddDrone(Id, (IDAL.DO.WeightCategories)MaxWeight, Model);//צריך לטפל בפונ' שבדאטה סורס
            drones.Add( DroneId=Id, Model=Model, MaxWeight= MaxWeight, DroneState=Enums.DroneStatuses.Maintenance, Battery=(double)r.Next(20,40)/100, CurrentLocation.Longitude= BStationLocation.Longitude, CurrentLocation.Latitude= BStationLocation.Latitude);
        }
        public void UpdateDrone(int Id, string Model)
        {
            int Check = 0;
            foreach(IDAL.DO.Drone drone in myDalObject.CopyDronesList())
            {
                if(drone.DroneId==Id)
                {
                    drone.Model = Model;
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
                    droneForList.Model = Model;
                    break;
                }
            }
        }//צריך להבין מה הבעיה עם מיי דאל אובג'קט
        public void DroneToCharge(int Id)
        {
            double NBattery;
            foreach (DroneForList drone in drones)
            {
                double Battery;
                if (drone.DroneState == Enums.DroneStatuses.Available)
                {
                    Battery = myDalObject.DronePowerConsumingPerKM()[0] * distanceFromBS(drone.CurrentLocation)[0];
                    if (Battery > drone.Battery)
                    {
                        throw new Exception();//////////////////////////////צריך להגדיר חריגה מתאימה
                        
                    }
                    foreach (IDAL.DO.Drone dalDrone in myDalObject.CopyDronesList())
                    {
                        if (dalDrone.Id == drone.DroneId)
                        {
                            NBattery = dalDrone.Battery - Battery;
                            dalDrone.Battery -= Battery;
                            //dalDrone.Longitude = location.Longitude;
                            //dalDrone.Latittude = location.Latitude;
                            dalDrone.DroneState = Enums.DroneStatuses.Maintenance;
                        }
                    }
                    foreach (IDAL.DO.BaseStation baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((baseStation.Longitude == location.Longitude) && (baseStation.Latitude == location.Latitude))
                        {
                            baseStation.AvailableChargeSlots -= 1;
                        }

                    }
                    foreach (BaseStation baseStation1 in baseStations)//רק בביאל..
                    {
                        if (baseStation1.StationLocation==location)
                        {
                            baseStation1.DInChargeList.Add({ DroneId = drone.DroneId, Battery = NBattery });//זה קיים רק ב בי אל..
                        }
                    }
            
                }
            }
        }//לממש
        public void ReleaseDroneFromCharge(int Id, DateTime TimeInCharge)
        {
            DroneForList nDrone = new DroneForList();
            foreach (DroneForList drone in drones)
            {
                if(drone.DroneId==Id)
                {
                    if(!(drone.DroneStatus==Enums.DroneStatuses.Maintenance))
                    {
                        throw new IDAL.DO.DroneIdNotFoundException();
                    }
                    nDrone = drone;
                    nDrone.Battery += ChargingBattery();//לממש!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    nDrone.DroneState = Enums.DroneStatuses.Available;
                    foreach (var baseStation in myDalObject.CopyBaseStations())
                    {
                        if ((nDrone.CurrentLocation.Long == baseStation.Longitude) && (nDrone.CurrentLocation.Lat == baseStation.Lattitude)) 
                        {
                            baseStation.AvailableChargeSlots++;////////////////////////////////////
                        }
                    }
                    foreach (var baseStation1 in baseStations)//רק בביאל..
                    {
                        if (baseStation1.StationLocation == nDrone.CurrentLocation)
                        {
                            baseStation1.DInChargeList.remove(nDrone);//זה קיים רק ב בי אל..
                        }
                    }

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
        }//לממש
        public IEnumerable<DroneForList> DisplayDronesList()
        {
            IEnumerable<DroneForList> DronesList = drones;
            return DronesList;
        }//לממש
        
    }
}
