using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL.BL//?????????????????
    {

        void AddDrone(int Id, string Model, IBL.BO.Enums.WeightCategories MaxWeight, int Bstation)
        {
            dal.DalObject.AddDrone(Id, MaxWeight, Model, Bstation);//צריך לטפל בפונ' שבדאטה סורס
        }
        void UpdateDrone(int Id, string Model);
        void DroneToCharge(int Id);
        void ReleaseDroneFromCharge(int Id, DateTime TimeInCharge);
        void DisplayDrone(int id);
        void DisplayDronesList();

    }
}
