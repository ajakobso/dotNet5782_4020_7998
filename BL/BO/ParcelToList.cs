using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
    public class ParcelToList
    {
        public int ParcelId { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public Enums.WeightCategories ParcelWC { get; set; }
        public Enums.Priorities ParcelPriority { get; set; }
        public Enums.ParcelState ParcelState { get; set; }
    }
}
