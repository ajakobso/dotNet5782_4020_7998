using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerForList
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public int NumOfdeliveredParcelsarcels { get; set; }
        public int NumOfUndeliveredParcelsarcels { get; set; }
        public int NumOfRecivedParcels { get; set; }
        public int NumOfParcelsOnTheWay { get; set; }
        public override string ToString()
        {
            return $"id = {CustomerId}, name = {CustomerName}, phone = {CustomerPhone}, number of delivered parcels = {NumOfdeliveredParcelsarcels}, number of undelivered parcels = {NumOfUndeliveredParcelsarcels}, number of recived parcels = {NumOfRecivedParcels},number of undelivered parcels = {NumOfUndeliveredParcelsarcels}, number of parcels on the way = {NumOfParcelsOnTheWay} ";
        }
    }
}
