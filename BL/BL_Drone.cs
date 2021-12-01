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
            BL.drones.Add({ DroneId=Id, Model=Model, MaxWeight= MaxWeight, DroneState=Enums.DroneStatuses.Maintenance, Battery=r, CurrentLocation.Longitude= BStationLocation.Longitude, CurrentLocation.Latitude= BStationLocation.Latitude});
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
                throw new DroneIdNotFoundException();
            foreach(DroneForList droneForList in drones)
            {
                if (droneForList.DroneId == Id)
                {
                    droneForList.Model = Model;
                    break;
                }
            }
        }//לממש
        void IBL.BO.IBL.DroneToCharge(int Id) { }//לממש
        void IBL.BO.IBL.ReleaseDroneFromCharge(int Id, DateTime TimeInCharge) { }//לממש
        void IBL.BO.IBL.DisplayDrone(int id) { }//לממש
        void IBL.BO.IBL.DisplayDronesList() { }//לממש

    }
}
