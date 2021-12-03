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
        
        void IBL.AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority)
        {
            myDalObject.AddParcel(0, SCustomerId, DCustomerId, (IDAL.DO.Priorities)Priority, (IDAL.DO.WeightCategories)Weight, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }
        void IBL.DeliveringParcelByDrone(int Id)
        { 
            foreach (IDAL.DO.Parcel parcel in myDalObject.CopyParcelsList())
            {

            }
        }//
        void IBL.AscriptionParcelToDrone(int Id) { }//foreach to dinf an empty drone then call the ascription func in dal.
        void IBL.DisplayParcel(int id) { }//
        void IBL.PickUpParcel(int DId) { }//
        void IBL.DisplayParcelsList() { }//
        void IBL.DisplayUnAscriptedParcelsList() { }//

    }
}
