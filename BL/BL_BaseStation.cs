using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public partial class BL : IBL
    {

        void AddBaseStation(int num, string name, string location, int numOfAvailableDCharge)
        {
            
        }
        void UpdateBaseStation(int Id, string Name, int NumOfChargeSlots);
        void DisplayBaseStation(int id);
        void DisplayBaseStationsList();
        void DisplayAvailableChargingStation();

    }
}
