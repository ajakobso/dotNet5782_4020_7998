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
            public DateTime Requested { get; set; }
            public DateTime Scheduleded { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return $"id = Id, sender id = SenderId, target id = TargetId, drone id = DroneId, weight = Weight, priority = Priority, requested time = Requested, scheduleded time = Scheduleded, pick up time = PickedUp, delivering time = Delivered ";
            }
        }
    }
}
