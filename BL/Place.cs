using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    interface Place
    {
        partial class BL//מימוש הממשק IBL
        {
            double longitude;
            double latitude;
        }
    }
}
