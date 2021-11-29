using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {

        void AddParcelToDeliver(int SCustomerId, int DCustomerId, Enums.WeightCategories Weight, Enums.Priorities Priority);
        void DeliveringParcelByDrone(int Id);
        void AscriptionParcelToDrone(int Id);
        void DisplayParcel(int id);
        void PickUpParcel(int DId);
        void DisplayParcelsList();
        void DisplayUnAscriptedParcelsList();

    }
}
