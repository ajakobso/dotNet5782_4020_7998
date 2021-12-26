﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.BO;

namespace BL
{
    internal sealed partial class BL
    {
        private IEnumerable<ParcelInCustomer> ListOfParcelsInC(string option, int Pid)
        {
            List<ParcelInCustomer> res1 = new List<ParcelInCustomer> { };
            List<ParcelInCustomer> res2 = new List<ParcelInCustomer> { };
            foreach (var parcel in myDalObject.CopyParcelsList())//////////////////not linq-doesnt working
            {
                ParcelInCustomer nParcel;
                Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be DAL.DOne is to catch exception if parcel not found
                if (parcel.Requested != null && parcel.Scheduleded == null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Created;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp == null && parcel.Delivered == null)
                    pState = Enums.ParcelState.Ascripted;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered == null)
                    pState = Enums.ParcelState.PickedUp;
                if (parcel.Requested != null && parcel.Scheduleded != null && parcel.PickedUp != null && parcel.Delivered != null)
                    pState = Enums.ParcelState.Delivered;
                CustomerInParcel source = new CustomerInParcel { CustomerId = parcel.SenderId, CustomerName = myDalObject.CopyCustomer(parcel.SenderId).Name };
                CustomerInParcel destination = new CustomerInParcel { CustomerId = parcel.TargetId, CustomerName = myDalObject.CopyCustomer(parcel.TargetId).Name };
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
        public void AddCustomer(int Id, string Name, string PhoneNum, Location Location)
        {
            //try { Location = AddLocation(Location.Long, Location.Lat); }
            //catch (LocationOutOfRangeException) { throw new LocationOutOfRangeException(); }//catch this in pl
            if ((myDalObject.CopyLongitudeRange()[0] > Location.Long) || (myDalObject.CopyLongitudeRange()[1] < Location.Long) || (myDalObject.CopyLattitudeRange()[0] > Location.Lat) || (myDalObject.CopyLattitudeRange()[1] < Location.Lat))
            {
                throw new LocationOutOfRangeException();
            }
            try { myDalObject.AddCustomer(Id, Name, PhoneNum, Location.Long, Location.Lat); }
            catch (DAL.DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }


        }//
        public void UpdateCustomer(int Id, string Name, string PhoneNum)
        {
            var customer = myDalObject.CopyCustomer(Id);
            try { myDalObject.RemoveCustomer(Id); }
            catch (DAL.DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }
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
            try { myDalObject.AddCustomer(Id, name, phone, customer.Longitude, customer.Lattitude); }
            catch (DAL.DO.AddExistingCustomerException) { throw new AddExistingCustomerException(); }
        }
        public Customer DisplayCustomer(int id)
        {
            var customer = myDalObject.CopyCustomer(id);
            Location location = AddLocation(customer.Longitude, customer.Lattitude);
            IEnumerable<ParcelInCustomer> PtoC = ListOfParcelsInC("PtoC", id);
            IEnumerable<ParcelInCustomer> PfromC = ListOfParcelsInC("PfromC", id);
            Customer nCustomer = new Customer { CustomerId = customer.Id, CustomerName = customer.Name, CustomerPhone = customer.Phone, Place = location, ParcelsToCustomer = PtoC, ParcelsFromCustomer = PfromC };
            return nCustomer;
        }
        public IEnumerable<CustomerForList> DisplayCustomersList(Predicate<CustomerForList> predicate)
        {
            IEnumerable<CustomerForList> responce = new List<CustomerForList>();
            //foreach (var (blC, deliveredP, unDeliveredP, recivedP, onTheWayP) in from customer in myDalObject.CopyCustomersList()//linq-doesnt working
            //                                                                     let blC = DisplayCustomer(customer.Id)
            //                                                                     let deliveredP = 0
            //                                                                     let unDeliveredP = 0
            //                                                                     let recivedP = 0
            //                                                                     let onTheWayP = 0
            //                                                                     select (blC, deliveredP, unDeliveredP, recivedP, onTheWayP))
            //{
            //    foreach (var parcel in blC.ParcelsToCustomer)
            //    {
            //        recivedP++;
            //    }

            //    foreach (var parcel in blC.ParcelsFromCustomer)
            //    {
            //        if (parcel.ParcelState == Enums.ParcelState.Delivered)
            //            deliveredP++;
            //        if (parcel.ParcelState == Enums.ParcelState.PickedUp)
            //            onTheWayP++;
            //        if (parcel.ParcelState == Enums.ParcelState.Created || parcel.ParcelState == Enums.ParcelState.Ascripted)
            //            unDeliveredP++;
            //    }

            //    CustomerForList nCustomer = new CustomerForList { CustomerId = blC.CustomerId, CustomerName = blC.CustomerName, CustomerPhone = blC.CustomerPhone, NumOfDeliveredParcels = deliveredP, NumOfParcelsOnTheWay = onTheWayP, NumOfRecivedParcels = recivedP, NumOfUnDeliveredParcels = unDeliveredP };
            //    responce.Add(nCustomer);
            //}

            foreach (var customer in myDalObject.CopyCustomersList())
            {
                Customer blC = DisplayCustomer(customer.Id);
                int deliveredP = 0, unDeliveredP = 0, recivedP = 0, onTheWayP = 0;
                foreach (var parcel in blC.ParcelsToCustomer)
                {
                    recivedP++;
                }
                foreach (var parcel in blC.ParcelsFromCustomer)
                {
                    if (parcel.ParcelState == Enums.ParcelState.Delivered)
                        deliveredP++;
                    if (parcel.ParcelState == Enums.ParcelState.PickedUp)
                        onTheWayP++;
                    if (parcel.ParcelState == Enums.ParcelState.Created || parcel.ParcelState == Enums.ParcelState.Ascripted)
                        unDeliveredP++;
                }

                CustomerForList nCustomer = new CustomerForList { CustomerId = blC.CustomerId, CustomerName = blC.CustomerName, CustomerPhone = blC.CustomerPhone, NumOfDeliveredParcels = deliveredP, NumOfParcelsOnTheWay = onTheWayP, NumOfRecivedParcels = recivedP, NumOfUnDeliveredParcels = unDeliveredP };
                (responce as List<CustomerForList>).Add(nCustomer);
            }
            responce = (responce as List<CustomerForList>).FindAll(predicate);
            return responce;
        }//

    }
}
