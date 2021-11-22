using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    interface CustomerToList
    {
        partial class BL//מימוש הממשק IBL
        {
            int CustomerId;
            string CustomerName;
            int CustomerPhone;
            int NumOfProvidedParcels;
            int NumOfUnProvidedParcels;
            int NumOfRecivedParcels;
            int NumOfParcelsOnTheWay;
        }
    }
}
