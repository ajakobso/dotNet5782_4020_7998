using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public int DroneId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public datetime Requested { get; set; }
            public datetime Scheduleded { get; set; }
            public datetime PickedUp { get; set; }
            public datetime Delivered { get; set; }
            public override string ToString()
            {
                return base.ToString();
            }
        }
    }
}
