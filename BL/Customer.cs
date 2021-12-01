using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class customer
    {
        //partial class BL//מימוש הממשק IBL
        //{
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public Location Place { get; set; }
            public ParcelInCustomer PICustomer { get; set; }//bc parcelincustomer is inernal but when it was public it casue problems
            public IEnumerable<List<Drone>> ParcelsFromCustomer { get; set; }
            public IEnumerable<List<Drone>> ParcelsToCustomer { get; set; }
        //}
    }
}
