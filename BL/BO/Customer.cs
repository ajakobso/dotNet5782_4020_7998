using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public Location Place { get; set; }
        public IEnumerable<ParcelInCustomer> ParcelsFromCustomer { get; set; }
        public IEnumerable<ParcelInCustomer> ParcelsToCustomer { get; set; }
        public override string ToString()
        {
            return $"id = {CustomerId}, name = {CustomerName}, phone = {CustomerPhone}, location = {Place}, list of parcels sent from {CustomerName} = {ParcelsFromCustomer}, list of parcels {CustomerName} recieved = {ParcelsToCustomer}";
        }

    }
}
