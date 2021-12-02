using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL : IBL.BO.IBL
    {
        
        void IBL.BO.IBL.AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority)
        {
            IDAL.IDal myDalObject = new DalObject.DalObject();
            myDalObject.AddParcel(0, SCustomerId, DCustomerId, Priority, Weight, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }
        void IBL.BO.IBL.DeliveringParcelByDrone(int Id)
        { 
            
            foreach (Parcel parcel in DalObject.DataSource.Config.Parcels)
            {

            }
        }//
        void IBL.BO.IBL.AscriptionParcelToDrone(int Id) { }//foreach to dinf an empty drone then call the ascription func in dal.
        void IBL.BO.IBL.DisplayParcel(int id) { }//
        void IBL.BO.IBL.PickUpParcel(int DId) { }//
        void IBL.BO.IBL.DisplayParcelsList() { }//
        void IBL.BO.IBL.DisplayUnAscriptedParcelsList() { }//

    }
}
