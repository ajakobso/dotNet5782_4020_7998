using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class Parcel
    {
        partial class BL//מימוש הממשק IBL
        {
            int ParcelId;
            CustomerInParcel DCIParcel;//השולח
            CustomerInParcel RCIParcel;//המקבל
            Enums.WeightCategories ParcelWC;
        }
    }
}
