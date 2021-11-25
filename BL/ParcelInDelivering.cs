using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInDelivering
    {
        public int ParcelId { get; set; }
        public bool ParcelState { get; set; }//true if the parcel picked up, false if its waiting for pick up    
        public Enums.WeightCategories ParcelWC { get; set; }
        public Enums.Priorities ParcelPriority { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location SenderLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double Distance { get; set; }
    }
}
