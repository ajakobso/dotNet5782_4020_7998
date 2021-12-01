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

        void IBL.BO.IBL.AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority) { }//impliment all the func here
        void IBL.BO.IBL.DeliveringParcelByDrone(int Id) { }//
        void IBL.BO.IBL.AscriptionParcelToDrone(int Id) { }//
        void IBL.BO.IBL.DisplayParcel(int id) { }//
        void IBL.BO.IBL.PickUpParcel(int DId) { }//
        void IBL.BO.IBL.DisplayParcelsList() { }//
        void IBL.BO.IBL.DisplayUnAscriptedParcelsList() { }//

    }
}
