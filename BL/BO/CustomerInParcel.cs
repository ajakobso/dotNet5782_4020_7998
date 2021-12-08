using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerInParcel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public override string ToString()
        {
            return $"id = {CustomerId}, name = {CustomerName}";
        }
    }
}
