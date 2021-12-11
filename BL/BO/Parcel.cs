using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Parcel
    {
        //partial class BL//מימוש הממשק BlApi
        //{
            public int ParcelId { get; set; }
            public CustomerInParcel SCIParcel { get; set; }//the sender
            public CustomerInParcel DCIParcel { get; set; }//the reciever
            public Enums.WeightCategories ParcelWC { get; set; }
            public Enums.Priorities ParcelPriority { get; set; }
            public DroneInParcel DInParcel { get; set; }
            public DateTime? ParcelCreationTime { get; set; }
            public DateTime? ParcelAscriptionTime { get; set; }
            public DateTime? ParcelPickUpTime { get; set; }
            public DateTime? ParcelDeliveringTime { get; set; }

       
        //}
    }
}
