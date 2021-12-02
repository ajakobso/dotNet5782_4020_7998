using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.BO.IBL//?????????????????
    {
        
        void IBL.BO.IBL.AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation)
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
                // throw new IDAL.DO.BaseStationNotFoundException();
                throw new myDalObject.BaseStationNotFoundException();

                myDalObject.AddDrone(Id, MaxWeight, Model);//צריך לטפל בפונ' שבדאטה סורס
            BL.drones.Add({ DroneId=Id, Model=Model, MaxWeight= MaxWeight, DroneState=Enums.DroneStatuses.Maintenance, Battery=(double)r.Next(20,40)/100, CurrentLocation.Longitude= BStationLocation.Longitude, CurrentLocation.Latitude= BStationLocation.Latitude});
        }
        void IBL.BO.IBL.UpdateDrone(int Id, string Model)
        {
            int Check = 0;
            foreach(Drone drone in myDalObject.DataSource.Config.Drones)
            {
                if(drone.DroneId==Id)
                {
                    drone.Model = Model;
                    Check++;
                    break;
                }
            }
            if (Check == 0)
                throw new myDalObject.DroneIdNotFoundException();
            foreach(DroneForList droneForList in drones)
            {
                if (droneForList.DroneId == Id)
                {
                    droneForList.Model = Model;
                    break;
                }
            }
        }//צריך להבין מה הבעיה עם מיי דאל אובג'קט
        void IBL.BO.IBL.DroneToCharge(int Id)
        {
            double NBattery;
            foreach (DroneForList drone in BL.drones)
            {
                double Battery;
                if (drone.DroneState==Enums.DroneStatuses.Available)
                {
                    Location location = findCloseBaseStationLocation(drone);//////////אנחנו צריכות להגדיר פונקציה שתחשב מרחק וגם פונקציה שתחשב כמה בטריה לוקח כל מרחק
                    Battery = myDalObject.BatteryUsage()[BatteryUsage.Available] * distance(drone.CurrentLocation, location);
                    if (Battery > drone.Battery)
                    {
                        throw new Exception();//////////////////////////////צריך להגדיר חריגה מתאימה
                        break;
                    }
                    foreach(myDalObject.Drone dalDrone in myDalObject.DataSource.Config.Drones)
                    {
                        if(dalDrone.Id==drone.DroneId)
                        {
                            NBattery = dalDrone.Battery - Battery;
                            dalDrone.Battery -= Battery;
                            //dalDrone.Longitude = location.Longitude;
                            //dalDrone.Latittude = location.Latitude;
                            dalDrone.DroneState = Enums.DroneStatuses.Maintenance;
                        }
                    }
                    foreach(myObject.BaseStation baseStation in myObject.DataSource.Config.BaseStations)
                    {
                        if ((baseStation.Longitude == location.Longitude) && (baseStation.Latitude == location.Latitude))
                        {
                            baseStation.AvailableChargeSlots -= 1;
                        }
                        
                    }
                    DronesInCharge.Add({ DroneId=drone.DroneId, Battery= NBattery });
                }
            }
        }//לממש
        void IBL.BO.IBL.ReleaseDroneFromCharge(int Id, DateTime TimeInCharge)
        {

            foreach(Drone drone in drones)
            {
                if(drone.DroneId==Id)
                {
                    if(!(drone.DroneStatus==Enums.DroneStatuses.Maintenance))
                    {
                        throw new myDalObject.DroneIdNotFoundException();
                        break;
                    }

                }
            }
        }//לממש
        void IBL.BO.IBL.DisplayDrone(int id) { }//לממש
        void IBL.BO.IBL.DisplayDronesList() { }//לממש

    }
}
