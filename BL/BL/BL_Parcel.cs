using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
namespace BL
{
    internal sealed partial class BL// : BO.BlApi
    {
        internal static double Distance(double long1, double lat1, double long2, double lat2)
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
        internal double minimumBattery(DroneForList drone, DO.Parcel parcel)
        {
            lock (myDal)
            {
                Location location;
                location = AddLocation(myDal.CopyCustomer(parcel.TargetId).Longitude, myDal.CopyCustomer(parcel.TargetId).Lattitude);
                double distanceBetweenSenderAndDst = Distance(myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude, myDal.CopyCustomer(parcel.TargetId).Longitude, myDal.CopyCustomer(parcel.TargetId).Lattitude);//distance between sender and target
                double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                double distanceBetweenDstAndBs = distanceFromBS(location)[0];//distance from target to closest bs
                double batteryPerKM = 0;//defult
                switch ((Enums.WeightCategories)parcel.Weight)
                {
                    case Enums.WeightCategories.Light:
                        batteryPerKM = myDal.DronePowerConsumingPerKM()[1];
                        break;
                    case Enums.WeightCategories.Middle:
                        batteryPerKM = myDal.DronePowerConsumingPerKM()[2];
                        break;
                    case Enums.WeightCategories.Heavy:
                        batteryPerKM = myDal.DronePowerConsumingPerKM()[3];
                        break;
                    default:
                        break;
                }
                double possBlApieDistance = (myDal.DronePowerConsumingPerKM()[0] * distanceBetweenDroneAndSender) + (myDal.DronePowerConsumingPerKM()[0] * distanceBetweenDstAndBs) + (batteryPerKM * distanceBetweenSenderAndDst);//calculation of the amount of battery in percent needed for the
                return possBlApieDistance;
            }                                                                                                                                                                                                                             //
        }
        internal bool droneMakeIt(DroneForList drone, DO.Parcel parcel)//בודק אם לרחפן יש מספיק בטריה להגיע לשולח, ליעד ולתחנה הקרובה ביותר מיעד המשלוח
        {
            double minimum = minimumBattery(drone, parcel);
            if (minimum <= drone.Battery)//check if there is enough battery
                return true;
            else
                return false;
        }
        internal double[] distanceFromBS(Location location)//return ths distance from the closest base station and its id
        {
            lock (myDal)
            {
                double[] res = new double[2];
                double dis;
                res[0] = 1000;//very big number just to start with, since our company DO delivering in jerusalem so 1000 km is higher than the distance between the actual places.
                foreach (var bs in myDal.CopyBaseStations())
                {
                    dis = Distance(location.Long, location.Lat, bs.Longitude, bs.Lattitude);
                    if (res[0] > dis)
                    {
                        res[0] = dis;
                        res[1] = bs.Id;
                    }
                }
                ///res[0] = distance;//the distance between the location to the closest bs
                ///res[1] = bsID;//the bs id
                return res;
            }
        }
        internal Enums.WeightCategories WeightParcel(DO.WeightCategories v)//convert frome DO.WeightCategories to BO.WeightCategories 
        {
            lock (myDal)
            {
                Enums.WeightCategories w;
                switch (v)
                {

                    case DO.WeightCategories.Light:
                        w = Enums.WeightCategories.Light;
                        return w;

                    case DO.WeightCategories.Middle:
                        w = Enums.WeightCategories.Middle;
                        return w;
                    case DO.WeightCategories.Heavy:
                        w = Enums.WeightCategories.Heavy;
                        return w;
                    default:
                        w = Enums.WeightCategories.Heavy;
                        return w;

                }
            }
        }
        internal bool ProperWeight(Enums.WeightCategories dWeight, Enums.WeightCategories pWeight)//check if the drone can carry the parcel
        {
            lock (myDal)
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
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority)
        {
            lock (myDal)
            {
                try { myDal.AddParcel(-1, 0, SCustomerId, DCustomerId, (DO.Priorities)Priority, (DO.WeightCategories)Weight, DateTime.Now, null, null, null); }
                catch (DO.AddParcelToAnAsscriptedDroneException) { }
            }
        }//
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AscriptionParcelToDrone(int Id)
        {
            lock (myDal)
            {
                DroneForList drone = DisplayDrone(Id);
                bool parcelFound = false;
                if (drone.DroneState == Enums.DroneStatuses.Available)
                {
                    foreach (var parcel in from parcel in myDal.CopyParcelsList()
                                           where parcel.Scheduleded == null && parcel.Priority == (DO.Priorities)Enums.Priorities.Urgent && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel)
                                           select parcel)
                    {
                        try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                        catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                        catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                        drone.DroneState = Enums.DroneStatuses.Shipping;
                        drone.InDeliveringParcelId = parcel.Id;
                        parcelFound = true;
                        break;
                    }
                    //foreach (var parcel in myDal.CopyParcelsList())-not linq
                    //{
                    //    if (parcel.Priority == (DO.Priorities)Enums.Priorities.Urgent && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel))
                    //    {
                    //        try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                    //        catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    //        catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                    //        drone.DroneState = Enums.DroneStatuses.Shipping;
                    //        drone.InDeliveringParcelId = parcel.Id;
                    //        parcelFound = true;
                    //        return;
                    //    }
                    //}
                    if (parcelFound == false)
                    {
                        foreach (var parcel in from parcel in myDal.CopyParcelsList()
                                               where parcel.Scheduleded == null && parcel.Priority == (DO.Priorities)Enums.Priorities.Fast && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel)
                                               select parcel)//linq
                        {
                            try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                            catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                            catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                            drone.DroneState = Enums.DroneStatuses.Shipping;
                            drone.InDeliveringParcelId = parcel.Id;
                            parcelFound = true;
                            break;
                        }
                        //foreach (var parcel in myDal.CopyParcelsList())-not linq
                        //{
                        //    if (drone.MaxWeight == WeightParcel(parcel.Weight) && droneMakeIt(drone, parcel))
                        //    {
                        //        try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                        //        catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                        //        catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                        //        drone.DroneState = Enums.DroneStatuses.Shipping;
                        //        drone.InDeliveringParcelId = parcel.Id;
                        //        parcelFound = true;
                        //        return;
                        //    }
                        //}
                        if (parcelFound == false)
                        {
                            double maxDistance = 1000;
                            foreach (var (parcel, distanceBetweenDroneAndSender) in from parcel in myDal.CopyParcelsList()
                                                                                    let distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude)//distance between the drone and the sender's location
                                                                                    where parcel.Scheduleded == null && maxDistance > distanceBetweenDroneAndSender && ProperWeight(drone.MaxWeight, WeightParcel(parcel.Weight)) && droneMakeIt(drone, parcel)
                                                                                    select (parcel, distanceBetweenDroneAndSender))//linq
                            {
                                maxDistance = distanceBetweenDroneAndSender;
                                try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                                catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                                catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }

                                drone.DroneState = Enums.DroneStatuses.Shipping;
                                drone.InDeliveringParcelId = parcel.Id;
                                parcelFound = true;
                                break;
                            }
                            //foreach (var parcel in myDal.CopyParcelsList())-not linq
                            //{

                            //    double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                            //    if (maxDistance > distanceBetweenDroneAndSender && droneMakeIt(drone, parcel))
                            //    {
                            //        maxDistance = distanceBetweenDroneAndSender;
                            //        try { myDal.AscriptionPtoD(parcel.Id, drone.DroneId); }
                            //        catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                            //        catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
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
                else
                {
                    //drones.Remove(DisplayDrone(drone.DroneId));//remove the not updated drone from the list - no need to update dal because nothing changed there
                    //drones.Add(drone);//add the updated drone
                }
            }
        }//
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickUpParcel(int DId)
        {
            lock (myDal)
            {
                DroneForList drone = DisplayDrone(DId);
                var parcel = myDal.CopyParcel(drone.InDeliveringParcelId);
                if (parcel.DroneId == DId && parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null)
                {
                    double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                    drone.Battery -= distanceBetweenDroneAndSender * myDal.DronePowerConsumingPerKM()[0];
                    drone.CurrentLocation = AddLocation(myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude);
                    try { myDal.RemoveParcel(parcel.Id); }
                    catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                    parcel.PickedUp = DateTime.Now;
                    try { myDal.AddParcel(parcel.Id, parcel.DroneId, parcel.SenderId, parcel.TargetId, parcel.Priority, parcel.Weight, parcel.Requested, parcel.Scheduleded, parcel.PickedUp, parcel.Delivered); }
                    catch (DO.DroneIdNotFoundException) { throw new DroneIdNotFoundException(); }
                    catch (DO.ParcelIdNotFoundException) { throw new ParcelIdNotFoundException(); }
                }
                else
                    throw new ParcelCantBePickedUPException();
            }
        }//
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveringParcelByDrone(int Id)
        {
            lock (myDal)
            {
                DroneForList drone = DisplayDrone(Id);
                var parcel = myDal.CopyParcel(drone.InDeliveringParcelId);
                double batterySpent;
                if (parcel.PickedUp != null && parcel.Requested != null && parcel.Scheduleded != null && parcel.Delivered == null)
                {
                    double distanceBetweenSenderAndDst = Distance(myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude, myDal.CopyCustomer(parcel.TargetId).Longitude, myDal.CopyCustomer(parcel.TargetId).Lattitude);//distance between sender and target
                    double distanceBetweenDroneAndSender = Distance(drone.CurrentLocation.Long, drone.CurrentLocation.Lat, myDal.CopyCustomer(parcel.SenderId).Longitude, myDal.CopyCustomer(parcel.SenderId).Lattitude);//distance between the drone and the sender's location
                    double batteryPerKM = 0;//defult
                    switch ((Enums.WeightCategories)parcel.Weight)
                    {
                        case Enums.WeightCategories.Light:
                            batteryPerKM = myDal.DronePowerConsumingPerKM()[1];
                            break;
                        case Enums.WeightCategories.Middle:
                            batteryPerKM = myDal.DronePowerConsumingPerKM()[2];
                            break;
                        case Enums.WeightCategories.Heavy:
                            batteryPerKM = myDal.DronePowerConsumingPerKM()[3];
                            break;
                        default:
                            break;
                    }
                    batterySpent = (myDal.DronePowerConsumingPerKM()[0] * distanceBetweenDroneAndSender) + (batteryPerKM * distanceBetweenSenderAndDst);
                    drone.Battery -= batterySpent;
                    drone.CurrentLocation = AddLocation(myDal.CopyCustomer(parcel.TargetId).Longitude, myDal.CopyCustomer(parcel.TargetId).Lattitude);
                    drone.DroneState = Enums.DroneStatuses.Available;
                    drone.InDeliveringParcelId = 0;
                    updateDroneForList(drone);
                    try { myDal.RemoveParcel(parcel.Id); }
                    catch (DO.DroneIdNotFoundException) { throw new BO.DroneIdNotFoundException(); }
                    catch (DO.ParcelIdNotFoundException) { throw new BO.ParcelIdNotFoundException(); }
                    parcel.Delivered = DateTime.Now;
                    try { myDal.AddParcel(parcel.Id, parcel.DroneId, parcel.SenderId, parcel.TargetId, parcel.Priority, parcel.Weight, parcel.Requested, parcel.Scheduleded, parcel.PickedUp, DateTime.Now); }
                    catch (DO.DroneIdNotFoundException) { throw new BO.DroneIdNotFoundException(); }
                    catch (DO.ParcelIdNotFoundException) { throw new BO.ParcelIdNotFoundException(); }
                }
                else
                    throw new ParcelCantBeDeliveredException();
            }
        }//
        [MethodImpl(MethodImplOptions.Synchronized)]
        public BO.Parcel DisplayParcel(int id)
        {
            lock (myDal)
            {
                var parcel = myDal.CopyParcel(id);
                DroneInParcel droneInParcel;
                try
                {
                    DroneForList drone = DisplayDrone(parcel.DroneId);
                    droneInParcel = new DroneInParcel { BatteryState = drone.Battery, DroneId = drone.DroneId, CurrentLocation = drone.CurrentLocation };
                }
                catch (DroneIdNotFoundException) { droneInParcel = new DroneInParcel { DroneId = 0 }; }
                CustomerInParcel source = new CustomerInParcel { CustomerId = parcel.SenderId, CustomerName = myDal.CopyCustomer(parcel.SenderId).Name };
                CustomerInParcel destination = new CustomerInParcel { CustomerId = parcel.TargetId, CustomerName = myDal.CopyCustomer(parcel.TargetId).Name };
                Parcel nParcel = new Parcel { ParcelId = parcel.Id, ParcelWC = WeightParcel(parcel.Weight), ParcelCreationTime = parcel.Requested, ParcelAscriptionTime = parcel.Scheduleded, ParcelPickUpTime = parcel.PickedUp, ParcelDeliveringTime = parcel.Delivered, ParcelPriority = (Enums.Priorities)parcel.Priority, DInParcel = droneInParcel, DCIParcel = destination, SCIParcel = source };
                return nParcel;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int id)
        {
            lock (myDal)
            {
                foreach (var parcel in myDal.CopyParcelsList())
                {
                    if (parcel.Id == id)
                    {
                        try { myDal.RemoveParcel(id); }
                        catch (DO.ParcelIdNotFoundException) { throw new BO.ParcelIdNotFoundException(); }
                        return;
                    }
                }
                throw new BO.ParcelIdNotFoundException();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> DisplayParcelsList(Predicate<ParcelToList> predicate)
        {
            lock (myDal)
            {
                IEnumerable<DO.Parcel> p = myDal.CopyParcelsList();
                List<ParcelToList> nPList = new List<ParcelToList>();
                //foreach (var (parcel, pState) in from parcel in p
                //                                 let pState = Enums.ParcelState.Created//for now just fpr defalt, what need ro be DOne is to catch exception if parcel not found
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
                //    ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDal.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDal.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
                //    nPList.Add(nP);
                //}
                foreach (var parcel in p)//the linq doesnt working
                {
                    Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be DOne is to catch exception if parcel not found
                    if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Created;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Ascripted;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                        pState = Enums.ParcelState.PickedUp;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                        pState = Enums.ParcelState.Delivered;
                    ParcelToList nP = new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDal.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDal.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState };
                    nPList.Add(nP);
                }
                nPList = nPList.FindAll(predicate);
                return nPList;
            }
        }//
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> DisplayUnAscriptedParcelsList()
        {
            lock (myDal)
            {
                IEnumerable<DO.Parcel> p = myDal.UnAscriptedParcels();
                List<ParcelToList> nPList = new List<ParcelToList>();
                foreach (var parcel in from parcel in p
                                       select parcel)//linq
                {
                    Enums.ParcelState pState = new();
                    if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Created;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Ascripted;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                        pState = Enums.ParcelState.PickedUp;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                        pState = Enums.ParcelState.Delivered;
                    nPList.Add(new ParcelToList { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, SenderName = myDal.CopyCustomer(parcel.SenderId).Name, ReceiverName = myDal.CopyCustomer(parcel.TargetId).Name, ParcelWC = WeightParcel(parcel.Weight), ParcelState = pState });
                }
                return nPList;
            }
        }
    }

}