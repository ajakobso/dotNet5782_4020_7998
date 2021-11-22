using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class customer
    {
        partial class BL//מימוש הממשק IBL
        {
            int CustomerId;
            string CustomerName;
            int CustomerPhone;
            Place place;
            ParcelInCustomer PICustomer;
        }
    }
}
