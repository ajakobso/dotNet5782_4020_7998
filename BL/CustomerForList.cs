﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerForList
    {
        //partial class BL//מימוש הממשק IBL
        //{
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public string CustomerPhone { get; set; }
            public int NumOfDeliveredParcels { get; set; }
            public int NumOfUnDeliveredParcels { get; set; }
            public int NumOfRecivedParcels { get; set; }
            public int NumOfParcelsOnTheWay { get; set; }
        //}
    }
}
