using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;

namespace BL
{
    internal sealed partial class BL// : BO.BlApi
    {
        private static double Distance(double long1, double lat1, double long2, double lat2)
        {
            //lat1 *= (Math.PI / 180.0);
            //long1 *= (Math.PI / 180.0);
            //lat2 *= (Math.PI / 180.0);
            //long2 *= (Math.PI / 180.0) - long1;
            //double distance = Math.Pow(Math.Sin((lat2 - lat1) / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(long2 / 2.0), 2.0);
            //return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(distance), Math.Sqrt(1.0 - distance)));
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = long1 - long2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                (Math.Sin(rlat1) * Math.Sin(rlat2)) + (Math.Cos(rlat1) *
            Math.Cos(rlat2) * Math.Cos(rtheta));
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            return dist * 1.609344;
        }
        private double minimumBattery(DroneForList drone, DAL.DO.Parcel parcel)
        {
            Location location;
            location = AddLocation(myDalObject.CopyCustomer(parcel.TargetId).Longitude, myDalObject.CopyCustomer(parcel.TargetId).Lattitude);
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

            double possBlApieDistance = (myDalObject.DronePowerConsumingPerKM()[0] * distanceBetweenDroneAndSender) + (myDalObject.DronePowerConsumingPerKM()[0] * distanceBetweenDstAndBs) + (batteryPerKM * distanceBetweenSenderAndDst);//calculation of the amount of battery in percent needed for the
            return possBlApieDistance;                                                                                                                                                                                                                             //
        }
        private bool droneMakeIt(DroneForList drone, DAL.DO.Parcel parcel)//בודק אם לרחפן יש מספיק בטריה להגיע לשולח, ליעד ולתחנה הקרובה ביותר מיעד המשלוח
        {
            double minimum = minimumBattery(drone, parcel);
            if (minimum <= drone.Battery)//check if there is enough battery
                return true;
            else
                return false;
        }
        private double[] distanceFromBS(Location location)//return ths distance from the closest base station
        {
            double[] res = new double[2];
            double dis;
            res[0] = 1000;//very big number just to start with, since our company DAL.DO delivering in jerusalem so 1000 km is higher than the distance between the actual places.
            foreach (var bs in myDalObject.CopyBaseStations())////////////////////////////////////////////not linq
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
        private Enums.WeightCategories WeightParcel(DAL.DO.WeightCategories v)//convert frome DAL.DO.WeightCategories to BO.WeightCategories 
        {
            Enums.WeightCategories w;
            switch (v)
            {

                case DAL.DO.WeightCategories.Light:
                    w = Enums.WeightCategories.Light;
                    return w;

                case DAL.DO.WeightCategories.Middle:
                    w = Enums.WeightCategories.Middle;
                    return w;
                case DAL.DO.WeightCategories.Heavy:
                    w = Enums.WeightCategories.Heavy;
                    return w;
                default:
                    w = Enums.WeightCategories.Heavy;
                    return w;

            }
        }
        private bool ProperWeight(Enums.WeightCategories dWeight, Enums.WeightCategories pWeight)//check if the drone can carry the parcel
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
        public void AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority)
        {
            try { myDalObject.AddParcel(-1,0, SCustomerId, DCustomerId, (DAL.DO.Priorities)Priority, (DAL.DO.WeightCategories)Weight, DateTime.Now, null, null, null); }
            catch (DAL.DO.AddParcelToAnAsscriptedDroneException) { }
        }//
        public void AscriptionParcelToDrone(int Id)
        {
            DroneForList drone = DisplayDrone(Id);
            bool parcelFound = false;
            if (drone.DroneState == Enums.DroneStatuses.Available)
            {
                foreach (var parcel in from parcel in myDalObject.CopyParcelsList()
                                       where parcel.Priority == (DAL.DO.Priorities)Enums.Priorities.Urgent && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel)
                                       select parcel)//linq
                {
                    try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                    catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }

                    drone.DroneState = Enums.DroneStatuses.Shipping;
                    drone.InDeliveringParcelId = parcel.Id;
                    parcelFound = true;
                    return;
                }
                //foreach (var parcel in myDalObject.CopyParcelsList())-not linq
                //{
                //    if (parcel.Priority == (DAL.DO.Priorities)Enums.Priorities.Urgent && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel))
                //    {
                //        try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                //        catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                //        catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                //        drone.DroneState = Enums.DroneStatuses.Shipping;
                //        drone.InDeliveringParcelId = parcel.Id;
                //        parcelFound = true;
                //        return;
                //    }
                //}
                if (parcelFound == false)
                {
                    foreach (var parcel in from parcel in myDalObject.CopyParcelsList()
                                           where drone.MaxWeight == WeightParcel(parcel.Weight) && droneMakeIt(drone, parcel)
                                           select parcel)//linq
                    {
                        try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                        catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                        catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }

                        drone.DroneState = Enums.DroneStatuses.Shipping;
                        drone.InDeliveringParcelId = parcel.Id;
                        parcelFound = true;
                        return;
                    }
                    //foreach (var parcel in myDalObject.CopyParcelsList())-not linq
                    //{
                    //    if (drone.MaxWeight == WeightParcel(parcel.Weight) && droneMakeIt(drone, parcel))
                    //    {
                    //        try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                    //        catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    //        catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                    //        drone.DroneState = Enums.DroneStatuses.Shipping;
                    //        drone.InDeliveringParcelId = parcel.Id;
                    //        parcelFound = true;
                    //        return;
                    //    }
                    //}
                    if (parcelFound == false)
                    {
                        double maxDistance = 10000;
                        foreach (var (parcel, distanceBetweenDroneAndSender) in from parcel in myDalObject.CopyParcelsList()
                                                                                let distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude)//distance between the drone and the sender's location
                                                                                where maxDistance > distanceBetweenDroneAndSender && droneMakeIt(drone, parcel)
                                                                                select (parcel, distanceBetweenDroneAndSender))//linq
                        {
                            maxDistance = distanceBetweenDroneAndSender;
                            try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                            catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                            catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }

                            drone.DroneState = Enums.DroneStatuses.Shipping;
                            drone.InDeliveringParcelId = parcel.Id;
                            parcelFound = true;
                            return;
                        }
                        //foreach (var parcel in myDalObject.CopyParcelsList())-not linq
                        //{

                        //    double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                        //    if (maxDistance > distanceBetweenDroneAndSender && droneMakeIt(drone, parcel))
                        //    {
                        //        maxDistance = distanceBetweenDroneAndSender;
                        //        try { myDalObject.AscriptionPtoD(parcel.Id, drone.DroneId); }
                        //        catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                        //        catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                        //        drone.DroneState = Enums.DroneStatuses.Shipping;
                        //        drone.InDeliveringParcelId = parcel.Id;
                        //        parcelFound = true;
                        //        return;
                        //    }
                        //}
                    }
                }

            }
            if (parcelFound == false) //no matching parcel, throw proper exception
            {
                throw new NoParcelAscriptedToDroneException();
            }
        }//
        public void PickUpParcel(int DId)
        {
            DroneForList drone = DisplayDrone(DId);
            var parcel = myDalObject.CopyParcel(drone.InDeliveringParcelId);
            if (parcel.DroneId == DId && parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null)
            {
                double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                drone.Battery -= distanceBetweenDroneAndSender * myDalObject.DronePowerConsumingPerKM()[0];
                drone.CurrentLocation = AddLocation(myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);
                try { myDalObject.RemoveParcel(parcel.Id); }
                catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                parcel.PickedUp = DateTime.Now;
                try { myDalObject.AddParcel(parcel.Id, parcel.DroneId, parcel.SenderId, parcel.TargetId, parcel.Priority, parcel.Weight, parcel.Requested, parcel.Scheduleded, parcel.PickedUp, parcel.Delivered); }
                catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
            }
            else
                throw new ParcelCantBePickedUPException();
        }//
        public void DeliveringParcelByDrone(int Id)
        {
            DroneForList drone = DisplayDrone(Id);
            var parcel = myDalObject.CopyParcel(drone.InDeliveringParcelId);
            double batterySpent;
            if (parcel.PickedUp != null && parcel.Requested != null && parcel.Scheduleded != null && parcel.Delivered == null)
            {
                double distanceBetweenSenderAndDst = Distance(myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude, myDalObject.CopyCustomer(parcel.TargetId).Longitude, myDalObject.CopyCustomer(parcel.TargetId).Lattitude);//distance between sender and target
                double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDalObject.CopyCustomer(parcel.SenderId).Longitude, myDalObject.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
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
                batterySpent = (myDalObject.DronePowerConsumingPerKM()[0] * distanceBetweenDroneAndSender) + (batteryPerKM * distanceBetweenSenderAndDst);
                drone.Battery -= batterySpent;
                drone.CurrentLocation = AddLocation(myDalObject.CopyCustomer(parcel.TargetId).Longitude, myDalObject.CopyCustomer(parcel.TargetId).Lattitude);
                drone.DroneState = Enums.DroneStatuses.Available;
                try { myDalObject.RemoveParcel(parcel.Id); }
                catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                parcel.Delivered = DateTime.Now;
                try { myDalObject.AddParcel(parcel.Id, parcel.DroneId, parcel.SenderId, parcel.TargetId, parcel.Priority, parcel.Weight, parcel.Requested, parcel.Scheduleded, parcel.PickedUp, parcel.Delivered); }
                catch (DAL.DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
            }
            else
                throw new ParcelCantBeDeliveredException();
        }//
        public Parcel DisplayParcel(int id)
        {
            var parcel = myDalObject.CopyParcel(id);
            DroneForList drone = DisplayDrone(parcel.DroneId);
            DroneInParcel droneInParcel = new DroneInParcel { BatteryState = drone.Battery, DroneId = drone.DroneId, CurrentLocation = drone.CurrentLocation };
            CustomerInParcel source = new CustomerInParcel { CustomerId = parcel.SenderId, CustomerName = myDalObject.CopyCustomer(parcel.SenderId).Name };
            CustomerInParcel destination = new CustomerInParcel { CustomerId = parcel.TargetId, CustomerName = myDalObject.CopyCustomer(parcel.TargetId).Name };
            Parcel nParcel = new Parcel { ParcelId = parcel.Id, ParcelWC = WeightParcel(parcel.Weight), ParcelCreationTime = parcel.Requested, ParcelAscriptionTime = parcel.Scheduleded, ParcelPickUpTime = parcel.PickedUp, ParcelDeliveringTime = parcel.Delivered, ParcelPriority = (Enums.Priorities)parcel.Priority, DInParcel = droneInParcel, DCIParcel = destination, SCIParcel = source };
            return nParcel;
        }
        public void RemoveParcel(int id)
        {
            foreach (var parcel in myDalObject.CopyParcelsList())
            {
                if(parcel.Id==id)
                {
                    try { myDalObject.RemoveParcel(id); }
                    catch (DAL.DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                    return;
                }
            }
            throw new ParcelIdNotFoundException();
        }
        public IEnumerable<ParcelToList> DisplayParcelsList(Predicate<ParcelToList> predicate)
        {
            IEnumerable<DAL.DO.Parcel> p = myDalObject.CopyParcelsList();
            List<ParcelToList> nPList = new List<ParcelToList>();
            //foreach (var (parcel, pState) in from parcel in p
            //                                 let pState = Enums.ParcelState.Created//for now just fpr defalt, what need ro be DAL.DOne is to catch exception if parcel not found
            //                                 select (parcel, pState))//linq
            //{
            //    if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.Created;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.Ascripted;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.PickedUp;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
            //        pState = Enums.ParcelState.Delivered;
            //    ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDalObject.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDalObject.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
            //    nPList.Add(nP);
            //}
            foreach (var parcel in p)//the linq doesnt working
            {
                Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be DAL.DOne is to catch exception if parcel not found
                if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Created;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Ascripted;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                    pState = Enums.ParcelState.PickedUp;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                    pState = Enums.ParcelState.Delivered;
                ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDalObject.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDalObject.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
                nPList.Add(nP);
            }
            nPList = nPList.FindAll(predicate);
            return nPList;
        }//
        public IEnumerable<ParcelToList> DisplayUnAscriptedParcelsList()
        {
            IEnumerable<DAL.DO.Parcel> p = myDalObject.UnAscriptedParcels();
            List<ParcelToList> nPList = new List<ParcelToList>();
            //foreach (var (parcel, pState) in from parcel in p
            //                                 let pState = Enums.ParcelState.Created//for now just fpr defalt, what need ro be DAL.DOne is to catch exception if parcel not found
            //                                 select (parcel, pState))//linq
            //{
            //    if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.Created;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.Ascripted;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
            //        pState = Enums.ParcelState.PickedUp;
            //    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
            //        pState = Enums.ParcelState.Delivered;
            //    ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDalObject.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDalObject.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
            //    nPList.Add(nP);
            //}
            foreach (var parcel in p)//linq doesnt working
            {
                Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be DAL.DOne is to catch exception if parcel not found
                if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Created;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Ascripted;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                    pState = Enums.ParcelState.PickedUp;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                    pState = Enums.ParcelState.Delivered;
                ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDalObject.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDalObject.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
                nPList.Add(nP);
            }
            return nPList;
        }

    }

}