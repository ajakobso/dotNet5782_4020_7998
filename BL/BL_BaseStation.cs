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
        public void AddBaseStation(int num, string name, Location location, int numOfAvailableDCharge)
        {
            IEnumerable<DroneInCharge> DronesInCharge; //= new IEnumerable<IBL.BO.DroneInCharge>();
            baseStations.Add({ BaseStationId = num, StationName = name,AvailableChargingS= numOfAvailableDCharge, StationLocation = location, DInChargeList=new IEnumerable<DroneInCharge>()  });
             DalObject.DataSource.Config.BaseStations.Add(new IDAL.DO.BaseStation {Id = num, Name = name, ChargeSlots = numOfAvailableDCharge, Longitude = location.Longitude, Lattitude = location.Latitude });//חייב לבדוק מה הבעיה עם זה..זה פשוט תוקע את כל המחלקה
            myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
             myDalObject.AddBaseStation(num, name, numOfAvailableDCharge, location.Longitude, location.Latitude);
        }


        void IBL.UpdateBaseStation(int Id, string Name, int NumOfChargeSlots)
        {
            int Check = 0;
            foreach(IDAL.DO.BaseStation baseStation in myDalObject.CopyBaseStations())
            {
                if(baseStation.Id==Id)
                {
                    Check++;
                    IDAL.DO.BaseStation nBaseStation = new IDAL.DO.BaseStation();
                    nBaseStation = baseStation;

                    if (!(Name==" "))
                    {
                        nBaseStation.Name = Name;////////remove the old bs, and add the updated one.
                    }
                    if (!(NumOfChargeSlots == -1))
                    {
                        nBaseStation.ChargeSlots = NumOfChargeSlots;
                    }
                    break;
                }
            }
            foreach(BaseStationForList baseStation1 in baseStations)
            {
                if(baseStation1.BaseStationId==Id)
                {
                    BaseStationForList nbaseStation = new BaseStationForList();
                    nbaseStation = baseStation1;

                    if (!(Name==" "))
                    {
                        nbaseStation.StationName = Name;
                    }
                    if (!(NumOfChargeSlots==-1))
                    {
                        nbaseStation.AvailableChargingS = (NumOfChargeSlots - nbaseStation.UnAvailableChargingS);
                    }
                }
            }
            if (Check == 0)
                throw new IDAL.DO.BaseStationNotFoundException();
        }//
        BaseStationForList IBL.DisplayBaseStation(int id)
        {
            BaseStationForList nBaseStation = new BaseStationForList();
            foreach(BaseStationForList baseStation in baseStations)
            {
                if(baseStation.BaseStationId==id)
                {
                    nBaseStation = baseStation;
                    return nBaseStation;
                }
            }
            throw new BaseStationNotFoundException();
        }//
        IEnumerable<BaseStationForList> IBL.DisplayBaseStationsList()
        {
            IEnumerable<BaseStationForList> nStationsList = new IEnumerable<BaseStationForList>();
            nStationsList = baseStations;
            return nStationsList;
        }//
        void IBL.DisplayAvailableChargingStation() { }//
    public Location findCloseBaseStationLocation(Ilocatable fromLocation)
    {
        List<BaseStation> Locations = new List<BaseStation>();
        foreach (var baseStation in myDalObject.DataSource.Config.BaseStations)
        {
            Locations.Add(new BaseStation
            {
                Location = new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude }
            });
        }
        Location location = Locations[0].Location;
            return location;/////////not true, just did it to delete an error
        ///////////////////////
    }
}


}


