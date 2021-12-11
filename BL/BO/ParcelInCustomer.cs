using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInCustomer
    {
        //partial class BL
        //{
        public int ParcelId { get; set; }
        public Enums.WeightCategories ParcelWC { get; set; }
        public Enums.Priorities ParcelPriority { get; set; }
        public Enums.ParcelState ParcelState { get; set; }
        public CustomerInParcel SourceCustomer { get; set; }
        public CustomerInParcel DestinationCustomer { get; set; }
        //}
    }
}
