using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.BO.IBL//?????????????????
    {
        
        void IBL.BO.IBL.AddDrone(int Id, string Model, Enums.WeightCategories MaxWeight, int Bstation)
        {
            //myDalObject.AddDrone(Id, MaxWeight, Model/*, Bstation*/);//צריך לטפל בפונ' שבדאטה סורס
        }
        void IBL.BO.IBL.UpdateDrone(int Id, string Model){ }//לממש
        void IBL.BO.IBL.DroneToCharge(int Id) { }//לממש
        void IBL.BO.IBL.ReleaseDroneFromCharge(int Id, DateTime TimeInCharge) { }//לממש
        void IBL.BO.IBL.DisplayDrone(int id) { }//לממש
        void IBL.BO.IBL.DisplayDronesList() { }//לממש

    }
}
