using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        //partial class BL//מימוש הממשק IBL
        //{
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public Location Place { get; set; }
            public IEnumerable<ParcelInCustomer> ParcelsFromCustomer { get; set; }
            public IEnumerable<ParcelInCustomer> ParcelsToCustomer { get; set; }
        //}
    }
}
