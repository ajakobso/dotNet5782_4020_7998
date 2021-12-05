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
        private IEnumerable<ParcelInCustomer> listOfParcelsInC(string option, int Pid)
        {
            List<ParcelInCustomer> res1 = new List<ParcelInCustomer> { };
            List<ParcelInCustomer> res2 = new List<ParcelInCustomer> { };
            foreach (var parcel in myDalObject.CopyParcelsList())
            {
                ParcelInCustomer nParcel;
                Enums.ParcelState pState = Enums.ParcelState.Created;//for now just fpr defalt, what need ro be done is to catch exception if parcel not found
                if (parcel.Requested > DateTime.MinValue && parcel.Scheduleded == DateTime.MinValue && parcel.PickedUp == DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                    pState = Enums.ParcelState.Created;
                if (parcel.Requested > DateTime.MinValue && parcel.Scheduleded > DateTime.MinValue && parcel.PickedUp == DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                    pState = Enums.ParcelState.Ascripted;
                if (parcel.Requested > DateTime.MinValue && parcel.Scheduleded > DateTime.MinValue && parcel.PickedUp > DateTime.MinValue && parcel.Delivered == DateTime.MinValue)
                    pState = Enums.ParcelState.PickedUp;
                if (parcel.Requested > DateTime.MinValue && parcel.Scheduleded > DateTime.MinValue && parcel.PickedUp > DateTime.MinValue && parcel.Delivered > DateTime.MinValue)
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
            myDalObject.AddCustomer(Id, Name, PhoneNum, Location.Long, Location.Lat);
        }//
        public void UpdateCustomer(int Id, string Name, string PhoneNum)
        {
            var customer = myDalObject.CopyCustomer(Id);
            myDalObject.RemoveCustomer(Id);
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
            myDalObject.AddCustomer(Id, name, phone, customer.Longitude, customer.Lattitude);
        }//
        public Customer DisplayCustomer(int id)
        {
            var customer = myDalObject.CopyCustomer(id);
            Location location = new Location(customer.Longitude, customer.Lattitude);
            IEnumerable<ParcelInCustomer> PtoC = listOfParcelsInC("PtoC", id);
            IEnumerable<ParcelInCustomer> PfromC = listOfParcelsInC("PfromC", id);
            Customer nCustomer = new Customer { CustomerId = customer.Id, CustomerName = customer.Name, CustomerPhone = customer.Phone, Place = location, ParcelsToCustomer = PtoC, ParcelsFromCustomer = PfromC };
            return nCustomer;
        }
        public IEnumerable<CustomerForList> DisplayCustomersList()
        {
            List<CustomerForList> responce = new List<CustomerForList>();
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
                responce.Add(nCustomer);
            }
            return responce;
        }//

    }
}
