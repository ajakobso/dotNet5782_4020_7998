using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    public partial class BL// : IBL.BO.IBL
    {
        private double Distance(double long1, double lat1, double long2, double lat2)
        {
            lat1 *= (Math.PI / 180.0);
            long1 *= (Math.PI / 180.0);
            lat2 *= (Math.PI / 180.0);
            long2 *= (Math.PI / 180.0) - long1;
            double distance = Math.Pow(Math.Sin((lat2 - lat1) / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(long2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(distance), Math.Sqrt(1.0 - distance)));
        }
        private bool droneMakeIt(DroneForList drone,IDAL.DO.Parcel parcel)//בודק אם לרחפן יש מספיק בטריה להגיע לשולח, ליעד ולתחנה הקרובה ביותר מיעד המשלוח
        {
            Location location;
            location = new Location(myDalObject.CopyCustomer(parcel.TargetId).Longitude, myDalObject.CopyCustomer(parcel.TargetId).Lattitude);
            double distanceBetweenSenderAndDst = Distance(myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude, myDalObject.CopyCustomer(parcel.TargetId).Longitude, myDalObject.CopyCustomer(parcel.TargetId).Lattitude);//distance between sender and target
            double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
            double distanceBetweenDstAndBs = distanceFromBS(location)[0];//distance from target to closest bs
            double batteryPerKM = 0;//defult
            switch ((Enums.WeightCategories)parcel.Weight)
            {
                case Enums.WeightCategories.Light:
                    batteryPerKM = myDalObject.DronePowerConsumingPerKM()[1];
                    break;
                case Enums.WeightCategories.Middle:
                    batteryPerKM = myDalObject.DronePowerConsumingPerKM()[2];
                    break;
                case Enums.WeightCategories.Heavy:
                    batteryPerKM = myDalObject.DronePowerConsumingPerKM()[3];
                    break;
                default:
                    break;
            }
            
            double possibleDistance = (myDalObject.DronePowerConsumingPerKM()[0] * distanceBetweenDroneAndSender) + (myDalObject.DronePowerConsumingPerKM()[0] * distanceBetweenDstAndBs) + (batteryPerKM * distanceBetweenSenderAndDst);//calculation of the amount of battery in percent needed for the 
            if (possibleDistance <= drone.Battery)//check if there is enough battery
                return true;
            else
                return false;
        }
        private double[] distanceFromBS(Location location)//return ths distance from the closest base station
        {
            double[] res = new double[2];
            double dis;
            res[0] = 1000;//very big number just to start with, since our company do delivering in jerusalem so 1000 km is higher than the distance between the actual places.
            foreach(IDAL.DO.BaseStation bs in myDalObject.CopyBaseStations())
            {
                dis = Distance(location.Long, location.Lat, bs.Longitude, bs.Lattitude);
                if (res[0] > dis) 
                {
                    res[0] = dis;
                    res[1] = bs.Id;
                }
            }
            //res[0] = distance;//the distance between the location to the closest bs
            //res[1] = bsID;//the bs id
            return res;
        }
        void IBL.AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority)
        {
            myDalObject.AddParcel(0, SCustomerId, DCustomerId, (IDAL.DO.Priorities)Priority, (IDAL.DO.WeightCategories)Weight, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }
        void IBL.DeliveringParcelByDrone(int Id)
        {
            DroneForList drone = DisplayDrone(Id);
            if ()
        }//
        private Enums.WeightCategories WeightParcel(IDAL.DO.WeightCategories v)//convert frome IDAL.DO.WeightCategories to IBL.BO.WeightCategories 
        {
            Enums.WeightCategories w;
            switch (v)
            {

                case IDAL.DO.WeightCategories.Light:
                    w = Enums.WeightCategories.Light;
                    return w;

                case IDAL.DO.WeightCategories.Middle:
                    w = Enums.WeightCategories.Middle;
                    return w;
                case IDAL.DO.WeightCategories.Heavy:
                    w = Enums.WeightCategories.Heavy;
                    return w;
                default:
                    w = Enums.WeightCategories.Heavy;
                    return w;

            }
        }
        private bool ProperWeight(Enums.WeightCategories dWeight,Enums.WeightCategories pWeight)//check if the drone can carry the parcel
        {
            switch (pWeight)
            {
                case Enums.WeightCategories.Light:
                    if (dWeight == Enums.WeightCategories.Light || dWeight == Enums.WeightCategories.Middle || dWeight == Enums.WeightCategories.Heavy)
                        return true;
                    else
                        return false;
                case Enums.WeightCategories.Middle:
                    if (dWeight == Enums.WeightCategories.Heavy || dWeight == Enums.WeightCategories.Middle)
                        return true;
                    else
                        return false;  
                case Enums.WeightCategories.Heavy:
                    if (dWeight == Enums.WeightCategories.Heavy)
                        return true;
                    else
                        return false;
                default:
                    break;
            }
            return false;
        }
        void IBL.AscriptionParcelToDrone(int Id) 
        {
            DroneForList drone = DisplayDrone(Id);
            bool parcelFound = false;
            if (drone.DroneState == Enums.DroneStatuses.Available)
            {
                foreach (var parcel in myDalObject.CopyParcelsList())
                {
                    if (parcel.Priority == (IDAL.DO.Priorities)Enums.Priorities.Urgent && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel)) 
                    {
                        myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId);
                        drone.DroneState = Enums.DroneStatuses.Shipping;
                        drone.InDeliveringParcelId = parcel.Id;
                        parcelFound = true;
                    } 
                }
                if (parcelFound == false)
                {
                    foreach (var parcel in myDalObject.CopyParcelsList())
                    {
                        if (drone.MaxWeight == WeightParcel(parcel.Weight) && droneMakeIt(drone, parcel))
                        {
                            myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId);
                            drone.DroneState = Enums.DroneStatuses.Shipping;
                            drone.InDeliveringParcelId = parcel.Id;
                            parcelFound = true;
                        }
                    }
                    if (parcelFound == false)
                    {
                        double maxDistance = 10000; 
                        foreach (var parcel in myDalObject.CopyParcelsList())
                        {
                
                            double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                            if (maxDistance > distanceBetweenDroneAndSender && droneMakeIt(drone,parcel)) 
                            {
                                maxDistance = distanceBetweenDroneAndSender;
                                myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId);
                                drone.DroneState = Enums.DroneStatuses.Shipping;
                                drone.InDeliveringParcelId = parcel.Id;
                                parcelFound = true;
                            }
                        }
                    }
                }

            }
            if (parcelFound == false) //no matching parcel, throw proper exception
            {

            }
        }
        void IBL.DisplayParcel(int id) { }//
        void IBL.PickUpParcel(int DId) { }//
        void IBL.DisplayParcelsList() { }//
        void IBL.DisplayUnAscriptedParcelsList() { }//

    }
}
