using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    internal sealed partial class BL
    {
        private IEnumerable<ParcelInCustomer> ListOfParcelsInC(string option, int Pid)
        {
            lock (myDal)
            {
                List<ParcelInCustomer> res1 = new List<ParcelInCustomer> { };
                List<ParcelInCustomer> res2 = new List<ParcelInCustomer> { };
                foreach (var parcel in myDal.CopyParcelsList())//////////////////not linq-doesnt working
                {
                    ParcelInCustomer nParcel;
                    Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be DOne is to catch exception if parcel not found
                    if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Created;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                        pState = Enums.ParcelState.Ascripted;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                        pState = Enums.ParcelState.PickedUp;
                    if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                        pState = Enums.ParcelState.Delivered;
                    CustomerInParcel source = new CustomerInParcel { CustomerId = parcel.SenderId, CustomerName = myDal.CopyCustomer(parcel.SenderId).Name };
                    CustomerInParcel destination = new CustomerInParcel { CustomerId = parcel.TargetId, CustomerName = myDal.CopyCustomer(parcel.TargetId).Name };
                    if (option == "PtoC" && parcel.TargetId == Pid)
                    {
                        nParcel = new ParcelInCustomer { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, ParcelState = pState, ParcelWC = WeightParcel(parcel.Weight), SourceCustomer = source, DestinationCustomer = destination };
                        res1.Add(nParcel);
                    }
                    if (option == "PfromC" && parcel.SenderId == Pid)
                    {
                        nParcel = new ParcelInCustomer { ParcelId = parcel.Id, ParcelPriority = (Enums.Priorities)parcel.Priority, ParcelState = pState, ParcelWC = WeightParcel(parcel.Weight), SourceCustomer = source, DestinationCustomer = destination };
                        res2.Add(nParcel);
                    }
                }

                if (option == "PtoC")
                    return res1;
                else
                    return res2;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(int Id, string Name, string PhoneNum, Location Location)
        {
            //try { Location = AddLocation(Location.Long, Location.Lat); }
            //catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//catch this in pl
            lock (myDal)
            {
                if ((myDal.CopyLongitudeRange()[0] > Location.Long) || (myDal.CopyLongitudeRange()[1] < Location.Long) || (myDal.CopyLattitudeRange()[0] > Location.Lat) || (myDal.CopyLattitudeRange()[1] < Location.Lat))
                {
                    throw new LocationOutOfRangeException();
                }
                try { myDal.AddCustomer(Id, Name, PhoneNum, Location.Long, Location.Lat); }
                catch (DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int Id, string Name, string PhoneNum)
        {
            lock (myDal)
            {
                var customer = myDal.CopyCustomer(Id);
                try { myDal.RemoveCustomer(Id); }
                catch (DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }
                string name = customer.Name;
                if (Name != " ")
                {
                    name = Name;
                }
                string phone = customer.Phone;
                if (PhoneNum != " ")
                {
                    phone = PhoneNum;
                }
                try { myDal.AddCustomer(Id, name, phone, customer.Longitude, customer.Lattitude); }
                catch (DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer DisplayCustomer(int id)
        {
            lock (myDal)
            {
                var customer = myDal.CopyCustomer(id);
                Location location = AddLocation(customer.Longitude, customer.Lattitude);
                IEnumerable<ParcelInCustomer> PtoC = ListOfParcelsInC("PtoC", id);
                IEnumerable<ParcelInCustomer> PfromC = ListOfParcelsInC("PfromC", id);
                Customer nCustomer = new Customer { CustomerId = customer.Id, CustomerName = customer.Name, CustomerPhone = customer.Phone, Place = location, ParcelsToCustomer = PtoC, ParcelsFromCustomer = PfromC };
                return nCustomer;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerForList> DisplayCustomersList(Predicate<CustomerForList> predicate)
        {
            IEnumerable<CustomerForList> responce = new List<CustomerForList>();
            #region notes
            /*
             foreach (var (blC, deliveredParcels, undeliveredParcels, recivedP, onTheWayP) in from customer in myDal.CopyCustomersList()//linq-doesnt working
            //                                                                     let blC = DisplayCustomer(customer.Id)
            //                                                                     let deliveredParcels = 0
            //                                                                     let undeliveredParcels = 0
            //                                                                     let recivedP = 0
            //                                                                     let onTheWayP = 0
            //                                                                     select (blC, deliveredParcels, undeliveredParcels, recivedP, onTheWayP))
            //{
            //    foreach (var parcel in blC.ParcelsToCustomer)
            //    {
            //        recivedP++;
            //    }

            //    foreach (var parcel in blC.ParcelsFromCustomer)
            //    {
            //        if (parcel.ParcelState == Enums.ParcelState.Delivered)
            //            deliveredParcels++;
            //        if (parcel.ParcelState == Enums.ParcelState.PickedUp)
            //            onTheWayP++;
            //        if (parcel.ParcelState == Enums.ParcelState.Created || parcel.ParcelState == Enums.ParcelState.Ascripted)
            //            undeliveredParcels++;
            //    }

            //    CustomerForList nCustomer = new CustomerForList { CustomerId = blC.CustomerId, CustomerName = blC.CustomerName, CustomerPhone = blC.CustomerPhone, NumOfdeliveredParcelsarcels = deliveredParcels, NumOfParcelsOnTheWay = onTheWayP, NumOfRecivedParcels = recivedP, NumOfUndeliveredParcelsarcels = undeliveredParcels };
            //    responce.Add(nCustomer);
            //}*/
            #endregion
            lock (myDal)
            {
                responce = from DO.Customer DalCustomer in myDal.CopyCustomersList()
                           let blCustomer = DisplayCustomer(DalCustomer.Id)
                           let deliveredParcels = blCustomer.ParcelsFromCustomer.Where(x => x.ParcelState == Enums.ParcelState.Delivered).Count()
                           let undeliveredParcels = blCustomer.ParcelsFromCustomer.Where(x => x.ParcelState == Enums.ParcelState.Created || x.ParcelState == Enums.ParcelState.Ascripted).Count()
                           let recivedParcels = blCustomer.ParcelsToCustomer.Count()
                           let onTheWayParcels = blCustomer.ParcelsFromCustomer.Where(x => x.ParcelState == Enums.ParcelState.PickedUp).Count()
                           select new CustomerForList { CustomerId = blCustomer.CustomerId, CustomerName = blCustomer.CustomerName, CustomerPhone = blCustomer.CustomerPhone, NumOfdeliveredParcelsarcels = deliveredParcels, NumOfParcelsOnTheWay = onTheWayParcels, NumOfRecivedParcels = recivedParcels, NumOfUndeliveredParcelsarcels = undeliveredParcels };
                Func<CustomerForList, bool> func = new(predicate);//non-direct convertion between the predicate to Fucn in order to use the Where method
                return responce.Where(func);
                
            }
        }
    }
}
