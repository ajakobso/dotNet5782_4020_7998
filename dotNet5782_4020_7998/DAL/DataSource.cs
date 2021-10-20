using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    class DataSource
    {
        internal List<Drone> Drones = new List<Drone>();
        internal List<BaseStation> BaseStations = new List<BaseStation>();
        internal Customer[] Customers = new Customer[100];
        internal Parcel[] Parcels = new Parcel[1000];
       internal class Config//fields of the first empty element in every array.
        {
            internal static int FDroneAvailable = 0;///
            internal static int FBaseStationAvailable = 0;
            internal static int FCustomerAvailable = 0;
            internal static int FParcelavailable = 0;
            //שדה ליצירת מספר מזהה עבור חבילות
        }
        //מתודה סטטית  לאיתחול מופעי הישויות עם נתונים וכו
        static void Initialize ()
        {
            Random r = new Random();
            BaseStations[0].Name = "station1";
            for (int i = 0; i < 10; i++)
            {


            }
        }
    }
}
